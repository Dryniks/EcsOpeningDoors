using UnityEngine;
using Zenject;
using EcsOpeningDoors.Component;

namespace EcsOpeningDoors.Unity
{
    public class Startup : MonoBehaviour
    {
        private Server _server;

        [Inject]
        private void Construct(Server server)
        {
            _server = server;
        }

        public void Start()
        {
            _server.Init(LevelCreator.Build());
        }

        private void Update()
        {
            if (!Input.GetMouseButtonDown(0))
                return;

            var cam = Camera.main;
            if (cam == null)
                return;

            var ray = cam.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var hit, Mathf.Infinity))
                return;

            Vector3? targetPosition = hit.transform.CompareTag(TagManager.Ground) ? hit.point : null;
            if (!targetPosition.HasValue)
                return;

            var data = new ClickPosition {Value = targetPosition.Value};
            _server.AppendData(data.GetType(), data);
        }

        private void OnDestroy()
        {
            _server.Dispose();
        }
    }
}