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

namespace Project12
{
    internal class Account
    {
        private int remainder;
        private string id;
        private string name;
        private int hashed_password;
       
        public int Remainder{ get => remainder; set => remainder = value; }
        public string Id { get => id; set => id = value; }
        public string Name { get => name;   set => name = value; }
        public int hashed_passward { get => hashed_password; set => hashed_password = value; }

        [JsonConstructor]
        public Account (int remainder, string id, string name, int hashed_password)
        {
            Remainder = remainder;
            Id = id;
            Name = name;
            this.hashed_password = hashed_password;
            Remainder = remainder;
            Id = id;
            Name = name;
            this.hashed_passward = hashed_passward;
        }
    }
}