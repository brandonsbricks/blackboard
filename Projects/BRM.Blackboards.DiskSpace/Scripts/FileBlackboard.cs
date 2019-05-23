using System.Collections.Generic;
using BRM.Blackboards.Interfaces;
using BRM.FileSerializers.Interfaces;
using BRM.TextSerializers.Interfaces;

namespace BRM.Blackboards.Files
{
    public sealed class FileBlackboard : IBlackboard
    {
        private readonly IReadFiles _fileReader;
        private readonly IWriteFiles _fileWriter;
        private readonly ISerializeText _serializer;
        private readonly string _path;
        private readonly Dictionary<string, string> _values;
        
        public FileBlackboard(string path, IReadFiles fileReader, IWriteFiles fileWriter, ISerializeText serializer)
        {
            _path = path;
            _fileReader = fileReader;
            _fileWriter = fileWriter;
            _serializer = serializer;
            
            _values = _fileReader.Read<Dictionary<string, string>>(_path);
        }

        public void Post<T>(string key, T value)
        {
            var stringValue = _serializer.AsString(value);
            if (!_values.ContainsKey(key))
            {
                _values.Add(key, stringValue);
            }
            else
            {
                _values[key] = stringValue;
            }

            _fileWriter.Write(_path, _values);
        }

        public T Get<T>(string key)
        {
            var values = _fileReader.Read<Dictionary<string, string>>(_path);
            var value = values[key];
            return _serializer.AsObject<T>(value);
        }

        public void Delete(string key)
        {
            _values.Remove(key);
            _fileWriter.Write(_path, _values);
        }

        public void ClearData()
        {
            _values.Clear();
            _fileWriter.Write(_path, _values);
        }
    }
}