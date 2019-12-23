using System;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#if UNITY_IOS
using FakesbookSDK.Utilities;
#endif

namespace Trackbook.Network.Data
{
    public class FacebookPurchaseData
    {
        public class DataObject
        {
            public class FacebookObject
            {
                [JsonProperty("bundleShortVersion")]
                protected string BundleShortVersion { get; set; }
                [JsonProperty("user_id")]
                protected string UserId { get; set; }
                [JsonProperty("advertiser_id")]
                protected string AdvertiserId { get; set; }
                [JsonProperty("advertiser_tracking_enabled")]
                protected bool AdvertiserTrackingEnabled { get; set; }
                [JsonProperty("application_tracking_enabled")]
                protected bool ApplicationTrackingEnabled { get; set; }
                [JsonProperty("productId")]
                protected string ProductId { get; set; }
                [JsonProperty("productQuantity")]
                protected double ProductQuantity { get; set; }
                [JsonProperty("productTitle")]
                protected string ProductTitle { get; set; }
                [JsonProperty("productDescription")]
                protected string ProductDescription { get; set; }
                [JsonProperty("valueToSum")]
                protected decimal ValueToSum { get; set; }
                [JsonProperty("logTime")]
                protected int LogTime { get; set; }
                [JsonProperty("numItems")]
                protected int NumItems { get; set; } = 1;
                [JsonProperty("currency")]
                protected string Currency { get; set; }
                [JsonProperty("transactionDate")]
                protected string TransactionDate { get; set; }
                [JsonProperty("extinfo")]
                protected List<string> ExtInfo { get; set; }

                public FacebookObject(string productId,
                    double productQuantity,
                    string productTitle,
                    string productDescription,
                    decimal valueToSum,
                    string currency,
                    string userId = "")
                {
                    DetectCommonData();

                    UserId = userId;

                    ProductId = productId;
                    ProductQuantity = productQuantity;
                    ProductTitle = productTitle;
                    ProductDescription = productDescription;
                    ValueToSum = valueToSum;
                    Currency = currency;
                }

                protected void DetectCommonData()
                {
                    LogTime = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                    TransactionDate = DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss+ffff");

                    AdvertiserId = Trackbook.AdvertiserId;
                    AdvertiserTrackingEnabled = Trackbook.IsTrackingEnabled;
                    ApplicationTrackingEnabled = true;
#if UNITY_IOS
                    BundleShortVersion = IOSHelper.ShortVersionName;

                    ExtInfo = new List<string>
                    {
                        "i2",
                        IOSHelper.PackageName,
                        IOSHelper.PackageVersionCode,
                        BundleShortVersion,
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

            [JsonProperty("receiptData")]
            protected JObject ReceiptData { get; set; }
            [JsonProperty("fb")]
            protected FacebookObject Facebook { get; set; }

            public DataObject(string transactionId,
                string payload,
                string productId,
                double productQuantity,
                string productTitle,
                string productDescription,
                decimal valueToSum,
                string currency,
                string userId = "")
            {
                var store = Application.platform == RuntimePlatform.IPhonePlayer ? "AppleAppStore" : "";
                ReceiptData = JObject.Parse($@"{{""Store"":""{store}"", ""TransactionID"": ""{transactionId}"", ""Payload"": ""{payload}""}}");
                Facebook = new FacebookObject(productId,
                    productQuantity,
                    productTitle,
                    productDescription,
                    valueToSum,
                    currency,
                    userId);
            }
        }

        [JsonProperty("platform")]
        protected string Platform { get; set; }
        [JsonProperty("isSandbox")]
        protected bool IsSandbox { get; set; }
        [JsonProperty("data")]
        protected DataObject Data { get; set; }

        public FacebookPurchaseData(string transactionId,
            string payload,
            string productId,
            double productQuantity,
            string productTitle,
            string productDescription,
            decimal valueToSum,
            string currency,
            string userId = "")
        {
            DetectPlatform();
            IsSandbox = false;
            Data = new DataObject(transactionId,
                payload,
                productId,
                productQuantity,
                productTitle,
                productDescription,
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