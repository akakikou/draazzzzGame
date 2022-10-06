using CsvHelper;
using Newtonsoft.Json;
using System.Globalization;
using System;
using System.Net;

public class DraazzzzGame
{
    public string walletAddress { get; set;}
    public string GnosisBaseUrl { get; set;}
    public string EthBaseUrl { get; set;}

    private List<MyArray> officialRealtTokenList { get; set;}


    public DraazzzzGame(string wallet)
    {
        walletAddress = wallet;

        GnosisBaseUrl = "https://blockscout.com/xdai/mainnet/api?module=account&action=tokenlist&address=";
        EthBaseUrl = "https://api.etherscan.io/api?module=account&action=addresstokenbalance&address=0x983e3660c0bE01991785F80f266A84B911ab59b0&page=1&offset=100&apikey=YourApiKeyToken";
        
        List<MyArray> officialRealtTokenList = GetRealTokensFromAPI();
        List<RealToken> walletTokens = GetRealTokensFromMultipleAdresses(new List<String>(){walletAddress});


    }

    public string GetValue(string url)
    {
        var client = new HttpClient();
        var webRequest = new HttpRequestMessage(HttpMethod.Get, url);

        var response = client.Send(webRequest);

        using var reader = new StreamReader(response.Content.ReadAsStream());
        return reader.ReadToEnd();
    }


    public List<MyArray> GetRealTokensFromAPI()
    {
        Console.WriteLine("=> Getting datas from API");
        var response = GetValue("https://api.realt.community/v1/token");
        var tokens = JsonConvert.DeserializeObject<List<MyArray>>(response);

        //Remove OLD tokens
        tokens = tokens.AsEnumerable().Where(t => !t.shortName.StartsWith("OLD-")).ToList();
        //Remove (D) tokens
        tokens = tokens.AsEnumerable().Where(t => !t.symbol.StartsWith("REALTOKEN-D")).ToList();

        Console.WriteLine("{0} RealtToken(s) found from the API", tokens.Count());

        return tokens;
    }

    public List<RealToken> GetRealTokensFromMultipleAdresses(List<String> walletAddresses)
    {
        List<RealToken> result = new List<RealToken>();

        foreach(string address in walletAddresses)
        {
            Console.WriteLine("=> Getting datas from {0}...", address);
            result.AddRange(GetRealTokensFromWallet(address));
            Console.WriteLine("[i] Total RealToken(s) found : {0}", result.Count());
        }
        return result;
    }

    public List<RealToken> GetRealTokensFromWallet(string walletAddress)
    {
        List<RealToken> result = new List<RealToken>();

        ///////////////////////////////////////

        Console.WriteLine("=> Getting datas on gnosis chain");
        var gcRes = GetValue(GnosisBaseUrl+walletAddress);
        Root gctokens = JsonConvert.DeserializeObject<Root>(gcRes);

        IEnumerable<RealToken> gcIRes = gctokens.result.AsEnumerable();
        
        //Remove others tokens
        var gcTokensList = gcIRes.AsEnumerable().Where(t => t.symbol.StartsWith("REALTOKEN")).ToList();
        
        result.AddRange(gcTokensList);
        Console.WriteLine("{0} RealtToken(s) found from inside the wallet {1}", gcTokensList.Count(),walletAddress);

        /////////////////////////////////////// 

        // Console.WriteLine("=> Getting datas on eth chain");
        // var ecRes = GetValue(EthBaseUrl+walletAddress);
        // Root ectokens = JsonConvert.DeserializeObject<Root>(ecRes);

        // IEnumerable<RealToken> ecIRes = ectokens.result.AsEnumerable();
        
        // //Remove others tokens
        // var ecTokensList = ecIRes.AsEnumerable().Where(t => t.symbol.StartsWith("REALTOKEN")).ToList();
        
        // result.AddRange(ecTokensList);
        // Console.WriteLine("{0} RealtToken(s) found from inside the wallet {1}", ecTokensList.Count(),walletAddress);

        ///////////////////////////////////////

        return result;
    }














    // public async Task<bool> CheckAsync()
    // {
    //     var client = new HttpClient();
    //     //client.DefaultRequestHeaders.Add("x-api-key", "Syj4wSrL71aasuHntobv19ECxQvJxEDZ7IufOO5u");
    //     Console.WriteLine("=> Getting data from API");

    //     var body = await client.GetStringAsync(BaseUrl);
        
    //     Console.WriteLine("I got the data");
    //     Console.WriteLine("Processing the report");

    //     Root data = JsonConvert.DeserializeObject<Root>(body);
       
    //    //List<Result> res = data.result;
       
    //     IEnumerable<Result> ires = data.result.AsEnumerable();
       
    //     var rs2 = ires.Where(d => d.name.Contains("RealToken"));
    //     int cb = rs2.Count();
    //     var aaar = rs2.ToList();

    //     return true;
    // }

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
        public List<RealToken> result { get; set; }
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