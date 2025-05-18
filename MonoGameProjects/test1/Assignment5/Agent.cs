using CPI311.GameEngine;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System;
using CPI311.Labs;

namespace CPI311.Labs
{
    public class Agent : GameObject
    {
        public AStarSearch search;
        List<Vector3> path;
        private float speed = 5f; //moving speed
        private int gridSize = 20; //grid size
        private TerrainRenderer Terrain;
        private Player player;

        public Agent(TerrainRenderer terrain, ContentManager Content,
            Camera camera, GraphicsDevice graphicsDevice, Light light, Player theplayer) : base()
        {
            Terrain = terrain;
            player = theplayer;
            path = null;

            // *** Copied from Player *****
            Rigidbody rigidbody = new Rigidbody();
            rigidbody.Transform = Transform;
            rigidbody.Mass = 1;
            Add<Rigidbody>(rigidbody);

            SphereCollider sphereCollider = new SphereCollider();
            sphereCollider.Radius = 1f;
            sphereCollider.Transform = Transform;
            Add<SphereCollider>(sphereCollider);

            Texture2D texture = Content.Load<Texture2D>("Square");
            Renderer renderer = new Renderer(
                Content.Load<Model>("Torus"), Transform, camera, Content, graphicsDevice, light,
                1, "SimpleShading", 20f, texture);
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
        } // end of constructor

        public override void Update()
        {
            // Collision detection: Check if the prize collides with the player
            if (IsCollidingWithPlayer())
            {
                Assignment5.caughtAliens++;
                Assignment5.timeSpent = 0f;
                JumpToRandomPosition();
            }

            if (path != null && path.Count > 0)
            {
                // Move to the destination along the path
                Vector3 currP = Transform.Position;
                Vector3 destP = GetGridPosition(path[0]);
                currP.Y = 0;
                destP.Y = 0;
                Vector3 direction =
                    (Vector3.Distance(destP, currP) == 0 ? Vector3.Zero : Vector3.Normalize(destP - currP));

                this.Rigidbody.Velocity = new Vector3(direction.X, 0, direction.Z) * speed;

                if (Vector3.Distance(currP, destP) < 1f) // if it reaches a point, go to the next in path
                {
                    path.RemoveAt(0);
                    if (path.Count == 0) // if it reached the goal
                    {
                        path = null;
                        return;
                    }
                }
            }
            else
            {
                // Follow the player
                FollowPlayer();
            }

            this.Transform.LocalPosition = new Vector3(
               this.Transform.LocalPosition.X,
               Terrain.GetAltitude(this.Transform.LocalPosition),
               this.Transform.LocalPosition.Z) + Vector3.Up;

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

        private Vector3 GetGridPosition(Vector3 gridPos)
        {
            float gridW = Terrain.size.X / search.Cols;
            float gridH = Terrain.size.Y / search.Rows;
            return new Vector3(
                gridW * gridPos.X + gridW / 2 - Terrain.size.X / 2,
                0,
                gridH * gridPos.Z + gridH / 2 - Terrain.size.Y / 2);
        }

        private void FollowPlayer()
        {
            Vector3 playerPos = player.Transform.Position;
            Vector3 agentPos = this.Transform.Position;

            // Convert positions to grid space
            Vector3 agentGridPos = new Vector3(
                (int)((agentPos.X + Terrain.size.X / 2) / (Terrain.size.X / gridSize)),
                0,
                (int)((agentPos.Z + Terrain.size.Y / 2) / (Terrain.size.Y / gridSize)));

            Vector3 playerGridPos = new Vector3(
                (int)((playerPos.X + Terrain.size.X / 2) / (Terrain.size.X / gridSize)),
                0,
                (int)((playerPos.Z + Terrain.size.Y / 2) / (Terrain.size.Y / gridSize)));

            if (search.Nodes[(int)playerGridPos.Z, (int)playerGridPos.X].Passable)
            {
                search.Start = search.Nodes[(int)agentGridPos.Z, (int)agentGridPos.X];
                search.End = search.Nodes[(int)playerGridPos.Z, (int)playerGridPos.X];
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
}
