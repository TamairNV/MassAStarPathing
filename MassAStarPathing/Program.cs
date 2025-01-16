





using System.Numerics;
using MassAStarPathing;
using UI;
using Raylib_cs;


class Program
{
    public static void Main()
    {
        Raylib.InitWindow(1300, 700, "Crown Simulation");
        Raylib.SetTargetFPS(300);
        Text.InitFonts();


        Grid grid = new Grid(85, 45);
        Painter painter = new Painter(grid, 15);

        float targetInter = 1 / 00.5f;
        float timer = 0;
        
        while (!Raylib.WindowShouldClose())
        {
            float deltaTime = Raylib.GetFrameTime();
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.RayWhite);
            painter.Draw();
            

            timer += deltaTime;
            if (timer >= targetInter)
            {
                foreach (var cluster in Cluster.Clusters)
                {
                    if (cluster.Target != null)
                    {
                        cluster.runAStar();
                    }
                    
                }
                Agent.updateAgents(deltaTime);

                timer = 0;
            }
            
            Raylib.EndDrawing();
        }
        
        Raylib.CloseWindow();
        
    }
}