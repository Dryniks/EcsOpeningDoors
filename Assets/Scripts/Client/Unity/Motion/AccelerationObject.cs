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

        public override Dictionary<Type, object> GetData()
        {
            return new() {{typeof(AccelerationTo), new AccelerationTo {Value = _value}}};
        }

        public override void SetData(object data)
        {
        }
    }
}