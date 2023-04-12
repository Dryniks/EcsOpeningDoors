using UnityEngine;

namespace EcsOpeningDoors.Unity
{
    [RequireComponent(typeof(Rigidbody))]
    public class PhysicRotatable : Rotatable
    {
        [SerializeField] private Rigidbody _rigidbody;

        public override void SetRotation(Quaternion rotation)
        {
            _rigidbody.MoveRotation(rotation);
        }
    }
}