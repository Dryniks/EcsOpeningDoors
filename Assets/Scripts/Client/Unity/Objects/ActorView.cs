using System;
using System.Collections.Generic;
using UnityEngine;
using EcsOpeningDoors.Component;

namespace EcsOpeningDoors.Unity
{
    [RequireComponent(typeof(MovingObject))]
    public class ActorView : ObjectView
    {
        [SerializeField] private Animator _animator;

        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int MovementSpeed = Animator.StringToHash("MovementSpeed");

        public override Type Label => typeof(Actor);

        private Vector3? _targetPos;

        public override List<object> GetData()
        {
            var list = base.GetData();
            list.Add(new Actor());

            return list;
        }

        protected override void HandleData(object data)
        {
            switch (data)
            {
                case MovementRequest movementRequest:
                    _targetPos = movementRequest.Value;
                    _animator.SetFloat(MovementSpeed, 0);
                    break;
                case MovementSpeed speed:
                    _animator.SetFloat(MovementSpeed, speed.Value);
                    break;
            }
        }

        private void Update()
        {
            if (!_targetPos.HasValue)
                return;

            var distance = Vector3.Distance(_targetPos.Value, transform.position);
            _animator.SetFloat(Speed, distance > 0 ? 1.0f : 0.0f);
        }
    }
}