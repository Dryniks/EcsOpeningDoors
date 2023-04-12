using UnityEngine;

namespace EcsOpeningDoors.Unity
{
    public class OrdinaryMovable : Movable
    {
        public override void SetPosition(Vector3 position)
        {
            transform.position = position;
        }
    }
}