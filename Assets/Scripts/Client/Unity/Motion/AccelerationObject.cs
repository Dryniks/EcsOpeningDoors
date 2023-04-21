using System;
using System.Collections.Generic;
using UnityEngine;
using EcsOpeningDoors.Component;

namespace EcsOpeningDoors.Unity
{
    public sealed class AccelerationObject : MotionObject
    {
        [SerializeField] private float _value;

        public override Type Label => typeof(AccelerationTo);

        public override List<object> GetData()
        {
            return new() {new AccelerationTo {Value = _value}};
        }

        public override void SetData(object data)
        {
            //Для визуального изменения ускорения, дебажные данные
#if UNITY_EDITOR
            if (data is not Acceleration acceleration)
                return;

            _value = acceleration.Value;
#endif
        }
    }
}