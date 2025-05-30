﻿using System;
using Microsoft.Xna.Framework;

namespace CPI311.GameEngine
{
    public class SphereCollider : Collider
    {
        public float Radius { get; set; }

        public override bool Collides(Collider other, out Vector3 normal)
        {
            if (other is SphereCollider)
            {
                SphereCollider collider = other as SphereCollider;
                if ((Transform.Position - collider.Transform.Position).LengthSquared() <
                    System.Math.Pow(Radius + collider.Radius, 2))
                {
                    System.Console.WriteLine("Collided");
                    normal = Vector3.Normalize(Transform.Position - collider.Transform.Position);
                    return true;
                }
            }
            else if (other is BoxCollider)
                return other.Collides(this, out normal);
            return base.Collides(other, out normal);
        }

        // Lab08

        public override float? Intersects(Ray ray)
        {
            Matrix worldInverted = Matrix.Invert(Transform.World);
            ray.Position = Vector3.Transform(ray.Position, worldInverted);
            ray.Direction = Vector3.Normalize(Vector3.TransformNormal(ray.Direction, worldInverted));
            BoundingSphere sphere = new BoundingSphere(Vector3.Zero, Radius);
            return sphere.Intersects(ray);
        }


    }
}
