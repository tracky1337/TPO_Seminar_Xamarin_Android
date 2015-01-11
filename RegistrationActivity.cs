using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using TPO_Seminar_Xamarin_Android.Models;

namespace TPO_Seminar_Xamarin_Android
{
    [Activity(Label = "RegistrationActivity")]
    public class RegistrationActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Registration);

            var bt_Nazaj = FindViewById<Button>(Resource.Id.bt_Register_Nazaj);
            var bt_Register = FindViewById<Button>(Resource.Id.bt_Register);

            var et_Register_Ime = FindViewById<EditText>(Resource.Id.et_Register_Ime);
            var et_Register_Priimek = FindViewById<EditText>(Resource.Id.et_Register_Priimek);
            var et_Register_UpIme = FindViewById<EditText>(Resource.Id.et_Register_UpIme);
            var et_Register_Email = FindViewById<EditText>(Resource.Id.et_Register_Email);
            var et_Register_Geslo = FindViewById<EditText>(Resource.Id.et_Register_Geslo);
            var et_Register_Sola = FindViewById<EditText>(Resource.Id.et_Register_Sola);
            var et_Register_Rojstvo = FindViewById<EditText>(Resource.Id.et_Register_Rojstvo);






            bt_Nazaj.Click += delegate
            {
                var activity = new Intent(this, typeof(MainActivity));
                StartActivity(activity);
            };

            bt_Register.Click += delegate
            {

                var register = Register(et_Register_Ime.Text,et_Register_Priimek.Text,
                    et_Register_UpIme.Text,et_Register_Email.Text,et_Register_Geslo.Text,
                    et_Register_Sola.Text,et_Register_Rojstvo.Text);
                if (register.Success == 1)
                {
                    Helpers.UserId = register.Id;
                    Helpers.StudentId = register.StudentId;

                    Toast.MakeText(this.BaseContext, "Registracija uspela", ToastLength.Short).Show();
                    var activity = new Intent(this, typeof(ContainerActivity));
                    StartActivity(activity);
                }
                else
                {
                    Toast.MakeText(this.BaseContext, register.ErrorMessage, ToastLength.Short).Show();
                }
            };
        }

        private LoginResponse Register(string ime, string priimek, string upime, string email, string geslo, string sola, string rojstvo)
        {
            int leto;
            if (!Int32.TryParse(rojstvo, out leto))
            {
                return new LoginResponse()
                {
                    Success = -1,
                    ErrorMessage = "Napaèen format leta rojstva"
                };
            }

            var registerModel = new Register()
            {
                BirthYear = leto,
                Email = email,
                FirstName = ime,
                LastName = priimek,
                Password = geslo,
                School = sola,
                UserName = upime
            };
            var nameValue = new NameValueCollection()
            {
                {"", JsonConvert.SerializeObject(registerModel)}
            };
            using (var wc = new WebClient())
            {
                var uploadData = wc.UploadValues("http://seminar-1.apphb.com/api/register", nameValue);
                var response = Encoding.Default.GetString(uploadData);

                //znebimo se nadleznih backslashev
                var stringSerialize = JsonConvert.DeserializeObject<string>(response);

                //deserialize v pravilno obliko
                var model = JsonConvert.DeserializeObject<LoginResponse>(stringSerialize);
                return model;
            }
        }
    }
}