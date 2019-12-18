using Newtonsoft.Json.Linq;

namespace Trackbook
{
    public struct FakesbookData
    {
        public string transactionId;
        public string payload;
        public string productId;
        public double productQuantity;
        public string productTitle;
        public string productDescription;
        public decimal valueToSum;
        public string currency;
        public string userId;

        public FakesbookData(string transactionId,
            string payload,
            string productId,
            double productQuantity,
            string productTitle,
            string productDescription,
            decimal valueToSum,
            string currency,
            string userId = "")
        {
            this.transactionId = transactionId;
            this.payload = payload;
            this.productId = productId;
            this.productQuantity = productQuantity;
            this.productTitle = productTitle;
            this.productDescription = productDescription;
            this.valueToSum = valueToSum;
            this.currency = currency;
            this.userId = userId;
        }

        // Use with UnityIAP Product.receipt
        public FakesbookData(string receipt,
            string productId,
            double productQuantity,
            string productTitle,
            string productDescription,
            decimal valueToSum,
            string currency,
            string userId = "")
        {
            var json = JObject.Parse(receipt);

            transactionId = json["TransactionID"].ToString();
            payload = json["Payload"].ToString();
            this.productId = productId;
            this.productQuantity = productQuantity;
            this.productTitle = productTitle;
            this.productDescription = productDescription;
            this.valueToSum = valueToSum;
            this.currency = currency;
            this.userId = userId;
        }
    }
}
