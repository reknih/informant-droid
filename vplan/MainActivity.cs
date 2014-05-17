using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.OS;
using UntisExp;

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
			Typeface.Default = Typeface.CreateFromAsset (Assets, "SourceSansPro-Regular.ttf");
			Typeface.DefaultBold = Typeface.CreateFromAsset (Assets, "SourceSansPro-Bold.ttf");
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
				fetcher = new Fetcher(clear, toast, Refresh, add);
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
				fetcher = new Fetcher (clear, toast, Refresh, add);
				fetcher.getTimes (group, false);
				list.Clear();
			} catch {
				var set = new Intent(this, typeof(SettingsActivity));
				StartActivity(set);
			}
		}
		public void Refresh(List<Data> v1) {
			RunOnUiThread(() => 
				{
					list.AddRange(v1);
					if (list.Count == 0) {
						list.Add(new Data());
					}
					pd.Dismiss();
					lv.Adapter = new DataAdapter (this, list, Assets);
					FindViewById<ImageButton> (Resource.Id.button2).Clickable = true;
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
				Toast.MakeText (this, str, ToastLength.Long).Show ();
			});
		}

	}
}


