using System;
using System.Collections.Generic;
using UnityEngine;
using EcsOpeningDoors.Component;

namespace EcsOpeningDoors.Unity
{
    public abstract class ObjectView : MonoBehaviour
    {
        private readonly Dictionary<Type, MotionObject> _motionObjects = new();

        public abstract Type Label { get; }

        private void Start()
        {
            var motions = GetComponents<MotionObject>();
            foreach (var motion in motions)
                _motionObjects.Add(motion.Label, motion);
        }

        public virtual List<object> GetData()
        {
            var tr = transform;
            var list = new List<object>
            {
                new Scale {Value = tr.localScale}
            };

            if (!_motionObjects.ContainsKey(typeof(Position)))
                list.Add(new Position {Value = tr.position});

            if (!_motionObjects.ContainsKey(typeof(Rotation)))
                list.Add(new Rotation {Value = tr.rotation});

            foreach (var motionObject in _motionObjects.Values)
                list.AddRange(motionObject.GetData());

            return list;
        }

        protected abstract void HandleData(object data);

        public void SetData(IEnumerable<object> data)
        {
            foreach (var element in data)
            {
                HandleData(element);

                var key = element.GetType();
                if (_motionObjects.ContainsKey(key))
                {
                    _motionObjects[key].SetData(element);
                    continue;
                }

                switch (element)
                {
                    case Position position:
                        transform.position = position.Value;
                        break;
                    case Rotation rotation:
                        transform.rotation = rotation.Value;
                        break;
                    case Scale scale:
                        transform.localScale = scale.Value;
                        break;
                }
            }
        }
    }
}