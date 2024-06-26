namespace Dahua.Server.Model;

public class BoundingBox(int left, int top, int right, int bottom) {
    public int Left { get; set; } = left;
    
    public int Top { get; set; } = top;
    
    public int Right { get; set; } = right;
    
    public int Bottom { get; set; } = bottom;
}