using UnityEngine;
using EcsOpeningDoors.Unity;

namespace EcsOpeningDoors
{
    public class Painter : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;

        private static readonly int Color = Shader.PropertyToID("_Color");

        private MaterialPropertyBlock _materialProperty;

        public void SetColor(string id)
        {
            if (_materialProperty == null)
            {
                _materialProperty = new MaterialPropertyBlock();
                _renderer.GetPropertyBlock(_materialProperty);
            }

            var color = ColorConfig.GetColor(id);
            _materialProperty.SetColor(Color, color);
            _renderer.SetPropertyBlock(_materialProperty);
        }
    }
}