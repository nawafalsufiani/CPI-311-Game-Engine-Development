using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CPI311.GameEngine;

namespace Lab5HW
{
    public class Lab5HW : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Model model;
        private Model childModel;
        private Camera camera;
        private Transform modelTransform;
        private Transform cameraTransform;
        private Transform parentTransform;
        private Transform childTransform;

        public Lab5HW()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            InputManager.Initialize();
            Time.Initialize();

            // Load the model
            model = Content.Load<Model>("Torus");
            childModel = Content.Load<Model>("Sphere");
            // Initialize parent and child transforms
            parentTransform = new Transform();
            childTransform = new Transform();
            childTransform.Parent = parentTransform;

            parentTransform.LocalPosition = Vector3.Zero;
            childTransform.LocalPosition = new Vector3(0, 0, 2);

            // Initialize camera and its transform
            cameraTransform = new Transform();
            cameraTransform.LocalPosition = Vector3.Backward * 5;

            camera = new Camera();
            camera.Transform = cameraTransform;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            Time.Update(gameTime);
            InputManager.Update();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Camera movement with W, A, S, D
            if (InputManager.IsKeyDown(Keys.W))
                cameraTransform.LocalPosition += cameraTransform.Forward * Time.ElapsedGameTime;
            if (InputManager.IsKeyDown(Keys.S))
                cameraTransform.LocalPosition += cameraTransform.Backward * Time.ElapsedGameTime;
            if (InputManager.IsKeyDown(Keys.A))
                cameraTransform.Rotate(Vector3.Up, Time.ElapsedGameTime);
            if (InputManager.IsKeyDown(Keys.D))
                cameraTransform.Rotate(Vector3.Up, -Time.ElapsedGameTime);

            // Move the parent with arrow keys
            if (InputManager.IsKeyDown(Keys.Up))
                parentTransform.LocalPosition += Vector3.Forward * Time.ElapsedGameTime;
            if (InputManager.IsKeyDown(Keys.Down))
                parentTransform.LocalPosition += Vector3.Backward * Time.ElapsedGameTime;
            if (InputManager.IsKeyDown(Keys.Left))
                parentTransform.LocalPosition += Vector3.Left * Time.ElapsedGameTime;
            if (InputManager.IsKeyDown(Keys.Right))
                parentTransform.LocalPosition += Vector3.Right * Time.ElapsedGameTime;



            // Rotate the child object around its parent
            childTransform.Rotate(Vector3.Up, Time.ElapsedGameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Draw the parent model
            childModel.Draw(parentTransform.World, camera.View, camera.Projection);

            // Draw the child model (rotating around the parent)
            model.Draw(childTransform.World, camera.View, camera.Projection);

            foreach (var mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                }
                mesh.Draw();
            }

            foreach (var mesh in childModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                }
                mesh.Draw();
            }

        }
    }
}
