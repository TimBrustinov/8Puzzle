using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Xna.Framework;

namespace _8Puzzle
{
    internal class GameState
    {
        public GridNode[,] Grid;
        public GameState Previous;
        public GameState(GridNode[,] grid) 
        { 
            Grid = grid;
        }

        public void GenerateSuccessors(GridNode emptyNode)
        {
            if(emptyNode.GridX - 1 > 0)
            {
                Swap(emptyNode);
            }
        }

        private GridNode[,] CreateNewSuccessorGrid()
        {
            GridNode[,] newGrid = new GridNode[Grid.GetLength(0), Grid.GetLength(1)];
            for (int i = 0; i < newGrid.GetLength(0); i++)
            {
                for (int j = 0; j < newGrid.GetLength(1); j++)
                {
                    GridNode nodeToCopy = Grid[i, j];
                    newGrid[i, j] = new GridNode(nodeToCopy.Value, nodeToCopy.Color, nodeToCopy.Texture, nodeToCopy.GridPosition, nodeToCopy.Rect.Location.ToVector2(), new Point(nodeToCopy.Rect.Width, nodeToCopy.Rect.Height), nodeToCopy.Font);
                    if (newGrid[i, j].Value == "0")
                    {
                        emptyNode = ;
                    }
                    colorIndex++;
                }
            }
        }
        private void Swap(GridNode a, GridNode b)
        {
            var tempVal = a.Value;
            var tempColor = a.Color;

            a.Value = b.Value;
            a.Color = b.Color;
            b.Value = tempVal;
            b.Color = tempColor;
        }
    }
}
