using System;
using System.Collections.Generic;
using System.Text;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Content;

using Android.Support.V7;
using Android.Support.V7.App;
using Toolbar = Android.Support.V7.Widget.Toolbar;

using UntisExp;

namespace vplan
{
	[Activity (Label = "Einstellungen", ParentActivity = typeof(MainActivity))]			
	public class SettingsActivity : ActionBarActivity
	{
		private ListView lv;
		private Fetcher fetcher;
		private ProgressDialog pd;
		private Settings setti;
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.Settings);
			setti = new Settings (this);
			var toolbar = FindViewById<Toolbar> (Resource.Id.toolbar);
			//Toolbar will now take on default actionbar characteristics
			SetSupportActionBar (toolbar);
			SupportActionBar.Title = "Klasse / Kurs";
			if (setti.read ("group") != null) {
				SupportActionBar.SetHomeButtonEnabled (true);
				SupportActionBar.SetDefaultDisplayHomeAsUpEnabled (true);
			}
			lv = FindViewById<ListView>(Resource.Id.lv);

			pd = new ProgressDialog (this);
			pd.SetMessage("Klassen werden geladen");
			pd.Show ();

			fetcher = new Fetcher (toast, refresh);
			fetcher.getClasses();
		}
		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			if (setti.read ("group") != null) {
				MenuInflater.Inflate (Resource.Menu.groups, menu);
			}
			return base.OnCreateOptionsMenu (menu);
		}
		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId)
			{
			case Resource.Id.buttonBack:
				if (setti.read ("group") != null) {
					var set = new Intent (this, typeof(MainActivity));
					StartActivity (set);
				}
				return true;

			default:
				return base.OnOptionsItemSelected(item);
			}
		}
		public void refresh(List<Group> v1) {
			RunOnUiThread(() => 
				{
					lv.Adapter = new GroupAdapter (this, v1, Assets);
					pd.Dismiss();
				});
			//

		}
		protected void OnListItemClick(ListView l, View v, int position, long id)
		{
			setti.write ("group", position + 1);
			this.OnBackPressed ();
		}
		public void toast(string t, string str, string i) {
			RunOnUiThread (() => {
				Toast.MakeText (this, str, ToastLength.Long).Show ();
				pd.Dismiss ();
			});
		}

	}
}

