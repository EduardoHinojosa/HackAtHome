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
using System.Threading.Tasks;
using HackAtHome.Entities;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using Newtonsoft.Json;
using HackAtHome.CustomAdapters;

namespace HackAtHomeClient
{
    [Activity(Label = "@string/ApplicationName", Icon = "@drawable/icon", Theme = "@android:style/Theme.Holo")]
    public class EvidencesActivity : Activity
    {
        public static readonly string FullName = "FullName";
        public static readonly string Token = "Token";
        Complex ListData;
        List<Evidence> Result = null;
        EvidencesAdapter adapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Evidences);
            var lblName = FindViewById<TextView>(Resource.Id.textViewName);

            var name = Intent.GetStringExtra(FullName);
            var token = Intent.GetStringExtra(Token);
            lblName.Text = name;

            FillList(token);
            var ListEvidences = FindViewById<ListView>(Resource.Id.listViewEvidences);

            ListEvidences.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) =>
            {
                var intent = new Intent(this, typeof(EvidenceDetailActivity));
                intent.PutExtra("FullName", lblName.Text);

                intent.PutExtra("EvidenceID", ListData.EvidencesList[e.Position].EvidenceID);
                intent.PutExtra("Token", token);

                intent.PutExtra("NameLab", Result[e.Position].Title);
                intent.PutExtra("StatusLab", Result[e.Position].Status);

                StartActivity(intent);
            };

        }

        protected async void FillList(string token)
        {
            ListData = (Complex)this.FragmentManager.FindFragmentByTag("ListData");
            if (ListData == null)
            {
                ListData = new Complex();
                var FragmentTransaction = this.FragmentManager.BeginTransaction();
                FragmentTransaction.Add(ListData, "ListData");
                FragmentTransaction.Commit();
                
                ListData.EvidencesList = await GetEvidencesAsync(token);
            }

            Result = ListData.EvidencesList;

            var ListEvidences = FindViewById<ListView>(Resource.Id.listViewEvidences);
            if (!IsLandScape(this))
            {
                ListEvidences.Adapter = new EvidencesAdapter(this, Result,
                            Resource.Layout.ListItem, Resource.Id.textView1, Resource.Id.textView2);
            }
            else
            {
                ListEvidences.Adapter = new EvidencesAdapter(this, Result,
                            Resource.Layout.ListItem, Resource.Id.textView1, Resource.Id.textView2);
            }
        }

        public async Task<List<Evidence>> GetEvidencesAsync(string token)
        {
            List<Evidence> Evidences = null;
            string WebAPIBaseAddress = "https://ticapacitacion.com/hackathome/";
            string URI = $"{WebAPIBaseAddress}api/evidence/getevidences?token={token}";

            using (var Client = new HttpClient())
            {
                Client.DefaultRequestHeaders.Accept.Clear();
                Client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var Response = await Client.GetAsync(URI);

                    if (Response.StatusCode == HttpStatusCode.OK)
                    {
                        var ResultWebAPI = await Response.Content.ReadAsStringAsync();
                        Evidences = JsonConvert.DeserializeObject<List<Evidence>>(ResultWebAPI);
                    }
                }
                catch (Exception)
                {
                    Evidences = null;
                }
            }
            return Evidences;
        }

        bool IsLandScape(Activity activity)
        {
            var Orientacion = activity.WindowManager.DefaultDisplay.Rotation;
            return Orientacion == SurfaceOrientation.Rotation90 || Orientacion == SurfaceOrientation.Rotation270;
        }

    }
}