using UnityEngine;

namespace EcsOpeningDoors.Unity
{
    [RequireComponent(typeof(ObjectView))]
    public abstract class MovingObject : MonoBehaviour, IMovable
    {
        public float Speed;

        public abstract void SetPosition(Vector3 position);
    }
}