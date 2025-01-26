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

namespace Project12
{
    internal class Transfer
    {
        private string date, dest;
        int ammount;

        public Transfer(string date, string dest, int ammount)
        {
            this.date = date;
            this.dest = dest;
            this.ammount = ammount;
        }

        public string Date{ get => date; set => this.date = value; }
        public string Dest { get => dest; set => this.dest = value; }
        public int Amount { get => ammount; set => ammount = value; }

    }
}