using System;
using BRM.Blackboards.Interfaces;

namespace BRM.Blackboards.V1
{
    /// <summary>
    /// Simple wrapper to a static instance of a BlackboardLocalInstance
    /// </summary>
    public class BlackboardStaticInstance : IBlackboard
    {
        private static BlackboardLocalInstance _blackboard = new BlackboardLocalInstance();
        
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

        public void Clear()
        {
            _blackboard.Clear();
        }
    }
}