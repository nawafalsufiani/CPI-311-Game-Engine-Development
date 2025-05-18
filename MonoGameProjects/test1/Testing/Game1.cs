using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Testing
{


    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Camera variables
        Vector3 cameraPosition = new Vector3(0, 0, 10);
        Vector3 cameraTarget = Vector3.Zero;
        Vector3 cameraUp = Vector3.Up;

        // Model transformation variables
        Vector3 modelPosition = Vector3.Zero;
        Vector3 modelRotation = Vector3.Zero;
        float modelScale = 1.0f;

        // Projection parameters
        float projectionWidth = 10f;
        float projectionHeight = 10f;
        float projectionCenterX = 0f;
        float projectionCenterY = 0f;
        float nearPlane = 0.1f;
        float farPlane = 1000f;

        // Model
        Model torusModel;
        Model SphereModel;
        // Matrices
        Matrix world;
        Matrix view;
        Matrix projection;

        // State variables
        bool isPerspective = true;  // Toggle between orthographic and perspective
        bool useScaleRotationTranslation = true; // Toggle between two matrix multiplication orders

        // Font
        SpriteFont font;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
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

            // Load the torus model
            torusModel = Content.Load<Model>("Torus");
            torusModel = Content.Load<Model>("Sphere");
            // Load the font for displaying text
            font = Content.Load<SpriteFont>("Font");

            foreach (ModelMesh mesh in torusModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                }
            }

            // Set up initial camera view and projection
            view = Matrix.CreateLookAt(cameraPosition, cameraTarget, cameraUp);
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver2, _graphics.GraphicsDevice.Viewport.AspectRatio, nearPlane, farPlane);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Get keyboard state for transformations
            KeyboardState keyState = Keyboard.GetState();

            // Scale: Shift + Up/Down to increase/decrease scale
            if (keyState.IsKeyDown(Keys.LeftShift) || keyState.IsKeyDown(Keys.RightShift))
            {
                if (keyState.IsKeyDown(Keys.Up)) modelScale += 0.01f;
                if (keyState.IsKeyDown(Keys.Down)) modelScale -= 0.01f;
            }

            if (!keyState.IsKeyDown(Keys.LeftShift) && !keyState.IsKeyDown(Keys.RightShift))
            {
                // Translation: Arrow keys move the model on the x- and y- axes
                if (keyState.IsKeyDown(Keys.Left))
                {
                    modelPosition.X -= 0.1f;
                }
                if (keyState.IsKeyDown(Keys.Right))
                {
                    modelPosition.X += 0.1f;
                }
                if (keyState.IsKeyDown(Keys.Up))
                {
                    modelPosition.Y += 0.1f;
                }
                if (keyState.IsKeyDown(Keys.Down))
                {
                    modelPosition.Y -= 0.1f;
                }
            }

            // Rotation: Insert/Delete, Home/End, PageUp/PageDown for yaw, pitch, roll
            if (keyState.IsKeyDown(Keys.Insert))
            {
                modelRotation.Y += MathHelper.ToRadians(1f);
            } // Yaw (Y-axis)
            if (keyState.IsKeyDown(Keys.Delete))
            {
                modelRotation.Y -= MathHelper.ToRadians(1f);
            }
            if (keyState.IsKeyDown(Keys.Home))
            {
                modelRotation.X += MathHelper.ToRadians(1f);
            }  // Pitch (X-axis)
            if (keyState.IsKeyDown(Keys.End))
            {
                modelRotation.X -= MathHelper.ToRadians(1f);
            }
            if (keyState.IsKeyDown(Keys.PageUp))
            {
                modelRotation.Z += MathHelper.ToRadians(1f);
            }// Roll (Z-axis)
            if (keyState.IsKeyDown(Keys.PageDown))
            {
                modelRotation.Z -= MathHelper.ToRadians(1f);
            }

            // Update view (camera) - using WASD for simple camera movement
            if (keyState.IsKeyDown(Keys.W)) cameraPosition.Z -= 0.1f;
            if (keyState.IsKeyDown(Keys.S)) cameraPosition.Z += 0.1f;
            if (keyState.IsKeyDown(Keys.A)) cameraPosition.X -= 0.1f;
            if (keyState.IsKeyDown(Keys.D)) cameraPosition.X += 0.1f;

            // Toggle between Perspective and Orthographic projection using Tab
            if (keyState.IsKeyDown(Keys.Tab))
            {
                isPerspective = !isPerspective;  // Toggle the mode
                if (isPerspective)
                {
                    projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver2, _graphics.GraphicsDevice.Viewport.AspectRatio, nearPlane, farPlane);
                }
                else
                {
                    projection = Matrix.CreateOrthographicOffCenter(
                        projectionCenterX - (projectionWidth / 2), // left
                        projectionCenterX + (projectionWidth / 2), // right
                        projectionCenterY - (projectionHeight / 2), // bottom
                        projectionCenterY + (projectionHeight / 2), // top
                        nearPlane,
                        farPlane);
                }
            }

            // Toggle matrix multiplication order with Space key
            if (keyState.IsKeyDown(Keys.Space))
            {
                useScaleRotationTranslation = !useScaleRotationTranslation;
            }

            // Projection adjustments using Shift + WASD (move the center) and Ctrl + WASD (adjust width/height)
            if (keyState.IsKeyDown(Keys.LeftControl) || keyState.IsKeyDown(Keys.RightControl))
            {
                // Ctrl + WASD to change width/height
                if (keyState.IsKeyDown(Keys.W)) projectionHeight += 0.1f;
                if (keyState.IsKeyDown(Keys.S)) projectionHeight -= 0.1f;
                if (keyState.IsKeyDown(Keys.A)) projectionWidth -= 0.1f;
                if (keyState.IsKeyDown(Keys.D)) projectionWidth += 0.1f;
            }
            else if (keyState.IsKeyDown(Keys.LeftShift) || keyState.IsKeyDown(Keys.RightShift))
            {
                // Shift + WASD to move the center
                if (keyState.IsKeyDown(Keys.W)) projectionCenterY += 0.1f;
                if (keyState.IsKeyDown(Keys.S)) projectionCenterY -= 0.1f;
                if (keyState.IsKeyDown(Keys.A)) projectionCenterX -= 0.1f;
                if (keyState.IsKeyDown(Keys.D)) projectionCenterX += 0.1f;
            }

            // Rebuild the orthographic projection if in orthographic mode
            if (!isPerspective)
            {
                projection = Matrix.CreateOrthographicOffCenter(
                    projectionCenterX - (projectionWidth / 2), // left
                    projectionCenterX + (projectionWidth / 2), // right
                    projectionCenterY - (projectionHeight / 2), // bottom
                    projectionCenterY + (projectionHeight / 2), // top
                    nearPlane,
                    farPlane);
            }

            // Update the view matrix with the new camera position
            view = Matrix.CreateLookAt(cameraPosition, cameraTarget, cameraUp);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Apply transformations based on the current order
            if (useScaleRotationTranslation)
            {
                // World = Scale * Rotation * Translation
                world = Matrix.CreateScale(modelScale) *
                        Matrix.CreateRotationX(modelRotation.X) *
                        Matrix.CreateRotationY(modelRotation.Y) *
                        Matrix.CreateRotationZ(modelRotation.Z) *
                        Matrix.CreateTranslation(modelPosition);
            }
            else
            {
                // World = Translation * Rotation * Scale
                world = Matrix.CreateTranslation(modelPosition) *
                        Matrix.CreateRotationX(modelRotation.X) *
                        Matrix.CreateRotationY(modelRotation.Y) *
                        Matrix.CreateRotationZ(modelRotation.Z) *
                        Matrix.CreateScale(modelScale);
            }

            // Draw the model with the current world, view, and projection matrices
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

            // Display the instructions and current matrix order
            _spriteBatch.Begin();
            _spriteBatch.DrawString(font, "Controls:", new Vector2(10, 10), Color.White);
            _spriteBatch.DrawString(font, "Arrow Keys:Move 3D model", new Vector2(10, 30), Color.White);
            _spriteBatch.DrawString(font, "Insert/Delete: Yaw (rotate left/right)", new Vector2(10, 50), Color.White);
            _spriteBatch.DrawString(font, "Home/End: Pitch (rotate up/down)", new Vector2(10, 70), Color.White);
            _spriteBatch.DrawString(font, "PageUp/PageDown: Roll (tilt left/right)", new Vector2(10, 90), Color.White);
            _spriteBatch.DrawString(font, "Shift + Up/Down: Scaling", new Vector2(10, 110), Color.White);
            _spriteBatch.DrawString(font, "Space: Change Matrix Order", new Vector2(10, 130), Color.White);
            _spriteBatch.DrawString(font, "Tab: Toggle Projection Mode (Orthographic/Perspective)", new Vector2(10, 150), Color.White);
            _spriteBatch.DrawString(font, "WASD: Control Camera moviment", new Vector2(10, 170), Color.White);
            _spriteBatch.DrawString(font, "Shift + WASD: Move Projection Center", new Vector2(10, 190), Color.White);
            _spriteBatch.DrawString(font, "Ctrl + WASD: Adjust Projection Width/Height", new Vector2(10, 210), Color.White);

            // Display the current matrix order
            string orderText = useScaleRotationTranslation ?
                "Current Order: Scale * Rotation * Translation" :
                "Current Order: Translation * Rotation * Scale";
            _spriteBatch.DrawString(font, orderText, new Vector2(10, 230), Color.White);

            // Display the object position
            string positionText = $"Position: X={modelPosition.X:F2}, Y={modelPosition.Y:F2}, Z={modelPosition.Z:F2}";
            _spriteBatch.DrawString(font, positionText, new Vector2(10, 250), Color.White);

            _spriteBatch.End();

            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = new DepthStencilState();

            base.Draw(gameTime);
        }
    }

}