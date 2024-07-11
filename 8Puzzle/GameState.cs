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

        public List<GridNode[,]> GenerateSuccessors(GridNode emptyNode)
        {
            List<GridNode[,]> successors = new List<GridNode[,]>();
            if(emptyNode.GridX - 1 >= 0)
            {
                successors.Add(CreateNewSuccessorGrid(emptyNode.GridX - 1, emptyNode.GridY));
            }
            if(emptyNode.GridX + 1 < 3)
            {
                successors.Add(CreateNewSuccessorGrid(emptyNode.GridX + 1, emptyNode.GridY));
            }
            if(emptyNode.GridY + 1 < Grid.GetLength(0))
            {
                successors.Add(CreateNewSuccessorGrid(emptyNode.GridX, emptyNode.GridY + 1));
            }
            if(emptyNode.GridY - 1 >= 0)
            {
                successors.Add(CreateNewSuccessorGrid(emptyNode.GridX, emptyNode.GridY - 1));
            }
            return successors;
        }

        private GridNode[,] CreateNewSuccessorGrid(int newEmptyNodePositionX, int newEmptyNodePositionY)
        {
            GridNode[,] newGrid = new GridNode[Grid.GetLength(0), Grid.GetLength(1)];

            //create new grid with duplicate values of the current grid
            for (int i = 0; i < newGrid.GetLength(0); i++)
            {
                for (int j = 0; j < newGrid.GetLength(1); j++)
                {
                    GridNode nodeToCopy = Grid[i, j];
                    newGrid[i, j] = new GridNode(nodeToCopy);
                }
            }

            //find the empty node
            foreach (var node in newGrid)
            {
                //node will be the empty node
                if(node.Value == "0")
                {
                    Swap(node, newGrid[newEmptyNodePositionX, newEmptyNodePositionY]);
                }
            }

            return newGrid;
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
