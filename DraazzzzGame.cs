using CsvHelper;
using Newtonsoft.Json;
using System.Globalization;
using System;
using System.Net;

public class DraazzzzGame
{
    public string walletAddress { get; set;}
    public string BaseUrl { get; set;}


    public DraazzzzGame(string wallet)
    {
        walletAddress = wallet;
        BaseUrl = string.Format("https://blockscout.com/xdai/mainnet/api?module=account&action=tokenlist&address={0}",walletAddress);
        var response = GetRealTokens();

        var a = 3;

    }

    public string GetValue(string url)
    {
        var client = new HttpClient();
        var webRequest = new HttpRequestMessage(HttpMethod.Get, url);

        var response = client.Send(webRequest);

        using var reader = new StreamReader(response.Content.ReadAsStream());
        return reader.ReadToEnd();
    }

    private async Task<string> CallAPIAsync(string baseUrl)
    {
        Console.WriteLine("=> Getting data from : {0}", baseUrl);

        var client = new HttpClient();
        var result = await client.GetStringAsync(baseUrl);

        return result;
    }

    public Root2 GetRealTokens()
    {
        var response = GetValue("https://api.realt.community/v1/token");
        var myDeserializedClass = JsonConvert.DeserializeObject<Root2>(response);
        return myDeserializedClass;
    }

    public async Task<List<RealToken>> GetRealTokensFromWallet()
    {
        return null;

    }

    public async Task<bool> CheckAsync()
    {
        var client = new HttpClient();
        //client.DefaultRequestHeaders.Add("x-api-key", "Syj4wSrL71aasuHntobv19ECxQvJxEDZ7IufOO5u");
        Console.WriteLine("=> Getting data from API");

        var body = await client.GetStringAsync(BaseUrl);
        
        Console.WriteLine("I got the data");
        Console.WriteLine("Processing the report");

        Root data = JsonConvert.DeserializeObject<Root>(body);
       
       //List<Result> res = data.result;
       
        IEnumerable<Result> ires = data.result.AsEnumerable();
       
        var rs2 = ires.Where(d => d.name.Contains("RealToken"));
        int cb = rs2.Count();
        var aaar = rs2.ToList();

        return true;
    }

    private static bool ListCheck<T>(IEnumerable<T> l1, IEnumerable<T> l2)
{
    // TODO: Null parm checks
    if (l1.Intersect(l2).Any())
    {
        Console.WriteLine("matched");
        return true;
    }
    else
    {
        Console.WriteLine("not matched");
        return false;
    }
}
}

//Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Result
    {
        public string balance { get; set; }
        public string contractAddress { get; set; }
        public string decimals { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string symbol { get; set; }
        public string type { get; set; }
    }

    
    public class RealToken
    {
        public string balance { get; set; }
        public string contractAddress { get; set; }
        public string decimals { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string symbol { get; set; }
        public string type { get; set; }
    }

    public class Root
    {
        public string message { get; set; }
        public List<Result> result { get; set; }
        public string status { get; set; }
    }

    public class LastUpdate
    {
        public string date { get; set; }
        public int timezone_type { get; set; }
        public string timezone { get; set; }
    }

    public class MyArray
    {
        public string fullName { get; set; }
        public string shortName { get; set; }
        public string symbol { get; set; }
        public double tokenPrice { get; set; }
        public string currency { get; set; }
        public string uuid { get; set; }
        public string ethereumContract { get; set; }
        public string xDaiContract { get; set; }
        public string gnosisContract { get; set; }
        public LastUpdate lastUpdate { get; set; }
    }

    public class Root2
    {
        public List<MyArray> MyArray { get; set; }
    }

