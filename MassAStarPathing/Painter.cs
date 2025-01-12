using System.Numerics;
using Raylib_cs;

namespace MassAStarPathing;

public class Painter
{
    private Grid Grid;
    public int nodeSize;

    public string brush = "Agent";


    public Painter(Grid grid,int size)
    {
        Grid = grid;
        nodeSize = size;
    }

    public void Draw()
    {
        if (Cluster.Clusters.Count == 0)
        {
            DrawNodes(Grid.grid);
        }
        else
        {
            DrawNodes(Cluster.Clusters[0].Grid);
        }
        
        DrawAgents();
        DrawTargets();
        Update();
        
    }

    private void SetBrushToAgent()
    {
        brush = "Agent";
    }
    private void SetBrushToTarget()
    {
        brush = "Target";
    }

    
    



    private void Update()
    {
        if (Raylib.IsKeyPressed(KeyboardKey.Enter))
        {
            foreach (var cluster in Cluster.Clusters)
            {
                cluster.runAStar();
            }
        }

        if (Raylib.IsKeyPressed(KeyboardKey.A))
        {
            SetBrushToAgent();
        }
        if (Raylib.IsKeyPressed(KeyboardKey.T))
        {
            SetBrushToTarget();
        }
        if (Raylib.IsMouseButtonDown(MouseButton.Left))
        {
            RemoveNodes();
        }

        if ( currentClusterPoints.Count == 0) 
        {
            if (Raylib.IsMouseButtonPressed(MouseButton.Right) && brush == "Agent")
            {
                CreateAgents();
            }
            if (Raylib.IsMouseButtonPressed(MouseButton.Right) && brush == "Target")
            {
                CreateTarget();
            }
            
        }

        CreateClusterBounds();
        HandelTargetSetting();
    }

    private void CreateAgents()
    {
        Vector2 mousePosition = Raylib.GetMousePosition();
        Agent newAgent = new Agent(mousePosition, Grid);
    }

    private void CreateTarget()
    {
        Vector2 mousePosition = Raylib.GetMousePosition();
        Target newTarget = new Target(mousePosition.X,mousePosition.Y, Grid,this);
    }

    private Target currentSelectedTarget;
    private void HandelTargetSetting()
    {
        if (Raylib.IsMouseButtonPressed(MouseButton.Left))
        {
            Vector2 mousePosition = Raylib.GetMousePosition();
            if (currentSelectedTarget == null)
            {
                foreach (var target in Target.Targets)
                {
                    if (Vector2.Distance(mousePosition, target.worldPosition) <nodeSize)
                    {
                        currentSelectedTarget = target;
                        break;
                    }
                }
            }
            else
            {
                foreach (var agent in Agent.Agents)
                {
                    if (Vector2.Distance(mousePosition, agent.worldPosition) <nodeSize && agent.CurrentCluster != null)
                    {
                        currentSelectedTarget.Color = agent.Color;
                        currentSelectedTarget.Cluster = agent.CurrentCluster;
                        agent.CurrentCluster.Target = currentSelectedTarget;
                        foreach (var agentInCluster in agent.CurrentCluster.Agents)
                        {
                            agentInCluster.Target = currentSelectedTarget;
                        }
                        currentSelectedTarget = null;
                        break;
                    }
                }
            }

        

        }

    }

    private void DrawTargets()
    {
        foreach (var target in Target.Targets)
        {
            target.DrawTarget();
        }
    }

    private List<Vector2> currentClusterPoints = new List<Vector2>();
    private void CreateClusterBounds()
    {
        if (Raylib.IsMouseButtonPressed(MouseButton.Right))
        {
            currentClusterPoints = new List<Vector2>();
        }
        Vector2 mousePosition = Raylib.GetMousePosition();
        if (Raylib.IsMouseButtonPressed(MouseButton.Middle))
        {
            if (currentClusterPoints.Count < 2 || Vector2.Distance(mousePosition, currentClusterPoints[0]) > 5)
            {
                currentClusterPoints.Add(mousePosition);
            }
            else if(currentClusterPoints.Count >= 2 )
            {
                List<Agent> agentsInBounds = GetAgentsInBounds();
                if (agentsInBounds.Count > 0)
                {
                    Cluster newCluster = new Cluster(agentsInBounds,Grid,nodeSize);
                }
               
                currentClusterPoints = new List<Vector2>();
            }
            
        }

        

        if (currentClusterPoints.Count > 0)
        {
            for (int i = 0; i < currentClusterPoints.Count-1; i++)
            {
                Raylib.DrawLineEx(currentClusterPoints[i],currentClusterPoints[i+1],2,Color.Magenta);
            }
            Raylib.DrawLineEx(currentClusterPoints[currentClusterPoints.Count-1],mousePosition,2,Color.Magenta);
            foreach (var point in currentClusterPoints)
            {
                Raylib.DrawCircle((int)point.X,(int)point.Y,2,Color.Gold);
            
            }
        }

    }

    private List<Agent> GetAgentsInBounds()
    {
        List<Agent> agentsInside = new List<Agent>();
        foreach (var agent in Agent.Agents)
        {
            bool inside = false;
            for (int i = 0; i < currentClusterPoints.Count; i++)
            {
                Vector2 vertex1 = currentClusterPoints[i];
                Vector2 vertex2 = currentClusterPoints[(i+1)%currentClusterPoints.Count];
                if ((vertex1.Y > agent.worldPosition.Y) != (vertex2.Y > agent.worldPosition.Y))
                {
                    float intersectX = vertex1.X + (agent.worldPosition.Y - vertex1.Y) *
                        (vertex2.X - vertex1.X) / (vertex2.Y - vertex1.Y);

                    if (agent.worldPosition.X < intersectX)
                    {
                        inside = !inside;

                    }
                }
            }

            if (inside)
            {
                agentsInside.Add(agent);
            }
        }

        return agentsInside;
    }

    private void RemoveNodes()
    {

            Vector2 mousePosition = Raylib.GetMousePosition();
            foreach (var agent in Agent.Agents)
            {
                if (Vector2.Distance(mousePosition, agent.worldPosition) <= nodeSize)
                {
                    return;
                } 
            }
            foreach (var target in Target.Targets)
            {
                if (Vector2.Distance(mousePosition, target.worldPosition) <= nodeSize)
                {
                    return;
                } 
            }
            IntVector2 mouseClosestIndex = new IntVector2((int)(mousePosition.X / nodeSize), (int)(mousePosition.Y / nodeSize));
            Node closestNode = Grid.grid[mouseClosestIndex.X, mouseClosestIndex.Y];
            foreach (var child in closestNode.Children)
            {
                foreach (var childOchild in child.Child.Children)
                {
                    if (childOchild.Child == closestNode)
                    {
                        child.Child.Children.Remove(childOchild);
                        break;
                    }
                }
                
            }

            closestNode.Children = new List<Connection>();

            closestNode.state = 0;
        
    }

    private void DrawNodes(Node[,] grid)
    {



        for (int x = 0; x < Grid.Width; x++)
        {
            for (int y = 0; y < Grid.Height; y++)
            {
                if (grid[x, y].state == 0)
                {
                    continue;
                }
                Raylib.DrawRectangle(x*nodeSize,y*nodeSize,nodeSize,nodeSize,GetColor(grid[x,y].state));
                Raylib.DrawRectangleLines(x*nodeSize,y*nodeSize,nodeSize,nodeSize,Color.Black);
            }
        }
        for (int x = 0; x < Grid.Width; x++)
        {
            for (int y = 0; y < Grid.Height; y++)
            {
                foreach (var connection in Grid.grid[x,y].Children)
                {
                    Vector2 startPos = new Vector2(x * nodeSize + nodeSize/2, y * nodeSize+nodeSize/2);
                    Vector2 endPos = new Vector2(connection.Child.gridPosition.X * nodeSize + nodeSize/2, connection.Child.gridPosition.Y * nodeSize+nodeSize/2);
                    Raylib.DrawLineEx(startPos,endPos,1f,Color.Black);
                }
            }
        }
    }

    private void DrawAgents()
    {
        foreach (var agent in Agent.Agents)
        {
            Raylib.DrawCircle((int)agent.worldPosition.X,(int)agent.worldPosition.Y,nodeSize/1.5f,agent.Color);
        }
    }
    public static Color GetColor(int input)
    {
        input = Math.Abs(input);
        int red = (input * 3 + 57) % 256;   
        int green = (input * 7 + 42) % 256; 
        int blue = (input * 11 + 129) % 256; 


        return new Color(red, green, blue);
    }

}