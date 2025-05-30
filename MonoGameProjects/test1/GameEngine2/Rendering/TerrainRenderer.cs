﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CPI311.GameEngine
{
    public class TerrainRenderer : Component
    {
        private VertexPositionTexture[] Vertices { get; set; }
        private int[] Indices { get; set; }

        public Texture2D HeightMap { get; set; }
        public Texture2D NormalMap { get; set; }

        public Vector2 size; //size of Terrain

        public float[] Heights { get; set; } // list of heights


        // *** Constructor
        public TerrainRenderer(Texture2D texture, Vector2 size, Vector2 res)
        {

            HeightMap = texture;
            this.size = size;

            CreateHeights(); //  Heights[] data is crated 

            // We should also save the value of size somewhere
            int rows = (int)res.Y + 1;
            int cols = (int)res.X + 1;

            Vector3 offset = new Vector3(-size.X / 2, 0, -size.Y / 2);
            float stepX = size.X / res.X;
            float stepZ = size.Y / res.Y;
            Vertices = new VertexPositionTexture[rows * cols];
            for (int r = 0; r < rows; r++)
                for (int c = 0; c < cols; c++)
                    Vertices[r * cols + c] = new VertexPositionTexture(
                         // ***OLD*** offset + new Vector3(c * stepX, 0, r * stepZ),
                         offset + new Vector3(
                             c * stepX,
                             GetHeight(new Vector2(c / res.X, r / res.Y)),
                             r * stepZ),
                        new Vector2(c / res.X, r / res.Y));

            Indices = new int[(rows - 1) * (cols - 1) * 6];
            int index = 0;
            for (int r = 0; r < rows - 1; r++)
                for (int c = 0; c < cols - 1; c++)
                {
                    Indices[index++] = r * cols + c;
                    Indices[index++] = r * cols + c + 1;
                    Indices[index++] = (r + 1) * cols + c;

                    Indices[index++] = (r + 1) * cols + c;
                    Indices[index++] = r * cols + c + 1;
                    Indices[index++] = (r + 1) * cols + c + 1;
                }
        }

        public float GetHeight(Vector2 tex)
        {
            // First, scale it to dimensions of the image
            tex = Vector2.Clamp(tex, Vector2.Zero, Vector2.One) *
                    new Vector2(HeightMap.Width - 1, HeightMap.Height - 1);
            int x = (int)tex.X;
            float u = tex.X - x;
            int y = (int)tex.Y;
            float v = tex.Y - y;
            return Heights[y * HeightMap.Width + x] * (1 - u) * (1 - v) +
                Heights[y * HeightMap.Width + Math.Min((x + 1), HeightMap.Width - 1)] * u * (1 - v) +
                Heights[Math.Min((y + 1), HeightMap.Height - 1) * HeightMap.Width + x] * (1 - u) * v +
                Heights[Math.Min((y + 1), HeightMap.Height - 1) * HeightMap.Width + Math.Min((x + 1), HeightMap.Width - 1)] * u * v;
        }
        private void CreateHeights()
        {
            Color[] data = new Color[HeightMap.Width * HeightMap.Height];
            HeightMap.GetData<Color>(data); // Color Image --> Color[]: Color (255, 255, 255)

            Heights = new float[HeightMap.Width * HeightMap.Height];
            for (int i = 0; i < Heights.Length; i++)
                Heights[i] = data[i].G / 255f;
        }

        // From (x,y,z) pos in the world to get the Height.
        public float GetAltitude(Vector3 position)
        {
            position = Vector3.Transform(position, Matrix.Invert(Transform.World));
            if (position.X > -size.X / 2 && position.X < size.X / 2 &&
                              position.Z > -size.Y / 2 && position.Z < size.Y / 2)
                return GetHeight(new Vector2(
                    (position.X + size.X / 2) / size.X,
                    (position.Z + size.Y / 2) / size.Y)) * Transform.LocalScale.Y;
            return -1;
        }

        public void Draw()
        {
            // Setup custom shader etc.
            ScreenManager.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionTexture>(
                PrimitiveType.TriangleList,
                Vertices, 0, Vertices.Length,
                Indices, 0, Indices.Length / 3);
        }


    }
}
