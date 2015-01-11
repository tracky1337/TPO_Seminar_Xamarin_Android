using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace TPO_Seminar_Xamarin_Android
{
    [Activity(Label = "ContainerActivity")]
    public class ContainerActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Container);

            var bt_Container_Dogodki = FindViewById<Button>(Resource.Id.bt_Container_Dogodki);
            var bt_Container_Nazaj = FindViewById<Button>(Resource.Id.bt_Container_Nazaj);
            var bt_Container_Profesorji = FindViewById<Button>(Resource.Id.bt_Container_Profesorji);
            var bt_Container_Nastavtive = FindViewById<Button>(Resource.Id.bt_Container_Nastavitve);

            bt_Container_Dogodki.Click += delegate
            {
                var activity = new Intent(this, typeof(DogodkiActivity));
                StartActivity(activity);
            };

            bt_Container_Nazaj.Click += delegate
            {
                var activity = new Intent(this, typeof(MainActivity));
                StartActivity(activity);
            };

            bt_Container_Profesorji.Click += delegate
            {
                var activity = new Intent(this, typeof(ProfesorActivity));
                StartActivity(activity);
            };

            bt_Container_Nastavtive.Click += delegate
            {
                var activity = new Intent(this, typeof(NastavitveActivity));
                StartActivity(activity);
            };
        }
    }
}