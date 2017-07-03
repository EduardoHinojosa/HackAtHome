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
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Net;
using Microsoft.WindowsAzure.MobileServices;
using HackAtHome.Entities;

namespace HackAtHome.SAL
{
    public class MicrosoftServiceClient
    {
        MobileServiceClient Client;

        private IMobileServiceTable<LabItem> LabItemTable;

        public async Task SenEvidence(LabItem userEvidence)
        {
            Client = new MobileServiceClient(@"http://xamarin-diplomado.azurewebsites.net/");
            LabItemTable = Client.GetTable<LabItem>();
            await LabItemTable.InsertAsync(userEvidence);
        }
    }
}