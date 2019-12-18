using System;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Net.Http;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Trackbook.Network.Scheduling
{
    internal class RequestScheduler
    {
        private SchedulerData _data;

        private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);
        private readonly string _savePath;

        internal RequestScheduler(string saveFileName)
        {
            _savePath = Path.Combine(Application.persistentDataPath, saveFileName);
            _data = new SchedulerData();
            Load();
        }

        internal async Task<Tuple<HttpResponseMessage, string>> ScheduleAsync(string content)
        {
            await _semaphoreSlim.WaitAsync();

            _data.contents.Add(content);
            Save();

            Tuple<HttpResponseMessage, string> result;
            try
            {
                result = await Client.SendPostAsync(content);
                if (result.Item1.IsSuccessStatusCode)
                {
                    _data.contents.Remove(content);
                    Save();
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                throw e;
            }
            finally
            {
                _semaphoreSlim.Release();
            }

            return result;
        }

        internal void Execute()
        {
            try
            {
                _ = ExecuteAsync();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                throw e;
            }
        }

        internal async Task ExecuteAsync()
        {
            await _semaphoreSlim.WaitAsync();

            List<string> contents;
            contents = _data.contents.Select(item => (string)item.Clone()).ToList();
            var contentsNum = contents.Count;

            try
            {
                if (contentsNum > 0)
                {
                    Client.Log("Schedule execution started...");

                    foreach (var content in contents)
                    {
                        var result = await Client.SendPostAsync(content);
                        if (result.Item1.IsSuccessStatusCode)
                        {
                            _data.contents.Remove(content);
                            Save();
                        }
                    }

                    Client.Log($"Schedule execution completed: {_data.contents.Count}/{contentsNum} remaining");
                }
            }
            catch (Exception e)
            {
                Client.LogError($"Schedule execution failed: {_data.contents.Count}/{contentsNum} remaining");
                Debug.LogException(e);
                throw e;
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        private void Load()
        {
            if (!File.Exists(_savePath))
            {
                Save();
                Load();
                return;
            }

            string data = File.ReadAllText(_savePath);

            try
            {
                _data = JsonUtility.FromJson<SchedulerData>(data);
            }
            catch
            {
                _data = new SchedulerData();
            }

            if (_data == null)
            {
                _data = new SchedulerData();
            }

            Save();
        }

        private void Save()
        {
            if (!File.Exists(_savePath))
            {
                File.Create(_savePath).Dispose();
            }

            var data = JsonUtility.ToJson(_data);
            File.WriteAllText(_savePath, data);
        }
    }
}
