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
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // *** Lab 10
        TerrainRenderer terrain;
        Camera camera;
        Light light;
        Effect effect;


        // Variables for tracking caught aliens and time spent

        private SpriteFont font; // Font for displaying text

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            //**********************************************************
            _graphics.GraphicsProfile = GraphicsProfile.HiDef; // ShadeDX -> Error
            //**********************************************************
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
                Content.Load<Texture2D>("mazeH2"),
                Vector2.One * 100, Vector2.One * 200);
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
            light.Transform.LocalPosition = new Vector3(50, 50, 50); ; // No position for directional light




            // Load font for displaying text
            font = Content.Load<SpriteFont>("Arial"); // Ensure you have "Arial" or another font in the content folder
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Time.Update(gameTime);
            InputManager.Update();

           

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
            //effect.Parameters["LightDirection"].SetValue(light.Direction); // Use light direction instead of position
            effect.Parameters["NormalMap"].SetValue(terrain.NormalMap);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                terrain.Draw();
              
            }

            

            base.Draw(gameTime);
        }
    }
}
