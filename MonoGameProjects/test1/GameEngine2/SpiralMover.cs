

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
namespace CPI311.GameEngine
{
    public class SpiralMover
    {
        public Sprite Sprite { get; set; }
        public Vector2 Position { get; set; }
        public float Radius { get; set; }
        public float Speed { get; set; }
        public float Frequency { get; set; }
        public float Amplitute { get; set; }
        public float Phase { get; set; }
        public SpiralMover(Texture2D texture, Vector2 position,
        float radius = 150, float speed = 0.01f, float frequency = 20,
        float amplitude = 10, float phase = 0)
        {
            Sprite = new Sprite(texture);
            Position = position;
            Radius = radius;
            Speed = speed;
            Frequency = frequency;
            Amplitute = amplitude;
            Phase = phase;
        }
        public void Update()
        {
            // Key Controls here ** (Option)
            if (InputManager.IsKeyDown(Keys.Left))
                Amplitute -= Time.ElapsedGameTime * 10;
            if (InputManager.IsKeyDown(Keys.Right))
                Amplitute += Time.ElapsedGameTime * 10;
            if (InputManager.IsKeyDown(Keys.Up))
                Radius += Time.ElapsedGameTime * 10;
            if (InputManager.IsKeyDown(Keys.Down))
                Radius -= Time.ElapsedGameTime * 10;
            // ************************
            Phase += Time.ElapsedGameTime; // Phase (t) is increased
            Sprite.Position = Position + new Vector2(
            // (float)(Radius * Math.Cos(Phase)),
            // (float)(Radius * Math.Sin(Phase)));
            (float)((Radius + Amplitute * Math.Cos(Phase * Frequency)) *
            Math.Cos(Phase)),
            (float)((Radius + Amplitute * Math.Cos(Phase * Frequency)) *
            Math.Sin(Phase)));
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch);
        }
    }
}
