using System;
using System.Collections.Generic;
using UnityEngine;
using EcsOpeningDoors.Component;

namespace EcsOpeningDoors.Unity
{
    public class MovingObject : MotionObject
    {
        [SerializeField] private float _speed;

        public override Type Label => typeof(Position);

        public override Dictionary<Type, object> GetData()
        {
            return new()
            {
                {typeof(Position), new Position {Value = transform.position}},
                {typeof(MovementSpeedTo), new MovementSpeedTo {Value = _speed}},
            };
        }

        public override void SetData(object data)
        {
            if (data is not Position position)
                return;

            transform.position = position.Value;
        }
    }
}