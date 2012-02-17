using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OcarinaHeroLibrary.Physics;

namespace OcarinaHeroLibrary.Graphics
{
    public class EngineModel : EngineObject
    {
        protected Model _model;
        protected Vector3 _scale = Vector3.One;
        protected Matrix worldMatrix;
        protected Dictionary<ModelMesh, Texture2D> textures = new Dictionary<ModelMesh, Texture2D>();

        public EngineModel(Model baseModel)
        {
            _model = baseModel;
            Matrix[] transforms = new Matrix[baseModel.Bones.Count];
            _model.CopyAbsoluteBoneTransformsTo(transforms);
            AmbientColor = Color.White;
            foreach (ModelMesh mesh in _model.Meshes)
                textures.Add(mesh, (mesh.Effects[0] as BasicEffect).Texture);
            Opacity = 100;
        }
        /*public EngineModel(EngineModel baseModel)
        {
            _position = baseModel._position;
            _rotation = baseModel._rotation;
            _scale = baseModel._scale;
            Matrix[] transforms = new Matrix[baseModel._model.Bones.Count];
            _model.CopyAbsoluteBoneTransformsTo(transforms);
            AmbientColor = baseModel.AmbientColor;
        }*/
        public Vector3 Scale { get { return _scale; } set { _scale = value; } }
        public float ScaleX { get { return _scale.X; } set { _scale.X = value; } }
        public float ScaleY { get { return _scale.Y; } set { _scale.Y = value; } }
        public float ScaleZ { get { return _scale.Z; } set { _scale.Z = value; } }
        public ConstantTranslation Translation { get; set; }
        public Color AmbientColor { get; set; }
        /// <summary>
        /// Opacity of the model (0-100)
        /// </summary>
        public int Opacity { get; set; }
        public Texture2D[] Textures
        {
            get { return textures.Values.ToArray<Texture2D>(); }
        }
        public void SetTexture(int index, Texture2D texture)
        {
            textures[textures.ElementAt(index).Key] = texture;
            (_model.Meshes[index].Effects[0] as BasicEffect).Texture = texture;
            (_model.Meshes[index].Effects[0] as BasicEffect).TextureEnabled = true;
        }
        public static implicit operator EngineModel(Model baseModel)
        {
            return new EngineModel(baseModel);
        }
        public override void Draw(GraphicsDevice graphicsDevice, EngineCamera forCamera)
        {
            foreach (ModelMesh mesh in _model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = Matrix.CreateScale(_scale) * Matrix.CreateRotationX(_rotation.X) *
                        Matrix.CreateRotationY(_rotation.Y) * Matrix.CreateRotationZ(_rotation.Z) *
                        Matrix.CreateTranslation(_position - forCamera.Position) *
                        Matrix.CreateRotationX(forCamera.RotationX) * Matrix.CreateRotationZ(forCamera.RotationZ);
                    if (textures.Keys.Contains<ModelMesh>(mesh))
                    {
                        effect.Texture = textures[mesh];
                        effect.TextureEnabled = true;
                    }
                    effect.Alpha = (Opacity / 100f);
                    effect.View = forCamera.ViewMatrix;
                    effect.Projection = forCamera.ProjectionMatrix;
                    Vector3 color = new Vector3(AmbientColor.R / 255f, AmbientColor.G / 255f,
                        AmbientColor.B / 255f);
                    effect.LightingEnabled = true;
                    effect.DirectionalLight0.DiffuseColor = color;
                    effect.DirectionalLight0.Direction = new Vector3(0, 1, 0);
                    effect.DirectionalLight0.SpecularColor = color;
  //                  effect.LightingEnabled = true;
                    effect.AmbientLightColor = color;
 //                   effect.DiffuseColor = color;
//                    effect.SpecularColor = color;
                    effect.EmissiveColor = color;
                    //effect.EnableDefaultLighting();
                }
                mesh.Draw();
            }
            base.Draw(graphicsDevice, forCamera);
        }
    }
}
