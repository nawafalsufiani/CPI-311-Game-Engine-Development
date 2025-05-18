﻿using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;


namespace CPI311.GameEngine
{
    public class GameObject
    {
        public Transform Transform { get; protected set; }
        public Camera Camera { get { return Get<Camera>(); } }  // Check if Camera is Component
        public Rigidbody Rigidbody { get { return Get<Rigidbody>(); } }

        public Collider Collider { get { return Get<Collider>(); } }


        private Dictionary<Type, Component> Components { get; set; }
        private List<IUpdateable> Updateables { get; set; }
        private List<IRenderable> Renderables { get; set; }
        private List<IDrawable> Drawables { get; set; }


        public GameObject()
        {
            Transform = new Transform();
            Components = new Dictionary<Type, Component>();
            Updateables = new List<IUpdateable>();
            Renderables = new List<IRenderable>();
            Drawables = new List<IDrawable>();
        }

        // *** A new Add (T) function here for Assignment3-(2), 4, and 5 ***********
        public void Add<T>(T component) where T : Component
        {
            Remove<T>();
            component.GameObject = this;
            component.Transform = Transform;
            Components.Add(typeof(T), component);
            if (component is IUpdateable) Updateables.Add(component as IUpdateable);
            if (component is IRenderable) Renderables.Add(component as IRenderable);
            if (component is IDrawable) Drawables.Add(component as IDrawable);
        }
        //**************************************************************************


        public T Add<T>() where T : Component, new()
        {
            Remove<T>();
            T component = new T();
            component.GameObject = this;
            component.Transform = Transform;
            Components.Add(typeof(T), component);
            if (component is IUpdateable) Updateables.Add(component as IUpdateable);
            if (component is IRenderable) Renderables.Add(component as IRenderable);
            if (component is IDrawable) Drawables.Add(component as IDrawable);
            return component;
        }

        public T Get<T>() where T : Component
        {
            if (Components.ContainsKey(typeof(T)))
                return Components[typeof(T)] as T;
            else
                return null;
        }

        public void Remove<T>() where T : Component
        {
            if (Components.ContainsKey(typeof(T)))
            {
                Component component = Components[typeof(T)];
                Components.Remove(typeof(T));
                if (component is IUpdateable)
                    Updateables.Remove(component as IUpdateable);
                if (component is IRenderable)
                    Renderables.Remove(component as IRenderable);
                if (component is IDrawable)
                    Drawables.Remove(component as IDrawable);
            }
        }

        public virtual void Update()
        {
            foreach (IUpdateable component in Updateables) component.Update();
        }
        public virtual void Draw()
        {
            foreach (IRenderable component in Renderables) component.Draw();
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (IDrawable component in Drawables) component.Draw(spriteBatch);
        }
    }
}
