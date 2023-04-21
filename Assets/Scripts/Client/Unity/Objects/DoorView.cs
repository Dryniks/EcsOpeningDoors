using System;
using System.Collections.Generic;
using UnityEngine;
using EcsOpeningDoors.Component;

namespace EcsOpeningDoors.Unity
{
    [RequireComponent(typeof(Painter))]
    public class DoorView : ObjectView
    {
        [SerializeField] private string _id;
        [SerializeField] private Painter _painter;
        [SerializeField] private Transform _openPoint;

        public override Type Label => typeof(Door);

        public override List<object> GetData()
        {
            var list = base.GetData();
            list.Add(new Link {Id = _id});
            list.Add(new Door {OpenPoint = _openPoint.position});

            return list;
        }

        protected override void HandleData(object data)
        {
            if (data is Link link)
            {
                _painter.SetColor(link.Id);

#if UNITY_EDITOR
                _id = link.Id;
#endif
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (string.IsNullOrEmpty(_id))
                return;

            _painter.SetColor(_id);
        }
#endif
    }
}