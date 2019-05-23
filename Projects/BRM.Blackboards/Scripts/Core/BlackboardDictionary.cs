using System.Collections.Generic;
using BRM.Blackboards.Interfaces;
using BRM.Blackboards.Utilities;
using BRM.DebugAdapter.Interfaces;

namespace BRM.Blackboards
{
    /// <summary>
    /// String, object dictionary for decoupled storage + reference across classes
    /// Boxing + Unboxing will occur for stored value types (primitives + structs)
    /// </summary>
    public sealed class BlackboardDictionary : IBlackboard
    {
        internal readonly Dictionary<string, object> _valueStorage = new Dictionary<string, object>();
        private readonly IDebug _debugger;

        public BlackboardDictionary(IDebug debugger = null)
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
        }

        public T Get<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                _debugger?.LogWarning(DebugMessages.KeyIsNullOrEmpty);
                return default(T);
            }
            
            if (!_valueStorage.TryGetValue(key, out var val))
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
        
        public void ClearData()
        {
            _valueStorage.Clear();
        }
    }
}