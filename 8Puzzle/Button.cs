using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8Puzzle
{
    public class Button
    {
        public Rectangle Rectangle { get; }
        Texture2D Texture { get; set; }
        SpriteFont Font { get; set; }
        string ButtonText { get; set; }
        Color ButtonColor { get; set; }
        Color TextColor { get; set; }
        public Button(Point position, Texture2D texture, SpriteFont font, string buttonText, Color buttonColor, Color textColor) 
        { 
            Texture = texture;
            Font = font;
            ButtonText = buttonText;
            ButtonColor = buttonColor;
            TextColor = textColor;
            
            Vector2 textSize = Font.MeasureString(ButtonText);
            Rectangle = new Rectangle(position, new Point((int)textSize.X, 50));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Rectangle, ButtonColor);
            spriteBatch.DrawString(Font, ButtonText, Rectangle.Location.ToVector2(), TextColor);
        }
    }
}
