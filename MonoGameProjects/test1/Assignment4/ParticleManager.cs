using CPI311.GameEngine;
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
    public class ParticleManager
    {
        public struct VertexInfo : IVertexType
        {
            public Vector4 xyz; // xyz coordinate
            public Vector2 uv;  // uv coordiante
            public Vector4 pos; // position
            public Vector4 param; // parameter

            public VertexInfo(Vector4 _xyz, Vector2 _uv)
            {
                xyz = _xyz;
                uv = _uv;
                pos = Vector4.Zero;
                param = Vector4.Zero;
            }
            public readonly static VertexDeclaration VertexDeclaration =
             new VertexDeclaration(
                new VertexElement(0, VertexElementFormat.Vector4,
                                                     VertexElementUsage.Position, 0),
                new VertexElement(sizeof(float) * 4, VertexElementFormat.Vector2,
                                                     VertexElementUsage.TextureCoordinate, 0),
                new VertexElement(sizeof(float) * 6, VertexElementFormat.Vector4,
                                                     VertexElementUsage.Position, 1),
                new VertexElement(sizeof(float) * 10, VertexElementFormat.Vector4,
                                                     VertexElementUsage.Position, 2)
                );
            VertexDeclaration IVertexType.VertexDeclaration
            { get { return VertexDeclaration; } }
        }

        public Particle[] particles;
        VertexInfo[] vtx;

    }

}