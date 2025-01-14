using System.Numerics;

namespace MassAStarPathing;

public class Node
{
    public IntVector2 gridPosition;
    public List<Connection> Children = new List<Connection>();
    public Vector2 worldPos;
    public int state = 1;
    public Node aStarParent = null;

    public int PathID;

    private float fCost;
    private float hCost;
    private float gCost;
    public float Discomfort = 0;
    private float speedMultiplier = 1;

    public Node(int x , int y)
    {
      
        gridPosition = new IntVector2(x, y);
        ResetNode(0,0);
    }
    
    public void ResetNode(int pathID, int state)
    {
        fCost = 0;
        hCost = 0;
        gCost = 0;
        PathID = pathID;
        this.state = state;
        aStarParent = null;
        Discomfort = 0;
    }

    private float AStarHCostCal(Vector2 endnode)
    {
        return Vector2.Distance(new Vector2(gridPosition.X, gridPosition.Y), endnode);
    }


    public bool ExpandParent(int ID, List<Node> agentNodes, ref PriorityQueue<Node,float> priorityQueue,ref Grid baseGraph,Node[,] graph)
    {

        if (PathID != ID)
        {
            ResetNode(ID,2);
        }
        
        state = 2;
        baseGraph.grid[gridPosition.X,gridPosition.Y].PathID = PathID;

        
        foreach (var child in baseGraph.grid[gridPosition.X,gridPosition.Y].Children)
        {

            Node currentNode = graph[child.Child.gridPosition.X,child.Child.gridPosition.Y];

            if (child.Child.PathID != ID )
            {
                child.Child.PathID = ID;
                currentNode.ResetNode(ID,baseGraph.grid[gridPosition.X,gridPosition.Y].state);
            }

            if (agentNodes.Contains(currentNode))
            {
                currentNode.Discomfort += 10;
            }
            if (currentNode.state == 0)
            {
                currentNode.state = 1;
                currentNode.hCost = AStarHCostCal(new Vector2(agentNodes[0].gridPosition.X,agentNodes[0].gridPosition.Y));
                currentNode.gCost = gCost + child.Distance + (currentNode.Discomfort*100);
                currentNode.fCost = currentNode.hCost*1.15f + currentNode.gCost ;
                priorityQueue.Enqueue(currentNode,currentNode.fCost);
                currentNode.aStarParent = this;
            }
            
            else if (currentNode.state == 1 && gCost + child.Distance  < currentNode.gCost +(currentNode.Discomfort*100))
            {
                currentNode.gCost = gCost + child.Distance +(currentNode.Discomfort*100);
                currentNode.fCost = currentNode.hCost + currentNode.gCost;
                currentNode.aStarParent = this;
            }
        }
        
        return false;
    }
}


public class Connection
{
    public Node Child;
    public float Distance;

    public Connection(Node child , float distance)
    {
        Child = child;
        Distance = distance;
    }
}

public struct IntVector2
    {

        public int X;
        public int Y;
        public IntVector2(int x,int y)
        {
            X = x;
            Y = y;
        }
    }