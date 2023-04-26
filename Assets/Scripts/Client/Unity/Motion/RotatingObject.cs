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

        public override List<object> GetData()
        {
            return new()
            {
                new Component.RotatingObject(),
                new Rotation {Value = transform.rotation},
                new RotationSpeedTo {Value = _speed}
            };
        }

        public override void SetData(object data)
        {
            if (data is Rotation rotation)
                transform.rotation = rotation.Value;

            //Для визуального отображения скорости, дебажные данные
#if UNITY_EDITOR
            if (data is RotationSpeedTo speedTo)
                _speed = speedTo.Value;
#endif
        }
    }
}