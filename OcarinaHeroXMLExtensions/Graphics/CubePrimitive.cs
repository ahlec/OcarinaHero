using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OcarinaHeroLibrary.Graphics
{
    public class CubePrimitive : EngineObject
    {
        public CubePrimitive(Vector3 position, Vector3 size)
        {
            
            AttachedObjects.Add(new PlanePrimitive(position - size.Y * Vector3.UnitY,
                new Vector2(size.X, size.Z), Color.LightCoral, new Vector3(MathHelper.PiOver2, 0, 0)));
            AttachedObjects.Add(new PlanePrimitive(position + new Vector3(size.X, 0, -size.Z),
                new Vector2(size.X, size.Y), Color.Purple, new Vector3(0, MathHelper.Pi, 0)));
            AttachedObjects.Add(new PlanePrimitive(position, new Vector2(size.X, size.Y),
                Color.LightGoldenrodYellow));
            AttachedObjects.Add(new PlanePrimitive(position - size.Z * Vector3.UnitZ,
                new Vector2(size.Z, size.Y), Color.Blue, new Vector3(0, -MathHelper.PiOver2, 0)));
            AttachedObjects.Add(new PlanePrimitive(position + size.X * Vector3.UnitX,
                new Vector2(size.Z, size.Y), Color.Black, new Vector3(0, MathHelper.PiOver2, 0)));
            AttachedObjects.Add(new PlanePrimitive(position - size.Z * Vector3.UnitZ, new Vector2(size.X, size.Z), Color.LightGreen,
                new Vector3(-MathHelper.PiOver2, 0, 0)));
            _position = position;
        }
        public override void UpdateRotation(Vector3 oldRotation, Vector3 newRotation)
        {
        }
    }
}
