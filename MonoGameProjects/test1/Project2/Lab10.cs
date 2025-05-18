#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CPI311.GameEngine;
#endregion

namespace CPI311.Labs
{
    public class Lab10 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        TerrainRenderer terrain;
        Camera camera;
        Effect effect;
        SpriteFont font;

        int sky = 0;

        public Lab10(): base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
        }

        protected override void Initialize()
        {
            Time.Initialize();
            InputManager.Initialize();
            ScreenManager.Initialize(graphics);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load terrain and other content
            terrain = new TerrainRenderer(Content.Load<Texture2D>("Heightmap"), Vector2.One * 100, Vector2.One * 200);
            terrain.NormalMap = Content.Load<Texture2D>("Normalmap");
            float height = terrain.GetHeight(new Vector2(0.5f, 0.5f));
            terrain.Transform = new Transform();
            terrain.Transform.LocalScale *= new Vector3(1, 5, 1);
            effect = Content.Load<Effect>("TerrainShader");
            effect.Parameters["AmbientColor"].SetValue(new Vector3(0.1f, 0.1f, 0.1f));
            effect.Parameters["DiffuseColor"].SetValue(new Vector3(0.3f, 0.1f, 0.1f));
            effect.Parameters["SpecularColor"].SetValue(new Vector3(0, 0, 0.2f));
            effect.Parameters["Shininess"].SetValue(20f);

            // inistalise camera
            camera = new Camera();
            camera.Transform = new Transform();
            camera.Transform.LocalPosition = Vector3.Backward * 5 + Vector3.Right * 5 + Vector3.Up * 5;
            camera.Transform.Rotate(Vector3.Right, -MathHelper.PiOver4);

            // Load a SpriteFont for rendering text
            font = Content.Load<SpriteFont>("font"); 
        }

        protected override void Update(GameTime gameTime)
        {
            Time.Update(gameTime);
            InputManager.Update();

            if (InputManager.IsKeyDown(Keys.Escape))
                Exit();

            // Control the camera
            if (InputManager.IsKeyDown(Keys.W)) // move forward
                camera.Transform.LocalPosition += camera.Transform.Forward * Time.ElapsedGameTime;
            if (InputManager.IsKeyDown(Keys.S)) // move backward
                camera.Transform.LocalPosition += camera.Transform.Backward * Time.ElapsedGameTime;
            if (InputManager.IsKeyDown(Keys.A)) // rotate left
                camera.Transform.Rotate(Vector3.Up, Time.ElapsedGameTime);
            if (InputManager.IsKeyDown(Keys.D)) // rotate right
                camera.Transform.Rotate(Vector3.Down, Time.ElapsedGameTime);
            if (InputManager.IsKeyDown(Keys.Q)) // look up
                camera.Transform.Rotate(Vector3.Right, Time.ElapsedGameTime);
            if (InputManager.IsKeyDown(Keys.E)) // look down
                camera.Transform.Rotate(Vector3.Left, Time.ElapsedGameTime);

            if (InputManager.IsKeyPressed(Keys.L)) // look down
                sky = 1;

            if (InputManager.IsKeyPressed(Keys.K)) // look down
                sky = 0;

            // update camera position
            camera.Transform.LocalPosition = new Vector3(camera.Transform.LocalPosition.X,terrain.GetAltitude(camera.Transform.LocalPosition),camera.Transform.LocalPosition.Z) + Vector3.Up;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if ( sky == 0)
            GraphicsDevice.Clear(Color.LightBlue);
            else
            {
                GraphicsDevice.Clear(Color.DarkBlue);
            }

            GraphicsDevice.DepthStencilState = new DepthStencilState();

            effect.Parameters["NormalMap"].SetValue(terrain.NormalMap);
            effect.Parameters["World"].SetValue(terrain.Transform.World);
            effect.Parameters["View"].SetValue(camera.View);
            effect.Parameters["Projection"].SetValue(camera.Projection);
            effect.Parameters["LightPosition"].SetValue(camera.Transform.Position + Vector3.Up * 10);
            effect.Parameters["CameraPosition"].SetValue(camera.Transform.Position);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                terrain.Draw();
            }

            // key menu
            spriteBatch.Begin();

            // Display camera control keys
            spriteBatch.DrawString(font, "Camera Controls:", new Vector2(10, 10), Color.White);
            spriteBatch.DrawString(font, "W - Move Forward", new Vector2(10, 30), Color.White);
            spriteBatch.DrawString(font, "S - Move Backward", new Vector2(10, 50), Color.White);
            spriteBatch.DrawString(font, "A - Rotate Left", new Vector2(10, 70), Color.White);
            spriteBatch.DrawString(font, "D - Rotate Right", new Vector2(10, 90), Color.White);
            spriteBatch.DrawString(font, "Q - Look Up", new Vector2(10, 110), Color.White);
            spriteBatch.DrawString(font, "E - Look Down", new Vector2(10, 130), Color.White);
            spriteBatch.DrawString(font, "L/K change Sky colors day and night", new Vector2(10, 170), Color.White);

            // camera position
            Vector3 camPos = camera.Transform.LocalPosition;
            spriteBatch.DrawString(font, $"Camera Position: X={camPos.X:F2}, Y={camPos.Y:F2}, Z={camPos.Z:F2}", new Vector2(10, 150), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
