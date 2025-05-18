using CPI311.GameEngine;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Audio;

namespace CPI311.Labs
{
    public class Fruit : GameObject
    {
        public AStarSearch search;
        List<Vector3> path;
        private float speed = 5f; //moving speed
        private int gridSize = 20; //grid size
        private TerrainRenderer Terrain;
        private Player player;
        // Audio components
        SoundEffect pacManEat;
        SoundEffectInstance soundInstance;
        public Fruit(TerrainRenderer terrain, ContentManager Content,
            Camera camera, GraphicsDevice graphicsDevice, Light light, Player theplayer) : base()
        {
            Terrain = terrain;
            player = theplayer;
            path = null;

            // *** Copied from Player *****m
            Rigidbody rigidbody = new Rigidbody();
            rigidbody.Transform = Transform;
            rigidbody.Mass = 1;
            Add<Rigidbody>(rigidbody);

            SphereCollider sphereCollider = new SphereCollider();
            sphereCollider.Radius = 1f;
            sphereCollider.Transform = Transform;
            Add<SphereCollider>(sphereCollider);

            Texture2D texture = Content.Load<Texture2D>("Square");
            pacManEat = Content.Load<SoundEffect>("pacman_eatfruit");
            Renderer renderer = new Renderer(
                Content.Load<Model>("Sphere"), Transform, camera, Content, graphicsDevice, light,
                1, "SimpleShading", 20f, texture);

            renderer.Material.Diffuse = Color.Pink.ToVector3();
            renderer.Material.Specular = Color.Pink.ToVector3();
            renderer.Material.Ambienet = Color.Pink.ToVector3();

            Add<Renderer>(renderer);
            // *****************************

            search = new AStarSearch(gridSize, gridSize);
            float gridW = Terrain.size.X / gridSize;
            float gridH = Terrain.size.Y / gridSize;

            for (int i = 0; i < gridSize; i++)
                for (int j = 0; j < gridSize; j++)
                {
                    Vector3 pos = new Vector3(
                        gridW * i + gridW / 2 - terrain.size.X / 2,
                        0,
                        gridH * j + gridH / 2 - terrain.size.Y / 2);
                    if (Terrain.GetAltitude(pos) > 1.0)
                        search.Nodes[j, i].Passable = false;
                }

            JumpToRandomPosition();
        } // end of constructor

        public override void Update()
        {
            // Collision detection: Check if the prize collides with the player
            if (IsCollidingWithPlayer())
            {
                FinalProject.currentScore = FinalProject.currentScore + 60;
               // FinalProject.dotCounter++;
                DestroyObject();
                SoundEffectInstance soundInstance = pacManEat.CreateInstance();
                soundInstance.IsLooped = false;
                soundInstance.Play();
            }

            // Make sure Prize stays in its current position and doesn't move
            this.Transform.LocalPosition = new Vector3(
                this.Transform.LocalPosition.X,
                Terrain.GetAltitude(this.Transform.LocalPosition),
                this.Transform.LocalPosition.Z) + Vector3.Up;

            // Update Transform (if required for other purposes)
            Transform.Update();

            base.Update();
        }

        // *********************************
        // ***  Private Methods ************
        // *********************************

        private bool IsCollidingWithPlayer()
        {
            // Calculate the distance between the prize and the player
            float distance = Vector3.Distance(player.Transform.Position, this.Transform.Position);
            return distance < 2f; // Adjust the threshold if necessary (e.g., if radius is 1)
        }

        private void JumpToRandomPosition()
        {
            Random random = new Random();
            int randomX, randomZ;
            bool foundValidPosition = false;
            while (!foundValidPosition)
            {
                // Generate random position within grid bounds
                randomX = random.Next(0, gridSize);
                randomZ = random.Next(0, gridSize);

                // Check if the generated position is passable (no walls)
                Vector3 randomPosition = GetGridPosition(new Vector3(randomX, 0, randomZ));
                if (Terrain.GetAltitude(randomPosition) <= 1.0f) // Passable terrain check
                {
                    this.Transform.LocalPosition = randomPosition + Vector3.Up; // Keep it above the ground
                    foundValidPosition = true;
                }
            }
        }

        private void DestroyObject()
        {
            Random random = new Random();
            int randomX, randomZ;
            bool foundValidPosition = false;
            while (!foundValidPosition)
            {
                // Generate random position within grid bounds
                randomX = random.Next(0, gridSize);
                randomZ = random.Next(0, gridSize);

                // Check if the generated position is passable (no walls)
                Vector3 randomPosition = GetGridPosition(new Vector3(1000, 0, 1000));
                if (Terrain.GetAltitude(randomPosition) <= 1.0f) // Passable terrain check
                {
                    this.Transform.LocalPosition = randomPosition + Vector3.Up; // Keep it above the ground
                    foundValidPosition = true;
                }
            }
        }


        private Vector3 GetGridPosition(Vector3 gridPos)
        {
            float gridW = Terrain.size.X / search.Cols;
            float gridH = Terrain.size.Y / search.Rows;
            return new Vector3(
                gridW * gridPos.X + gridW / 2 - Terrain.size.X / 2,
                0,
                gridH * gridPos.Z + gridH / 2 - Terrain.size.Y / 2);
        }

        private void RandomPathFinding()
        {
            Random random = new Random();
            while (!(search.Start = search.Nodes[random.Next(search.Rows), random.Next(search.Cols)]).Passable) ;
            search.End = search.Nodes[search.Rows / 2, search.Cols / 2]; // Make sure that the center is passable
            search.Search();
            path = new List<Vector3>();
            AStarNode current = search.End;
            while (current != null)
            {
                path.Insert(0, current.Position);
                current = current.Parent;
            }
        }
    }
}
