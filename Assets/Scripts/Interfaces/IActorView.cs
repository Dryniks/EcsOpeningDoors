namespace EcsOpeningDoors
{
    public interface IActorView
    {
        void StartRun();

        void EndRun();

        void SetMovementSpeed(float speed);
    }
}