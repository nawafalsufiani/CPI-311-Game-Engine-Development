﻿using Microsoft.Xna.Framework;
using System.ComponentModel;

namespace CPI311.GameEngine
{
    public class Collider : Component
    {
        public virtual bool Collides(Collider other, out Vector3 normal)
        {
            normal = Vector3.Zero;
            return false;
        }


        // Lab08 Update
        public virtual float? Intersects(Ray ray)
        {
            return null;
        }
    }
}
