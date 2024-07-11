using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D9;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using WarmupProgram;
namespace _8Puzzle
{
    public class Game1 : Game
    {
        private MouseState previousMouseState = default;
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private GridNode[,] gridNodes = new GridNode[3, 3];
        private Graph Graph;
        private GridNode emptyNode;
        private GridNode selectedNode;

        private Point cellSize = new Point(100, 100);
        private SpriteFont font;
        private int spacing = 1;

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
            int[,] nodeValues = new int[3, 3]
            {
                { 8, 2, 4 },
                { 3, 5, 6 },
                { 7, 1, 0 }
            };
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Texture2D pixel = new Texture2D(graphics.GraphicsDevice, 1, 1);
            pixel.SetData(new Color[] { Color.White });

            font = Content.Load<SpriteFont>("Font");
            int colorIndex = 0;
            for (int i = 0; i < gridNodes.GetLength(0); i++)
            {
                for (int j = 0; j < gridNodes.GetLength(1); j++)
                {
                    gridNodes[i, j] = new GridNode(nodeValues[i, j].ToString(), colors[nodeValues[i, j]], pixel, new Point(i, j), new Vector2(i * (cellSize.X + spacing), j * (cellSize.Y + spacing)), cellSize, font);
                    if (nodeValues[i, j] == 0)
                    {
                        emptyNode = gridNodes[i, j];
                    }
                    colorIndex++;
                }
            }

            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            MouseState ms = Mouse.GetState();
            foreach (var node in gridNodes)
            {
                if (ms.LeftButton == ButtonState.Pressed && ms != previousMouseState && node.Rect.Contains(ms.Position))
                {
                    //check if you can swap the selectedNode with the empty node
                    if (selectedNode != null && node == emptyNode)
                    {
                        if (CheckMove(selectedNode))
                        {
                            Swap(selectedNode, emptyNode);
                            emptyNode = selectedNode;
                            selectedNode = null;
                            Console.WriteLine("New empty node position: " + emptyNode.GridPosition.ToString());
                            break;
                        }
                    }

                    //check if the new selectedNode is not empty node
                    if (node != emptyNode)
                    {
                        selectedNode = node;
                        Console.WriteLine("Selected Node: " + selectedNode.Value);
                    }
                }
            }
            previousMouseState = ms;
            // TODO: Add your update logic here

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            foreach (GridNode node in gridNodes)
            {
                node.Draw(spriteBatch);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}