using BRM.Blackboards.Interfaces;

namespace BRM.Blackboards
{
    /// <summary>
    /// Wrapper to a static instance of an <see cref="IBlackboard"/>
    /// </summary>
    public sealed class StaticBlackboardDictionary : IBlackboard
    {
        private static IBlackboard _blackboard;

        public StaticBlackboardDictionary(IBlackboard blackboardToStoreAsStatic = null)
        {
            _blackboard = _blackboard ?? blackboardToStoreAsStatic;
        }

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

        public void ClearData()
        {
            _blackboard.ClearData();
        }
    }
}