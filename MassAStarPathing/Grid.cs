namespace MassAStarPathing;

public class Grid
{
    public int Width;
    public int Height;
    public Node[,] grid;
    public static int PathID = 0;

    public Grid(int width, int height)
    {
        Width = width;
        Height = height;
        grid = CreateGrid();
    }

    public Node[,] CreateGrid()
    {
        Node[,] newGrid = new Node[Width, Height];
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                newGrid[x, y] = new Node(x, y);
            }
        }
        
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Node currentNode = newGrid[x, y];
                for (int xIndex = x - 1; xIndex <= x + 1; xIndex++)
                {
                    for (int yIndex = y - 1; yIndex <= y + 1; yIndex++)
                    {
                        if (yIndex >= 0 && yIndex < Height && xIndex >= 0 && xIndex < Width)
                        {
                            float distance = 1.4142f;
                            if (xIndex == x || yIndex == y)
                            {
                                distance = 1f;
                            }

                            if (newGrid[xIndex, yIndex] != currentNode)
                            {
                                currentNode.Children.Add(new Connection(newGrid[xIndex, yIndex], distance));
                            }
                        }


                    }
                }
            }

        }

        return newGrid;

    }
}
    
    
