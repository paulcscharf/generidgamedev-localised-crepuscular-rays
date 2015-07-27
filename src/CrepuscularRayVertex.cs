using amulware.Graphics;
using OpenTK;

namespace GenericGamedev.LocalisedCrepuscularRays
{
    public struct CrepuscularRayVertex : IVertexData
    {
        private readonly Vector3 position;
        private readonly float radius;
        private readonly float alpha;

        public CrepuscularRayVertex(Vector3 position, float radius, float alpha)
        {
            this.position = position;
            this.radius = radius;
            this.alpha = alpha;
        }

        private static VertexAttribute[] makeAttributes()
        {
            return VertexData.MakeAttributeArray(
                VertexData.MakeAttributeTemplate<Vector3>("v_position"),
                VertexData.MakeAttributeTemplate<float>("v_radius"),
                VertexData.MakeAttributeTemplate<float>("v_alpha")
                );
        }

        private static readonly int bytesize = VertexData.SizeOf<CrepuscularRayVertex>();
        private static VertexAttribute[] attributes;


        public VertexAttribute[] VertexAttributes()
        {
            return attributes ?? (attributes = makeAttributes());
        }

        public int Size()
        {
            return bytesize;
        }
    }
}