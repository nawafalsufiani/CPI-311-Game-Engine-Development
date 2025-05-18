using CPI311.GameEngine;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CPI311.Labs
{


    public static class GameConstants
    {
        public const float PlayfieldSizeX = 50f;
        public const float PlayfieldSizeY = 50f;
        public const float ShipSpeed = 10f;
        public const float BulletSpeed = 15f;
        public const float AsteroidSpeed = 5f;

        public static int NumBullets { get; internal set; }
        public static int NumAsteroids { get; internal set; }
        // Other constants can be added as needed
    }


}
