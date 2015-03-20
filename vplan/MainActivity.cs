using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.OS;
using UntisExp;
using com.refractored.fab;
using Android.Support.V7;
using Android.Support.V7.App;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace vplan
{
	[Activity (Label = "CWS Informant", MainLauncher = true)]
	public class MainActivity : ActionBarActivity
	{

		private Fetcher fetcher;
		private bool fetching;
		private ListView lv;
		private List<Data> list = new List<Data>();
		private ProgressDialog pd;
		private Settings settings;
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			//Typeface.Default = Typeface.CreateFromAsset (Assets, "SourceSansPro-Regular.ttf");
			//Typeface.DefaultBold = Typeface.CreateFromAsset (Assets, "SourceSansPro-Bold.ttf");
			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);
			var toolbar = FindViewById<Toolbar> (Resource.Id.toolbar);
			//Toolbar will now take on default actionbar characteristics
			SetSupportActionBar (toolbar);
			SupportActionBar.Title = "CWS Informant";
			pd = ProgressDialog.Show (this, "", "Vertretungen werden geladen" );
			lv = FindViewById<ListView>(Resource.Id.lv);
			var fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
			fab.Click += (sender, e) => {
				var set1 = new Intent(this, typeof(NewsActivity));
				StartActivity(set1);
			};

			if (Build.VERSION.SdkInt >= BuildVersionCodes.IceCreamSandwich)
				fab.AttachToListView (lv);

			settings = new Settings (this); 
			try {
				int not = (int)settings.read("notifies");
			} catch {
				StartService (new Intent ("setup", Android.Net.Uri.Parse (VConfig.url), this, typeof(NotifyService)));
				settings.write ("notifies", 1);
			}
			try {
				int group = (int)settings.read ("group");
				fetcher = new Fetcher (clear, toast, Refresh, add);
				if (!fetching) {
					fetcher.getTimes (group);
					fetching = true;
				}
				list.Clear();
			} catch {
				var set = new Intent(this, typeof(SettingsActivity));
				StartActivity(set);
			}

		}
		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			MenuInflater.Inflate (Resource.Menu.home, menu);
			return base.OnCreateOptionsMenu (menu);
		}
		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			switch (item.ItemId) {
			case Resource.Id.button1:
				var set = new Intent(this, typeof(SettingsActivity));
				StartActivity(set);
				break;
			case Resource.Id.button2:
				fetcher = new Fetcher(clear, toast, Refresh, add);
				if (!fetching) {
					fetcher.getTimes ((int)settings.read("group"), UntisExp.Activity.ParseFirstSchedule);
					fetching = true;
					list.Clear();
					pd = ProgressDialog.Show(this, "", "Vertretungen werden geladen" );
				}

				break;
			default:
				break;
			}
			return true;
		}
		protected override void OnResume ()
		{
			base.OnResume ();
			try {
				int group = (int)settings.read ("group");
				fetcher = new Fetcher (clear, toast, Refresh, add);
				if (!fetching) {
					fetcher.getTimes (group);
					fetching = true;
				}
				list.Clear();
			} catch {
			}
		}
		public void Refresh(List<Data> v1) {
			RunOnUiThread(() => 
				{
					list.AddRange(v1);
					pd.Dismiss();
					try {
						if (list.Count == 0) {
							list.Add(new Data());
						} else if (v1[0].Line1 == list[0].Line1 && list[0].Line1 == "Keine Vertretungen."){
							return;
						} else if (list[0].Line1 == "Keine Vertretungen." && v1.Count > 0) {
							list.RemoveAt(0);
						}
					} catch {}
					fetching = false;
					lv.Adapter = new DataAdapter (this, list, Assets);
					//FindViewById<ImageButton> (Resource.Id.button2).Clickable = true;
				});
		}
		public void clear() {
			RunOnUiThread(() => 
				{
					list.Clear();
					lv.Adapter = new DataAdapter (this, list, Assets);
				});
		}
		public void add(Data v1) {
			var l = new List<Data>();
			l.Add(v1);
			Refresh(l);
		}
		public void toast(string t, string str, string i) {
			RunOnUiThread (() => {
				pd.Dismiss ();
				//Toast.MakeText (this, str, ToastLength.Long).Show ();
			});
		}

	}
}


