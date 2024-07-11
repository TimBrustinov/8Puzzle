using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8Puzzle
{
    public class GridNode
    {
        public string Value { get; set; }
        public Point GridPosition { get; set; }
        public int GridX => GridPosition.X;
        public int GridY => GridPosition.Y;

        public Color Color { get; set; }
        public Texture2D Texture { get; set; }
        public Rectangle Rect { get; set; }
        public SpriteFont Font { get; set; }

        public GridNode(string value, Color color, Texture2D texture, Point gridPosition, Vector2 position, Point size, SpriteFont font)
        {
            Value = value;
            Color = color;
            Texture = texture;
            GridPosition = gridPosition;
            Rect = new Rectangle(position.ToPoint(), size);
            Font = font;
        }

        public GridNode(GridNode node)
        {
            Value = node.Value;
            Color = node.Color;
            Texture = node.Texture;
            GridPosition = node.GridPosition;
            Font = node.Font;
            Rect = node.Rect;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Rect, Color);

            if(Value != "0")
            {
                spriteBatch.DrawString(Font, Value, new Vector2(Rect.X, Rect.Y), Color.White);
            }
        }
    }
}
