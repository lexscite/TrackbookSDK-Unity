using System;
using UnityEngine;

using Newtonsoft.Json.Linq;

#if UNITY_IOS
using Newtonsoft.Json;
using Trackbook.Network;
using Trackbook.Network.Data;
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
                return Resources.Load<TrackbookSettings>("FakesbookSDKSettings");
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

        public static void LogPurchase(FakesbookData data)
        {
            LogPurchase(data.transactionId,
                data.payload,
                data.productId,
                data.productQuantity,
                data.productTitle,
                data.productDescription,
                data.valueToSum,
                data.currency,
                data.userId);
        }

        public static void LogSchedule()
        {
            Client.PostScheduler.Execute();
        }

        // Use with UnityIAP Product.receipt
        public static void LogPurchase(string receipt,
            string productId,
            double productQuantity,
            string productTitle,
            string productDescription,
            decimal valueToSum,
            string currency,
            string userId = "")
        {
            var json = JObject.Parse(receipt);

            LogPurchase(json["TransactionID"].ToString(),
                json["Payload"].ToString(),
                productId,
                productQuantity,
                productTitle,
                productDescription,
                valueToSum,
                currency,
                userId);
        }

        public static void LogPurchase(string transactionId,
            string payload,
            string productId,
            double productQuantity,
            string productTitle,
            string productDescription,
            decimal valueToSum,
            string currency,
            string userId = "")
        {
#if UNITY_IOS
            if (!Application.isEditor)
            {
                if (IsInitialized)
                {
                    Log($"Logging: TransactionId={transactionId} | Payload={payload} | ProductId={productId} | ProductQuantity={productQuantity} | ProductTitle={productTitle} | ProductDescription={productDescription} | ValueToSum={valueToSum} | Currency={currency} | UserId={userId}");

                    var data = new FacebookPurchaseData(transactionId,
                        payload,
                        productId,
                        productQuantity,
                        productTitle,
                        productDescription,
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

            Log($"Initialized | Host: {Settings.host} | Post schedule file name: {Settings.postScheduleFileName}");

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
