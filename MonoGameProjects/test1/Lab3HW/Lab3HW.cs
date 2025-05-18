using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lab3HW
{
    public class Lab3HW : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Model torusModel;
        Matrix worldMatrix;
        Matrix scaleMatrix = Matrix.Identity;
        Matrix rotationMatrix = Matrix.Identity;
        Matrix translationMatrix = Matrix.Identity;

        float scale = 1.0f;
        float yaw = 0f, pitch = 0f, roll = 0f;
        Vector3 position = Vector3.Zero;

        bool useFirstOrder = true; // Toggle between matrix multiplication orders

        Vector3 cameraPosition = new Vector3(0, 0, 10);
        Matrix viewMatrix;
        Matrix projectionMatrix;
        bool isOrthographic = false; // Toggle between orthographic and perspective

        // Off-center projection parameters
        float left = -1, right = 1, bottom = -1, top = 1;
        float nearPlane = 0.1f, farPlane = 100f;

        SpriteBatch spriteBatch;
        SpriteFont font;
        public Lab3HW()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            torusModel = Content.Load<Model>("torus");
            font = Content.Load<SpriteFont>("Font");
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();

            // Handle translation (arrow keys)
            if (state.IsKeyDown(Keys.Left)) position.X -= 0.1f;
            if (state.IsKeyDown(Keys.Right)) position.X += 0.1f;
            if (state.IsKeyDown(Keys.Up)) position.Y += 0.1f;
            if (state.IsKeyDown(Keys.Down)) position.Y -= 0.1f;

            // Handle rotation (Insert/Delete, Home/End, PageUp/PageDown)
            if (state.IsKeyDown(Keys.Insert)) yaw -= 0.01f;
            if (state.IsKeyDown(Keys.Delete)) yaw += 0.01f;
            if (state.IsKeyDown(Keys.Home)) pitch -= 0.01f;
            if (state.IsKeyDown(Keys.End)) pitch += 0.01f;
            if (state.IsKeyDown(Keys.PageUp)) roll -= 0.01f;
            if (state.IsKeyDown(Keys.PageDown)) roll += 0.01f;

            // Update rotation matrix
            rotationMatrix = Matrix.CreateFromYawPitchRoll(yaw, pitch, roll);

            // Handle scaling (Shift + Up/Down)
            if (state.IsKeyDown(Keys.LeftShift) || state.IsKeyDown(Keys.RightShift))
            {
                if (state.IsKeyDown(Keys.Up)) scale += 0.01f;
                if (state.IsKeyDown(Keys.Down)) scale -= 0.01f;
            }
            scaleMatrix = Matrix.CreateScale(scale);

            // Handle translation matrix
            translationMatrix = Matrix.CreateTranslation(position);

            // Toggle between multiplication orders with Space key
            if (state.IsKeyDown(Keys.Space))
            {
                useFirstOrder = !useFirstOrder;
            }

            // Choose the World matrix order based on the toggle
            if (useFirstOrder)
            {
                worldMatrix = scaleMatrix * rotationMatrix * translationMatrix;
            }
            else
            {
                worldMatrix = translationMatrix * rotationMatrix * scaleMatrix;
            }

            // Toggle between orthographic and perspective projection with Tab key
            if (state.IsKeyDown(Keys.Tab))
            {
                isOrthographic = !isOrthographic;
            }

            // Update projection matrix based on the current mode
            if (isOrthographic)
            {
                projectionMatrix = Matrix.CreateOrthographicOffCenter(left, right, bottom, top, nearPlane, farPlane);
            }
            else
            {
                projectionMatrix = Matrix.CreatePerspectiveOffCenter(left, right, bottom, top, nearPlane, farPlane);
            }

            // Handle WASD camera movement
            if (state.IsKeyDown(Keys.W)) cameraPosition.Z -= 0.1f;
            if (state.IsKeyDown(Keys.S)) cameraPosition.Z += 0.1f;
            if (state.IsKeyDown(Keys.A)) cameraPosition.X -= 0.1f;
            if (state.IsKeyDown(Keys.D)) cameraPosition.X += 0.1f;

            // Handle Shift + WASD to adjust projection center
            if (state.IsKeyDown(Keys.LeftShift) || state.IsKeyDown(Keys.RightShift))
            {
                if (state.IsKeyDown(Keys.W)) top += 0.1f;
                if (state.IsKeyDown(Keys.S)) bottom -= 0.1f;
                if (state.IsKeyDown(Keys.A)) left -= 0.1f;
                if (state.IsKeyDown(Keys.D)) right += 0.1f;
            }

            // Handle Ctrl + WASD to adjust width/height of the projection
            if (state.IsKeyDown(Keys.LeftControl) || state.IsKeyDown(Keys.RightControl))
            {
                if (state.IsKeyDown(Keys.W)) top += 0.1f;
                if (state.IsKeyDown(Keys.S)) top -= 0.1f;
                if (state.IsKeyDown(Keys.A)) left += 0.1f;
                if (state.IsKeyDown(Keys.D)) right -= 0.1f;
            }

            viewMatrix = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            foreach (ModelMesh mesh in torusModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = worldMatrix;
                    effect.View = viewMatrix;
                    effect.Projection = projectionMatrix;
                }
                mesh.Draw();
            }

            // Display the instructions
            _spriteBatch.Begin();
            _spriteBatch.DrawString(font, "Controls:", new Vector2(10, 10), Color.White);
            _spriteBatch.DrawString(font, "Arrow Keys: Move object (X, Y)", new Vector2(10, 30), Color.White);
            _spriteBatch.DrawString(font, "Insert/Delete: Yaw (rotate left/right)", new Vector2(10, 50), Color.White);
            _spriteBatch.DrawString(font, "Home/End: Pitch (rotate up/down)", new Vector2(10, 70), Color.White);
            _spriteBatch.DrawString(font, "PageUp/PageDown: Roll (tilt left/right)", new Vector2(10, 90), Color.White);
            _spriteBatch.DrawString(font, "Shift + Up/Down: Scale object", new Vector2(10, 110), Color.White);
            _spriteBatch.DrawString(font, "Space: Toggle Matrix Order", new Vector2(10, 130), Color.White);
            _spriteBatch.DrawString(font, "Tab: Toggle Projection Mode (Orthographic/Perspective)", new Vector2(10, 150), Color.White);
            _spriteBatch.DrawString(font, "WASD: Move Camera", new Vector2(10, 170), Color.White);
            _spriteBatch.DrawString(font, "Shift + WASD: Move Projection Center", new Vector2(10, 190), Color.White);
            _spriteBatch.DrawString(font, "Ctrl + WASD: Adjust Projection Width/Height", new Vector2(10, 210), Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}