﻿using CPI311.GameEngine;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Microsoft.Xna.Framework;





namespace CPI311.Labs
{
    public class Particle
    {
        public Vector3 Position { get; set; }
        public Vector3 Velocity { get; set; }
        public Vector3 Acceleration { get; set; }
        public float Age { get; set; }
        public float MaxAge { get; set; }
        public Vector3 Color { get; set; }
        public float Size { get; set; }
        public float SizeVelocity { get; set; }
        public float SizeAcceleration { get; set; }
        public Particle() { Age = -1; }
        public bool Update()
        {
            if (Age < 0) return false;
            Velocity += Acceleration * Time.ElapsedGameTime;
            Position += Velocity * Time.ElapsedGameTime;
            SizeVelocity += SizeAcceleration * Time.ElapsedGameTime;
            Size += SizeVelocity * Time.ElapsedGameTime;
            Age += Time.ElapsedGameTime;
            if (Age > MaxAge)
            {
                Age = -1;
                return false;
            }
            return true;
        }
        public bool IsActive() { return Age < 0 ? false : true; }
        public void Activate() { Age = 0; }
        public void Init()
        {
            Age = 0; Size = 1; SizeVelocity = SizeAcceleration = 0;
        }
    }
}


