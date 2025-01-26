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
    internal class Account
    {

        private string name;
        private string hashed_password;
        private int remainder;
        private List<Transfer> transfers;

       
        public int Remainder{ get => remainder; set => remainder = value; }

        public string Name { get => name;   set => name = value; }
        public string Hashed_passward { get => hashed_password; set => hashed_password = value; }
        public List<Transfer> Transfers{ get => transfers; set => transfers = value; }

        [JsonConstructor]
        public Account (string name, string hashed_password, int remainder = 5000, List<Transfer> transfers = null)
        {
            Name = name;
            Hashed_passward = hashed_password;
            Remainder = remainder;
            this.transfers = transfers;
        }
    }
}