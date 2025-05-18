#region Using Statements
using System;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using CPI311.GameEngine;
using Microsoft.Xna.Framework.Audio;
using System.IO;

#endregion

namespace CPI311.Labs
{
    public class Lab08 : Game
    {
        // initialize and control the presentation of the graphics device.
        GraphicsDeviceManager graphics;
        //Helper class for drawing text strings and sprites in one or more optimized batches.
        SpriteBatch spriteBatch;
        //Represents a font texture.
        SpriteFont font;
        //Used to set and query shader effects, and to choose techniques.
        Effect effect;
        //Represents a 2D grid of texels.
        Texture2D texture;
        //Represents a loaded sound resource.
         SoundEffect gunSound;

        Model cube;
        List<Transform> transforms;
        List<Collider> colliders;
        Camera camera, topDownCamera;
        List<Camera> cameras;

        public Lab08()
            : base()
        {   
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            transforms = new List<Transform>();
            colliders = new List<Collider>();
            cameras = new List<Camera>();
            
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Time.Initialize();
            InputManager.Initialize();
            ScreenManager.Initialize(graphics);
            base.Initialize();
        }


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Arial");

            texture = Content.Load<Texture2D>("Square");
            cube = Content.Load<Model>("Sphere");
            effect = Content.Load<Effect>("SimpleShading");
            
            gunSound = Content.Load<SoundEffect>("Gun");
            //gunSound = SoundEffect.FromStream(new FileStream("Gun.wav", FileMode.Open));
            
            (cube.Meshes[0].Effects[0] as BasicEffect).EnableDefaultLighting();

            Transform transform = new Transform();
            SphereCollider collider = new SphereCollider();
            collider.Radius = 1f;
            collider.Transform = transform;
            transforms.Add(transform);
            colliders.Add(collider);

            ScreenManager.Setup(true, 1920, 1080);
            // Camera Left
            camera = new Camera();
            camera.Transform = new Transform();
            camera.Transform.LocalPosition = Vector3.Backward * 5;
            camera.Position = new Vector2(0f, 0f);
            camera.Size = new Vector2(0.5f, 1f);
            camera.AspectRatio = camera.Viewport.AspectRatio;

            //Camera Right
            topDownCamera = new Camera();
            topDownCamera.Transform = new Transform();
            topDownCamera.Transform.LocalPosition = Vector3.Up * 10;
            topDownCamera.Transform.Rotate(Vector3.Right, -MathHelper.PiOver2);
            topDownCamera.Position = new Vector2(0.5f, 0f);
            topDownCamera.Size = new Vector2(0.5f, 01f);
            topDownCamera.AspectRatio = topDownCamera.Viewport.AspectRatio;


            // Add Both Cameras to both Camera List
            cameras.Add(topDownCamera);
            cameras.Add(camera);
        }

        protected override void Update(GameTime gameTime)
        {
            Time.Update(gameTime);
            InputManager.Update();
            if (InputManager.IsKeyDown(Keys.Escape))
                Exit();

            Ray ray = camera.ScreenPointToWorldRay(InputManager.GetMousePosition());
            foreach (Collider collider in colliders)
            {
                collider.Transform.Rotate(Vector3.Up, Time.ElapsedGameTime);
                collider.Transform.Rotate(Vector3.Right, Time.ElapsedGameTime);
                collider.Transform.Rotate(Vector3.Forward, Time.ElapsedGameTime);

                if (collider.Intersects(ray) != null)
                {
                    effect.Parameters["DiffuseColor"].SetValue(
                        Color.Red.ToVector3());
                    (cube.Meshes[0].Effects[0] as BasicEffect).DiffuseColor = Color.Blue.ToVector3();
                    SoundEffectInstance soundInstance = gunSound.CreateInstance();
                    if (InputManager.IsMousePressed(0))
                    {
                        soundInstance.IsLooped = false;
                        soundInstance.Play();
                    }
                }
                else
                {
                    effect.Parameters["DiffuseColor"].SetValue(
                        Color.Blue.ToVector3());
                    (cube.Meshes[0].Effects[0] as BasicEffect).DiffuseColor = Color.Red.ToVector3();
                }
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Purple);
            foreach (Camera camera in cameras)
            {
                GraphicsDevice.DepthStencilState = new DepthStencilState();
                GraphicsDevice.Viewport = camera.Viewport;
                Matrix view = camera.View;
                Matrix projection = camera.Projection;
                //cube.Draw(transforms[0].World, view, projection);

                effect.CurrentTechnique = effect.Techniques[1];
                effect.Parameters["View"].SetValue(view);
                effect.Parameters["Projection"].SetValue(projection);
                effect.Parameters["LightPosition"].SetValue(Vector3.Backward * 10 + Vector3.Right * 5);
                effect.Parameters["CameraPosition"].SetValue(camera.Transform.Position);
                effect.Parameters["Shininess"].SetValue(20f);
                effect.Parameters["AmbientColor"].SetValue(new Vector3(0.2f, 0.2f, 0.2f));
                effect.Parameters["SpecularColor"].SetValue(new Vector3(0, 0, 0.5f));
                effect.Parameters["DiffuseTexture"].SetValue(texture);
                foreach (Transform transform in transforms)
                {
                    effect.Parameters["World"].SetValue(transform.World);
                    foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                    {
                        pass.Apply();
                        foreach (ModelMesh mesh in cube.Meshes)
                            foreach (ModelMeshPart part in mesh.MeshParts)
                            {
                                GraphicsDevice.SetVertexBuffer(part.VertexBuffer);
                                GraphicsDevice.Indices = part.IndexBuffer;
                                GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, part.VertexOffset, 0,part.NumVertices, part.StartIndex, part.PrimitiveCount);
                            }
                    }
                }

                spriteBatch.Begin();
                spriteBatch.DrawString(font, "The size of the screen width: " + ScreenManager.Width, Vector2.UnitY * 20, Color.PaleGreen);
                spriteBatch.DrawString(font, "The size of the screen height:" + ScreenManager.Height, Vector2.UnitY * 40, Color.PaleGreen);
                spriteBatch.End();
            }
            base.Draw(gameTime);
        }
    }
}
