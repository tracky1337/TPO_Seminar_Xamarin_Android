using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Text;
using Android.Views;
using Android.Widget;
using Android.OS;
using Newtonsoft.Json;
using TPO_Seminar_Xamarin_Android.Models;

namespace TPO_Seminar_Xamarin_Android
{
    [Activity(Label = "TPO instrukcije", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);


            var tv_Email = FindViewById<EditText>(Resource.Id.et_Email);
            var tv_Password = FindViewById<EditText>(Resource.Id.et_Password);

            var tv_Register = FindViewById<TextView>(Resource.Id.tv_Register);
            tv_Register.SetText(Html.FromHtml("<u>Registriraj</u>  se."), TextView.BufferType.Normal);
            tv_Register.Click += delegate
            {
                var activity = new Intent(this, typeof(RegistrationActivity));
                StartActivity(activity);
            };

            var bt_Login = FindViewById<Button>(Resource.Id.bt_Prijava);
            bt_Login.Click += delegate
            {

                var loginModel = Login(tv_Email.Text, tv_Password.Text);
                if (loginModel.Success == 1)
                {
                    //nastavi obvestila
                    var prefs = Application.Context.GetSharedPreferences("TPOinstrukcije", FileCreationMode.Private);
                    if (prefs == null || !prefs.Contains("Obvestila"))
                    {
                        var prefEditor = prefs.Edit();
                        prefEditor.PutBoolean("Obvestila", true);
                        prefEditor.Commit();

                        var th = new Thread(() =>
                        {
                            if (Helpers.HasUpcomingEvents(Helpers.StudentId))
                            {
                                RunOnUiThread(() => Helpers.LoadNotifications(BaseContext, NotificationService));
                            }
                        });
                        th.Start();
                    }
                    else
                    {
                        var naloziObvestila = prefs.GetBoolean("Obvestila", true);
                        if (naloziObvestila)
                        {
                            var th = new Thread(() =>
                            {
                                if (Helpers.HasUpcomingEvents(Helpers.StudentId))
                                {
                                    RunOnUiThread(() => Helpers.LoadNotifications(BaseContext, NotificationService));
                                }
                            });
                            th.Start();
                        }
                        else
                        {
                            Helpers.RemoveNotifications(BaseContext, NotificationService);
                        }
                    }

                    Helpers.UserId = loginModel.Id;
                    Helpers.StudentId = loginModel.StudentId;

                    var activity = new Intent(this, typeof(ContainerActivity));
                    StartActivity(activity);

                }
                else
                {
                    Toast.MakeText(this.BaseContext, loginModel.ErrorMessage, ToastLength.Short).Show();
                }

            };
        }

        private LoginResponse Login(string username, string password)
        {
            var loginModel = new Login() { Username = username, Password = password };
            var nameValue = new NameValueCollection()
            {
                {"", JsonConvert.SerializeObject(loginModel)}
            };
            using (var wc = new WebClient())
            {
                var uploadData = wc.UploadValues("http://seminar-1.apphb.com/api/login", nameValue);
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

