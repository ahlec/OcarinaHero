using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OcarinaHeroLibrary.Graphics
{
    public class PlanePrimitive : EngineObject
    {
        Vector2 _size;
        PrimitiveCoveringType _coveringType;
        VertexPositionColorTexture[] _vertice = new VertexPositionColorTexture[6];
        Color _color;
        VertexBuffer _vertexBuffer;
        Vector3[] _corners = new Vector3[4];

        public PlanePrimitive(Vector3 position, Vector2 size)
        {
            _position = position;
            _size = size;
            _rotation = Vector3.Zero;
            _vertexBuffer = null;
            UpdateVertices();
            _coveringType = PrimitiveCoveringType.None;
            ObjectHasMoved += new ObjectMoved(UpdateVerticesFromEvents);
            ObjectHasRotated += new ObjectHasRotatedHandler(UpdateVerticesFromEvents);
        }
        public PlanePrimitive(Vector3 position, Vector2 size, Vector3 rotation)
        {
            _position = position;
            _size = size;
            _rotation = rotation;
            UpdateVertices();
            _coveringType = PrimitiveCoveringType.None;
            ObjectHasMoved += new ObjectMoved(UpdateVerticesFromEvents);
            ObjectHasRotated += new ObjectHasRotatedHandler(UpdateVerticesFromEvents);
        }
        public PlanePrimitive(Vector3 position, Vector2 size, Color color)
        {
            _position = position;
            _size = size;
            _color = color;
            UpdateVertices();
            _coveringType = PrimitiveCoveringType.Color;
            ObjectHasMoved += new ObjectMoved(UpdateVerticesFromEvents);
            ObjectHasRotated += new ObjectHasRotatedHandler(UpdateVerticesFromEvents);
        }
        public PlanePrimitive(Vector3 position, Vector2 size, Color color, Vector3 rotation)
        {
            _position = position;
            _size = size;
            _rotation = rotation;
            _color = color;
            UpdateVertices();
            _coveringType = PrimitiveCoveringType.Color;
            ObjectHasMoved += new ObjectMoved(UpdateVerticesFromEvents);
            ObjectHasRotated += new ObjectHasRotatedHandler(UpdateVerticesFromEvents);
        }
        public Vector2 Size { get { return _size; } set { _size = value; UpdateVertices(); } }
        public float Width { get { return _size.X; } set { _size.X = value; UpdateVertices(); } }
        public float Height { get { return _size.Y; } set { _size.Y = value; UpdateVertices(); } }
        public PrimitiveCoveringType CoveringType { get { return _coveringType; } }
        public Color Color { get { return _color; } set { _color = value; UpdateVertices(); } }
        public Vector3[] Corners { get { return _corners; } }

        private void UpdateVerticesFromEvents(Vector3 oldVals, Vector3 newVals)
        {
            UpdateVertices();
        }
        private void UpdateVertices()
        {
            Vector3 _vector3Size = new Vector3(_size.X, -_size.Y, 0);
            Matrix rotation = Matrix.Identity * Matrix.CreateRotationX(_rotation.X) *
                Matrix.CreateRotationY(_rotation.Y) * Matrix.CreateRotationZ(_rotation.Z);
            _corners = new Vector3[4];
            _corners[0] = Vector3.Transform(Vector3.Zero, rotation) + Position;
            _corners[1] = Vector3.Transform(Vector3.Zero + _vector3Size * Vector3.UnitX, rotation) + Position;
            _corners[2] = Vector3.Transform(Vector3.Zero + _vector3Size * new Vector3(1, 1, 0), rotation) + Position;
            _corners[3] = Vector3.Transform(Vector3.Zero + _vector3Size * Vector3.UnitY, rotation) + Position;

            _vertice[0] = new VertexPositionColorTexture(_corners[0], _color, Vector2.Zero);
            _vertice[1] = new VertexPositionColorTexture(_corners[1], _color, Vector2.UnitX * _size);
            _vertice[2] = new VertexPositionColorTexture(_corners[2], _color, _size);
            _vertice[3] = _vertice[2];
            _vertice[4] = new VertexPositionColorTexture(_corners[3], _color, Vector2.UnitY * _size);
            _vertice[5] = _vertice[0];
        }

        public override void Draw(GraphicsDevice graphicsDevice, EngineCamera forCamera)
        {
            if (_vertexBuffer == null)
                _vertexBuffer = new VertexBuffer(graphicsDevice, VertexPositionColorTexture.SizeInBytes *
                    _vertice.Length, BufferUsage.WriteOnly);
            _vertexBuffer.SetData(_vertice);
            graphicsDevice.Vertices[0].SetSource(_vertexBuffer, 0, VertexPositionColorTexture.SizeInBytes);
            graphicsDevice.VertexDeclaration = new VertexDeclaration(graphicsDevice,
                VertexPositionColorTexture.VertexElements);
            graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);
        }
    }
}
