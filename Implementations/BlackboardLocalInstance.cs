using System;
using System.Collections.Generic;
using BRM.Blackboards.Interfaces;
using BRM.TheDebugAdapter.Interfaces.V1;

namespace BRM.Blackboards.V1
{
    public class BlackboardLocalInstance : IBlackboard
    {
        private Dictionary<string, object> _valueStorage = new Dictionary<string, object>();
        private Dictionary<string, List<object>> _valueModifiedSubscriptions = new Dictionary<string, List<object>>();
        private IDebug _debugger;

        private static class DebugMessages
        {
            public const string KeyIsNullOrEmpty = "Key is null or empty";
            public const string KeyNotFound = "Key not found: {0}";
        }
        
        public BlackboardLocalInstance(IDebug debugger = null)
        {
            _debugger = debugger;
        }

        public void Post<T>(string key, T value)
        {
            if (string.IsNullOrEmpty(key))
            {
                _debugger?.LogWarning(DebugMessages.KeyIsNullOrEmpty);
                return;
            }

            _valueStorage[key] = value;
            UpdateListeners<T>(key);
        }

        public T Get<T>(string key)
        {
            object val;
            
            if (string.IsNullOrEmpty(key))
            {
                _debugger?.LogWarning(DebugMessages.KeyIsNullOrEmpty);
                return default(T);
            }
            
            if (!_valueStorage.TryGetValue(key, out val))
            {
                _debugger?.LogWarningFormat(DebugMessages.KeyNotFound, key);
                return default(T);
            }
            
            return (T)val;
        }

        public void Delete(string key)
        {
            _valueStorage.Remove(key);
        }

        public void Subscribe<T>(string key, Action<T> onUpdate)
        {
            if (string.IsNullOrEmpty(key))
            {
                _debugger?.LogWarning(DebugMessages.KeyIsNullOrEmpty);
                return;
            }

            List<object> events;
            if (_valueModifiedSubscriptions.TryGetValue(key, out events))
            {
                events.Add(onUpdate);
                return;
            }

            events = new List<object>() { onUpdate };
            _valueModifiedSubscriptions[key] = events;
        }

        public void Unsubscribe<T>(string key, Action<T> onUpdate)
        {
            if (string.IsNullOrEmpty(key))
            {
                _debugger?.LogWarning(DebugMessages.KeyIsNullOrEmpty);
                return;
            }

            List<object> events;
            if (_valueModifiedSubscriptions.TryGetValue(key, out events))
            {
                events.Remove(onUpdate);
            }
        }

        public void Clear()
        {
            _valueStorage.Clear();
            _valueModifiedSubscriptions.Clear();
        }

        private void UpdateListeners<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                _debugger?.LogWarning(DebugMessages.KeyIsNullOrEmpty);
                return;
            }

            if (!_valueStorage.ContainsKey(key))
            {
                _debugger?.LogWarningFormat(DebugMessages.KeyNotFound, key);
                return;
            }

            List<object> subscriptions;
            if (!_valueModifiedSubscriptions.TryGetValue(key, out subscriptions))
            {
                return;
            }

            var newValue = (T)_valueStorage[key];
            foreach (var subObj in subscriptions)
            {
                var sub = subObj as Action<T>;
                sub?.Invoke(newValue);
            }
        }
    }
}