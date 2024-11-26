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
        private string id;
        private string name;
        private string hashed_password;
        private int remainder;

        public static int id_counter;
       
        public int Remainder{ get => remainder; set => remainder = value; }
        public string Id { get => id; set => id = value; }
        public string Name { get => name;   set => name = value; }
        public string Hashed_passward { get => hashed_password; set => hashed_password = value; }

        [JsonConstructor]
        public Account (string id, string name, string hashed_password, int remainder = 5000)
        {
            Id = id;
            Name = name;
            Hashed_passward = hashed_password;
            Remainder = remainder;
        }
    }
}