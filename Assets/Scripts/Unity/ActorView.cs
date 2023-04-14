using UnityEngine;

namespace EcsOpeningDoors.Unity
{
    [RequireComponent(typeof(ObjectView))]
    public class ActorView : MonoBehaviour, IActor
    {
        [SerializeField] private Animator _animator;

        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int MovementSpeed = Animator.StringToHash("MovementSpeed");

        public void StartRun()
        {
            _animator.SetFloat(Speed, 1.0f);
        }

        public void SetMovementSpeed(float speed)
        {
            _animator.SetFloat(MovementSpeed, speed);
        }

        public void EndRun()
        {
            _animator.SetFloat(Speed, 0.0f);
        }
    }
}