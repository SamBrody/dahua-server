namespace Dahua.Server.Model;

public class EventInfo {
    public string DeviceSerialNubmer { get; set; }
    
    public int EventId { get; set; }
    
    public int ChannelId { get; set; }
    
    public ICollection<Point> DetectRegion { get; set; }
    
    public Point Center { get; set; }
    
    public BoundingBox BoundingBox { get; set; }
    
    public string ObjectType { get; set; }
    
    public DateTimeOffset RegisteredAt { get; set; }
}