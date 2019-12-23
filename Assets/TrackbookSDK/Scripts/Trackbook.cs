using System;
using UnityEngine;

using Newtonsoft.Json.Linq;

#if UNITY_IOS
using Newtonsoft.Json;
using Trackbook.Network;
#endif

#if UNITY_PURCHASING
using UnityEngine.Purchasing;
#endif

namespace Trackbook
{
    public static class Trackbook
    {
        public static event Action OnInitialized = () => { };

        internal static string AdvertiserId { get; private set; }
        internal static bool IsTrackingEnabled { get; private set; }

        internal static bool IsInitialized { get; private set; }

        internal static TrackbookSettings Settings
        {
            get
            {
                return Resources.Load<TrackbookSettings>("TrackbookSettings");
            }
        }

        public static void Init()
        {
            if (Application.isEditor)
            {
                OnAdvertiserIdRequestDone("Editor", true, string.Empty);
            }
            else
            {
                Application.RequestAdvertisingIdentifierAsync(OnAdvertiserIdRequestDone);
            }
        }

#if UNITY_PURCHASING
        public static void LogPurchase(Product product, string userId = "")
        {
            var json = JObject.Parse(product.receipt);

            var transactionId = json["TransactionID"].ToString();
            var receiptData = json["Payload"].ToString();

            LogPurchase(transactionId,
                receiptData,
                product.definition.id,
                product.metadata.localizedTitle,
                product.metadata.localizedPrice,
                product.metadata.isoCurrencyCode,
                userId);
        }
#endif

        public static void LogPurchase(TrackbookData data)
        {
            LogPurchase(data.transactionId,
                data.receiptData,
                data.productId,
                data.productTitle,
                data.valueToSum,
                data.currency,
                data.userId);
        }

        public static void LogSchedule()
        {
            Client.PostScheduler.Execute();
        }

        public static void LogPurchase(string transactionId,
            string receiptData,
            string productId,
            string productTitle,
            decimal valueToSum,
            string currency,
            string userId = "")
        {
#if UNITY_IOS
            if (!Application.isEditor)
            {
                if (IsInitialized)
                {
                    Log($"Logging: TransactionId={transactionId} | Payload={receiptData} | ProductId={productId} |  ProductTitle={productTitle}  | ValueToSum={valueToSum} | Currency={currency} | UserId={userId}");

                    var data = new LogData(transactionId,
                        receiptData,
                        productId,
                        productTitle,
                        valueToSum,
                        currency,
                        userId);
                    var requestContents = JsonConvert.SerializeObject(data);

                    Log($"Contents serialized: {requestContents}");
                    Client.Schedule(requestContents);
                }
                else
                {
                    LogWarning("Purchase wasn't logged. Consider wait for Fakesbook.IsInitialized to become \"true\"");
                }
            }
            else
            {
                PretendToLogPurchase();
            }
#else
            PretendToLogPurchase();
#endif
        }

        private static void PretendToLogPurchase()
        {
            Log("Pretending to send logs");
        }

        private static void OnAdvertiserIdRequestDone(string id, bool isTrackingEnabled, string errorMsg)
        {
            AdvertiserId = id;
            IsTrackingEnabled = isTrackingEnabled;
            Log("Advertiser ID requested");
            IsInitialized = true;
            Log($"Initialized | Post schedule file name: {Settings.postScheduleFileName} | App ID: {Settings.appId} | API Key: {Settings.apiKey}");
            OnInitialized.Invoke();
        }

        internal static void Log(string message)
        {
            Debug.Log($"<b>FakesbookSDK</b>: {message}");
        }

        internal static void LogWarning(string message)
        {
            Debug.LogWarning($"<b>Fakesbook</b>: {message}");
        }

        internal static void LogError(string message)
        {
            Debug.LogError($"<b>FakesbookSDK</b>: {message}");
        }
    }
}
