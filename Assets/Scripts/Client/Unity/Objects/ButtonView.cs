using System;
using System.Collections.Generic;
using UnityEngine;
using EcsOpeningDoors.Component;

namespace EcsOpeningDoors.Unity
{
    [RequireComponent(typeof(Painter))]
    public class ButtonView : ObjectView
    {
        [SerializeField] private float _radius;
        [SerializeField] private string _id;
        [SerializeField] private Painter _painter;

        public override Type Label => typeof(Button);

        public override List<object> GetData()
        {
            var list = base.GetData();
            list.Add(new Link {Id = _id});
            list.Add(new Button {Radius = _radius});

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

            //Для визуального отображения радиуса, дебажные данные
#if UNITY_EDITOR
            if (data is Button button)
                _radius = button.Radius;
#endif
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (string.IsNullOrEmpty(_id))
                return;

            _painter.SetColor(_id);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
#endif
    }
}