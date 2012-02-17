using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OcarinaHeroLibrary.Graphics
{
    public static class XNAModelExtensions
    {
        public static Model Clone(this Model model)
        {
            Model clone = Activator.CreateInstance<Model>();
            /*PropertyInfo boneProperty = typeof(Model).GetProperty("Bones");
            ModelBone[] bonesArray = new ModelBone[model.Bones.Count];
            model.Bones.CopyTo(bonesArray, 0);
            List<ModelBone> bones = new List<ModelBone>();
            foreach (ModelBone transfer in bonesArray)
                bones.Add(transfer);
            ModelBoneCollection clonedBoneCollection = (ModelBoneCollection)Activator.CreateInstance(typeof(ModelBoneCollection),
                new object[] { bones },);*/
            //PropertyInfo boneItemsProperty = typeof(ModelBoneCollection).GetProperty("Items", BindingFlags.NonPublic);
            //boneItemsProperty.SetValue(clonedBoneCollection, bones, null);
            //boneProperty.SetValue(clone, clonedBoneCollection, null);
            PropertyInfo rootProperty = typeof(Model).GetProperty("root", (BindingFlags.NonPublic | BindingFlags.Instance));
            rootProperty.SetValue(clone, model.Root, null);
            return clone;
        }
    }
}
