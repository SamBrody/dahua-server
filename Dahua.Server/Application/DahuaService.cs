using System.Runtime.InteropServices;
using Dahua.Server.Configuration;
using Dahua.Server.Model;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using NetSDKCS;

namespace Dahua.Server.Application;

public interface IDahuaService {
    void Start();
    void Stop();
}

public class DahuaService : IDahuaService {
    private readonly DahuaOptions cfg;
    private readonly ILogger logger;
    private readonly IHubContext<EventHub> hub;
    
    private static fDisConnectCallBack m_DisConnectCallBack;
    private static fHaveReConnectCallBack m_ReConnectCallBack;
    private static fAnalyzerDataCallBack m_AnalyzeHandleCallBack;
    
    private IntPtr m_LoginID = IntPtr.Zero;
    private NET_DEVICEINFO_Ex m_DeviceInfo;
    private Int64 m_ID = 1;
    private IntPtr m_RealPlayID = IntPtr.Zero;
    private IntPtr m_EventID = IntPtr.Zero;
    
    public DahuaService(
        IOptions<DahuaOptions> options,
        ILogger<DahuaService> logger,
        IHubContext<EventHub> hub
    ) {
        cfg = options.Value;
        this.logger = logger;
        this.hub = hub;

        m_DisConnectCallBack = DisConnectCallBack;
        m_ReConnectCallBack = ReConnectCallBack;
        m_AnalyzeHandleCallBack = AnalyzerDataCallBack;
    }

    public void Start() {
        NETClient.Init(m_DisConnectCallBack, IntPtr.Zero, null);
        NETClient.SetAutoReconnect(m_ReConnectCallBack, IntPtr.Zero);
        Login();

        m_EventID = NETClient.RealLoadPicture(m_LoginID, 0, (uint)EM_EVENT_IVS_TYPE.ALL, true, m_AnalyzeHandleCallBack, m_LoginID, IntPtr.Zero);
        if (IntPtr.Zero == m_EventID) logger.LogError(NETClient.GetLastError());
    }

    public void Stop() {
        NETClient.Logout(m_LoginID);
        NETClient.Cleanup();
    }

    private int AnalyzerDataCallBack(
        IntPtr lAnalyzerHandle, uint dwEventType, IntPtr pEventInfo, IntPtr pBuffer, uint dwBufSize, IntPtr dwUser, int nSequence, IntPtr reserved
    ) {
        var type = (EM_EVENT_IVS_TYPE)dwEventType;

        if (type == EM_EVENT_IVS_TYPE.CROSSREGIONDETECTION) {
            var info = (NET_DEV_EVENT_CROSSREGION_INFO)Marshal.PtrToStructure(pEventInfo, typeof(NET_DEV_EVENT_CROSSREGION_INFO));
                
            var eventInfo = new EventInfo {
                EventId = info.nEventID,
                ChannelId = info.nChannelID,
                DetectRegion = info.DetectRegion.Take(4).ToArray(),
                Center = info.stuObject.Center,
                BoundingBox = info.stuObject.BoundingBox,
                ObjectType = System.Text.Encoding.Default.GetString(info.stuObject.szObjectType),
                RegisteredAt = info.UTC.ToDateTime().ToUniversalTime(),
            };
                
            logger.LogInformation($"Detect {type} event");

            hub.Clients.All.SendAsync("Receive", eventInfo);
        }

        return 0;
    }
    
    private void DisConnectCallBack(IntPtr lLoginID, IntPtr pchDVRIP, int nDVRPort, IntPtr dwUser) {
        logger.LogInformation("Dahua --- DISCONNECTED");
    }
    
    private void ReConnectCallBack(IntPtr lLoginID, IntPtr pchDVRIP, int nDVRPort, IntPtr dwUser) {
        logger.LogInformation("Dahua --- RECONNECT");
    }
    
    private void Login() {
        m_DeviceInfo = new NET_DEVICEINFO_Ex();
        m_LoginID = NETClient.LoginWithHighLevelSecurity (cfg.Ip, cfg.Port, cfg.Username, cfg.Password, EM_LOGIN_SPAC_CAP_TYPE.TCP, IntPtr.Zero, ref m_DeviceInfo);
        
        if (IntPtr.Zero == m_LoginID) {
            logger.LogError(NETClient.GetLastError());
        }
    }
}