using System;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CPI311.GameEngine;
using Microsoft.Xna.Framework.Audio;
using System.Text;
using System.Reflection;

namespace CPI311.Labs
{
    public class Assignment4 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Random random;
        private Texture2D myfireTexture, astroidTexture, fire, spaceShipTexture;
        Transform cameraTransform;
        Camera camera;
        Light light;

        // Audio components
        SoundEffect gunSound, explosion;
        SoundEffectInstance soundInstance;

        // Visual components
        Ship ship;
        Asteroid[] asteroidList = new Asteroid[GameConstants.NumAsteroids];
        Bullet[] bulletList = new Bullet[GameConstants.NumBullets];
        SpriteFont font;

        // Score & background
        Texture2D stars;
        SpriteFont LucidaConsole;

        // Particles
        ParticleManager particleManager;
        Texture2D particleTex;
        Effect particleEffect;

        SoundEffect asteroidExplosion, shipExplosion, engineSound;
        private SoundEffectInstance engineSoundInstance;
        int bulletCount, asteroidCount;

        bool sound;
        private readonly Vector2 scorePosition = new Vector2(20, 20);
        private readonly Color textColor = Color.White;
        private readonly StringBuilder stringBuilder = new StringBuilder();

        private int score = 0;

        Asteroid asteroid;
        Bullet bullet;
        GameObject gameObject;

        
        private Model shipSpace;
        private Model bulletSpace, bulletAstroid; 
        private Vector3 spherePosition = new Vector3(0, -20, -10); 

        
        private List<Model> bulletLists = new List<Model>(); 
        private List<Vector3> bulletPositions = new List<Vector3>();  
        private KeyboardState _previousKeyboardState;

        
        private Model asteroidModel;
        private Vector3 asteroidPosition = new Vector3(0, 1000, -10); 
        private MouseState _previousMouseState;
        
        private List<Vector3> asteroidPositions = new List<Vector3>();

        
        private List<Vector2> firePositions = new List<Vector2>();

       
        private float asteroidRotationAngle = 0.0f;

        
        private float asteroidSpeed = 0.1f; 

        
        private float asteroidShootTimer = 0.0f;
        private float asteroidShootInterval = 1.0f; 

        public Assignment4()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
        }

        protected override void Initialize()
        {
            
            InputManager.Initialize();
            Time.Initialize();
            ScreenManager.Initialize(_graphics);
            light = new Light();
            camera = new Camera();
            cameraTransform = new Transform();
            cameraTransform.LocalPosition = Vector3.Backward * 20;  
            camera = new Camera();
            camera.Transform = cameraTransform;

           
            List<Bullet> bullets = new List<Bullet>();
            bullet = new Bullet(Content, camera, GraphicsDevice, light);
            asteroid = new Asteroid(Content, camera, GraphicsDevice, light);

            
            ship = new Ship(Content, camera, GraphicsDevice, light, bullets);

            ship.Transform.LocalPosition = new Vector3(0, 0, 0); 

            
            random = new Random();
            for (int i = 0; i < 5; i++)
            {
                float x = (float)(random.NextDouble() * 20 - 10); 
                float y = (float)(random.NextDouble() * 10 - 5);  
                float z = (float)(random.NextDouble() * -20 - 5); 
                asteroidPositions.Add(new Vector3(x, y, -10));
            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Arial");

           
            shipSpace = Content.Load<Model>("sphere");

           
            bulletSpace = Content.Load<Model>("bulletSpace");  

          
            asteroidModel = Content.Load<Model>("Asteroid"); 

            bulletAstroid = Content.Load<Model>("bulletAstroid");

            gunSound = Content.Load<SoundEffect>("tx0_fire1");

            explosion = Content.Load<SoundEffect>("explosion2");

            myfireTexture = Content.Load<Texture2D>("BulletTexture");

            astroidTexture = Content.Load<Texture2D>("Square");

            spaceShipTexture = Content.Load<Texture2D>("spaceshipTexture");

            fire = Content.Load<Texture2D>("fire");




            ScreenManager.Setup(false, 1920, 1080);
        }

        protected override void Update(GameTime gameTime)
        {
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            MouseState mouseState;
            

            float moveSpeed = 0.6f; 
            KeyboardState keyboardState = Keyboard.GetState();
            

            if (keyboardState.IsKeyDown(Keys.W))
                spherePosition.Y += moveSpeed;
            if (keyboardState.IsKeyDown(Keys.S))
                spherePosition.Y -= moveSpeed;
            if (keyboardState.IsKeyDown(Keys.A))
                spherePosition.X -= moveSpeed;
            if (keyboardState.IsKeyDown(Keys.D))
                spherePosition.X += moveSpeed;

            
            if (keyboardState.IsKeyDown(Keys.M) && _previousKeyboardState.IsKeyUp(Keys.M))
            {
              
                bulletLists.Add(Content.Load<Model>("bulletSpace")); 
                bulletPositions.Add(new Vector3(spherePosition.X, spherePosition.Y, spherePosition.Z)); 

                SoundEffectInstance soundInstance = gunSound.CreateInstance();
                soundInstance.IsLooped = false;
                soundInstance.Play();
            }

           
            mouseState = Mouse.GetState();

            
            if (mouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
            {
                
                bulletLists.Add(Content.Load<Model>("bulletSpace")); 
                bulletPositions.Add(new Vector3(spherePosition.X, spherePosition.Y, spherePosition.Z));

                SoundEffectInstance soundInstance = gunSound.CreateInstance();
                soundInstance.IsLooped = false;
                soundInstance.Play();
            }



            _previousMouseState = mouseState;

        
            float bulletSpeed = 0.6f; 
            for (int i = 0; i < bulletPositions.Count; i++)
            {
                
                bulletPositions[i] = new Vector3(bulletPositions[i].X, bulletPositions[i].Y + bulletSpeed, bulletPositions[i].Z);
            }

           
            MoveAsteroidTowardsShip();

            
            asteroidShootTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (asteroidShootTimer >= asteroidShootInterval)
            {
                asteroidShootTimer = 0f;
                ShootBulletFromAsteroid();
            }

           
            CheckCollisions();

          
            bullet.Update();
            ship.Update();
            asteroid.Update();

           
            _previousKeyboardState = keyboardState;

            base.Update(gameTime);
        }

        private void ShootBulletFromAsteroid()
        {
           
            bulletLists.Add(Content.Load<Model>("bulletSpace"));
            bulletPositions.Add(new Vector3(asteroidPosition.X, asteroidPosition.Y, asteroidPosition.Z)); 
        }

        private void MoveAsteroidTowardsShip()
        {
            
            Vector3 directionToShip = ship.Transform.LocalPosition - asteroidPosition;

            
            directionToShip.Normalize();

         
            asteroidPosition += directionToShip * asteroidSpeed;
        }

        private void CheckCollisions()
        {
          
            float collisionThreshold = 1.0f;

            for (int i = bulletPositions.Count - 1; i >= 0; i--)
            {
                for (int j = asteroidPositions.Count - 1; j >= 0; j--)
                {
                  
                    float distance = Vector3.Distance(bulletPositions[i], asteroidPositions[j]);

                    if (distance < collisionThreshold)
                    {
                        SoundEffectInstance soundInstance = explosion.CreateInstance();
                        soundInstance.IsLooped = false;
                        soundInstance.Play();

                        score++;

                       
                        firePositions.Add(new Vector2(bulletPositions[i].X, bulletPositions[i].Y));

                        bulletLists.RemoveAt(i);
                        bulletPositions.RemoveAt(i);
                        asteroidPositions.RemoveAt(j);
                        break;  
                    }
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Blue);

           
            _spriteBatch.Begin();
            _spriteBatch.DrawString(font, "The score " + score, Vector2.UnitY * 20, Color.PaleGreen);
            _spriteBatch.End();

           
            asteroid.Draw();
            bullet.Draw();
            ship.Draw();  

         
            DrawSpaceShip(shipSpace, Matrix.CreateTranslation(spherePosition)); 

           
            for (int i = 0; i < bulletLists.Count; i++)
            {
                DrawBullet(bulletLists[i], Matrix.CreateTranslation(bulletPositions[i])); 
            }

            DrawAsteroid(asteroidModel, Matrix.CreateTranslation(asteroidPosition));

            
            foreach (var position in asteroidPositions)
            {
                DrawAsteroid(asteroidModel, Matrix.CreateTranslation(position));
            }

            
            _spriteBatch.Begin();
            foreach (var firePosition in firePositions)
            {
                _spriteBatch.Draw(fire, firePosition, Color.White);
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }

       
        private void DrawSpaceShip(Model model, Matrix worldMatrix)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    
                    effect.TextureEnabled = true; 
                    effect.Texture = spaceShipTexture;
                    effect.EnableDefaultLighting();
                    effect.World = worldMatrix;
                    effect.View = camera.View;
                    effect.Projection = camera.Projection;
                    effect.DiffuseColor = new Vector3(1f, 1f, 0f);  
                }

                mesh.Draw();
            }
        }

        private void DrawBullet(Model model, Matrix worldMatrix)
        {
            
            float scale = 0.6f; 
            Matrix scaleMatrix = Matrix.CreateScale(scale);

            
            worldMatrix = scaleMatrix * worldMatrix;

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.TextureEnabled = true; 
                    effect.Texture = myfireTexture; 
                    effect.EnableDefaultLighting(); 
                    effect.World = worldMatrix; 
                    effect.View = camera.View;
                    effect.Projection = camera.Projection;
                    effect.DiffuseColor = new Vector3(1f, 0f, 0f); 
                }
                mesh.Draw();
            }
        }

      
        private void DrawAsteroid(Model model, Matrix worldMatrix)
        {

            


            if (asteroidPosition.Y>spherePosition.Y)
            asteroidRotationAngle += 0.0f;

            else
            {
                asteroidRotationAngle += 0.01f;
            }

           
            Matrix rotationMatrix = Matrix.CreateRotationY(asteroidRotationAngle);

         
            Matrix finalWorldMatrix = rotationMatrix * worldMatrix;

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.TextureEnabled = true;
                    effect.Texture = astroidTexture;
                    effect.EnableDefaultLighting();
                    effect.World = finalWorldMatrix;  
                    effect.View = camera.View;
                    effect.Projection = camera.Projection;
                    effect.DiffuseColor = new Vector3(0.5f, 0.5f, 0.5f);  
                }
                mesh.Draw();
            }
        }
    }
}


