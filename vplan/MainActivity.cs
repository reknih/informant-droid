using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace vplan
{
	[Activity (Label = "CWS Informant", MainLauncher = true)]
	public class MainActivity : Activity
	{

		private Fetcher fetcher;
		private ListView lv;
		private List<Data> list = new List<Data>();
		private ProgressDialog pd;
		private Settings settings;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);
			lv = FindViewById<ListView>(Resource.Id.lv);
			pd = new ProgressDialog (this);
			pd.SetMessage ("Vertretungen werden geladen");
			pd.Show ();
			settings = new Settings (this);
			ImageButton options = FindViewById<ImageButton> (Resource.Id.button1);
			ImageButton refresh = FindViewById<ImageButton> (Resource.Id.button2);
			refresh.Clickable = false;
			refresh.Click += (sender, e) => {
				fetcher = new Fetcher(this);
				fetcher.getTimes ((int)settings.read("group"), false);
				list.Clear();
			};
			options.Click += (sender, e) => {
				var set = new Intent(this, typeof(SettingsActivity));
				StartActivity(set);
			};
		}
		protected override void OnResume () {
			base.OnResume ();
			pd.SetMessage ("Vertretungen werden geladen");
			pd.Show ();
			try {
				int group = (int)settings.read ("group");
				fetcher = new Fetcher (this);
				fetcher.getTimes (group, false);
				list.Clear();
			} catch {
				var set = new Intent(this, typeof(SettingsActivity));
				StartActivity(set);
			}
		}
		public void refresh(List<Data> v1) {
			RunOnUiThread(() => 
				{
					list.AddRange(v1);
					if (list.Count == 0) {
						list.Add(new Data());
					}
					pd.Dismiss();
					lv.Adapter = new DataAdapter (this, list);
					FindViewById<ImageButton> (Resource.Id.button2).Clickable = true;
				});
		}
		public void clear() {
			RunOnUiThread(() => 
				{
					list.Clear();
					lv.Adapter = new DataAdapter (this, list);
				});
		}
		public void add(Data v1) {
			var l = new List<Data>();
			l.Add(v1);
			refresh(l);
		}
		public void toast(string str) {
			RunOnUiThread (() => {
				pd.Dismiss ();
				Toast.MakeText (this, str, ToastLength.Long).Show ();
			});
		}

	}
}


