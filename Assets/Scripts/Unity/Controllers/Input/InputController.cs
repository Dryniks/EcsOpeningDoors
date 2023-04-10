using UnityEngine;

namespace EcsOpeningDoors.Unity
{
    public class InputController : IInputController
    {
        public Vector3? GetPosition()
        {
            if (!Input.GetMouseButtonDown(0))
                return null;

            var camera = Camera.main;
            if (camera == null)
                return null;

            var ray = camera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var hit, Mathf.Infinity))
                return null;

            return hit.transform.CompareTag(TagManager.Ground) ? hit.point : null;
        }
    }
}