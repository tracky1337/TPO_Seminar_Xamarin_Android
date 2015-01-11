using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using TPO_Seminar_Xamarin_Android.Models;

namespace TPO_Seminar_Xamarin_Android
{
    [Activity(Label = "ProfesorActivity")]
    public class ProfesorActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.PregledProfesorjev);

            var tableLayout = FindViewById<TableLayout>(Resource.Id.profesorjiTable);

            //delete all except the header
            for (int i = 1; i < tableLayout.ChildCount; i++)
                tableLayout.RemoveViewAt(i);

            var rowLayout = new TableRow.LayoutParams(TableRow.LayoutParams.MatchParent, TableRow.LayoutParams.MatchParent);
            rowLayout.LeftMargin = 15;

            using (var wc = new WebClient())
            {
                var returnData = wc.DownloadString("http://seminar-1.apphb.com/api/profesor");
                var stringSerialize = JsonConvert.DeserializeObject<string>(returnData);
                var model = JsonConvert.DeserializeObject<ProfesorResponse>(stringSerialize);

                foreach (var item in model.Profesorji)
                {
                    Helpers.FillProfesorTable(BaseContext,tableLayout, rowLayout, item.CompanyName, item.InstructionsCount, item.AvgRating);
                }
            }
        }


    }
}