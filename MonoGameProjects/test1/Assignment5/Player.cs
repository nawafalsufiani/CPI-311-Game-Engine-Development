using CPI311.GameEngine;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System;

namespace CPI311.Labs
{
    public class Player : GameObject
    {
        public TerrainRenderer Terrain { get; set; }

        public Player(TerrainRenderer terrain, ContentManager Content, Camera camera,
            GraphicsDevice graphicsDevice, Light light) : base()
        {
            Terrain = terrain;

            Rigidbody rigidbody = new Rigidbody();
            rigidbody.Transform = Transform;
            rigidbody.Mass = 1;
            Add<Rigidbody>(rigidbody);

            SphereCollider sphereCollider = new SphereCollider();
            sphereCollider.Radius = 1f;
            sphereCollider.Transform = Transform;
            Add<SphereCollider>(sphereCollider);

            Texture2D texture = Content.Load<Texture2D>("player");
            Renderer renderer = new Renderer( Content.Load<Model>("sun"), Transform, camera, Content, graphicsDevice, light, 1, "SimpleShading", 20f, texture);
            
            Add<Renderer>(renderer);

        }

        public override void Update()
        {
            Vector3 newPosition = this.Transform.LocalPosition;

            // Control the player movement with key inputs
            if (InputManager.IsKeyDown(Keys.W)) // Move forward
                newPosition += this.Transform.Forward * Time.ElapsedGameTime * 10f;
            if (InputManager.IsKeyDown(Keys.S)) // Move backward
                newPosition += this.Transform.Backward * Time.ElapsedGameTime * 10f;
            if (InputManager.IsKeyDown(Keys.A)) // Move left
                newPosition += this.Transform.Left * Time.ElapsedGameTime * 10f;
            if (InputManager.IsKeyDown(Keys.D)) // Move right
                newPosition += this.Transform.Right * Time.ElapsedGameTime * 10f;

            // Check if the new position is passable (no wall)
            if (IsPassable(newPosition))
            {
                // Update the player's position if it's valid (passable)
                this.Transform.LocalPosition = new Vector3(
                    newPosition.X,
                    Terrain.GetAltitude(newPosition), // Adjust Y position based on terrain
                    newPosition.Z) + Vector3.Up; // Keep player above the ground
            }

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
