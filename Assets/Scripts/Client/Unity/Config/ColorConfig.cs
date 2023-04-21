using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace EcsOpeningDoors.Unity
{
    public class ColorConfig : ScriptableObject
    {
        [SerializeField] private List<Model> _models;

        private const string ResourcesPath = "Assets/Resources";
        private const string FolderName = "ScriptableObjects";
        private const string Name = "ColorConfig";

        private readonly Dictionary<string, Color> _colors = new();

        private static ColorConfig _instance;

        private static ColorConfig Instance
        {
            get
            {
                if (_instance != null)
                {
#if UNITY_EDITOR
                    _instance.Init();
#endif
                    return _instance;
                }

                _instance = Resources.Load<ColorConfig>($"{FolderName}/{Name}");

                if (_instance == null)
                {
                    _instance = CreateInstance<ColorConfig>();
                    _instance.Init();
                    Debug.LogError($"{Name} not found.");
                }
                else
                    return _instance;

#if UNITY_EDITOR
                var path = $"{ResourcesPath}/{FolderName}";
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                UnityEditor.AssetDatabase.CreateAsset(_instance, $"{path}/{Name}.asset");
                UnityEditor.AssetDatabase.SaveAssets();
                UnityEditor.AssetDatabase.Refresh();

#endif
                return _instance;
            }
        }

        private void Init()
        {
            if (_models == null)
                return;

            _colors.Clear();
            foreach (var model in _models)
                _colors.Add(model.Id, model.Color);
        }

        public static Color GetColor(string id)
        {
            var dict = Instance._colors;
            return dict.ContainsKey(id) ? dict[id] : Color.white;
        }

#if UNITY_EDITOR

        [UnityEditor.MenuItem("Game/ColorConfig")]
        private static void Show()
        {
            UnityEditor.Selection.activeObject = Instance;
        }

#endif

        [Serializable]
        public class Model
        {
            public string Id;
            public Color Color;
        }
    }
}