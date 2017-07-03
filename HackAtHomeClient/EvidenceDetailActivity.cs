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
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using Newtonsoft.Json;
using HackAtHome.CustomAdapters;
using Android.Webkit;

namespace HackAtHomeClient
{
    [Activity(Label = "@string/ApplicationName", Icon = "@drawable/icon", Theme = "@android:style/Theme.Holo")]
    public class EvidenceDetailActivity : Activity
    {
        public static readonly string EvidenceID = "EvidenceID";
        public static readonly string Token = "Token";
        public static readonly string FullName = "FullName";
        public static readonly string NameLab = "NameLab";
        public static readonly string StatusLab = "StatusLab";
        
        

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EvidenceDetail);

            var lblname = FindViewById<TextView>(Resource.Id.textViewName);
            var nameLab = FindViewById<TextView>(Resource.Id.textNameLab);
            var statusLab = FindViewById<TextView>(Resource.Id.textStatusLab);

            int evidenceID = Intent.GetIntExtra(EvidenceID, 0);
            var token = Intent.GetStringExtra(Token);
            lblname.Text = Intent.GetStringExtra(FullName);


            nameLab.Text = Intent.GetStringExtra(NameLab);
            statusLab.Text = Intent.GetStringExtra(StatusLab);
            EvidenceDetail(token, evidenceID);
        }

        public async void EvidenceDetail(string token, int IDEvidence)
        {
            EvidenceDetail detailLab = new EvidenceDetail();
            var web_view = FindViewById<WebView>(Resource.Id.webViewLab);
            var img = FindViewById<ImageView>(Resource.Id.imageViewLab);
            detailLab = await GetEvidenceByIDAsync(token, IDEvidence);
            string WebViewContent = $"<div style='color:#C1C1C1'>{detailLab.Description}</div>";
            web_view.LoadDataWithBaseURL(null, WebViewContent, "text/html", "utf-8", null);
            web_view.SetBackgroundColor(Android.Graphics.Color.Transparent);
            Koush.UrlImageViewHelper.SetUrlDrawable(img, detailLab.URL);
        }

        public async Task<EvidenceDetail> GetEvidenceByIDAsync(string token, int evidenceID)
        {
            EvidenceDetail Result = null;
            string WebAPIBaseAddress = "https://ticapacitacion.com/hackathome/";
            string URI = $"{WebAPIBaseAddress}api/evidence/getevidencebyid?token={token}&&evidenceid={evidenceID}";

            using (var Client = new HttpClient())
            {
                Client.DefaultRequestHeaders.Accept.Clear();
                Client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var Response = await Client.GetAsync(URI);
                    var ResultWebAPI = await Response.Content.ReadAsStringAsync();

                    if (Response.StatusCode == HttpStatusCode.OK)
                    {
                        Result = JsonConvert.DeserializeObject<EvidenceDetail>(ResultWebAPI);
                    }
                }
                catch (Exception ex)
                {
                    Result = null;
                }
            }
            return Result;
        }
    }
}