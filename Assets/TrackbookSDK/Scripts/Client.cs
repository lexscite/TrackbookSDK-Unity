using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using UnityEngine;

using Trackbook.Network.Scheduling;

namespace Trackbook.Network
{
    internal static class Client
    {
        internal static RequestScheduler PostScheduler { get; set; }

        static Client()
        {
            PostScheduler = new RequestScheduler(Trackbook.Settings.postScheduleFileName);
        }

        internal static HttpClient _httpClient;
        internal static HttpClient HttpClient
        {
            get
            {
                if (_httpClient == null)
                {
                    _httpClient = new HttpClient();
                    _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{Trackbook.Settings.appId}:{Trackbook.Settings.apiKey}");
                }

                return _httpClient;
            }
        }

        internal static void Schedule(string contents)
        {
            try
            {
                _ = ScheduleAsync(contents);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                throw e;
            }
        }

        internal static async Task<Tuple<HttpResponseMessage, string>> ScheduleAsync(string contents)
        {
            Tuple<HttpResponseMessage, string> result;
            try
            {
                result = await PostScheduler.ScheduleAsync(contents);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                throw e;
            }

            return result;
        }

        internal static void SendPost(string contents)
        {
            try
            {
                _ = SendPostAsync(contents);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                throw e;
            }
        }

        internal static async Task<Tuple<HttpResponseMessage, string>> SendPostAsync(string contents)
        {
            var content = new StringContent(contents);

            Tuple<HttpResponseMessage, string> result;
            try
            {
                result = await SendPostAsync(content);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                throw e;
            }

            Log($"POST request sent\n\nContents:\n{contents}\n\nResponse:\n{result}\n");

            if (result.Item1.IsSuccessStatusCode)
            {
                Log($"POST request: Succeeded");
            }
            else
            {
                Log($"POST request: Failed");
            }

            return result;
        }

        internal static async Task<Tuple<HttpResponseMessage, string>> SendPostAsync(HttpContent content)
        {
            HttpResponseMessage response;
            string result;
            try
            {
                response = await HttpClient.PostAsync(Trackbook.Settings.host, content);
                result = await response.Content.ReadAsStringAsync();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                throw e;
            }

            return new Tuple<HttpResponseMessage, string>(response, result);
        }

        internal static void Log(string message)
        {
            Debug.Log($"<b>FakesbookSDK</b>: {message}");
        }

        internal static void LogError(string message)
        {
            Debug.LogError($"<b>FakesbookSDK</b>: {message}");
        }
    }
}