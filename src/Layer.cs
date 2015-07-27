using System.Collections.Generic;
using System.Linq;
using amulware.Graphics;
using Bearded.Utilities;
using Bearded.Utilities.Math;
using OpenTK;

namespace GenericGamedev.LocalisedCrepuscularRays
{
    public class Layer
    {
        private readonly CrepuscularRayGeometry rayGeo;
        private readonly PostProcessSurface surface;
        private readonly IndexedSurface<UVColorVertexData> wispSurface;
        private readonly Sprite2DGeometry wispGeo;
        private readonly List<Wisp> wisps;

        public Layer(ISurfaceShader shader, ISurfaceShader wispShader, CrepuscularRayGeometry rayGeo, string filename, float brightness, int wisps)
        {
            this.rayGeo = rayGeo;
            var texture = new Texture("gfx/" + filename + ".png", true);

            this.surface = new PostProcessSurface();
            this.surface.AddSettings(
                new TextureUniform("diffuseTexture", texture),
                new ColorUniform("color", Color.GrayScale((byte)(255 * brightness)))
                );
            shader.UseOnSurface(this.surface);

            this.wispSurface = new IndexedSurface<UVColorVertexData>();
            wispShader.UseOnSurface(this.wispSurface);

            this.wispGeo = new Sprite2DGeometry(this.wispSurface)
            {
                Color = Color.White.WithAlpha()
            };

            this.wisps = Enumerable.Range(0, wisps)
                .Select(i => new Wisp()).ToList();
        }

        private class Wisp
        {
            private float angle;

            private readonly float angleV;
            private readonly Vector2 center;
            private readonly Vector2 radius;
            private readonly float size;
            private readonly Color color;

            public Wisp()
            {
                this.angle = StaticRandom.Float(0, GameMath.TwoPi);
                this.angleV = StaticRandom.Float(0.2f, 0.5f) * StaticRandom.Sign();
                this.center = new Vector2(StaticRandom.Float(-0.6f, 0.6f), StaticRandom.Float(-1f, 1f));
                this.radius = new Vector2(StaticRandom.Float(0.3f, 0.8f), StaticRandom.Float(0.1f, 0.3f));
                this.size = StaticRandom.Float(0.05f, 0.3f);
                this.color = Color.FromHSVA(StaticRandom.Float(0, GameMath.TwoPi), 1, StaticRandom.Float(0.8f, 1), 0);
            }

            public void Update(float t)
            {
                this.angle += this.angleV * t;
            }

            public void Draw(Sprite2DGeometry geo, CrepuscularRayGeometry rayGeo)
            {
                var p = this.center + new Vector2(GameMath.Cos(this.angle), GameMath.Sin(this.angle)) * this.radius;

                geo.Color = this.color;
                geo.DrawSprite(p, 0, this.size);
                geo.Color = Color.White;
                geo.DrawSprite(p, 0, this.size * 0.7f);

                rayGeo.Draw(p.WithZ(), this.size * 1.5f, 3);
            }
        }

        public void Update(float t)
        {
            foreach (var wisp in this.wisps)
            {
                wisp.Update(t);
            }   
        }

        public void Draw()
        {
            this.surface.Render();

            foreach (var wisp in this.wisps)
            {
                wisp.Draw(this.wispGeo, this.rayGeo);
            }

            this.wispSurface.Render();
        }
    }
}