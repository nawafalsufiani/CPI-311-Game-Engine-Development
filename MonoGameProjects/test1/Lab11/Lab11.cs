#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CPI311.GameEngine;
using System.Collections.Generic;
#endregion

namespace CPI311.Labs
{
    public class Lab11 : Game
    {
        class Scene
        {
            public delegate void CallMethod();
            public CallMethod Update;
            public CallMethod Draw;
            public Scene(CallMethod update, CallMethod draw)
            { Update = update; Draw = draw; }
        }

        Dictionary<String, Scene> scenes;
        Scene currentScene;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;

        List<GUIElement> guiElements;

        public Lab11()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            guiElements = new List<GUIElement>();
            scenes = new Dictionary<string, Scene>();
        }

        protected override void Initialize()
        {
            Time.Initialize(); 
            InputManager.Initialize(); 
            ScreenManager.Initialize(graphics);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Arial");
            Texture2D texture = Content.Load<Texture2D>("Square");

            GUIGroup group = new GUIGroup();

            Button exitButton = new Button();
            exitButton.Texture = texture;
            exitButton.Bounds = new Rectangle(50, 50, 300, 20);
            exitButton.Action += ExitGame;
            exitButton.Text = "Exit";
            group.Children.Add(exitButton);

            CheckBox optionBox = new CheckBox();
            optionBox.Texture = texture;
            optionBox.Box = texture;
            optionBox.Bounds = new Rectangle(50, 75, 300, 20);
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
            Time.Update(gameTime); 
            InputManager.Update(); 
            currentScene.Update();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.DepthStencilState = new DepthStencilState();

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
