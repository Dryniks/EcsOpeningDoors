using System;
using System.Collections.Generic;
using UnityEngine;

namespace EcsOpeningDoors.Unity
{
    public class LevelCreator : MonoBehaviour
    {
        [SerializeField] private Transform _parent;
        [SerializeField] private List<ObjectView> _prefabs;

        private static LevelCreator _instance;

        private readonly Dictionary<Type, ObjectView> _elements = new();

        private void Awake()
        {
            _instance = this;

            foreach (var prefab in _prefabs)
                _elements.Add(prefab.Label, prefab);
        }

        public static Dictionary<int, List<object>> Build()
        {
            var dictionary = new Dictionary<int, List<object>>();

            var views = FindObjectsOfType<ObjectView>();
            for (var i = views.Length - 1; i >= 0; i--)
            {
                var view = views[i];
                var tr = view.transform;

                var id = tr.GetInstanceID();
                dictionary.Add(id, view.GetData());

                DestroyImmediate(tr.gameObject);
            }

            return dictionary;
        }

        public static ObjectView CreateView(IEnumerable<object> data)
        {
            var elements = _instance._elements;

            foreach (var element in data)
            {
                var type = element.GetType();

                if (elements.ContainsKey(type))
                    return Instantiate(elements[type], _instance._parent, true);
            }

            return null;
        }
    }
}