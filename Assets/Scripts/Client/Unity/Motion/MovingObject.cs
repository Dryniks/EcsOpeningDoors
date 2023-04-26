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

        public override List<object> GetData()
        {
            var list = new List<object>()
            {
                new Component.MovingObject(),
                new Position {Value = transform.position},
                new MovementSpeedTo {Value = _speed}
            };

            if (GetComponent<AccelerationObject>() == null)
                list.Add(new MovementSpeed {Value = _speed});

            return list;
        }

        public override void SetData(object data)
        {
            if (data is Position position)
                transform.position = position.Value;

            //Для визуального отображения данных скорости в инспекторе, дебажные данные
#if UNITY_EDITOR
            if (data is MovementSpeed movementSpeed)
                _speed = movementSpeed.Value;
#endif
        }
    }
}