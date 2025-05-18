#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CPI311.GameEngine;
#endregion

namespace CPI311.Labs
{
    public class Lab09 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private AStarSearch search;
        private List<Vector3> path;

        private Random random = new Random();
        private SpriteFont font;
        private Camera camera;

        private Model cube;
        private Model sphere;

        public Lab09() : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            Time.Initialize();
            InputManager.Initialize();
            ScreenManager.Initialize(graphics);

            search = new AStarSearch(100, 100);
            InitializeSearchGrid();

            path = BuildPath();
            base.Initialize();
        }

        private void InitializeSearchGrid()
        {
            foreach (AStarNode node in search.Nodes)
            {
                if (random.NextDouble() < 0.2)
                {
                    search.Nodes[random.Next(100), random.Next(100)].Passable = false;
                }
            }
            search.Start = search.Nodes[0, 0];
            search.Start.Passable = true;
            search.End = search.Nodes[99, 99];
            search.End.Passable = true;
            search.Search();
        }

        private List<Vector3> BuildPath()
        {
            List<Vector3> path = new List<Vector3>();
            AStarNode current = search.End;

            while (current != null)
            {
                path.Insert(0, current.Position);
                current = current.Parent;
            }

            return path;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            cube = Content.Load<Model>("cube");
            font = Content.Load<SpriteFont>("Arial");
            sphere = Content.Load<Model>("Sphere");

            camera = new Camera();
            camera.Transform = new Transform();
            camera.Transform.LocalPosition = Vector3.One * 50;
            camera.Transform.Rotate(Vector3.Right, -MathHelper.PiOver2);
        }

        protected override void Update(GameTime gameTime)
        {
            Time.Update(gameTime);
            InputManager.Update();

            if (InputManager.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if (InputManager.IsKeyPressed(Keys.Space))
            {
                RegeneratePath();
            }

            base.Update(gameTime);
        }

        private void RegeneratePath()
        {
            while (!(search.Start = search.Nodes[random.Next(search.Cols), random.Next(search.Rows)]).Passable) ;
            while (!(search.End = search.Nodes[random.Next(search.Cols), random.Next(search.Rows)]).Passable) ;
            search.Search();

            path.Clear();
            AStarNode current = search.End;
            while (current != null)
            {
                path.Insert(0, current.Position);
                current = current.Parent;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
          //  spriteBatch.DrawString(font, "Press Space", new Vector2(10, 10), Color.White);
            spriteBatch.End();

            Matrix view = camera.View;
            Matrix projection = camera.Projection;

            DrawGround(view, projection);
            DrawObstacles(view, projection);
            DrawPath(view, projection);

            base.Draw(gameTime);
        }

        private void DrawGround(Matrix view, Matrix projection)
        {
            (cube.Meshes[0].Effects[0] as BasicEffect).DiffuseColor = Color.DarkBlue.ToVector3();
            cube.Draw(Matrix.CreateScale(55, 0.1f, 55) * Matrix.CreateTranslation(50, -5, 50), view, projection);
        }

        private void DrawObstacles(Matrix view, Matrix projection)
        {
            (cube.Meshes[0].Effects[0] as BasicEffect).DiffuseColor = Color.DarkRed.ToVector3();
            foreach (AStarNode node in search.Nodes)
            {
                if (!node.Passable)
                {
                    cube.Draw(Matrix.CreateScale(0.5f, 0.05f, 0.5f) * Matrix.CreateTranslation(node.Position), view, projection);
                }
            }
        }

        private void DrawPath(Matrix view, Matrix projection)
        {
            (sphere.Meshes[0].Effects[0] as BasicEffect).DiffuseColor = Color.WhiteSmoke.ToVector3();
            foreach (Vector3 position in path)
            {
                sphere.Draw(Matrix.CreateScale(0.1f, 0.1f, 0.1f) * Matrix.CreateTranslation(position), view, projection);
            }
        }
    }
}
