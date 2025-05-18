using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Drawing.Design;
using System.Drawing.Configuration;

namespace HighShaderLanguageTest
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Model _model;             // The 3D model (Torus)
        private Effect _effect;           // The custom shader effect
        private Matrix _world;
        private Matrix _view;
        private Matrix _projection;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
        }



        protected override void Initialize()
        {
            // Set up the world, view, and projection matrices
            _world = Matrix.CreateRotationY(MathHelper.ToRadians(45f)); // Rotation
            _view = Matrix.CreateLookAt(new Vector3(0, 5, 10), Vector3.Zero, Vector3.Up); // Camera setup
            _projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 0.1f, 100f);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load the 3D model
            _model = Content.Load<Model>("Torus");

            // Try to load the custom effect (shader)
            try
            {
                _effect = Content.Load<Effect>("Effect"); // This is the custom shader
                if (_effect == null)
                    throw new Exception("Effect loaded but is null.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to load Effect: {ex.Message}");
            }

            // Use BasicEffect for debugging purposes to ensure the model works
            foreach (var mesh in _model.Meshes)
            {
                foreach (var part in mesh.MeshParts)
                {
                    part.Effect = new BasicEffect(GraphicsDevice)
                    {
                        World = _world,
                        View = _view,
                        Projection = _projection,
                        LightingEnabled = true,
                        DiffuseColor = new Vector3(1, 0, 0), // Red color
                    };
                }
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Rotate the model over time
            _world = Matrix.CreateRotationY((float)gameTime.TotalGameTime.TotalSeconds);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Check if the model is valid before drawing
            if (_model != null)
            {
                // Iterate through the model's meshes
                foreach (ModelMesh mesh in _model.Meshes)
                {
                    // Iterate through each mesh part (sub-part of the mesh)
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        // No custom effect here, MonoGame will use the default BasicEffect
                    }

                    // Draw the mesh with the default effect
                    mesh.Draw();
                }
            }

            base.Draw(gameTime);
        }



    }
}
