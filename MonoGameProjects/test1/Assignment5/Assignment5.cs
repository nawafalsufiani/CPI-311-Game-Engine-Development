#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CPI311.GameEngine;
using System.Collections.Generic;
#endregion

namespace CPI311.Labs
{
    public class Assignment5 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private int x, y, z;
        private int xLight, yLight, zLight;
        // *** Lab 10
        TerrainRenderer terrain;
        Camera camera;
        Light light, light2, light3, light4;
        Effect effect;

        Player player;
        Agent agent1, agent2, agent3;
        Prize prize1, prize2, prize3;
        Bomb bomb;
        public static int caughtAliens = 0; // Tracks how many aliens the player caught
        public static float timeSpent = 0f;  // Tracks the time spent in the game
        public static float timeBomb = 0f;

        // Variables for tracking caught aliens and time spent

        private SpriteFont font; // Font for displaying text

        public Assignment5()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            //**********************************************************
            _graphics.GraphicsProfile = GraphicsProfile.HiDef; // ShadeDX -> Error
            //**********************************************************

            x = 0; y = 0; z = 0;
        }

        protected override void Initialize()
        {
            Time.Initialize();
            InputManager.Initialize();
            ScreenManager.Initialize(_graphics);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            terrain = new TerrainRenderer(
                Content.Load<Texture2D>("mazeH2"), Vector2.One * 100, Vector2.One * 200);
            terrain.NormalMap = Content.Load<Texture2D>("mazeN2");
            terrain.Transform = new Transform();
            terrain.Transform.LocalScale *= new Vector3(1, 10, 1);
            effect = Content.Load<Effect>("TerrainShader");
            effect.Parameters["AmbientColor"].SetValue(new Vector3(0.1f, 0.1f, 0.1f));
            effect.Parameters["DiffuseColor"].SetValue(new Vector3(0.3f, 0.3f, 0.3f));
            effect.Parameters["SpecularColor"].SetValue(new Vector3(0f, 0f, 0.2f));
            effect.Parameters["Shininess"].SetValue(20f);

            camera = new Camera();
            camera.Transform = new Transform();
            camera.Transform.LocalPosition = Vector3.Up * 60;
            camera.Transform.Rotate(Vector3.Right, -MathHelper.PiOver2);

            // Set up a directional light (spreads light across the entire scene)
            light = new Light();
            light.Transform = new Transform();
            light.Transform.LocalPosition = new Vector3(50, 50, 50); // No position for directional light

            player = new Player(terrain, Content, camera, GraphicsDevice, light);
            prize1 = new Prize(terrain, Content, camera, GraphicsDevice, light, player);
            prize2 = new Prize(terrain, Content, camera, GraphicsDevice, light, player);
            prize3 = new Prize(terrain, Content, camera, GraphicsDevice, light, player);
            agent1 = new Agent(terrain, Content, camera, GraphicsDevice, light, player);
            agent2 = new Agent(terrain, Content, camera, GraphicsDevice, light, player);
            agent3 = new Agent(terrain, Content, camera, GraphicsDevice, light, player);
            bomb = new Bomb(terrain, Content, camera, GraphicsDevice, light, player);
            Vector3 targetPosition = new Vector3(1, 1, 3); // Example position (X, Y, Z)

            // Set agent1's position
            prize1.Transform.LocalPosition = targetPosition;
            // Load font for displaying text
            font = Content.Load<SpriteFont>("Arial"); // Ensure you have "Arial" or another font in the content folder
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Time.Update(gameTime);
            InputManager.Update();

            if (InputManager.IsKeyDown(Keys.Up)) // Move forward
                x++;
            if (InputManager.IsKeyDown(Keys.Down)) // Move backward
                x--;
            if (InputManager.IsKeyDown(Keys.Right)) // Move right
                z++;
            if (InputManager.IsKeyDown(Keys.Left)) // Move left
                z--;

            if (InputManager.IsKeyDown(Keys.N)) // Move downward
                y--;

            if (InputManager.IsKeyDown(Keys.M)) // Move upward
                y++;

            // Update all entities
            player.Update();
            prize1.Update();
            agent1.Update();
            prize2.Update();
            agent2.Update();
            prize3.Update();
            agent3.Update();
            bomb.Update();

            // Track time spent
            timeSpent += (float)gameTime.ElapsedGameTime.TotalSeconds;
            timeBomb += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Update camera to follow the player
            Vector3 cameraOffset = new Vector3(x, y, z); // Adjust Y and Z offsets for desired distance
            camera.Transform.LocalPosition = player.Transform.Position + cameraOffset;

            // Update the light position to follow the player (you can adjust the offset if needed)
            light.Transform.LocalPosition = player.Transform.Position + new Vector3(0, 0, 0);  // Light is above the player

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Update effect parameters for lighting
            effect.Parameters["View"].SetValue(camera.View);
            effect.Parameters["Projection"].SetValue(camera.Projection);
            effect.Parameters["World"].SetValue(terrain.Transform.World);
            effect.Parameters["CameraPosition"].SetValue(camera.Transform.Position);
            effect.Parameters["NormalMap"].SetValue(terrain.NormalMap);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                terrain.Draw();

                player.Draw();
                 agent1.Draw();
                 prize1.Draw();

                 agent2.Draw();
                // prize2.Draw();

                 agent3.Draw();
                // prize3.Draw();

                // bomb.Draw();
            }

            // Draw the caught aliens and time spent
            string text = $"Caught Aliens: {caughtAliens}\nTime Spent: {timeSpent:F2} seconds";
            _spriteBatch.Begin();
            _spriteBatch.DrawString(font, text, new Vector2(10, 10), Color.White);
            _spriteBatch.DrawString(font, "Time Bomb Counter: " + timeBomb.ToString(), new Vector2(10, 60), Color.White);
            _spriteBatch.DrawString(font," X" + prize1.Transform.LocalPosition.X + " Y" + prize1.Transform.LocalPosition.Y + " Z"+ prize1.Transform.LocalPosition.Z, new Vector2(10, 80), Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
