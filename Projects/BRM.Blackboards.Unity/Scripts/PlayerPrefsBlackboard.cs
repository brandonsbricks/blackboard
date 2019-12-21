using System.Collections.Generic;
using BRM.Blackboards.Interfaces;
using BRM.DataSerializers.Interfaces;
using UnityEngine;

namespace BRM.Blackboards.Unity
{
    /// <summary>
    /// Saves changes to PlayerPrefs immediately
    /// </summary>
    public sealed class PlayerPrefsBlackboard : IBlackboard
    {
        private readonly ISerializeText _serializer;
        private readonly HashSet<string> _keys = new HashSet<string>();
        
        public PlayerPrefsBlackboard(ISerializeText serializer)
        {
            _serializer = serializer;
        }

        public void Post<T>(string key, T value)
        {
            _keys.Add(key);
            var @string = _serializer.AsString(value, false);
            PlayerPrefs.SetString(key, @string);
            PlayerPrefs.Save();
        }

        public T Get<T>(string key)
        {
            var value = PlayerPrefs.GetString(key);
            return _serializer.AsObject<T>(value);
        }

        public void Delete(string key)
        {
            _keys.Remove(key);
            PlayerPrefs.DeleteKey(key);
            PlayerPrefs.Save();
        }

        public void ClearData()
        {
            foreach (string val in _keys)
            {
                PlayerPrefs.DeleteKey(val);
            }

            PlayerPrefs.Save();
        }
    }
}