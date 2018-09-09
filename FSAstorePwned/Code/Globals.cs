using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.IO;
using System.Web.Caching;
using System.Web.Configuration;
using FSAstorePwned.Models;
using System.Security.Cryptography;

namespace FSAstorePwned.Code
{
    public class Globals
    {
        // Api
        public const string apiBaseAddress = "https://api.pwnedpasswords.com/range/";

        // Client
        private static HttpClient _Client;
        public static HttpClient Client { get { return _Client ?? CreateNewClient(); } }

        // Handle Reusable client
        private static HttpClient CreateNewClient()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(Globals.apiBaseAddress);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _Client = client;
            return client;
        }

        // Return password model to be consumed
        public static PasswordPnwed passwordCheck(PasswordPnwed model)
        {
            // variables
            string password = model.Password.ToString();
            string fullHexString = SHA1Hash(password);
            string prefixHexString = fullHexString.Substring(0, 5).Trim();
            string sufixHexString = fullHexString.Substring(5).Trim();

            // Get results from API
            var returnSuffixes = SendAPIRequest(prefixHexString);

            // Initiate return item and set defaults
            PasswordPnwed returnItem = new PasswordPnwed();
            returnItem.retHex = "Not Pwned";
            returnItem.retCount = "0";


            // Loop through results to find matches
            foreach (KeyValuePair<string, string> entry in returnSuffixes)
            {
                if (entry.Key.ToString() == sufixHexString.ToUpper())
                {
                    returnItem.retHex = entry.Key.ToString();
                    returnItem.retCount = entry.Value.ToString();
                }
            }

            return returnItem;
        }

        private static Dictionary<string, string> SendAPIRequest(string prefix)
        {
            try
            {
                // setup buckets
                Dictionary<string, string> _dic = new Dictionary<string, string>();
                string[] dataReturn = { };
                string apiReturn = "";

                HttpResponseMessage response = Client.GetAsync(prefix).Result;

                if (response.IsSuccessStatusCode)
                {
                    Task<string> result = response.Content.ReadAsStringAsync();
                    apiReturn = result.Result;

                    // Split string to get each suffix
                    dataReturn = apiReturn.Split(
                            new[] { "\r\n", "\r", "\n" },
                            StringSplitOptions.None
                        );

                    // Split each suffix string to hex and count and add to dictionary
                    for (int i = 0; i < dataReturn.Length; i++)
                    {
                        string[] item = dataReturn[i].ToString().Split(new[] { ":" }, StringSplitOptions.None);
                        _dic.Add(item[0], item[1]);
                    }
                }

                return _dic;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static string SHA1Hash(string s)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(s);

            var sha1 = SHA1.Create();
            byte[] hashBytes = sha1.ComputeHash(bytes);

            return HexString(hashBytes);
        }

        private static string HexString(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }
            return sb.ToString();
        }

    }
}