namespace UI;

using System.Numerics;
using Raylib_cs;
public class Box
{
    public Vector2 Position;
    public Vector2 Bounds;
    public Color Colour;
    private bool fixedSize;
    private int outlineThickness;
    private bool outline;
    public Box()
    {
        
    }
    public Box(Vector2 position, Vector2 size, Color colour,bool fixedSize = false,bool outline = false,int outlineThickness = 3)
    {
        this.outline = outline;
        Position = position;
        Bounds = size;
        this.fixedSize = fixedSize;
        Colour = colour;
        this.outlineThickness = outlineThickness;
    }

    public virtual void Draw(bool square = false)
    {
    
        
        Raylib.DrawRectangle((int)Position.X,(int)Position.Y,(int)Bounds.X,(int)Bounds.Y,Colour);
        if (outline)
        {
            Vector2 offset = Vector2.Zero;
            if (outlineThickness < 0)
            {
                offset = new Vector2(outlineThickness, outlineThickness);
            }
            Raylib.DrawRectangleLinesEx(new Rectangle(Position.X+offset.X,Position.Y+offset.Y,(int)Bounds.X,(int)Bounds.Y),Math.Abs(outlineThickness),Color.Blue);
        }

    }
}