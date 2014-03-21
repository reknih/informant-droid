using System;
using System.Collections.Generic;
using System.Text;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace vplan
{
	[Activity (Label = "Einstellungen")]			
	public class SettingsActivity : Activity
	{
		private ListView lv;
		private Fetcher fetcher;
		private ProgressDialog pd;
		private Settings setti;
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.Settings);
			lv = FindViewById<ListView>(Resource.Id.lv);
			ImageButton bt = FindViewById<ImageButton>(Resource.Id.button1);
			setti = new Settings (this);
			if (setti.read ("group") != null) {
				bt.Click += (sender, e) => {
					this.OnBackPressed ();
				};
			} else {
				bt.Clickable = false;
			}
			pd = new ProgressDialog (this);
			pd.SetMessage("Klassen werden geladen");
			pd.Show ();

			fetcher = new Fetcher (this);
			fetcher.getClasses();
		}

		public void refresh(List<Group> v1) {
			RunOnUiThread(() => 
				{
					lv.Adapter = new GroupAdapter (this, v1);
					pd.Dismiss();
				});
			if (setti.read ("group") == null)
				toast ("Hallo und willkommen an Board. Es ist noch alles ziemlich neu hier, also schreib uns, wenn du Probleme hast. Aber fürs erste: Hab Spaß. Dein SR.");
		}
		protected void OnListItemClick(ListView l, View v, int position, long id)
		{
			setti.write ("group", position + 1);
			this.OnBackPressed ();
		}
		public void toast(string str) {
			RunOnUiThread (() => {
				Toast.MakeText (this, str, ToastLength.Long).Show ();
				pd.Dismiss ();
			});
		}

	}
}

