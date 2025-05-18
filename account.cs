using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Java.Util;

namespace Project12
{
    public class Account
    {
        private string name, hashedPassword;
        private int reminder;
        //private Dictionary<string, Transfer> transfers;

        [JsonProperty(NullValueHandling = NullValueHandling.Include)]
        public string Name { get => name; set => name = value; }

        [JsonProperty(NullValueHandling = NullValueHandling.Include)]
        public string Hashed_password { get => hashedPassword; set => hashedPassword = value; }

        [JsonProperty(NullValueHandling = NullValueHandling.Include)]
        public int Remainder { get => reminder; set => reminder = value; }

        [JsonProperty(NullValueHandling = NullValueHandling.Include)]
        public Dictionary<string, Transfer> Transfers { get; set; } = new Dictionary<string, Transfer>();

        public Account(string name, string hashed_password, List<Transfer> transfers, int remainder = 5000)
        {
            Name = name;
            Hashed_password = hashed_password;
            Remainder = remainder;
            Transfers = new Dictionary<string, Transfer>();
        }

        public Account() { }

        public override string ToString()
        {
            string result = $"Name: {Name}\nRemainder: {Remainder}\n";
            foreach (var kv in Transfers)
            {
                result += $"ID: {kv.Key}\n{kv.Value}\n";
            }
            return result;
        }

        public void addAmmount(int ammunnt)
        {
            Remainder += ammunnt;
        }

    }

}