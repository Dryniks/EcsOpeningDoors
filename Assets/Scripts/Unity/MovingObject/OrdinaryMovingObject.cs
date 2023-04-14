using UnityEngine;

namespace EcsOpeningDoors.Unity
{
    public sealed class OrdinaryMovingObject : MovingObject
    {
        public override void SetPosition(Vector3 position)
        {
            transform.position = position;
        }
    }
}