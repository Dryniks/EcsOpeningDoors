using UnityEngine;

namespace EcsOpeningDoors.Unity
{
    [RequireComponent(typeof(ObjectView))]
    public abstract class RotatingObject : MonoBehaviour, IRotatable
    {
        public float Speed;

        public abstract void SetRotation(Quaternion rotation);
    }
}