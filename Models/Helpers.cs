using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using Android.App;
using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace TPO_Seminar_Xamarin_Android.Models
{
    public static class Helpers
    {
        public static int UserId;
        public static int StudentId;

        public static bool HasUpcomingEvents(int studentId)
        {
            var dogodek = new DogodekRequest() { StudentId = studentId, TipDogodka = 2 };
            var nameValue = new NameValueCollection()
            {
                {"", JsonConvert.SerializeObject(dogodek)}
            };
            using (var wc = new WebClient())
            {
                var uploadData = wc.UploadValues("http://seminar-1.apphb.com/api/dogodek", nameValue);
                var response = Encoding.Default.GetString(uploadData);

                var stringSerialize = JsonConvert.DeserializeObject<string>(response);
                var model = JsonConvert.DeserializeObject<DogodekResponse>(stringSerialize);
                return model.Dogodki.Any();
            }
        }



        public static void FillFragment(View view, int studentId, int tipDogodka)
        {
            var dogodek = new DogodekRequest() { StudentId = studentId, TipDogodka = tipDogodka };
            var nameValue = new NameValueCollection()
            {
                {"", JsonConvert.SerializeObject(dogodek)}
            };
            using (var wc = new WebClient())
            {
                var uploadData = wc.UploadValues("http://seminar-1.apphb.com/api/dogodek", nameValue);
                var response = Encoding.Default.GetString(uploadData);

                //znebimo se nadleznih backslashev
                var stringSerialize = JsonConvert.DeserializeObject<string>(response);

                //deserialize v pravilno obliko
                var model = JsonConvert.DeserializeObject<DogodekResponse>(stringSerialize);

                var tableLayout = view.FindViewById<TableLayout>(Resource.Id.dogodekTable);

                //delete all except the header
                for(int i = 1 ; i < tableLayout.ChildCount;i++)
                    tableLayout.RemoveViewAt(i);

                var rowLayout = new TableRow.LayoutParams(TableRow.LayoutParams.MatchParent, TableRow.LayoutParams.MatchParent);
                rowLayout.LeftMargin = 15;

                foreach (var item in model.Dogodki)
                {
                    FillDogodekItem(view, rowLayout, tableLayout, item.SubjectName, item.OrderDate);
                }
            }
           
        }

        static void FillDogodekItem(View view, TableRow.LayoutParams rowLayout, TableLayout tableLayout, string subject, DateTime orderDate)
        {
            TableRow row = new TableRow(view.Context);
            row.Id = 0;
            row.LayoutParameters = rowLayout;

            var x = new TableRow.LayoutParams();
            x.Width = 0;
            x.Weight = 1;

            TextView tv = new TextView(view.Context);
            tv.LayoutParameters = x;
            tv.SetTextSize(ComplexUnitType.Dip, 15);
            tv.SetText(subject, TextView.BufferType.Normal);
            tv.Gravity = GravityFlags.Center;

            TextView tv2 = new TextView(view.Context);
            tv2.LayoutParameters = x;
            tv2.SetTextSize(ComplexUnitType.Dip, 15);
            tv2.SetText(orderDate.ToString("dd/MM/yyyy H:mm"), TextView.BufferType.Normal);
            tv2.Gravity = GravityFlags.Center;

            row.AddView(tv);
            row.AddView(tv2);
            tableLayout.AddView(row);
        }

        public static void FillProfesorTable(Context context,TableLayout tableLayout, TableRow.LayoutParams rowLayout, string company, int count, string avgrate)
        {
            TableRow row = new TableRow(context);
            row.Id = 0;
            row.LayoutParameters = rowLayout;

            var x = new TableRow.LayoutParams();
            x.Width = 0;
            x.Weight = 1;

            TextView tv = new TextView(context);
            tv.LayoutParameters = x;
            tv.SetTextSize(ComplexUnitType.Dip, 12);
            tv.SetText(company, TextView.BufferType.Normal);
            tv.Gravity = GravityFlags.Center;

            TextView tv2 = new TextView(context);
            tv2.LayoutParameters = x;
            tv2.SetTextSize(ComplexUnitType.Dip, 12);
            tv2.SetText(count.ToString(), TextView.BufferType.Normal);
            tv2.Gravity = GravityFlags.Center;


            TextView tv3 = new TextView(context);
            tv3.LayoutParameters = x;
            tv3.SetTextSize(ComplexUnitType.Dip, 12);
            tv3.SetText(avgrate, TextView.BufferType.Normal);
            tv3.Gravity = GravityFlags.Center;

            row.AddView(tv);
            row.AddView(tv2);
            row.AddView(tv3);
            tableLayout.AddView(row);
        }

        public static void LoadNotifications(Context ac, string notificatonService)
        {
            var nMgr = (NotificationManager)ac.GetSystemService(notificatonService);
            var notification = new Notification(Resource.Drawable.Icon, "");
            var intent = new Intent(ac, typeof(DogodkiActivity));
            var pendingIntent = PendingIntent.GetActivity(ac, 0, intent, 0);
            notification.SetLatestEventInfo(ac, "Prihajajoèe inštrukcije", "imate prihajajoèe inštrukcije", pendingIntent);
            nMgr.Notify(0, notification);
        }

        public static void RemoveNotifications(Context ac, string notificationService)
        {
            var nMgr = (NotificationManager)ac.GetSystemService(notificationService);
            nMgr.CancelAll();
        }
    }
}