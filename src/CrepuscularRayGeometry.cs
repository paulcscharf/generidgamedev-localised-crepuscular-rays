using amulware.Graphics;
using OpenTK;

namespace GenericGamedev.LocalisedCrepuscularRays
{
    public sealed class CrepuscularRayGeometry
    {
        private readonly VertexSurface<CrepuscularRayVertex> surface;

        public CrepuscularRayGeometry(VertexSurface<CrepuscularRayVertex> surface)
        {
            this.surface = surface;
        }

        public void Draw(Vector3 position, float radius, float alpha)
        {
            this.surface.AddVertex(new CrepuscularRayVertex(position, radius, alpha));
        }
    }
}