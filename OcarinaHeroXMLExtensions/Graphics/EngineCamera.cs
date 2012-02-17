using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OcarinaHeroLibrary.Graphics
{
    //Resource used: http://www.3dgameprogramming.net/2007/07/31/getting-started-with-xna-first-person-camera/
    public class EngineCamera : EngineObject
    {
        float _near, _far, _aspectRatio;
        Matrix _projectionMatrix, _viewMatrix;
        public EngineCamera(Viewport viewport) : base()
        {
            _aspectRatio = ((float)viewport.Width / (float)viewport.Height);
            _near = 0.1f;
            _far = 1000f;
            UpdateProjection();
            EngineCameraUpdate(Vector3.Zero, Position);
            ObjectHasMoved += new ObjectMoved(EngineCameraUpdate);
        }
        public float Near { get { return _near; } set { _near = value; UpdateProjection(); } }
        public float Far { get { return _far; } set { _far = value; UpdateProjection(); } }
        public Matrix ProjectionMatrix { get { return _projectionMatrix; } }
        public Matrix ViewMatrix { get { return _viewMatrix; } }
        void EngineCameraUpdate(Vector3 oldPosition, Vector3 newPosition)
        {
            _viewMatrix = Matrix.Identity;
            _viewMatrix *= Matrix.CreateRotationX(_rotation.X);
            _viewMatrix *= Matrix.CreateRotationY(_rotation.Y);
            _viewMatrix *= Matrix.CreateRotationZ(_rotation.Z);
            _viewMatrix += Matrix.CreateTranslation(-newPosition);
        }
        private void UpdateProjection()
        {
            _projectionMatrix =
                Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, _aspectRatio, _near, _far);
        }
    }
}
