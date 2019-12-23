using System;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#if UNITY_IOS
using FakesbookSDK.Utilities;
#endif

namespace Trackbook.Network
{
    public class LogData
    {
        public class DataObject
        {
            public class FacebookObject
            {
                [JsonProperty("user_id")]
                protected string UserId { get; set; }
                [JsonProperty("advertiser_tracking_enabled")]
                protected bool AdvertiserTrackingEnabled { get; set; }
                [JsonProperty("application_tracking_enabled")]
                protected bool ApplicationTrackingEnabled { get; set; }
                [JsonProperty("extinfo")]
                protected List<string> ExtInfo { get; set; }

                public FacebookObject(string userId = "")
                {
                    UserId = userId;
                    AdvertiserTrackingEnabled = Trackbook.IsTrackingEnabled;
                    ApplicationTrackingEnabled = true;
#if UNITY_IOS
                    ExtInfo = new List<string>
                    {
                        "i2",
                        IOSHelper.PackageName,
                        IOSHelper.PackageVersionCode,
                        IOSHelper.ShortVersionName,
                        IOSHelper.OSVersion,
                        IOSHelper.DeviceModelName,
                        IOSHelper.Locale,
                        IOSHelper.TimeZoneAbbreviation,
                        IOSHelper.CarrierName,
                        Screen.width.ToString(),
                        Screen.height.ToString(),
                        IOSHelper.ScreenDensity,
                        IOSHelper.CPUCores.ToString(),
                        IOSHelper.TotalDiskSpace.ToString(),
                        IOSHelper.RemainingDiskSpace.ToString(),
                        IOSHelper.TimeZone
                    };
#endif
                }
            }

            [JsonProperty("isSandbox")]
            protected bool IsSandbox { get; set; }
            [JsonProperty("transaction_id")]
            protected string TransactionId { get; set; }
            [JsonProperty("advertiser_id")]
            protected string AdvertiserId { get; set; }
            [JsonProperty("productTitle")]
            protected string ProductTitle { get; set; }
            [JsonProperty("product_id")]
            protected string ProductId { get; set; }
            [JsonProperty("valueToSum")]
            protected decimal ValueToSum { get; set; }
            [JsonProperty("currency")]
            protected string Currency { get; set; }
            [JsonProperty("bundleShortVersion")]
            protected string BundleShortVersion { get; set; }
            [JsonProperty("fb")]
            protected FacebookObject Facebook { get; set; }
            [JsonProperty("receipt-data")]
            protected string ReceiptData { get; set; }

            public DataObject(string transactionId,
                string receiptData,
                string productId,
                string productTitle,
                decimal valueToSum,
                string currency,
                string userId = "")
            {
                IsSandbox = false;
                TransactionId = transactionId;
                AdvertiserId = Trackbook.AdvertiserId;
                ProductTitle = productTitle;
                ProductId = productId;
                ValueToSum = valueToSum;
                Currency = currency;
#if UNITY_IOS
                BundleShortVersion = IOSHelper.ShortVersionName;
#endif
                Facebook = new FacebookObject(userId);
                ReceiptData = receiptData;
            }
        }

        [JsonProperty("platform")]
        protected string Platform { get; set; }
        [JsonProperty("data")]
        protected DataObject Data { get; set; }

        public LogData(string transactionId,
            string receiptData,
            string productId,
            string productTitle,
            decimal valueToSum,
            string currency,
            string userId = "")
        {
            DetectPlatform();
            Data = new DataObject(transactionId,
                receiptData,
                productId,
                productTitle,
                valueToSum,
                currency,
                userId);
        }

        protected void DetectPlatform()
        {
            var platform = "unsupported";
            if (Application.isEditor)
            {
                platform = "ios";
            }
            else
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    platform = "android";
                }
                else if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    platform = "ios";
                }
            }

            Platform = platform;
        }
    }
}