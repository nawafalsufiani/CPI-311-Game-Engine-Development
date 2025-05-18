#region Using Statements
using System;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using CPI311.GameEngine;
#endregion

namespace CPI311.Labs
{
    public class Game1 : Game
    {
        // Default speed multiplier
        private int SphereCounter = 0;
        private float speedMultiplier = 1.0f; 

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;
        Model model;
        Random random;

        private int _frameCount = 0;
        private float _elapsedTime = 0f;
        private float _fps = 0f;

        Transform cameraTransform;
        Camera camera;
        Texture2D texture;
        BoxCollider boxCollider;
        List<GameObject> gameObjects;

        List<Rigidbody> rigidbodies;
        List<Collider> colliders;
        List<Transform> transforms;

        int lastSecondCollisions = 0;
        int numberCollisions = 0;

        // Flag to toggle multi-threading for collisions
        bool haveThreadRunning = false;
        bool isMultithreaded = false;  

        // Diagnostic controls
        bool showDiagnostics = true;
        bool showSpeedColors = false;
        bool showTextures = false;

        // For canceling the collision thread
        private Thread collisionThread;
        private CancellationTokenSource cancellationTokenSource; 
        private CancellationToken cancellationToken;

        public Game1() : base()
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
             haveThreadRunning = true;
            ThreadPool.QueueUserWorkItem(new WaitCallback(CollisionReset));

            random = new Random();
            transforms = new List<Transform>();
            rigidbodies = new List<Rigidbody>();
            colliders = new List<Collider>();

            gameObjects = new List<GameObject>();
            boxCollider = new BoxCollider();
            boxCollider.Size = 10;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("font");
            model = Content.Load<Model>("Sphere");

            texture = Content.Load<Texture2D>("Square");

            foreach (ModelMesh mesh in model.Meshes)
                foreach (BasicEffect effect in mesh.Effects)
                    effect.EnableDefaultLighting();

            cameraTransform = new Transform();
            cameraTransform.LocalPosition = Vector3.Backward * 20;
            camera = new Camera();
            camera.Transform = cameraTransform;

            GameObject gameObject = new GameObject();
            Rigidbody rigidbody = new Rigidbody();
           
			gameObject.Add<Rigidbody>(rigidbody);
            SphereCollider sphereCollider = new SphereCollider();
            
			gameObject.Add<Collider>(sphereCollider);
            Renderer renderer = new Renderer();
            gameObject.Add<Renderer>(renderer);

            gameObjects.Add(gameObject);




            AddGameObject();

            // Initialize collision thread if multi-threading is enabled
            if (isMultithreaded)
            {
                StartCollisionThread();
            }
        }

        protected override void Update(GameTime gameTime)
        {
            Time.Update(gameTime);
            InputManager.Update();

            if (InputManager.IsKeyDown(Keys.Escape))
                Exit();

            // Increase speed with the Right arrow
            if (InputManager.IsKeyPressed(Keys.Right))
                speedMultiplier += 0.1f;

            // Decrease speed with the Left arrow
            if (InputManager.IsKeyPressed(Keys.Left))
                speedMultiplier = Math.Max(0.1f, speedMultiplier - 0.1f);

            if (InputManager.IsKeyPressed(Keys.Up))
                AddGameObject();

            if (InputManager.IsKeyPressed(Keys.Down))
                RemoveSphere();

            // Toggle diagnostics with Shift
            if (InputManager.IsKeyPressed(Keys.LeftShift) || InputManager.IsKeyPressed(Keys.RightShift))
                showDiagnostics = !showDiagnostics;

            // Toggle speed-based colors with Space
            if (InputManager.IsKeyPressed(Keys.Space))
                showSpeedColors = !showSpeedColors;

            // Toggle textures with Alt
            if (InputManager.IsKeyPressed(Keys.LeftAlt) || InputManager.IsKeyPressed(Keys.RightAlt))
                showTextures = !showTextures;

            // Toggle multi-threaded collision detection with M key
            if (InputManager.IsKeyPressed(Keys.M))
                ToggleMultiThreading();

            foreach (Rigidbody rigidbody in rigidbodies)
                rigidbody.Velocity *= speedMultiplier; // Apply speed multiplier



            


            foreach (Rigidbody rigidbody in rigidbodies)
                rigidbody.Update();

            // If multi-threading is disabled, run collision detection on the main thread
            if (!isMultithreaded)
            {
                PerformCollisionDetection();
            }

            //foreach (GameObject gameObject in gameObjects)
            //    gameObject.Update();


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGreen);
            GraphicsDevice.DepthStencilState = new DepthStencilState();

            foreach (Transform transform in transforms)
            {
                if (showSpeedColors)
                {
                    // Color based on speed
                    float speed = rigidbodies[transforms.IndexOf(transform)].Velocity.Length();
                    Color color = new Color(Math.Min(1, speed / 10), 0, 1 - Math.Min(1, speed / 10));
                    foreach (BasicEffect effect in model.Meshes[0].Effects)
                    {
                        effect.DiffuseColor = color.ToVector3();
                        effect.TextureEnabled = false;
                    }
                }
                else if (showTextures)
                {
                    // Texture on spheres
                    foreach (BasicEffect effect in model.Meshes[0].Effects)
                    {
                        effect.TextureEnabled = true;
                        effect.DiffuseColor = Color.White.ToVector3();
                    }
                }
                else
                {
                    // Default white color
                    foreach (BasicEffect effect in model.Meshes[0].Effects)
                    {
                        effect.DiffuseColor = Color.White.ToVector3();
                        effect.TextureEnabled = false;
                    }
                }
                foreach (GameObject gameObject in gameObjects)
                    gameObject.Draw();

                model.Draw(transform.World, camera.View, camera.Projection);
            }

            spriteBatch.Begin();
            // Menu display
            spriteBatch.DrawString(font, "Menu:", new Vector2(10, 10), Color.Black);
            spriteBatch.DrawString(font, $"<Shift> Diagnostics: {(showDiagnostics ? "ON" : "OFF")}", new Vector2(10, 40), Color.Black);
            spriteBatch.DrawString(font, $"<Space> Speed Colors: {(showSpeedColors ? "ON" : "OFF")}", new Vector2(10, 60), Color.Black);
            spriteBatch.DrawString(font, $"<Alt> Textures: {(showTextures ? "ON" : "OFF")}", new Vector2(10, 80), Color.Black);
            spriteBatch.DrawString(font, $"AVG Frame Per Second: {_fps}", new Vector2(10, 100), Color.Black);

            if (showDiagnostics)
            {
                spriteBatch.DrawString(font, "Calculating the number of Collisions in Last Secends: " + lastSecondCollisions, new Vector2(10, 120), Color.Black);
                spriteBatch.DrawString(font, "Press Up to Add Sphere", new Vector2(10, 140), Color.Black);
                spriteBatch.DrawString(font, "Press Down to Remove Sphere", new Vector2(10, 160), Color.Black);
                spriteBatch.DrawString(font, "Number of Spheres: " + SphereCounter, new Vector2(10, 180), Color.Black);
                spriteBatch.DrawString(font, "Speed Multiplier: " + speedMultiplier.ToString("0.0"), new Vector2(10, 200), Color.Black);
            }

            // Display whether multi-threading is enabled or not
            spriteBatch.DrawString(font, $"<M> Multi-threading: {(isMultithreaded ? "ENABLED" : "DISABLED")}", new Vector2(10, 220), Color.Black);

            spriteBatch.End();

            _frameCount++;
            _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // If 10 seconds have passed, calculate FPS
            if (_elapsedTime >= 10f)
            {
                _fps = _frameCount / _elapsedTime;
                // Reset the counters for the next second
                _frameCount = 0;
                _elapsedTime = 0f;
            }

            base.Draw(gameTime);
        }

        private void AddGameObject()
        {
            SphereCounter++;
            Transform transform = new Transform();

            // Correct scale initialization
            transform.LocalScale = new Vector3(random.Next(1, 10) * 0.25f);

            Rigidbody rigidbody = new Rigidbody();
            rigidbody.Transform = transform;

            // Use NextDouble for floating-point mass between 1 and 2
            rigidbody.Mass = (float)(random.NextDouble() + 1); // Random float between 1 and 2

            // Set a random position
            float x = (float)(random.NextDouble() * 20 - 10);
            float y = (float)(random.NextDouble() * 20 - 10);
            float z = (float)(random.NextDouble() * 20 - 10);
            transform.LocalPosition = new Vector3(x, y, z);

            // Set random velocity direction
            Vector3 direction = new Vector3((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
            direction.Normalize();
            rigidbody.Velocity = direction * ((float)random.NextDouble() * 5 + 5) * speedMultiplier;

            // Create and initialize the collider
            SphereCollider sphereCollider = new SphereCollider();
            sphereCollider.Radius = 2.5f * transform.LocalScale.Y;
            sphereCollider.Transform = transform;

            // Add transform, collider, and rigidbody to their respective lists
            transforms.Add(transform);
            colliders.Add(sphereCollider);
            rigidbodies.Add(rigidbody);

            // Create a new game object and add components to it
            GameObject gameObject = new GameObject();
           // gameObject.Add(sphereCollider);
           // gameObject.Add(rigidbody);
            
            // Add the game object to the game object list
            gameObjects.Add(gameObject);
        }


        private void RemoveSphere()
        {
            if (transforms.Count > 0)
            {
                transforms.RemoveAt(transforms.Count - 1);
                colliders.RemoveAt(colliders.Count - 1);
                rigidbodies.RemoveAt(rigidbodies.Count - 1);
                SphereCounter--;
            }
        }

        private void ToggleMultiThreading()
        {
            isMultithreaded = !isMultithreaded;

            if (isMultithreaded)
            {
                StartCollisionThread();
            }
            else
            {
                StopCollisionThread();
            }
        }

        private void StartCollisionThread()
        {
            cancellationTokenSource = new CancellationTokenSource();
            cancellationToken = cancellationTokenSource.Token;

            collisionThread = new Thread(() =>
            {
                while (!cancellationToken.IsCancellationRequested)
                { // Sleep for 100ms to yield to the main thread
                    Thread.Sleep(100); 
                    PerformCollisionDetection();
                }
            });

            collisionThread.IsBackground = true;
            collisionThread.Start();
        }
        private void PerformCollisionDetection()
        {
            Vector3 normal;
            for (int i = 0; i < transforms.Count; i++)
            {
                if (boxCollider.Collides(colliders[i], out normal))
                {
                    numberCollisions++;
                    if (Vector3.Dot(normal, rigidbodies[i].Velocity) < 0)
                        rigidbodies[i].Impulse += Vector3.Dot(normal, rigidbodies[i].Velocity) * -2 * normal;
                }
                for (int j = i + 1; j < transforms.Count; j++)
                {
                    if (colliders[i].Collides(colliders[j], out normal))
                    {
                        numberCollisions++;
                        if (Vector3.Dot(normal, rigidbodies[i].Velocity) > 0 &&
                            Vector3.Dot(normal, rigidbodies[j].Velocity) < 0)
                            continue;

                        Vector3 velocityNormal = Vector3.Dot(normal, rigidbodies[i].Velocity - rigidbodies[j].Velocity)
                                        * -2 * normal * rigidbodies[i].Mass * rigidbodies[j].Mass;
                        rigidbodies[i].Impulse += velocityNormal / 2;
                        rigidbodies[j].Impulse += -velocityNormal / 2;
                    }
                }
            }
        }
        private void StopCollisionThread()
        {
            // Signal cancellation
            // Wait for the thread to exit gracefully
            cancellationTokenSource?.Cancel(); 
            collisionThread?.Join(); 
        }

        private void CollisionReset(Object obj)
        {
            while (haveThreadRunning)
            {
                lastSecondCollisions = numberCollisions;
                numberCollisions = 0;
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}
