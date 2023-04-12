using UnityEngine;

namespace EcsOpeningDoors.Unity
{
    [RequireComponent(typeof(ObjectView))]
    public abstract class Movable : MonoBehaviour, IMovable
    {
        public float Speed;

        public abstract void SetPosition(Vector3 position);
    }
}