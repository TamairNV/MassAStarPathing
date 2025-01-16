using System.Drawing;
using System.Numerics;
using Raylib_cs;
using Color = Raylib_cs.Color;

namespace MassAStarPathing;

public class Agent
{
    public static List<Agent> Agents = new List<Agent>();
    public Vector2 worldPosition;
    public IntVector2 closestGridPosition;
    public Grid Grid;
    public Raylib_cs.Color Color = Color.Red;
    public Target Target;
    public Cluster CurrentCluster;
    public List<Node> Path;
    public float speed = 20;
    public int PathI = 2;

    private Node currentNode;
    private Node lastNode;

    public Agent(Vector2 worldPosition,Grid grid)
    {
        Agents.Add(this);
        this.worldPosition = worldPosition;
        Grid = grid;
    }

    private void moveThroughPath(float deltaTime)
    {
            /*
            Vector2 dir = Path[0].worldPos;
            if (Path.Count < 2)
            {
                 dir = Normalize(worldPosition - Target.worldPosition);
            }
            else if(PathI < Path.Count)
            {
                if (Vector2.Distance(worldPosition, Path[^PathI].worldPos) < CurrentCluster.nodeSize/2)
                {
                    PathI++;
                }
                dir = Normalize(worldPosition - Path[^PathI].worldPos);

            }
            else
            {
                return;
            }
            worldPosition += (-dir * speed * deltaTime);
            */

            currentNode = Grid.grid[(int)(worldPosition.X/CurrentCluster.nodeSize),(int)(worldPosition.Y/CurrentCluster.nodeSize)];
            worldPosition = new Vector2(currentNode.aStarParent.gridPosition.X,currentNode.aStarParent.gridPosition.Y) *
                            new Vector2(CurrentCluster.nodeSize, CurrentCluster.nodeSize);
            




    }

    private void update(float deltaTime)
    {
        if (Path != null)
        {

            moveThroughPath(deltaTime);
            
        }


        
    }

    public static void updateAgents(float deltaTime)
    {
        foreach (var agent in Agents)
        {
            agent.update(deltaTime);
        }
    }
    public static Vector2 Normalize(Vector2 vector)
    {
        float length = vector.Length();
        
        // Prevent division by zero
        if (length > 0)
        {
            return vector / length;
        }
        
        return Vector2.Zero;
    }
}



