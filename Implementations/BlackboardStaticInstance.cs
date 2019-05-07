using System;
using BRM.Blackboards.Interfaces;

namespace BRM.Blackboards.V1
{
    public class BlackboardStaticInstance : IBlackboard
    {
        private static BlackboardInstance _blackboard = new BlackboardInstance();
        public void Post<T>(string key, T value)
        {
            _blackboard.Post(key, value);
        }

        public T Get<T>(string key)
        {
            return _blackboard.Get<T>(key);
        }

        public void Delete(string key)
        {
            _blackboard.Delete(key);
        }

        public void Subscribe<T>(string key, Action<T> onUpdate)
        {
            _blackboard.Subscribe(key, onUpdate);
        }

        public void Unsubscribe<T>(string key, Action<T> onUpdate)
        {
            _blackboard.Unsubscribe(key, onUpdate);
        }
    }
}