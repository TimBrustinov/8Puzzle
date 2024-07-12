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
        public double CumalativeDistance = 0;
        public double FinalDistance = 0;
        public GameState(GridNode[,] grid, GameState previous) 
        { 
            Grid = (GridNode[,])grid.Clone();
            Previous = previous;
        }

        public List<GameState> GenerateSuccessors()
        {
            List<GridNode[,]> successorGrids = new List<GridNode[,]>();
            List<GameState> successors = new List<GameState>();

            GridNode currentEmptyNode = FindEmptyNode(Grid);
            if(currentEmptyNode.GridX - 1 >= 0)
            {
                successorGrids.Add(CreateSuccessorGrid(currentEmptyNode.GridX - 1, currentEmptyNode.GridY));
            }
            if(currentEmptyNode.GridX + 1 < 3)
            {
                successorGrids.Add(CreateSuccessorGrid(currentEmptyNode.GridX + 1, currentEmptyNode.GridY));
            }
            if(currentEmptyNode.GridY + 1 < Grid.GetLength(0))
            {
                successorGrids.Add(CreateSuccessorGrid(currentEmptyNode.GridX, currentEmptyNode.GridY + 1));
            }
            if(currentEmptyNode.GridY - 1 >= 0)
            {
                successorGrids.Add(CreateSuccessorGrid(currentEmptyNode.GridX, currentEmptyNode.GridY - 1));
            }

            foreach (var item in successorGrids)
            {
                successors.Add(new GameState(item, this));
            }

            return successors;
        }

        private GridNode[,] CreateSuccessorGrid(int newEmptyNodePositionX, int newEmptyNodePositionY)
        {
            // clone the current grid
            GridNode[,] newGrid = new GridNode[Grid.GetLength(0), Grid.GetLength(1)];

            for (int i = 0; i < Grid.GetLength(0); i++)
            {
                for (int j = 0; j < Grid.GetLength(1); j++)
                {
                    newGrid[i, j] = new GridNode(Grid[i, j]);
                }
            }
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
                if (node.Value == 0)
                {
                    return node;
                }
            }
            return null;
        }

        public string GetUniqueGameValue()
        {
            string value = "";
            for (int i = 0; i < Grid.GetLength(0); i++)
            {
                for(int j = 0; j < Grid.GetLength(1); j++)
                {
                    value += Grid[i, j].Value.ToString();
                }
            }
            return value;
        }

        public void Print()
        {
            foreach (GridNode node in Grid)
            {
                Console.WriteLine(node.GridPosition.ToString() + " has value: " + node.Value);
            }
        }

    }
}
