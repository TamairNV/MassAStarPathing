using System.Numerics;
using Raylib_cs;

namespace MassAStarPathing;

public class Cluster
{


    private static Random random = new Random();
    public List<Agent> Agents = new List<Agent>();
    private List<Node> AgentNodes = new List<Node>();
    public static List<Cluster> Clusters = new List<Cluster>();
    public Color ClusterColour;
    public Grid BaseGrid;
    public Target Target;
    public int nodeSize;
  

    public Node[,] Grid;


    public Cluster(List<Agent> agents,Grid baseGrid,int nodeSize)
    {
        this.nodeSize = nodeSize;
        BaseGrid = baseGrid;
        Agents = agents;
        ClusterColour = GetRandomColor();
        foreach (var agent in agents)
        {
            agent.Color = ClusterColour;
            agent.CurrentCluster = this;
        }
        Clusters.Add(this);
        Grid = BaseGrid.CreateGrid();
    }

    public void runAStar()
    {
        PriorityQueue<Node, float> priorityQueue = new PriorityQueue<Node, float>();
        Node startNode = Grid[(int)(Target.worldPosition.X / nodeSize), (int)(Target.worldPosition.Y / nodeSize)];
        priorityQueue.Enqueue(startNode,0);
        foreach (var agent in Agents)
        {
            AgentNodes.Add(Grid[(int)(agent.worldPosition.X / nodeSize), (int)(agent.worldPosition.Y / nodeSize)]);
        }
        while (!IsDone() && priorityQueue.Count > 0)
        {
            Node currentNode = priorityQueue.Dequeue();
            currentNode.ExpandParent(MassAStarPathing.Grid.PathID,AgentNodes, ref priorityQueue, ref BaseGrid, Grid);
            if (AgentNodes.Contains(currentNode))
            {
                AgentNodes.Remove(currentNode);
            }
        }

        MassAStarPathing.Grid.PathID++;
        getPaths();

    }

    private void getPaths()
    {
        foreach (var agent in Agents)
        {
            getPath(agent);
            agent.PathI = 2;
        }
    }
    private void getPath(Agent agent)
    {
        agent.Path = new List<Vector2>();
        Node currentNode = Grid[(int)(agent.worldPosition.X / nodeSize), (int)(agent.worldPosition.Y / nodeSize)];
        while (currentNode != null)
        {
            
            currentNode.state = 7;
            agent.Path.Add( Vector2.Multiply(new Vector2(currentNode.gridPosition.X,currentNode.gridPosition.Y),nodeSize));
            
            currentNode = currentNode.aStarParent;
        }

        agent.Path.Reverse();
    }
    public Vector2 getAveragePosition()
    {
        Vector2 avgPosition = new Vector2(0, 0);
        foreach (var agent in Agents)
        {
            avgPosition += agent.worldPosition;
        }

        return new Vector2(avgPosition.X / Agents.Count, avgPosition.Y / Agents.Count);

    }

    private bool IsDone()
    {
        bool done = AgentNodes.Count == 0;
        
        if (Grid[(int)(Target.worldPosition.X / nodeSize), (int)(Target.worldPosition.Y / nodeSize)].state != 2)
        {
            done = false;
        }

        return done;
    }

    public void AddAgent(Agent agent)
    {
        Agents.Add(agent);
        agent.Color = ClusterColour;
        agent.CurrentCluster = this;

    }
    
    public static Color GetRandomColor()
    {
        // Generate random values for R, G, and B (0-255)
        int r = random.Next(256); // 0 to 255
        int g = random.Next(256);
        int b = random.Next(256);

        // Create and return a Color object
        return new Color(r, g, b);
    }
}