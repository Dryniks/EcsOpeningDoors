using System.Collections.Generic;
using EcsOpeningDoors.Unity;

namespace EcsOpeningDoors
{
    public class Client : IClient
    {
        private readonly Dictionary<int, ObjectView> _views = new();

        public void UpdateData(Dictionary<int, List<object>> data)
        {
            foreach (var (entity, list) in data)
            {
                if (!_views.ContainsKey(entity))
                    _views[entity] = LevelCreator.CreateView(list);

                _views[entity].SetData(list);
            }
        }
    }
}