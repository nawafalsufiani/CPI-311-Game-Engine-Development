using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

using Microsoft.Xna.Framework.Input;


namespace CPI311.GameEngine
{
    public class Alert
    {
        private SpriteFont font;
        private string message;
        private Rectangle boxRect;
        private string buttonText;
        private bool isVisible;
        private Action onButtonClick;

        public Alert(SpriteFont font)
        {
            this.font = font;
            boxRect = new Rectangle(200, 150, 400, 200);
            buttonText = "OK";
            isVisible = false;
        }

        public void Show(string message, Action onButtonClick)
        {
            this.message = message;
            this.onButtonClick = onButtonClick;
            isVisible = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!isVisible) return;

            // Draw semi-transparent background
            spriteBatch.Begin();
            spriteBatch.Draw(CreateTexture(spriteBatch.GraphicsDevice, boxRect.Width, boxRect.Height, Color.Gray * 0.8f), boxRect, Color.White);

            // Draw the message text
            Vector2 textSize = font.MeasureString(message);
            spriteBatch.DrawString(font, message, new Vector2(boxRect.X + (boxRect.Width - textSize.X) / 2, boxRect.Y + 20), Color.White);

            // Draw the button
            var buttonRect = new Rectangle(boxRect.X + (boxRect.Width - 100) / 2, boxRect.Y + boxRect.Height - 60, 100, 40);
            spriteBatch.Draw(CreateTexture(spriteBatch.GraphicsDevice, 100, 40, Color.Blue), buttonRect, Color.White);
            spriteBatch.DrawString(font, buttonText, new Vector2(buttonRect.X + (buttonRect.Width - font.MeasureString(buttonText).X) / 2, buttonRect.Y + 10), Color.White);

            spriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {
            if (!isVisible) return;

            var mouseState = Mouse.GetState();

            // Handle button click
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                var buttonRect = new Rectangle(boxRect.X + (boxRect.Width - 100) / 2, boxRect.Y + boxRect.Height - 60, 100, 40);
                if (buttonRect.Contains(mouseState.X, mouseState.Y))
                {
                    isVisible = false; // Close the message box
                    onButtonClick?.Invoke();
                }
            }
        }

        // Helper method to create a texture for drawing the box or button
        private Texture2D CreateTexture(GraphicsDevice graphicsDevice, int width, int height, Color color)
        {
            Texture2D texture = new Texture2D(graphicsDevice, width, height);
            Color[] data = new Color[width * height];
            for (int i = 0; i < data.Length; ++i) data[i] = color;
            texture.SetData(data);
            return texture;
        }
    }
}
