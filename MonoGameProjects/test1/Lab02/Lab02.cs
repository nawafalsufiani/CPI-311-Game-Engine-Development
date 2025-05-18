

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using CPI311.GameEngine;
using GameEngine2.Managers;

namespace Lab02
{
    public class Lab02 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        //public Sprite sprite;
        public SpiralMover spiralMover;
        public Lab02()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }
        protected override void Initialize()
        {
            // *** Initialize Managers ***
            InputManager.Initialize();
            Time.Initialize();
            // ***************************
            base.Initialize();
        }
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Texture2D temp = Content.Load<Texture2D>("Square");
            //sprite = new Sprite(temp);
            spiralMover = new SpiralMover(temp, new Vector2(300, 300));
        }
        protected override void Update(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back ==
            //ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            // *** Update Managers *****
            InputManager.Update();
            Time.Update(gameTime);
            // *************************
            spiralMover.Update();
            // *** Key Actions
            /*
            if (InputManager.IsKeyDown(Keys.Left))
            sprite.Position += Vector2.UnitX * -100 * Time.ElapsedGameTime;
            if (InputManager.IsKeyDown(Keys.Right))
            sprite.Position += Vector2.UnitX * 100 * Time.ElapsedGameTime;
            if (InputManager.IsKeyDown(Keys.Up))
            sprite.Position += Vector2.UnitY * -100 * Time.ElapsedGameTime;
            if (InputManager.IsKeyDown(Keys.Down))
            sprite.Position += Vector2.UnitY * 100 * Time.ElapsedGameTime;
            if (InputManager.IsKeyDown(Keys.Space))
            sprite.Rotation += Time.ElapsedGameTime;
            */
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            // *** Draw my Sprite
            //sprite.Draw(_spriteBatch);
            spiralMover.Draw(_spriteBatch);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
