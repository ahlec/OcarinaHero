using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OcarinaHeroLibrary.Graphics
{
    public class EngineObject
    {
        /* Attached Objects */
        protected AttachedEngineObjectsList _attachedObjects = new AttachedEngineObjectsList();
        /// <summary>
        /// The EngineObjects attached to this EngineObject. EngineObjects attached to another EngineObject
        /// move and rotate with the given EngineObject, relative to the values when attached. Values
        /// may still be directly changed, however.
        /// </summary>
        public AttachedEngineObjectsList AttachedObjects
        {
            get { return _attachedObjects; }
            set { _attachedObjects = value; }
        }

        /* Position */
        protected Vector3 _position;
        /// <summary>
        /// The current position of the 3D object. Changing this value invokes the ObjectHasMoved event.
        /// </summary>
        public Vector3 Position
        {
            get { return _position; }
            set { UpdatePosition(_position, value); _position = value; }
        }
        /// <summary>
        /// The X coordinate in the current position. Changing this value invokes the ObjectHasMoved event.
        /// </summary>
        public virtual float X
        {
            get { return _position.X; }
            set { UpdatePosition(_position, new Vector3(value, _position.Y, _position.Z)); _position.X = value; }
        }
        /// <summary>
        /// The Y coordinate in the current position. Changing this value invokes the ObjectHasMoved event.
        /// </summary>
        public virtual float Y
        {
            get { return _position.Y; }
            set { UpdatePosition(_position, new Vector3(_position.X, value, _position.Z)); _position.Y = value; }
        }
        /// <summary>
        /// The Z coordinate in the current position. Changing this value invokes the ObjectHasMoved event.
        /// </summary>
        public virtual float Z
        {
            get { return _position.Z; }
            set { UpdatePosition(_position, new Vector3(_position.X, _position.Y, value)); _position.Z = value; }
        }
        protected virtual void UpdatePosition(Vector3 oldPosition, Vector3 newPosition)
        {
            Vector3 deltaPosition = newPosition - oldPosition;
            foreach (EngineObject attachedObj in AttachedObjects)
                attachedObj.Position += deltaPosition;
            if (ObjectHasMoved != null)
                ObjectHasMoved.Invoke(oldPosition, newPosition);
        }
        public delegate void ObjectMoved(Vector3 oldPosition, Vector3 newPosition);
        public event ObjectMoved ObjectHasMoved;

        /* Rotation */
        protected Vector3 _rotation;
        /// <summary>
        /// The rotation about the three axes. Changing this value invokes the ObjectHasRotated event.
        /// </summary>
        public Vector3 Rotation
        {
            get { return _rotation; }
            set { UpdateRotation(_rotation, value); _rotation = value; }
        }
        /// <summary>
        /// The rotation about the X axis. Changing this value invokes the ObjectHasRotated event.
        /// </summary>
        public float RotationX
        {
            get { return _rotation.X; }
            set { UpdateRotation(_rotation, new Vector3(value, _rotation.Y, _rotation.Z)); _rotation.X = value; }
        }
        /// <summary>
        /// The rotation about the Y axis. Changing this value invokes the ObjectHasRotated event.
        /// </summary>
        public float RotationY
        {
            get { return _rotation.Y; }
            set { UpdateRotation(_rotation, new Vector3(_rotation.X, value, _rotation.Y)); _rotation.Y = value; }
        }
        /// <summary>
        /// The rotation about the Z axis. Changing this value invokes the ObjectHasRotated event.
        /// </summary>
        public float RotationZ
        {
            get { return _rotation.Z; }
            set { UpdateRotation(_rotation, new Vector3(_rotation.X, _rotation.Y, value)); _rotation.Z = value; }
        }
        public virtual void UpdateRotation(Vector3 oldRotation, Vector3 newRotation)
        {
            Vector3 deltaRotation = newRotation - oldRotation;
            foreach (EngineObject attachedObj in AttachedObjects)
                attachedObj.Rotation += deltaRotation;
            if (ObjectHasRotated != null)
                ObjectHasRotated.Invoke(oldRotation, newRotation);
        }
        public delegate void ObjectHasRotatedHandler(Vector3 oldRotation, Vector3 newRotation);
        public event ObjectHasRotatedHandler ObjectHasRotated;

        /* Drawing */
        public virtual void Draw(GraphicsDevice graphicsDevice, EngineCamera forCamera)
        {
            
            foreach (EngineObject obj in AttachedObjects)
                obj.Draw(graphicsDevice, forCamera);
        }
    }

    public class AttachedEngineObjectsList : List<EngineObject>
    {
        public AttachedEngineObjectsList() : base() { }
        /// <summary>
        /// Adds the item to the end of the list of EngineObjects. Invokes the EngineObjectAttached event.
        /// </summary>
        /// <param name="obj">The EngineObject to attach to the list.</param>
        public new void Add(EngineObject obj)
        {
            base.Add(obj);
            if (EngineObjectAttached != null)
                EngineObjectAttached.Invoke(obj);
        }
        public delegate void EngineObjectAttachedHandler(EngineObject obj);
        public event EngineObjectAttachedHandler EngineObjectAttached;
        /// <summary>
        /// Removes an item from the list of EngineObjects. Invokes the EngineObjectRemoved event.
        /// </summary>
        /// <param name="obj">The EngineObject to remove from the list.</param>
        public new void Remove(EngineObject obj)
        {
            base.Remove(obj);
            if (EngineObjectRemoved != null)
                EngineObjectRemoved.Invoke(obj);
        }
        /// <summary>
        /// Removes an item from the list of EngineObjects at the given index. Invokes the
        /// EngineObjectRemoved event.
        /// </summary>
        /// <param name="index">The index within the list where the EngineObject should be removed.</param>
        public new void RemoveAt(int index)
        {
            EngineObject obj = base[index];
            this.Remove(obj);
        }
        public delegate void EngineObjectRemovedHandle(EngineObject obj);
        public event EngineObjectRemovedHandle EngineObjectRemoved;
    }
}
