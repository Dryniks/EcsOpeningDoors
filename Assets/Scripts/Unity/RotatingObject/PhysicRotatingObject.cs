using UnityEngine;

namespace EcsOpeningDoors.Unity
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class PhysicRotatingObject : RotatingObject
    {
        [SerializeField] private Rigidbody _rigidbody;

        public override void SetRotation(Quaternion rotation)
        {
            _rigidbody.MoveRotation(rotation);
        }
    }
}