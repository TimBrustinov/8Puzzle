using Microsoft.Xna.Framework.Graphics;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace _8Puzzle
{
    public class Solver
    {
        GridNode[,] CurrentGrid;
        int[,] GoalStateValues = new int[3, 3]
        {
                { 1, 4, 7 }, 
                { 2, 5, 8 }, 
                { 3, 6, 0 }
        };

        public Solver(GridNode[,] currentGrid)
        {
            CurrentGrid = currentGrid;
        }

        public (bool isSolved, List<GridNode[,]> path) Solve(GameState initialState)
        {
            var frontier = new PriorityQueue<GameState, double>();
            var visited = new List<GameState>();
            frontier.Enqueue(initialState, 0);
            while(frontier.Count > 0)
            {
                var curr = frontier.Dequeue();
                CurrentGrid = curr.Grid;
                visited.Add(curr);
                Console.WriteLine("Current Grid");
                curr.Print();
                foreach (var successor in curr.GenerateSuccessors())
                {
                    Console.WriteLine("Successor Grid");
                    successor.Print();
                    successor.CumalativeDistance = curr.CumalativeDistance + 1;
                    double tentativeDistance = Heuristic(successor.Grid);
                    double finalDistance = successor.CumalativeDistance + tentativeDistance;
                    successor.FinalDistance = finalDistance;

                    if(!VisitedContains(visited, successor))
                    {
                        frontier.Enqueue(successor, successor.FinalDistance);
                    }
                }

                if(IsSolved(curr))
                {
                    List<GridNode[,]> path = new List<GridNode[,]>();
                    var temp = curr;
                    while (temp != null)
                    {
                        path.Add(temp.Grid);
                        temp = temp.Previous;
                    }
                    path.Reverse(); 
                    return (true, path);
                }
            }
            return (false, null);
        }

        private bool IsSolved(GameState state)
        {
            for (int i = 0; i < state.Grid.GetLength(0); i++)
            {
                for (int j = 0; j < state.Grid.GetLength(1); j++)
                {
                    if (state.Grid[i, j].Value != GoalStateValues[i, j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public double Heuristic(GridNode[,]a)
        {
            double cost = 0;
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for(int j = 0; j < a.GetLength(1); j++)
                {
                    (int goalX, int goalY) = FindGoalIndex(a[i, j].Value);

                    double dx = Math.Abs(a[i, j].GridX - goalX);
                    double dy = Math.Abs(a[i, j].GridY - goalY);
                    cost += dx + dy;
                }
            }
            return cost;
        }

        

        private (int x, int y) FindGoalIndex(int value)
        {
            for(int i = 0; i < GoalStateValues.GetLength(0); i++)
            {
                for(int j = 0; j < GoalStateValues.GetLength(1); j++)
                {
                    if (GoalStateValues[i, j] == value)
                    {
                        return (i, j);
                    }
                }
            }
            return (0, 0);
        }

        private bool VisitedContains(List<GameState> games, GameState gameToFind) 
        {
            foreach (var game in games)
            {
                if(game.GetUniqueGameValue() == gameToFind.GetUniqueGameValue())
                {
                    return true;
                }
            }
            return false;
        }
    }
}
