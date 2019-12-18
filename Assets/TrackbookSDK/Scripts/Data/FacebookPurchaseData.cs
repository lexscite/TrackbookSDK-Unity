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
        private class DataObject
        {
            protected class FacebookObject
            {
                [JsonProperty("bundleShortVersion")]
                private string BundleShortVersion { get; set; }
                [JsonProperty("user_id")]
                private string UserId { get; set; }
                [JsonProperty("advertiser_id")]
                private string AdvertiserId { get; set; }
                [JsonProperty("advertiser_tracking_enabled")]
                private bool AdvertiserTrackingEnabled { get; set; }
                [JsonProperty("application_tracking_enabled")]
                private bool ApplicationTrackingEnabled { get; set; }
                [JsonProperty("productId")]
                private string ProductId { get; set; }
                [JsonProperty("productQuantity")]
                private double ProductQuantity { get; set; }
                [JsonProperty("productTitle")]
                private string ProductTitle { get; set; }
                [JsonProperty("productDescription")]
                private string ProductDescription { get; set; }
                [JsonProperty("valueToSum")]
                private decimal ValueToSum { get; set; }
                [JsonProperty("logTime")]
                private int LogTime { get; set; }
                [JsonProperty("numItems")]
                private int NumItems { get; set; } = 1;
                [JsonProperty("currency")]
                private string Currency { get; set; }
                [JsonProperty("transactionDate")]
                private string TransactionDate { get; set; }
                [JsonProperty("extinfo")]
                private List<string> ExtInfo { get; set; }

                protected FacebookObject(string productId,
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

                private void DetectCommonData()
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
            private JObject ReceiptData { get; set; }
            [JsonProperty("fb")]
            private FacebookObject Facebook { get; set; }

            private DataObject(string transactionId,
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
        private string Platform { get; set; }
        [JsonProperty("isSandbox")]
        private bool IsSandbox { get; set; }
        [JsonProperty("data")]
        private DataObject Data { get; set; }

        internal FacebookPurchaseData(string transactionId,
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

            Trackbook.Log($"FacebookPurchasedData: Platform={Platform}\n" +
                $"IsSandbox = {IsSandbox}\n" +
                $"ReceiptData={Data.ReceiptData}\n" +
                $"BundleShortVersion={Data.Facebook.BundleShortVersion}\n" +
                $"UserId={Data.Facebook.UserId}\n" +
                $"AdvertiserId={Data.Facebook.AdvertiserId}\n" +
                $"AdvertiserTrackingEnabled={Data.Facebook.AdvertiserTrackingEnabled}\n" +
                $"ApplicationTrackingEnabled={Data.Facebook.ApplicationTrackingEnabled}\n" +
                $"ProductId={Data.Facebook.ProductId}\n" +
                $"ProductQuantity={Data.Facebook.ProductQuantity}\n" +
                $"ProductTitle={Data.Facebook.ProductTitle}\n" +
                $"ProductDescription={Data.Facebook.ProductDescription}\n" +
                $"ValueToSum={Data.Facebook.ValueToSum}\n" +
                $"LogTime={Data.Facebook.LogTime}\n" +
                $"NumItems={Data.Facebook.NumItems}\n" +
                $"Currency={Data.Facebook.Currency}\n" +
                $"TransactionDate={Data.Facebook.TransactionDate}\n" +
                $"ExtInfo={Data.Facebook.ExtInfo}");
        }

        private void DetectPlatform()
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