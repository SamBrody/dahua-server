using NetSDKCS;

namespace Dahua.Server.Model;

public class EventInfo {
    public int EventId { get; set; }
    
    public int ChannelId { get; set; }
    
    public NET_POINT[] DetectRegion { get; set; }
    
    public NET_POINT Center { get; set; }
    
    public NET_A_RECT_EX BoundingBox { get; set; }
    
    public string ObjectType { get; set; }
    
    public DateTimeOffset RegisteredAt { get; set; }
}