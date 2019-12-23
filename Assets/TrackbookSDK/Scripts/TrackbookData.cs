using Newtonsoft.Json.Linq;

namespace Trackbook
{
    public struct TrackbookData
    {
        public string transactionId;
        public string receiptData;
        public string productId;
        public string productTitle;
        public decimal valueToSum;
        public string currency;
        public string userId;

        public TrackbookData(string transactionId,
            string receiptData,
            string productId,
            string productTitle,
            decimal valueToSum,
            string currency,
            string userId = "")
        {
            this.transactionId = transactionId;
            this.receiptData = receiptData;
            this.productId = productId;
            this.productTitle = productTitle;
            this.valueToSum = valueToSum;
            this.currency = currency;
            this.userId = userId;
        }
    }
}
