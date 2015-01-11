using Android.App;
using Android.OS;
using Android.Views;
using TPO_Seminar_Xamarin_Android.Models;

namespace TPO_Seminar_Xamarin_Android
{
    [Activity(Label = "Dogodki")]
    public class DogodkiActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
           // RequestWindowFeature(WindowFeatures.NoTitle);
            ActionBar.SetDisplayShowHomeEnabled(false);
            ActionBar.SetDisplayShowTitleEnabled(false);

            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Dogodki);



            this.ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;
            AddTab("Prihajajoèi", new PrihajajociTabFragment());
            AddTab("Pretekli",  new PretekliTabFragment());
            if (bundle != null)
                this.ActionBar.SelectTab(this.ActionBar.GetTabAt(bundle.GetInt("tab")));
        }

        void AddTab(string tabText,Fragment view)
        {
            var tab = this.ActionBar.NewTab();
            tab.SetText(tabText);

            // must set event handler before adding tab
            tab.TabSelected += delegate(object sender, ActionBar.TabEventArgs e)
            {
                var fragment = FragmentManager.FindFragmentById(Resource.Id.fragmentContainer);
                if (fragment != null)
                    e.FragmentTransaction.Remove(fragment);
                e.FragmentTransaction.Add(Resource.Id.fragmentContainer, view);
            };
            tab.TabUnselected += delegate(object sender, ActionBar.TabEventArgs e)
            {
                e.FragmentTransaction.Remove(view);
            };
            this.ActionBar.AddTab(tab);
        }

    }

    class PrihajajociTabFragment : Fragment
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.DogodekList, container, false);

            Helpers.FillFragment(view,Helpers.StudentId,2);

            return view;
        }

 

    }
    class PretekliTabFragment : Fragment
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.DogodekList, container, false);

            Helpers.FillFragment(view, Helpers.StudentId, 1);
            return view;
        }
    }

}