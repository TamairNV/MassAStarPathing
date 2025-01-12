using System.Numerics;
using Raylib_cs;

namespace MassAStarPathing;

public class Target
{
    public static List<Target> Targets = new List<Target>();
    public IntVector2 gridPosition;
    public Vector2 worldPosition;
    public Cluster Cluster;
    private Grid Grid;
    private Painter Painter;
    public Color Color = Color.Black;


    public Target(float x , float y,Grid grid,Painter painter)
    {
        Targets.Add(this);
        Painter = painter;
        Grid = grid;
        worldPosition = new Vector2(x, y);
        gridPosition = new IntVector2((int)(x/painter.nodeSize), (int)(y/painter.nodeSize));
    }



    public void DrawTarget()
    {
        Raylib.DrawTextEx(Raylib.GetFontDefault(), "T", worldPosition, 10, 1, Color.White);
        Raylib.DrawRectangle((int)worldPosition.X,(int)worldPosition.Y,(int)(Painter.nodeSize/1.5f),(int)(Painter.nodeSize/1.5f),Color);
    }
}