using System;
using Microsoft.Xna.Framework;

namespace OcarinaHeroLibrary.Physics
{
    public class ConstantTranslation
    {
        Vector3 _initialVelocity, _acceleration;
        float _elapsedTime; // In seconds
        public ConstantTranslation(Vector3 initialVelocity, Vector3 acceleration)
        {
            _initialVelocity = initialVelocity;
            _acceleration = acceleration;
        }
        public float ElapsedTimeInSeconds { get { return _elapsedTime; } }
        public Vector3 Acceleration { get { return _acceleration; } }
        public Vector3 Update(GameTime gametime)
        {
            float updateElapsedTime = _elapsedTime + (float)gametime.ElapsedGameTime.TotalSeconds;
            Vector3 updateVector = new Vector3(0, 0, 0);

            updateVector.X = _initialVelocity.X * updateElapsedTime + .5f * _acceleration.X *
                (float)Math.Pow(updateElapsedTime, 2);
            updateVector.Y = _initialVelocity.Y * updateElapsedTime + .5f * _acceleration.Y *
                (float)Math.Pow(updateElapsedTime, 2);
            updateVector.Z = _initialVelocity.Z * updateElapsedTime + .5f * _acceleration.Z *
                (float)Math.Pow(updateElapsedTime, 2);
            return updateVector;
        }
        public void MarkUpdate(GameTime gameTime)
        {
            _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
