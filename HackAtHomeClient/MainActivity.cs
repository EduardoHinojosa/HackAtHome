using Android.App;
using Android.Widget;
using Android.OS;
using System.Threading.Tasks;
using HackAtHome.Entities;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using System;
using Android.Content;
using HackAtHome.SAL;

namespace HackAtHomeClient
{
    [Activity(Label = "@string/ApplicationName", MainLauncher = true, Icon = "@drawable/icon",
        Theme = "@android:style/Theme.Holo")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            var Email = FindViewById<EditText>(Resource.Id.editTextEmail);
            var Password = FindViewById<EditText>(Resource.Id.editTextPassword);
            var ValidateButton = FindViewById<Button>(Resource.Id.ValidateButton);

            ValidateButton.Click += async (object sender, EventArgs e) =>
            {
                try
                {
                    string studentEmail = Email.Text;
                    string studentPassword = Password.Text;
                    var Result = await AuthenticateAsync(studentEmail, studentPassword);
                    if (Result != null)
                    {
                        await SendEvidence(studentEmail, studentPassword);
                        Intent intent = new Intent(this, typeof(EvidencesActivity));
                        intent.PutExtra(EvidencesActivity.FullName, Result.FullName);
                        intent.PutExtra(EvidencesActivity.Token, Result.Token);
                        StartActivity(intent);
                    }
                    else
                    {
                        AlertDialog.Builder Builder = new AlertDialog.Builder(this);
                        AlertDialog Alert = Builder.Create();
                        Alert.SetTitle("Validacion de Datos");
                        Alert.SetIcon(Resource.Drawable.Icon);
                        Alert.SetMessage("Hubo un Error - Verifique sus datos");
                        Alert.SetButton("Ok", (s, ev) => { });
                        Alert.Show();
                    }
                }
                catch (Exception ex)
                {
                    AlertDialog.Builder Builder = new AlertDialog.Builder(this);
                    AlertDialog Alert = Builder.Create();
                    Alert.SetTitle("Validacion de Proceso");
                    Alert.SetIcon(Resource.Drawable.Icon);
                    Alert.SetMessage("Hubo un Error - Intentelo de nuevo");
                    Alert.SetButton("Ok", (s, ev) => { });
                    Alert.Show();
                }
            };

        }

        public async Task<ResultInfo> AuthenticateAsync(string studentEmail, string studentPassword)
        {
            ResultInfo Result = null;
            string WebAPIBaseAddress = "https://ticapacitacion.com/hackathome/";
            string EventID = "xamarin30";
            string RequestUri = "api/evidence/Authenticate";

            UserInfo User = new UserInfo
            {
                Email = studentEmail,
                Password = studentPassword,
                EventID = EventID
            };

            using (var Client = new HttpClient())
            {
                Client.BaseAddress = new Uri(WebAPIBaseAddress);
                Client.DefaultRequestHeaders.Accept.Clear();

                Client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var JSONUserInfo = JsonConvert.SerializeObject(User);

                    HttpResponseMessage Response = await Client.PostAsync(RequestUri,
                        new StringContent(JSONUserInfo.ToString(), Encoding.UTF8, "application/json"));

                    var ResultWebAPI = await Response.Content.ReadAsStringAsync();
                    Result = JsonConvert.DeserializeObject<ResultInfo>(ResultWebAPI);
                }
                catch (Exception ex)
                {
                    Result = null;
                }
            }
            return Result;
        }

        public async Task SendEvidence(string studentEmail, string studentPassword)
        {
            var MicrosoftEvidence = new LabItem
            {
                Email = studentEmail,
                Lab = "Hack@Home",
                DeviceId = Android.Provider.Settings.Secure.GetString(
                    ContentResolver, Android.Provider.Settings.Secure.AndroidId)
            };

            var MicrosoftClient = new MicrosoftServiceClient();
            await MicrosoftClient.SenEvidence(MicrosoftEvidence);
        }
    }
}

