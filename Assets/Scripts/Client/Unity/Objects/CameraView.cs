using System;
using System.Collections.Generic;
using UnityEngine;
using EcsOpeningDoors.Component;

namespace EcsOpeningDoors.Unity
{
    public class CameraView : ObjectView
    {
        [SerializeField] private Vector3 _offset;

        public override Type Label => typeof(CameraComponent);

        public override List<object> GetData()
        {
            var list = base.GetData();
            list.Add(new CameraComponent {Offset = _offset});

            return list;
        }

        protected override void HandleData(object data)
        {
        }
    }
}