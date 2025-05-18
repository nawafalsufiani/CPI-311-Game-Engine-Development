using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameEngine;
namespace Project1
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Texture2D texture;
        private SpriteFont font;
        private int score = 100;
        private Fraction a = new Fraction(3, 4);
        private Fraction b = new Fraction(8, 3);
        public Game1()
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
            texture = Content.Load<Texture2D>("background");
            font = Content.Load<SpriteFont>("Score");
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            Fraction a = new Fraction(3, 4);

            Fraction b = new Fraction(8, 3);

            FractionCalculator fractionCalculator = new FractionCalculator();
            Fraction c = fractionCalculator.MultiplayFraction(a, b);

            SimplifyFraction simplifyFraction = new SimplifyFraction();
            simplifyFraction.Simplify(c);

            Fraction d = fractionCalculator.SumFraction(a, b);


            simplifyFraction.Simplify(d);

            Fraction e = fractionCalculator.Subdivision(a, b);


            simplifyFraction.Simplify(e);


            Fraction f = fractionCalculator.DivisionFraction(a, b);


            simplifyFraction.Simplify(f);



            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
             _spriteBatch.Draw(texture,new Rectangle(0, 0,GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height),Color.White);
            _spriteBatch.DrawString(font, a.Numerator + "/" + a.Denominator + " + " + b.Numerator + "/" + b.Denominator + " = " + d.Numerator + "/" + d.Denominator, new Vector2(10, 100), Color.Black);
            _spriteBatch.DrawString(font, a.Numerator + "/" + a.Denominator + " - " + b.Numerator + "/" + b.Denominator + " = " + e.Numerator + "/" + e.Denominator, new Vector2(10, 200), Color.Black);
            _spriteBatch.DrawString(font, a.Numerator + "/" + a.Denominator + " * " + b.Numerator + "/" + b.Denominator + " = " + c.Numerator + "/" + c.Denominator, new Vector2(10, 300), Color.Black);
            _spriteBatch.DrawString(font, a.Numerator + "/" + a.Denominator + " / " + b.Numerator + "/" + b.Denominator + " = " + f.Numerator + "/" + f.Denominator, new Vector2(10, 400), Color.Black);


            _spriteBatch.End();



            base.Draw(gameTime);
        }
    }
}