using System;
using System.Collections.Generic;
using UnityEngine;

namespace EcsOpeningDoors.Unity
{
    public abstract class MotionObject : MonoBehaviour
    {
        public abstract Type Label { get; }

        public abstract List<object> GetData();

        public abstract void SetData(object data);
    }
}