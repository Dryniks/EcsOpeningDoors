namespace EcsOpeningDoors
{
    public interface IActor : IObject
    {
        void StartRun();

        void EndRun();

        void SetMovementSpeed(float speed);
    }
}