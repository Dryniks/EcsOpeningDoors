using UnityEngine;

namespace EcsOpeningDoors.Unity
{
    public class OrdinaryRotatable : Rotatable
    {
        public override void SetRotation(Quaternion rotation)
        {
            transform.rotation = rotation;
        }
    }
}