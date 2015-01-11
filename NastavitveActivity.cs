using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TPO_Seminar_Xamarin_Android.Models;

namespace TPO_Seminar_Xamarin_Android
{
    [Activity(Label = "NastavitveActivity")]
    public class NastavitveActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Nastavitve);

            var prefs = Application.Context.GetSharedPreferences("TPOinstrukcije", FileCreationMode.Private);
            var naloziObvestila = prefs.GetBoolean("Obvestila", true);

            var cb_Obvestila = FindViewById<CheckBox>(Resource.Id.cb_Obvestila);
            cb_Obvestila.Checked = naloziObvestila;
            cb_Obvestila.CheckedChange += cb_Obvestila_CheckedChange;

            var bt_Obvestila_Nazaj = FindViewById<Button>(Resource.Id.bt_Obvestila_Nazaj);
            bt_Obvestila_Nazaj.Click += delegate
            {
                var activity = new Intent(this, typeof(ContainerActivity));
                StartActivity(activity);
            };

        }

        private void cb_Obvestila_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            var prefs = Application.Context.GetSharedPreferences("TPOinstrukcije", FileCreationMode.Private);
            var prefEditor = prefs.Edit();
            if (e.IsChecked)
            {
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
                prefEditor.PutBoolean("Obvestila", false);
                prefEditor.Commit();
                Helpers.RemoveNotifications(BaseContext, NotificationService);

            }
        }
    }
}