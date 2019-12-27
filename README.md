# TrackbookSDK for Unity
Logs clear purchases data to Facebook's Graph through the Trackbook service.
# Installation
1. Export package with top menu's TrackbookSDK/Export Package (or download it from [trackbook.io](https://trackbook.io))
2. Import package into your project.
3. TrackbookSDK using [JSON.NET](https://assetstore.unity.com/packages/tools/input-management/json-net-for-unity-11347?aid=1100l355n&gclid=CjwKCAiA9JbwBRAAEiwAnWa4Q1VC4WVh0a2kAh4nsOIhhQ1TnIhd1bAbx1FLEAEINPtTpg1kpgfkvxoCj4MQAvD_BwE&pubref=UnityAssets%2ADynNew06%2A1723478829%2A67594162255%2A336302017355%2Ag%2A1t1%2A%2Ab%2Ac%2Agclid%3DCjwKCAiA9JbwBRAAEiwAnWa4Q1VC4WVh0a2kAh4nsOIhhQ1TnIhd1bAbx1FLEAEINPtTpg1kpgfkvxoCj4MQAvD_BwE&utm_source=aff) library to build body of POST requests so make sure you have it in your project.
4. Provide AppID, APIKey and POST schedule file name in Assets/TrackbookSDK/TrackbookSettings.asset
# Usage
Use any of theese methods depending on your needs.
```csharp
// In case you're using UnityIAP
public static void LogPurchase(Product product, string userId = "");

// In case you need a struct
public static void LogPurchase(TrackbookData data);

// In other cases
public static void LogPurchase(string transactionId,
            string receiptData,
            string productId,
            string productTitle,
            decimal valueToSum,
            string currency,
            string userId = "");
```
TrackbookSDK keeps all requests in "schedule file" in case some of them won't reach service so you can use it to resend all of them at any time (for example at app's startup)
```csharp
public static void LogSchedule();
```
