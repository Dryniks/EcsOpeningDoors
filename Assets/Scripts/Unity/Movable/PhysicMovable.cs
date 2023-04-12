using UnityEngine;

namespace EcsOpeningDoors.Unity
{
    [RequireComponent(typeof(Rigidbody))]
    public class PhysicMovable : Movable
    {
        [SerializeField] private Rigidbody _rigidbody;

        public override void SetPosition(Vector3 position)
        {
            _rigidbody.MovePosition(position);
        }
    }
}