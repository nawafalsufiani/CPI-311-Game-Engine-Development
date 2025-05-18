using CPI311.GameEngine;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System;
using SharpDX.Direct2D1;
using Microsoft.Xna.Framework.Audio;

namespace CPI311.Labs
{
    public class Player : GameObject
    {
        public TerrainRenderer Terrain { get; set; }

        public static int cheker = 0;

        public Player(TerrainRenderer terrain, ContentManager Content, Camera camera,
            GraphicsDevice graphicsDevice, Light light) : base()
        {
            Terrain = terrain;
        cheker = 1;

        Rigidbody rigidbody = new Rigidbody();
            rigidbody.Transform = Transform;
            rigidbody.Mass = 1;
            Add<Rigidbody>(rigidbody);

            SphereCollider sphereCollider = new SphereCollider();
            sphereCollider.Radius = 1f;
            sphereCollider.Transform = Transform;
            Add<SphereCollider>(sphereCollider);

            Texture2D texture = Content.Load<Texture2D>("yellowTexture");
            Renderer renderer = new Renderer(Content.Load<Model>("Sphere"), Transform, camera, Content, graphicsDevice, light, 1, "SimpleShading", 20f, texture);

           // Color.Yellow.ToVector3();

            renderer.Material.Diffuse = Color.Yellow.ToVector3();
            renderer.Material.Specular = Color.Yellow.ToVector3();
            renderer.Material.Ambienet = Color.Yellow.ToVector3();
            
            // renderer.Material.Diffuse = new Vector3();


            Add<Renderer>(renderer);
            SoundEffect beginningTheme, normalGhostMoviment;
            SoundEffectInstance soundInstance;
            beginningTheme = Content.Load<SoundEffect>("pacman_beginning");
            
             soundInstance = beginningTheme.CreateInstance();
            soundInstance.IsLooped = false;

            soundInstance.Play();


            normalGhostMoviment = Content.Load<SoundEffect>("Ghost： Normal Move");



            SoundEffectInstance soundInstance2 = normalGhostMoviment.CreateInstance();
            soundInstance2.IsLooped = true;
            //if(cheker==0)
            soundInstance2.Play();

        }

        public override void Update()
        {
            Vector3 newPosition = this.Transform.LocalPosition;

            // Control the player movement with key inputs
            if (InputManager.IsKeyDown(Keys.W) && FinalProject.pauseGame != true) // Move forward
                newPosition += this.Transform.Forward * Time.ElapsedGameTime * 20f;
            if (InputManager.IsKeyDown(Keys.S) && FinalProject.pauseGame != true) // Move backward
                newPosition += this.Transform.Backward * Time.ElapsedGameTime * 20f;
            if (InputManager.IsKeyDown(Keys.A) && FinalProject.pauseGame != true) // Move left
                newPosition += this.Transform.Left * Time.ElapsedGameTime * 20f;
            if (InputManager.IsKeyDown(Keys.D) && FinalProject.pauseGame != true) // Move right
                newPosition += this.Transform.Right * Time.ElapsedGameTime * 20f;

            // Check if the new position is passable (no wall)
            if (IsPassable(newPosition))
            {
                // Update the player's position if it's valid (passable)
                this.Transform.LocalPosition = new Vector3(
                    newPosition.X,
                    Terrain.GetAltitude(newPosition), // Adjust Y position based on terrain
                    newPosition.Z) + Vector3.Up; // Keep player above the ground
            }

            // Rotate player when pressing 'A' or 'D'
            //float rotationSpeed = 1f; // Adjust rotation speed as needed
            //if (InputManager.IsKeyDown(Keys.A)) // Rotate left
            //    this.Transform.Rotate(Vector3.Up, rotationSpeed * Time.ElapsedGameTime); // Rotate around the Y axis (vertical axis)
            //if (InputManager.IsKeyDown(Keys.D)) // Rotate right
            //    this.Transform.Rotate(Vector3.Up, -rotationSpeed * Time.ElapsedGameTime); // Rotate around the Y axis (vertical axis)

            base.Update();
        }

        // Check if the position is passable (not a wall or obstacle)
        private bool IsPassable(Vector3 position)
        {
            // Get the altitude of the terrain at the given position
            float altitude = Terrain.GetAltitude(position);

            // If the altitude is greater than a threshold (e.g., 1.0f), it's considered a wall
            return altitude <= 1.0f; // Return true if the position is passable, false if it's blocked
        }
    }
}
