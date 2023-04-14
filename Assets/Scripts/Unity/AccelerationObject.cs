using UnityEngine;

namespace EcsOpeningDoors.Unity
{
    [RequireComponent(typeof(ObjectView))]
    public sealed class AccelerationObject : MonoBehaviour
    {
        public float MaxDuration;
    }
}