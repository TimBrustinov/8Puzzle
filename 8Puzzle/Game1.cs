using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace _8Puzzle
{
    public class Game1 : Game
    {
        private MouseState previousMouseState = default;
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Texture2D pixel;
        private Random random;

        private GridNode[,] gridNodes = new GridNode[3, 3];

        private Button RandomizeButton;
        private Button SolveButton;

        private Solver Solver;


        private List<GridNode[,]> Path;
        private int pathIndex = 0;

        private Point cellSize = new Point(100, 100);
        private SpriteFont font;
        private int spacing = 1;

        private double elapsedTime = 0;
        private const double UpdateInterval = 1.0; // Update every second

        private Dictionary<int, Color> colors = new Dictionary<int, Color>()
        {
            [0] = Color.Gray,
            [1] = Color.Red,
            [2] = Color.Green,
            [3] = Color.Blue,
            [4] = Color.Orange,
            [5] = Color.Orchid,
            [6] = Color.Cyan,
            [7] = Color.PaleGreen,
            [8] = Color.HotPink
        };

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            random = new Random();  
            spriteBatch = new SpriteBatch(GraphicsDevice);
            pixel = new Texture2D(graphics.GraphicsDevice, 1, 1);
            pixel.SetData(new Color[] { Color.White });
            font = Content.Load<SpriteFont>("Font");

            RandomizeButton = new Button(new Point(400, 0), pixel, font, "Randomize", Color.White, Color.Black);
            SolveButton = new Button(new Point(400, 60), pixel, font, "Solve", Color.White, Color.Black);
            Solver = new Solver();


            int[,] intialBoard = new int[3, 3]
            {
                { 1, 4, 7 },
                { 2, 5, 8 },
                { 3, 6, 0 }
            };

            for (int i = 0; i < gridNodes.GetLength(0); i++)
            {
                for (int j = 0; j < gridNodes.GetLength(1); j++)
                {
                    gridNodes[i, j] = new GridNode(intialBoard[i, j], colors[intialBoard[i, j]], pixel, new Point(i, j), new Vector2(i * (cellSize.X + spacing), j * (cellSize.Y + spacing)), cellSize, font);
                }
            }

            Path = new();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;
            MouseState ms = Mouse.GetState();
            if(ms.LeftButton == ButtonState.Pressed && previousMouseState != ms)
            {
                if(RandomizeButton.Rectangle.Contains(ms.Position))
                {
                    CreateBoard();
                }
                else if(SolveButton.Rectangle.Contains(ms.Position))
                {
                    Solve();
                }
            }

            if (elapsedTime >= UpdateInterval && pathIndex < Path.Count)
            {
                gridNodes = Path[pathIndex];
                pathIndex++;
                elapsedTime = 0;
            }

            previousMouseState = Mouse.GetState();
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            RandomizeButton.Draw(spriteBatch);
            SolveButton.Draw(spriteBatch);

            foreach (GridNode node in gridNodes)
            {
                node.Draw(spriteBatch);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void CreateBoard()
        {
            Random random = new Random();
            int[,] nodeValues;
            do
            {
                nodeValues = GenerateRandomBoard(random);
            } while (!IsSolvable(nodeValues));

            for (int i = 0; i < gridNodes.GetLength(0); i++)
            {
                for (int j = 0; j < gridNodes.GetLength(1); j++)
                {
                    gridNodes[i, j] = new GridNode(nodeValues[i, j], colors[nodeValues[i, j]], pixel, new Point(i, j), new Vector2(i * (cellSize.X + spacing), j * (cellSize.Y + spacing)), cellSize, font);
                }
            }
        }

        private int[,] GenerateRandomBoard(Random random)
        {
            List<int> values = new List<int>();
            for (int i = 0; i < 9; i++)
            {
                values.Add(i);
            }

            int[,] board = new int[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    int randomIndex = random.Next(values.Count);
                    board[i, j] = values[randomIndex];
                    values.RemoveAt(randomIndex);
                }
            }
            return board;
        }

        int getInvCount(int[] arr)
        {
            int inv_count = 0;
            for (int i = 0; i < 9; i++)
                for (int j = i + 1; j < 9; j++)
                    if (arr[i] > 0 && arr[j] > 0 && arr[i] > arr[j])
                        inv_count++;
            return inv_count;
        }

        // This function returns true
        // if given 8 puzzle is solvable.
        bool IsSolvable(int[,] puzzle)
        {
            int[] linearForm;
            linearForm = new int[9];
            int k = 0;

            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    linearForm[k++] = puzzle[i, j];

            // Count inversions in given 8 puzzle
            int invCount = getInvCount(linearForm);
            // return true if inversion count is even.
            return (invCount % 2 == 0);
        }

        private void Solve()
        {
            GameState gameState = new GameState(gridNodes, null);
            var solverOutput = Solver.Solve(gameState);
            Path = solverOutput.path;
        }

    }
}
