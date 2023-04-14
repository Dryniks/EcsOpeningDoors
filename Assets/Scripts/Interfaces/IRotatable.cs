using UnityEngine;

namespace EcsOpeningDoors
{
    public interface IRotatable : IObject
    {
        void SetRotation(Quaternion rotation);
    }
}