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

            var dict = new Dictionary<Type, object>();

            foreach (var motionObject in _motionObjects.Values)
            {
                foreach (var (key, value) in motionObject.GetData())
                    dict.Add(key, value);
            }

            if (!dict.ContainsKey(typeof(Position)))
                list.Add(new Position {Value = tr.position});

            if (!dict.ContainsKey(typeof(Rotation)))
                list.Add(new Rotation {Value = tr.rotation});

            list.AddRange(dict.Values);

            return list;
        }

        protected abstract void HandleData(object data);

        public void SetData(IEnumerable<object> data)
        {
            foreach (var element in data)
            {
                var key = element.GetType();
                if (_motionObjects.ContainsKey(key))
                    _motionObjects[key].SetData(element);

                if (element is Scale scale)
                    transform.localScale = scale.Value;

                HandleData(element);
            }
        }
    }
}