using BRM.Blackboards.Interfaces;
using BRM.EventBroker.Interfaces;

namespace BRM.Blackboards
{
    /// <summary>
    /// Publishes an event when <see cref="Post"/> is called
    /// </summary>
    public class BlackboardWithNotifications : IBlackboard
    {
        private IBlackboard _blackboard;
        private IPublishKeyedEvents _valueUpdatedPublisher;
        
        public BlackboardWithNotifications(IBlackboard blackboard, IPublishKeyedEvents valueUpdatedPublisher)
        {
            _blackboard = blackboard;
            _valueUpdatedPublisher = valueUpdatedPublisher;
        }
        
        public void Post<T>(string key, T value)
        {
            _blackboard.Post(key, value);
            _valueUpdatedPublisher.Publish(key, value);
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