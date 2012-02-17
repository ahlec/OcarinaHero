using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GraphicLibrary
{
    public class EngineModel
    {
        Model model;
        Vector3 position, rotation, scale;
        Matrix viewMatrix, projectionMatrix;
        BoundingBox boundingBox;
        public EngineModel()
        {
            scale = Vector3.One;
            rotation = Vector3.Zero;
            viewMatrix = Matrix.CreateLookAt(new Vector3(0, 0, 40), Vector3.Zero, Vector3.Up);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                (800f / 600f), 1, 10000);
        }
        public Model Model { get { return model; } }
        public Vector3 Position { get { return position; } set { position = value; RecalculateBoundingBox(); } }
        public float X { get { return position.X; } set { position.X = value; RecalculateBoundingBox(); } }
        public float Y { get { return position.Y; } set { position.Y = value; RecalculateBoundingBox(); } }
        public float Z { get { return position.Z; } set { position.Z = value; RecalculateBoundingBox(); } }
        public Vector3 Scale { get { return scale; } set { scale = value; RecalculateBoundingBox(); } }
        public float Width { get { return scale.X; } set { scale.X = value; RecalculateBoundingBox(); } }
        public float Height { get { return scale.Y; } set { scale.Y = value; RecalculateBoundingBox(); } }
        public float Depth { get { return scale.Z; } set { scale.Z = value; RecalculateBoundingBox(); } }
        public Vector3 Rotation { get { return rotation; } set { rotation = value; } }
        public float RotationX { get { return rotation.X; } set { rotation.X = value; } }
        public float RotationY { get { return rotation.Y; } set { rotation.Y = value; } }
        public float RotationZ { get { return rotation.Z; } set { rotation.Z = value; } }
        public BoundingBox BoundingBox { get { return boundingBox; } }
        public bool Visible { get; set; }
        private void RecalculateBoundingBox()
        {
        }
        public Vector3 FindNaturalMax()
        {
            Vector3 largest = model.Meshes[0].BoundingSphere.Center;
            for (int following = 1; following < model.Meshes.Count; following++)
            {
                if (model.Meshes[following].BoundingSphere.Center.X > largest.X)
                    largest.X = model.Meshes[following].BoundingSphere.Center.X;
                if (model.Meshes[following].BoundingSphere.Center.Y > largest.Y)
                    largest.Y = model.Meshes[following].BoundingSphere.Center.Y;
                if (model.Meshes[following].BoundingSphere.Center.Z < largest.Z)
                    largest.Z = model.Meshes[following].BoundingSphere.Center.Z;
            }
            return largest;
        }
        public Vector3 FindNaturalMin()
        {
            Vector3 smallest = model.Meshes[0].BoundingSphere.Center;
            for (int following = 1; following < model.Meshes.Count; following++)
            {
                if (model.Meshes[following].BoundingSphere.Center.X < smallest.X)
                    smallest.X = model.Meshes[following].BoundingSphere.Center.X;
                if (model.Meshes[following].BoundingSphere.Center.Y < smallest.Y)
                    smallest.Y = model.Meshes[following].BoundingSphere.Center.Y;
                if (model.Meshes[following].BoundingSphere.Center.Z > smallest.Z)
                    smallest.Z = model.Meshes[following].BoundingSphere.Center.Z;
            }
            return smallest;
        }
        public void Draw(EngineCamera forCamera)
        {
            if (!Visible)
                return;
            

            Matrix[] transformations = new Matrix[model.Bones.Count];
            Matrix objectMatrix = Matrix.CreateScale(scale) *
                Matrix.CreateRotationX(rotation.X) *
                Matrix.CreateRotationY(rotation.Y) *
                Matrix.CreateRotationZ(rotation.Z) *
                Matrix.CreateTranslation(position);
            model.CopyAbsoluteBoneTransformsTo(transformations);
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                    effect.World = objectMatrix;
                    effect.View = forCamera.ViewMatrix;
                    effect.Projection = forCamera.ProjectionMatrix;
                }
                mesh.Draw();
            }
        }
        public static explicit operator EngineModel(Model baseModel)
        {
            EngineModel convertedModel = new EngineModel();
            convertedModel.model = baseModel;
            return convertedModel;
        }
    }
}
