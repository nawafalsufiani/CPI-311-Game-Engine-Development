#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CPI311.GameEngine;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
#endregion

namespace CPI311.Labs
{

    public class FinalProject : Game
    {
        class Scene
        {
            public delegate void CallMethod();
            public CallMethod Update;
            public CallMethod Draw;
            public Scene(CallMethod update, CallMethod draw)
            { Update = update; Draw = draw; }
        }

        bool gamestarted=false;
        

        // Position for the logo image (you can adjust these values)
        Vector2 logoPosition = new Vector2(100, 100);
        public static bool pauseGame = false;
        public static bool pacmanDeath = false;
        Dictionary<String, Scene> scenes;
        Scene currentScene;
        PowerUp powerUp1, powerUp2, powerUp3, powerUp4;
        Fruit fruit;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;

        List<GUIElement> guiElements;

        SoundEffect smb_pause;
        SoundEffectInstance soundInstance;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private int x, y, z;
        private int xLight, yLight, zLight;
        // *** Lab 10
        TerrainRenderer terrain;
        Camera camera;
        Light light, light2, light3, light4;
        Effect effect;

        int cameraAngle = 0;
        public static int dotCounter = 0;
        public static int MaxDot = 50;
        Player player;
        Ghost agent1, agent2, agent3;
        Dot prize1, prize2, prize3, prize4, prize5, prize6, prize7, prize8, prize9, prize10, prize11, prize12, prize13, prize14, prize15, prize16, prize17, prize18, prize19, prize20, prize21, prize22, prize23, prize24, prize25, prize26, prize27, prize28, prize29, prize30, prize31, prize32, prize33, prize34, prize35, prize36, prize37, prize38, prize39, prize40, prize41, prize42, prize43, prize44, prize45, prize46, prize47, prize48, prize49, prize50;

        Texture2D pacmanLogo;
        public static int caughtAliens = 0; // Tracks how many aliens the player caught
        public static float timeSpent = 0f;  // Tracks the time spent in the game
        public static float powerUpTime = 0f;
        public static int currentScore = 0;
        public int highscore = 0;
        // Variables for tracking caught aliens and time spent
        Vector2 logoSize = new Vector2(200, 200);
        // Font for displaying text


        public FinalProject()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            //**********************************************************
            _graphics.GraphicsProfile = GraphicsProfile.HiDef; // ShadeDX -> Error
            //**********************************************************
            
            guiElements = new List<GUIElement>();
            scenes = new Dictionary<string, Scene>();
            x = 0; y = 50; z = 0;   
        }

        protected override void Initialize()
        {
            Time.Initialize();
            InputManager.Initialize();
            ScreenManager.Initialize(_graphics);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            terrain = new TerrainRenderer(
                Content.Load<Texture2D>("mazeH2"), Vector2.One * 100, Vector2.One * 200);
            terrain.NormalMap = Content.Load<Texture2D>("mazeN2");
            terrain.Transform = new Transform();
            terrain.Transform.LocalScale *= new Vector3(1, 10, 1);
            effect = Content.Load<Effect>("TerrainShader");
            effect.Parameters["AmbientColor"].SetValue(new Vector3(0.1f, 0.1f, 0.1f));
            effect.Parameters["DiffuseColor"].SetValue(new Vector3(0.3f, 0.5f, 0.9f));
            effect.Parameters["SpecularColor"].SetValue(new Vector3(0f, 0f, 0.2f));
            effect.Parameters["Shininess"].SetValue(20f);

             pacmanLogo = Content.Load<Texture2D>("pacmanLogo");
            //effect.Parameters["AmbientColor"].SetValue(new Vector3(0.2f, 0.4f, 0.8f));  // Light blue ambient light
            //effect.Parameters["DiffuseColor"].SetValue(new Vector3(0.3f, 0.5f, 0.9f));  // Light blue diffuse light
            //effect.Parameters["SpecularColor"].SetValue(new Vector3(0.2f, 0.4f, 1.0f)); // Bright blue specular highlight
            smb_pause = Content.Load<SoundEffect>("smb_pause");

            camera = new Camera();
            camera.Transform = new Transform();
            camera.Transform.LocalPosition = Vector3.Up * 60;
            
            camera.Transform.Rotate(Vector3.Right, -MathHelper.PiOver2);

            


            // Set up a directional light (spreads light across the entire scene)
            light = new Light();
            light.Transform = new Transform();
            light.Transform.LocalPosition = new Vector3(50, 50, 50); // No position for directional light
            light.Ambient = Color.Red; light.Diffuse=Color.Red;
           // light.Transform.Rotate(new Vector3(40, 50, 50), 0);
            light.Transform.Rotate(new Vector3(0, 90, 0),10);
            player = new Player(terrain, Content, camera, GraphicsDevice, light);
            
            agent1 = new Ghost(terrain, Content, camera, GraphicsDevice, light, player);
            agent2 = new Ghost(terrain, Content, camera, GraphicsDevice, light, player);
            agent3 = new Ghost(terrain, Content, camera, GraphicsDevice, light, player);

            fruit = new Fruit(terrain, Content, camera, GraphicsDevice, light, player);

            powerUp1 = new PowerUp(terrain, Content, camera, GraphicsDevice, light, player);
            powerUp2 = new PowerUp(terrain, Content, camera, GraphicsDevice, light, player);
            powerUp3 = new PowerUp(terrain, Content, camera, GraphicsDevice, light, player);
            powerUp4 = new PowerUp(terrain, Content, camera, GraphicsDevice, light, player);

            prize1 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
            prize2 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
            prize3 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
            prize4 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
            prize5 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
            prize6 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
            prize7 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
            prize8 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
            prize9 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
            prize10 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
            prize11 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
            prize12 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
            prize13 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
            prize14 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
            prize15 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
            prize16 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
            prize17 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
            prize18 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
            prize19 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
            prize20 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
            prize21 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
            prize22 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
            prize23 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
            prize24 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
            prize25 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
            prize26 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
            prize27 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
            prize28 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
            prize29 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
            prize30 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
            prize31 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
            prize32 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
            prize33 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
            prize34 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
            prize35 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
            prize36 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
            prize37 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
            prize38 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
            prize39 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
            prize40 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
            prize41 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
            prize42 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
            prize43 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
            prize44 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
            prize45 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
            prize46 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
            prize47 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
            prize48 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
            prize49 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
            prize50 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);


            player.Transform.LocalPosition = new Vector3(-39, 1, 42);
            agent1.Transform.LocalPosition = new Vector3(-2.6f, 1, 2.6f);
            agent2.Transform.LocalPosition = new Vector3(-41, 1, -10);
            agent3.Transform.LocalPosition = new Vector3(41, 1, -40);
            powerUp1.Transform.LocalPosition = new Vector3(-30, 1, -43);
            powerUp2.Transform.LocalPosition = new Vector3(-30, 1, 43);
            powerUp3.Transform.LocalPosition = new Vector3(41, 1, 43);
            powerUp4.Transform.LocalPosition = new Vector3(42, 1, -43);

            fruit.Transform.LocalPosition = new Vector3(100, 1, 1000);

            // Load font for displaying text
            font = Content.Load<SpriteFont>("Arial"); // Ensure you have "Arial" or another font in the content folder


            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Arial");
            Texture2D texture = Content.Load<Texture2D>("Square");

            GUIGroup group = new GUIGroup();

            Button exitButton = new Button();
            exitButton.Texture = texture;
            exitButton.Bounds = new Rectangle(20, 280, 100, 20);
            exitButton.Action += ExitGame;
            exitButton.Text = "Exit";
            group.Children.Add(exitButton);

            CheckBox optionBox = new CheckBox();
            optionBox.Texture = texture;
            optionBox.Box = texture;
            optionBox.Bounds = new Rectangle(20, 330, 100, 20);
            optionBox.Action += MakeFullScreen;
            optionBox.Text = "Full Screen";
            group.Children.Add(optionBox);

            guiElements.Add(group);

            scenes.Add("Menu", new Scene(MainMenuUpdate, MainMenuDraw));
            scenes.Add("Play", new Scene(PlayUpdate, PlayDraw));
            currentScene = scenes["Menu"];


        }

        protected override void Update(GameTime gameTime)
        {
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Time.Update(gameTime);
            InputManager.Update();

            

            if (InputManager.IsKeyDown(Keys.Up)) // Move forward
                x++;
            if (InputManager.IsKeyDown(Keys.Down)) // Move backward
                x--;
            if (InputManager.IsKeyDown(Keys.Right)) // Move right
                z++;
            if (InputManager.IsKeyDown(Keys.Left)) // Move left
                z--;

            if (InputManager.IsKeyDown(Keys.N) && y>30) // Move downward
                y--;

            if (InputManager.IsKeyDown(Keys.M)&&y<60) // Move upward
                y++;

            if (pauseGame == true)
            {
                agent1.Transform.LocalPosition = new Vector3(0, 1, 2.6f);
                agent2.Transform.LocalPosition = new Vector3(0, 1, -10);
                agent3.Transform.LocalPosition = new Vector3(0, 1, -40);
            }

            if (InputManager.IsKeyPressed(Keys.Enter)) // Move upward
            {
                gamestarted = true;
                //Player.cheker = 0;
                SoundEffectInstance soundInstance = smb_pause.CreateInstance();
                soundInstance.IsLooped = false;
                soundInstance.Play();
                if (pauseGame == true)
                {
                    pauseGame = false;
                }
                else
                {
                    pauseGame = true;
                }
            }

            if (pacmanDeath==true)
            {
                pauseGame = true;
            }

            if (InputManager.IsKeyPressed(Keys.R)) // Move upward
            {
                //Player.cheker = 1;
                currentScore = 0;
                pauseGame = false;
                pacmanDeath = false;
                

                agent1 = new Ghost(terrain, Content, camera, GraphicsDevice, light, player);
                agent2 = new Ghost(terrain, Content, camera, GraphicsDevice, light, player);
                agent3 = new Ghost(terrain, Content, camera, GraphicsDevice, light, player);

                powerUp1 = new PowerUp(terrain, Content, camera, GraphicsDevice, light, player);
                powerUp2 = new PowerUp(terrain, Content, camera, GraphicsDevice, light, player);
                powerUp3 = new PowerUp(terrain, Content, camera, GraphicsDevice, light, player);
                powerUp4 = new PowerUp(terrain, Content, camera, GraphicsDevice, light, player);

                prize1 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
                prize2 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
                prize3 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
                prize4 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
                prize5 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
                prize6 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
                prize7 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
                prize8 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
                prize9 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
                prize10 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
                prize11 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
                prize12 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
                prize13 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
                prize14 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
                prize15 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
                prize16 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
                prize17 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
                prize18 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
                prize19 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
                prize20 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
                prize21 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
                prize22 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
                prize23 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
                prize24 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
                prize25 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
                prize26 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
                prize27 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
                prize28 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
                prize29 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
                prize30 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
                prize31 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
                prize32 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
                prize33 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
                prize34 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
                prize35 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
                prize36 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
                prize37 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
                prize38 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
                prize39 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
                prize40 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
                prize41 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
                prize42 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
                prize43 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
                prize44 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
                prize45 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
                prize46 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
                prize47 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
                prize48 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
                prize49 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);
                prize50 = new Dot(terrain, Content, camera, GraphicsDevice, light, player);


                player.Transform.LocalPosition = new Vector3(-39, 1, 42);
                agent1.Transform.LocalPosition = new Vector3(-2.6f, 1, 2.6f);
                agent2.Transform.LocalPosition = new Vector3(-41, 1, -10);
                agent3.Transform.LocalPosition = new Vector3(41, 1, -40);
                powerUp1.Transform.LocalPosition = new Vector3(-30, 1, -43);
                powerUp2.Transform.LocalPosition = new Vector3(-30, 1, 43);
                powerUp3.Transform.LocalPosition = new Vector3(41, 1, 43);
                powerUp4.Transform.LocalPosition = new Vector3(42, 1, -43);
                fruit.Transform.LocalPosition = new Vector3(100, 1, 1000);

                dotCounter = 0;

            }


            player.Update();
            
            agent1.Update();
            
            agent2.Update();
            
            agent3.Update();

            powerUp1.Update();
            powerUp2.Update();
            powerUp3.Update();
            powerUp4.Update();

            fruit.Update();
           

            prize1.Update();
            prize2.Update();
            prize3.Update();
            prize4.Update();
            prize5.Update();
            prize6.Update();
            prize7.Update();
            prize8.Update();
            prize9.Update();
            prize10.Update();
            prize11.Update();
            prize12.Update();
            prize13.Update();
            prize14.Update();
            prize15.Update();
            prize16.Update();
            prize17.Update();
            prize18.Update();
            prize19.Update();
            prize20.Update();
            prize21.Update();
            prize22.Update();
            prize23.Update();
            prize24.Update();
            prize25.Update();
            prize26.Update();
            prize27.Update();
            prize28.Update();
            prize29.Update();
            prize30.Update();
            prize31.Update();
            prize32.Update();
            prize33.Update();
            prize34.Update();
            prize35.Update();
            prize36.Update();
            prize37.Update();
            prize38.Update();
            prize39.Update();
            prize40.Update();
            prize41.Update();
            prize42.Update();
            prize43.Update();
            prize44.Update();
            prize45.Update();
            prize46.Update();
            prize47.Update();
            prize48.Update();
            prize49.Update();
            prize50.Update();

            currentScene.Update();
            // Track time spent
            if(PowerUp.PowerStatus==true)
            {
            timeSpent += (float)gameTime.ElapsedGameTime.TotalSeconds;
            powerUpTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            }
            
            if (powerUpTime >= 5)
            {
                timeSpent += (float)gameTime.ElapsedGameTime.TotalSeconds;
                powerUpTime =0;
                PowerUp.PowerStatus = false;
                fruit = new Fruit(terrain, Content, camera, GraphicsDevice, light, player);
            }


            // Update camera to follow the player
            Vector3 cameraOffset = new Vector3(x, y, z); // Adjust Y and Z offsets for desired distance
            camera.Transform.LocalPosition = player.Transform.Position + cameraOffset;

            // Update the light position to follow the player (you can adjust the offset if needed)
            // Update the light position to follow the player (you can adjust the offset if needed)
            light.Transform.LocalPosition = player.Transform.Position + new Vector3(0, 5, 0);  // Light is above the player

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Update effect parameters for lighting
            effect.Parameters["View"].SetValue(camera.View);
            effect.Parameters["Projection"].SetValue(camera.Projection);
            effect.Parameters["World"].SetValue(terrain.Transform.World);
            effect.Parameters["CameraPosition"].SetValue(camera.Transform.Position);
            effect.Parameters["NormalMap"].SetValue(terrain.NormalMap);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                if (gamestarted == true)
                {
                    terrain.Draw();



                    player.Draw();
                    agent1.Draw();

                    agent2.Draw();
                    agent3.Draw();

                    powerUp1.Draw();
                    powerUp2.Draw();
                    powerUp3.Draw();
                    powerUp4.Draw();

                    fruit.Draw();

                    prize1.Draw();
                    prize2.Draw();
                    prize3.Draw();
                    prize4.Draw();
                    prize5.Draw();
                    prize6.Draw();
                    prize7.Draw();
                    prize8.Draw();
                    prize9.Draw();
                    prize10.Draw();
                    prize11.Draw();
                    prize12.Draw();
                    prize13.Draw();
                    prize14.Draw();
                    prize15.Draw();
                    prize16.Draw();
                    prize17.Draw();
                    prize18.Draw();
                    prize19.Draw();
                    prize20.Draw();
                    prize21.Draw();
                    prize22.Draw();
                    prize23.Draw();
                    prize24.Draw();
                    prize25.Draw();
                    prize26.Draw();
                    prize27.Draw();
                    prize28.Draw();
                    prize29.Draw();
                    prize30.Draw();
                    prize31.Draw();
                    prize32.Draw();
                    prize33.Draw();
                    prize34.Draw();
                    prize35.Draw();
                    prize36.Draw();
                    prize37.Draw();
                    prize38.Draw();
                    prize39.Draw();
                    prize40.Draw();
                    prize41.Draw();
                    prize42.Draw();
                    prize43.Draw();
                    prize44.Draw();
                    prize45.Draw();
                    prize46.Draw();
                    prize47.Draw();
                    prize48.Draw();
                    prize49.Draw();
                    prize50.Draw();
                }

            }

            // Draw the caught aliens and time spent
            
            _spriteBatch.Begin();

            if (gamestarted == false)
            {
                pauseGame = true;

                Vector2 position = new Vector2(20, 250);
                float scale = 2.0f; // Adjust the scale factor for font size (1.0f is normal size, 2.0f is double the size, etc.)

                _spriteBatch.DrawString(font, " press Enter to Start the Game", position, Color.Black, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                _spriteBatch.Draw(pacmanLogo, logoPosition, Color.White);

            }

            if (gamestarted == true)
            {
                //  _spriteBatch.Draw(pacmanLogo, logoPosition, null, Color.White, 0f, Vector2.Zero, logoSize, SpriteEffects.None, 0f);
                //_spriteBatch.DrawString(font, "Remaining Power Up Time: " + powerUpTime.ToString("F2")+ " / 5.00", new Vector2(10, 60), Color.White);
                _spriteBatch.DrawString(font, "Dots : " + dotCounter + " / " + MaxDot, new Vector2(10, 10), Color.White);
                if (powerUpTime != 0)
                {


                    Vector2 position = new Vector2(50, 40);
                    float scale = 2.0f; // Adjust the scale factor for font size (1.0f is normal size, 2.0f is double the size, etc.)

                    _spriteBatch.DrawString(font, "Remaining Power Up Time: " + powerUpTime.ToString("F2") + " / 5.00", position, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);


                }


                //_spriteBatch.DrawString(font, " Player : X" + player.Transform.LocalPosition.X + " Player : Y" + player.Transform.LocalPosition.Y + " Player : Z" + player.Transform.LocalPosition.Z, new Vector2(10, 80), Color.White);
                _spriteBatch.DrawString(font, "Enter: Pause and Start / R: Restart", new Vector2(10, 80), Color.White);
                //_spriteBatch.DrawString(font, " X" + prize1.Transform.LocalPosition.X + " Y" + prize1.Transform.LocalPosition.Y + " Z" + prize1.Transform.LocalPosition.Z, new Vector2(10, 100), Color.White);
                _spriteBatch.DrawString(font, "Control : WASD ", new Vector2(10, 100), Color.White);

                _spriteBatch.DrawString(font, "Score : " + currentScore, new Vector2(10, 130), Color.White);

                _spriteBatch.DrawString(font, "Highest Score : " + highscore, new Vector2(10, 150), Color.White);

                _spriteBatch.DrawString(font, "N / M : Zoom In/Out", new Vector2(10, 180), Color.White);

                _spriteBatch.DrawString(font, "Eat the 50 Dots to win the Game \n Eat The Power up dots to eat ghost", new Vector2(10, 200), Color.White);


                if (pacmanDeath == true)
                {

                    Vector2 position = new Vector2(20, 250);
                    float scale = 2.0f; // Adjust the scale factor for font size (1.0f is normal size, 2.0f is double the size, etc.)

                    _spriteBatch.DrawString(font, "Game Over  press R to restart the Game", position, Color.Red, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

                }






                if (dotCounter == MaxDot)
                {
                    pauseGame = true;

                    Vector2 position = new Vector2(20, 250);
                    float scale = 2.0f; // Adjust the scale factor for font size (1.0f is normal size, 2.0f is double the size, etc.)

                    _spriteBatch.DrawString(font, " Game Complete press R to restart the Game", position, Color.Green, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

                    if (currentScore > highscore)
                    {
                        highscore = currentScore;

                    }
                }

            }




            _spriteBatch.End();
            currentScene.Draw();
            base.Draw(gameTime);
        }


        void ExitGame(GUIElement element)
        {
            Exit();
        }


        void MakeFullScreen(GUIElement element)
        {
            bool isFullScreen = !ScreenManager.IsFullScreen;
            ScreenManager.Setup(isFullScreen, ScreenManager.Width, ScreenManager.Height); // Toggle the fullscreen correctly
        }

        void MainMenuUpdate()
        {
            foreach (GUIElement element in guiElements)
                element.Update();
        }

        void MainMenuDraw()
        {
            spriteBatch.Begin();
            foreach (GUIElement element in guiElements)
                element.Draw(spriteBatch, font);
            spriteBatch.End();
        }

        void PlayUpdate()
        {
            if (InputManager.IsKeyReleased(Keys.Escape))
                currentScene = scenes["Menu"];
        }

        void PlayDraw()
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(font, "Play Mode! Press \"Esc\" to go back", Vector2.Zero, Color.Black);
            spriteBatch.End();
        }




    }
}
