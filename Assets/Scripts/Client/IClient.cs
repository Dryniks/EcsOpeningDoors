using System.Collections.Generic;

namespace EcsOpeningDoors
{
    public interface IClient
    {
        void UpdateData(Dictionary<int, List<object>> data);
    }
}