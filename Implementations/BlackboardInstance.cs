using System;
using System.Collections.Generic;
using BRM.Blackboards.Interfaces;

namespace BRM.Blackboards.V1
{
    public class BlackboardInstance : IBlackboard
    {
        private Dictionary<string, object> _values = new Dictionary<string, object>();
        private Dictionary<string, List<object>> _eventSubscriptions = new Dictionary<string, List<object>>();
        public void Post<T>(string key, T value)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }

            _values[key] = value;
            UpdateListeners<T>(key);
        }

        public T Get<T>(string key)
        {
            object val;
            if (_values.TryGetValue(key, out val))
            {
                return (T)val;
            }

            return default(T);
        }

        public void Delete(string key)
        {
            _values.Remove(key);
        }

        public void Subscribe<T>(string key, Action<T> onUpdate)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }

            List<object> events;
            if (_eventSubscriptions.TryGetValue(key, out events))
            {
                events.Add(onUpdate);
                return;
            }

            events = new List<object>() { onUpdate };
            _eventSubscriptions[key] = events;
        }

        public void Unsubscribe<T>(string key, Action<T> onUpdate)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }

            List<object> events;
            if (_eventSubscriptions.TryGetValue(key, out events))
            {
                events.Remove(onUpdate);
            }
        }

        private void UpdateListeners<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }

            List<object> subscriptions;
            if (!_eventSubscriptions.TryGetValue(key, out subscriptions) || !_values.ContainsKey(key))
            {
                return;
            }

            var newValue = (T)_values[key];
            foreach (var subObj in subscriptions)
            {
                var sub = subObj as Action<T>;
                sub?.Invoke(newValue);
            }
        }
    }
}