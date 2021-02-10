using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Reflection;
using FlappyBird.Components.Components;
using FlappyBird.Components.Components.Input;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace FlappyBird.Components
{
    public abstract class GameObject : IGameObject
    {
        public Body Body { get; protected set; }

        protected GameObject(GameScene scene)
        {
            Body = new Body();
            Scene = scene;
        }

        protected IControlInputHandler InputHandler { get; set; }

        public bool IsStatic { get; set; }

        public void SetSize(SizeF sizeF)
        {
            Body.Size = sizeF;
        }

        public ICollider Collider
        {
            get => Body?.Collider;
            protected set
            {
                if (Body != null)
                {
                    Body.Collider = value;
                }
            }
        }

        public virtual void SetVelocity(Vector2 velocity)
        {
            Body.Velocity = velocity;
        }

        public virtual void SetBounceFactor(float val)
        {
            Body.BounceFactor = val;
        }

        public void SetPosition(Vector2 position)
        {
            Body.Position = position;
        }

        public GameScene Scene { get; }
        public Vector2 Position => Body.Position;
        public float Mass => Body.Mass;
        public SizeF Size => Body.Size;
        public Vector2 Velocity => Body.Velocity;

        public Vector2 MaxVelocity => Body.MaxVelocity;

        public RectangleF Bounds => Body.Bounds;

        public GameComponent Component { get; private set; }
        private readonly List<GameObject> childrens = new List<GameObject>();

        public virtual IEnumerable<GameObject> GetChildrens()
        {
            lock (childLock)
            {
                return new List<GameObject>(childrens);
            }
        }

        public void Clear()
        {
            lock (childLock)
            {
                childrens.Clear();
            }
        }

        public IEnumerable<T> GetGameObjects<T>() where T : GameObject
        {
            foreach (var obj in GetChildrens())
            {
                if (obj is T t)
                {
                    yield return t;
                }
            }
        }

        public virtual void RenderFrame(float delta)
        {
            Component?.Refresh();
            foreach (var child in GetChildrens())
            {
                child.RenderFrame(delta);
            }
        }

        public void UpdateGame(float delta)
        {
            var oldPosition = Position;

            if (!IsStatic) //static object can not have physics and can not move
            {
                Body.ApplyPhysics(delta);
                OnMove(oldPosition, Position);
            }

            OnGameUpdate(delta);
            foreach (var child in GetChildrens())
            {
                child.UpdateGame(delta);
            }
        }

        protected virtual void OnMove(Vector2 oldPosition, Vector2 newPosition)
        {
        }

        protected virtual void OnGameUpdate(float delta)
        {
        }

        private readonly object childLock = new object();

        public GameObject AddChild(GameObject obj)
        {
            lock (childLock)
            {
                childrens.Add(obj);
            }

            return obj;
        }

        public void RemoveChild(GameObject obj)
        {
            lock (childLock)
            {
                childrens.Remove(obj);
            }
        }

        public RenderFragment RenderComponent()
        {
            return builder =>
            {
                var componentType = GetComponentType();
                if (componentType != null)
                {
                    builder.OpenComponent(0, componentType);
                    builder.SetKey(this);
                    builder.AddComponentReferenceCapture(1, inst =>
                    {
                        var component = (GameComponent) inst;
                        component.GameObject = this;
                        Component = component;
                    });
                    builder.CloseComponent();
                }
            };
        }

        private Type GetComponentType()
        {
            var attr = GetType().GetCustomAttributes<ComponentTypeAttribute>()
                .FirstOrDefault();
            return attr?.ComponentType;
        }

        public void KeyDown(KeyboardEventArgs evt)
        {
            InputHandler?.OnKeyDown(evt);
            foreach (var child in GetChildrens())
            {
                child.InputHandler?.OnKeyDown(evt);
            }
        }

        public void ApplyForce(Vector2 vector2)
        {
            Body.AddForce(vector2);
        }
    }
}