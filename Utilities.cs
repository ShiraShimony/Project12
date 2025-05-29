using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Project12
{
    internal class Utilities
    {
        public static byte[] GetHash(string inputString)
        {
            using (HashAlgorithm algorithm = SHA256.Create())
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

       

        public async Task<double> GetCurrencyToShekelRateAsync(string currency, int amount)
        {
            if (currency == "ILS") return amount;

            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

            using (HttpClient client = new HttpClient())
            {
                string url = $"https://api.exchangerate.host/convert?access_key=7306466f48dc75d5c491d54e2ebf1807&from={currency}&to=ILS&amount={amount}";


                try
                {
                    var response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        JObject obj = JObject.Parse(json);
                        return obj["result"]?.Value<double>() ?? 0;
                    }
                    else
                    {
                        throw new Exception("HTTP Error: " + response.StatusCode);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Currency fetch failed: " + ex.Message);
                    throw;
                }
            }
        }

    }

}