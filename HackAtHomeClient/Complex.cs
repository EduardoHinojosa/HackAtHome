using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using HackAtHome.Entities;

namespace HackAtHomeClient
{
    public class Complex : Fragment
    {
        public string Name { get; set; }
        public string Token { get; set; }
        public List<Evidence> EvidencesList { get; set; }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RetainInstance = true;
        }
    }
}