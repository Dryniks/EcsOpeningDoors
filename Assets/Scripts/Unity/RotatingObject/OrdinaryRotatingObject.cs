using UnityEngine;

namespace EcsOpeningDoors.Unity
{
    public sealed class OrdinaryRotatingObject : RotatingObject
    {
        public override void SetRotation(Quaternion rotation)
        {
            transform.rotation = rotation;
        }
    }
}