using BRM.Blackboards.Interfaces;
using BRM.DebugAdapter.Interfaces;

namespace BRM.Blackboards
{
    /// <summary>
    /// Wrapper to a static instance of a <see cref="BlackboardDictionary"/>
    /// </summary>
    public class StaticBlackboardDictionary : IBlackboard
    {
        private static BlackboardDictionary _blackboard;

        public StaticBlackboardDictionary(IDebug debugger = null)
        {
            _blackboard = _blackboard ?? new BlackboardDictionary(debugger);
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