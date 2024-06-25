namespace Dahua.Server.Configuration;

public class DahuaOptions {
    public const string Section = "Dahua";
    
    public string Ip { get; set; }
    public ushort Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}