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
    public class GameState
    {
        public GridNode[,] Grid;
        public GameState Previous;
        public GameState(GridNode[,] grid, GameState previous) 
        { 
            Grid = (GridNode[,])grid.Clone();
            Previous = previous;
        }

        public List<GridNode[,]> GenerateSuccessors()
        {
            List<GridNode[,]> successors = new List<GridNode[,]>();
            GridNode currentEmptyNode = FindEmptyNode(Grid);
            if(currentEmptyNode.GridX - 1 >= 0)
            {
                successors.Add(CreateSuccessorGrid(currentEmptyNode.GridX - 1, currentEmptyNode.GridY));
            }
            if(currentEmptyNode.GridX + 1 < 3)
            {
                successors.Add(CreateSuccessorGrid(currentEmptyNode.GridX + 1, currentEmptyNode.GridY));
            }
            if(currentEmptyNode.GridY + 1 < Grid.GetLength(0))
            {
                successors.Add(CreateSuccessorGrid(currentEmptyNode.GridX, currentEmptyNode.GridY + 1));
            }
            if(currentEmptyNode.GridY - 1 >= 0)
            {
                successors.Add(CreateSuccessorGrid(currentEmptyNode.GridX, currentEmptyNode.GridY - 1));
            }
            return successors;
        }

        private GridNode[,] CreateSuccessorGrid(int newEmptyNodePositionX, int newEmptyNodePositionY)
        {
            // clone the current grid
            GridNode[,] newGrid = (GridNode[,])Grid.Clone();
            //find empty node
            var emptyNode = FindEmptyNode(newGrid);
            //swap the current empty node with node at (newEmptyNodePositionX, new emptyNodePositionY)
            Swap(emptyNode, newGrid[newEmptyNodePositionX, newEmptyNodePositionY]);
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

        private GridNode FindEmptyNode(GridNode[,] nodes)
        {
            foreach (var node in nodes)
            {
                if (node.Value == "0")
                {
                    return node;
                }
            }
            return null;
        }

    }
}
