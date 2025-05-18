using CPI311.GameEngine;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System;

namespace CPI311.Labs
{
    public class Ship : GameObject
    {
        bool isActive = true;
        List<Bullet> bulletList; // List to hold bullets

        public Ship(ContentManager content, Camera camera, GraphicsDevice graphicsDevice, Light light, List<Bullet> bullets) : base()
        {
            bulletList = bullets;

            // Rigidbody setup
            Rigidbody rigidbody = new Rigidbody();
            rigidbody.Transform = Transform;
            Add<Rigidbody>(rigidbody);

            // Renderer setup
            Model shipModel = content.Load<Model>("ship");
            if (shipModel == null)
            {
                Console.WriteLine("Failed to load ship model");
            }
            Renderer renderer = new Renderer(shipModel, Transform, camera, content, graphicsDevice, light, 1, null, 20f, null);
            Add<Renderer>(renderer);

            // Collider setup
            SphereCollider collider = new SphereCollider();
            collider.Transform = Transform;
            Add<Collider>(collider);
        }

        public override void Update()
        {
            if (!isActive) return;

            // Movement controls
            if (InputManager.IsKeyDown(Keys.W))
                Transform.LocalPosition += Transform.Forward * Time.ElapsedGameTime * GameConstants.ShipSpeed;
            if (InputManager.IsKeyDown(Keys.A))
                Transform.LocalPosition += Transform.Left * Time.ElapsedGameTime * GameConstants.ShipSpeed;
            if (InputManager.IsKeyDown(Keys.S))
                Transform.LocalPosition += Transform.Backward * Time.ElapsedGameTime * GameConstants.ShipSpeed;
            if (InputManager.IsKeyDown(Keys.D))
                Transform.LocalPosition += Transform.Right * Time.ElapsedGameTime * GameConstants.ShipSpeed;

            // Shooting logic
            if (InputManager.IsMousePressed(0)) // Check if the left mouse button is pressed
            {
                foreach (var bullet in bulletList)
                {
                    if (!bullet.isActive) // Find an inactive bullet to reuse
                    {
                        bullet.Rigidbody.Velocity = Transform.Forward * GameConstants.BulletSpeed;
                        bullet.Transform.Position = Transform.Position;
                        bullet.isActive = true; // Activate the bullet
                        break;
                    }
                }
            }

            base.Update();
        }

        public override void Draw()
        {
            // Add code for custom drawing if needed, but normally the Renderer takes care of it.
        }
    }
}
