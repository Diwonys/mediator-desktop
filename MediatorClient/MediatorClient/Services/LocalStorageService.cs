using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace MediatorClient.Services
{
    static class LocalStorageService
    {
        private static object _locker = new object();
        private static Dictionary<string, JObject> _localData;
        private static string _resourcePath;
        static LocalStorageService()
        {
            _resourcePath = Path.Combine(Path.GetTempPath(), "localConfig.json");
            InitialLoading();
        }

        private static void InitialLoading()
        {
            if (!File.Exists(_resourcePath))
                File.Create(_resourcePath);
            
            lock (_locker)
            {
                using (StreamReader reader = new StreamReader(_resourcePath))
                {
                    var json = reader.ReadToEnd();
                    var resourceDictionary = JsonConvert.DeserializeObject<Dictionary<string, JObject>>(json);
                    if (resourceDictionary == null)
                        _localData = new Dictionary<string, JObject>();
                    else
                        _localData = resourceDictionary;
                }
            }
        }

        private static async Task ImmutableSaveAsync()
        {
            var json = JsonConvert.SerializeObject(_localData, Formatting.Indented);
            using (StreamWriter writer = new StreamWriter(_resourcePath, false))
            {
                await writer.WriteLineAsync(json);
            }
        }

        public static async Task AddOrReplaceAsync(string key, object value)
        {
            if (value == null)
                throw new NullReferenceException();

            if (_localData.ContainsKey(key))
                _localData.Remove(key);

            _localData.Add(key, JObject.FromObject(value));
            await ImmutableSaveAsync();
        }
        public static async Task AddOrReplaceAsync<T>(object value)
        {
            var key = typeof(T).FullName;

            if (value == null)
                throw new NullReferenceException();

            if (_localData.ContainsKey(key))
                _localData.Remove(key);

            _localData.Add(key, JObject.FromObject(value));
            await ImmutableSaveAsync();
        }

        public static async Task RemoveAsync(string key)
        {
            if (!_localData.ContainsKey(key))
                return;
            _localData.Remove(key);
            await ImmutableSaveAsync();
        }

        public static async Task RemoveAsync<T>()
        {
            var key = typeof(T).FullName;

            if (!_localData.ContainsKey(key))
                return;
            _localData.Remove(key);
            await ImmutableSaveAsync();
        }

        public static T Get<T>()
        {
            var key = typeof(T).FullName;
            
            if (!_localData.ContainsKey(key))
                return (T)Activator.CreateInstance(typeof(T));

            return _localData[key].ToObject<T>();
        }
    }
}
