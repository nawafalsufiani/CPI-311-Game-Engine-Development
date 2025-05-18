using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;

namespace Testing2
{
    public class Lab05HWv2 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Random random = new Random();
        // Camera variables
        Vector3 cameraPosition = new Vector3(0, 0, 10);
        Vector3 cameraTarget = Vector3.Zero;
        Vector3 cameraUp = Vector3.Up;
        Effect simpleShadingEffect;
        // Torus position
        Vector3 torusPosition = Vector3.Zero;
        float torusScale = 1.0f;

        // Sphere parameters
        Vector3 spherePosition = Vector3.Zero; // Position of the sphere
        float sphereAngle = 0f; // Angle for sphere rotation
        float orbitSpeed = 0.01f; // Speed of sphere orbit
        float orbitRadius = 3f; // Distance from the torus

        // Projection parameters
        float nearPlane = 0.1f;
        float farPlane = 1000f;

        // Models
        Model torusModel;
        Model sphereModel;

        // Matrices
        Matrix world;
        Matrix view;
        Matrix projection;

        // Font
        SpriteFont font;

        public Lab05HWv2()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.GraphicsProfile = GraphicsProfile.HiDef; // HiDef for better quality
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load the torus and sphere models
            torusModel = Content.Load<Model>("Torus");
            sphereModel = Content.Load<Model>("Sphere");
            // Load the font for displaying text
            font = Content.Load<SpriteFont>("Font");

            // Load the custom shader (SimpleShading.fx)
           //simpleShadingEffect = Content.Load<Effect>("SimpleShading");

            foreach (ModelMesh mesh in torusModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                }
            }

            //foreach (ModelMesh mesh in sphereModel.Meshes)
            //{
            //    foreach (BasicEffect effect in mesh.Effects)
            //    {
            //        effect.EnableDefaultLighting();
            //        effect.PreferPerPixelLighting = true;
            //    }
            //}

            // Set up initial camera view and projection
            view = Matrix.CreateLookAt(cameraPosition, cameraTarget, cameraUp);
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver2, _graphics.GraphicsDevice.Viewport.AspectRatio, nearPlane, farPlane);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //// Get keyboard state for transformations
            //KeyboardState keyState = Keyboard.GetState();

            //// Update torus position based on arrow key input
            //if (keyState.IsKeyDown(Keys.Up))
            //    torusPosition.Y += 0.1f; // Move torus up
            //if (keyState.IsKeyDown(Keys.Down))
            //    torusPosition.Y -= 0.1f; // Move torus down
            //if (keyState.IsKeyDown(Keys.Left))
            //    torusPosition.X -= 0.1f; // Move torus left
            //if (keyState.IsKeyDown(Keys.Right))
            //    torusPosition.X += 0.1f; // Move torus right

            //// Update sphere's angle for orbit
            //sphereAngle += orbitSpeed;

            //// Calculate the sphere's position in relation to the torus
            //spherePosition.X = torusPosition.X + orbitRadius * (float)Math.Cos(sphereAngle); // X position based on cosine
            //spherePosition.Z = torusPosition.Z + orbitRadius * (float)Math.Sin(sphereAngle); // Z position based on sine
            //spherePosition.Y = torusPosition.Y; // Keep the sphere at the same height as the torus

            //// Camera movement controls
            //if (keyState.IsKeyDown(Keys.W))
            //    cameraPosition.Z -= 0.1f; // Move camera forward
            //if (keyState.IsKeyDown(Keys.S))
            //    cameraPosition.Z += 0.1f; // Move camera backward
            //if (keyState.IsKeyDown(Keys.A))
            //    cameraPosition.X -= 0.1f; // Move camera left
            //if (keyState.IsKeyDown(Keys.D))
            //    cameraPosition.X += 0.1f; // Move camera right

            //// Update the view matrix based on camera position
            //view = Matrix.CreateLookAt(cameraPosition, cameraTarget, cameraUp);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Draw the torus
            world = Matrix.CreateScale(torusScale) *
                    Matrix.CreateTranslation(torusPosition); // Position of torus
            foreach (ModelMesh mesh in torusModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = world;
                    effect.View = view;
                    effect.Projection = projection;
                }
                mesh.Draw();
            }

            // Draw the sphere (upright)
            //world = Matrix.CreateScale(1.0f) * // Adjust scale if needed
            //        Matrix.CreateTranslation(spherePosition); // Sphere position already calculated above
            //foreach (ModelMesh mesh2 in sphereModel.Meshes)
            //{
            //    foreach (BasicEffect effect in mesh2.Effects)
            //    {
            //        effect.World = world;
            //        effect.View = view;
            //        effect.Projection = projection;
            //    }
            //    mesh2.Draw();
            //}

            // Display instructions (optional)
            _spriteBatch.Begin();
            //_spriteBatch.DrawString(font, "Controls:", new Vector2(10, 10), Color.White);
            //_spriteBatch.DrawString(font, "Arrow Up: Move Torus Up", new Vector2(10, 30), Color.White);
            //_spriteBatch.DrawString(font, "Arrow Down: Move Torus Down", new Vector2(10, 50), Color.White);
            //_spriteBatch.DrawString(font, "Arrow Left: Move Torus Left", new Vector2(10, 70), Color.White);
            //_spriteBatch.DrawString(font, "Arrow Right: Move Torus Right", new Vector2(10, 90), Color.White);
            //_spriteBatch.DrawString(font, "W: Move Camera Forward", new Vector2(10, 110), Color.White);
            //_spriteBatch.DrawString(font, "S: Move Camera Backward", new Vector2(10, 130), Color.White);
            //_spriteBatch.DrawString(font, "A: Move Camera Left", new Vector2(10, 150), Color.White);
            //_spriteBatch.DrawString(font, "D: Move Camera Right", new Vector2(10, 170), Color.White);
            _spriteBatch.End();
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = new DepthStencilState();
            base.Draw(gameTime);
        }
    }
}