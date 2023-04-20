using System;
using System.Collections.Generic;
using UnityEngine;
using EcsOpeningDoors.Component;

namespace EcsOpeningDoors.Unity
{
    public class RotatingObject : MotionObject
    {
        [SerializeField] private float _speed;

        public override Type Label => typeof(Rotation);

        public override Dictionary<Type, object> GetData()
        {
            return new()
            {
                {typeof(Rotation), new Rotation {Value = transform.rotation}},
                {typeof(RotationSpeedTo), new RotationSpeedTo {Value = _speed}},
            };
        }

        public override void SetData(object data)
        {
            if (data is not Rotation rotation)
                return;

            transform.rotation = rotation.Value;
        }
    }
}