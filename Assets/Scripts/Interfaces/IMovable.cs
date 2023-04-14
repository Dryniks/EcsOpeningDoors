using UnityEngine;

namespace EcsOpeningDoors
{
    public interface IMovable : IObject
    {
        void SetPosition(Vector3 position);
    }
}