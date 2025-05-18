using CPI311.GameEngine;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CPI311.Labs
{
    public class Bullet : GameObject
    {
        public bool isActive =false;

        public Bullet(ContentManager content, Camera camera, GraphicsDevice graphicsDevice, Light light)
            : base()
        {
            // Rigidbody component
            Rigidbody rigidbody = new Rigidbody();
            rigidbody.Transform = Transform;
            rigidbody.Mass = 1;
            Add<Rigidbody>(rigidbody);

            // Renderer component
            Texture2D texture = content.Load<Texture2D>("BulletTexture");
            Renderer renderer = new Renderer(content.Load<Model>("bullet"),
                Transform, camera, content, graphicsDevice, light, 1, null, 20f, texture);
            Add<Renderer>(renderer);

            // Collider component
            SphereCollider sphereCollider = new SphereCollider();
            sphereCollider.Radius = renderer.ObjectModel.Meshes[0].BoundingSphere.Radius;
            sphereCollider.Transform = Transform;
            Add<Collider>(sphereCollider);

            isActive = false;
        }

        public override void Update()
        {
            if (!isActive) return;

            // Wrap around playfield if out of bounds
            if (Transform.Position.X > GameConstants.PlayfieldSizeX)
                Transform.Position -= Vector3.UnitX * 2 * GameConstants.PlayfieldSizeX;
            if (Transform.Position.X < -GameConstants.PlayfieldSizeX)
                Transform.Position += Vector3.UnitX * 2 * GameConstants.PlayfieldSizeX;
            if (Transform.Position.Z > GameConstants.PlayfieldSizeY)
                Transform.Position -= Vector3.UnitZ * 2 * GameConstants.PlayfieldSizeY;
            if (Transform.Position.Z < -GameConstants.PlayfieldSizeY)
                Transform.Position += Vector3.UnitZ * 2 * GameConstants.PlayfieldSizeY;

            base.Update();
        }




    }
}
