using System.Collections.Generic;
using BRM.Blackboards.Interfaces;
using BRM.Blackboards.Utilties;
using BRM.DebugAdapter.Interfaces;

namespace BRM.Blackboards
{
    /// <summary>
    /// String, object dictionary for decoupled storage + reference across classes
    /// Boxing will occur on first post for value types, but unboxing will not occur
    /// </summary>
    public class BlackboardDictionaryWithoutUnboxing : IBlackboard
    {
        internal readonly Dictionary<string, object> _valueStorage = new Dictionary<string, object>();
        private readonly IDebug _debugger;

        public BlackboardDictionaryWithoutUnboxing(IDebug debugger = null)
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

            if (typeof(T).IsValueType)
            {
                if (_valueStorage.TryGetValue(key, out var valueContainer))
                {
                    if (valueContainer is ValueContainer<T> valueContainerCast)
                    {
                        valueContainerCast.Value = value;
                    }
                    else
                    {
                        _debugger?.LogWarningFormat(DebugMessages.InvalidValueCast, key, typeof(T));
                    }
                }
                else
                {
                    _valueStorage[key] = new ValueContainer<T>(value);
                }

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

            if (typeof(T).IsValueType)
            {
                if (!(val is ValueContainer<T> container))
                {
                    _debugger?.LogWarningFormat(DebugMessages.InvalidValueCast, key, typeof(T));
                    return default(T);
                }

                return container.Value;
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