
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Android.Support.V7;
using Android.Support.V7.App;
using Toolbar = Android.Support.V7.Widget.Toolbar;

using UntisExp;

namespace vplan
{
	[Activity (Label = "Nachrichten-Liste")]			
	public class NewsActivity : ActionBarActivity
	{
		protected Fetcher fetcher;
		protected Press p = new Press();
		protected List<News> globNews = new List<News> ();
		protected ListView lv;
		protected int groupn;
		protected Settings settings;
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			settings = new Settings (this);
			SetContentView (Resource.Layout.News);
			var toolbar = FindViewById<Toolbar> (Resource.Id.toolbar);
			//Toolbar will now take on default actionbar characteristics
			SetSupportActionBar (toolbar);
			SupportActionBar.Title = "Nachrichten";
			SupportActionBar.SetDisplayHomeAsUpEnabled (true);
			SupportActionBar.SetHomeButtonEnabled (true);
			lv = FindViewById<ListView>(Resource.Id.lv);
			lv.ItemClick += delegate(object sender, AdapterView.ItemClickEventArgs e) {
				var t = globNews.ElementAt(e.Position);
				settings.writeNews ("lastClick", t);
				var set = new Intent(this, typeof(NewsItemActivity));
				StartActivity(set);
			};
			try {
				groupn = (int)settings.read ("group");
			} catch {
				groupn = 7;
			}
			getFreshNews ();

			// Create your application here
		}
		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId)
			{
			case Android.Resource.Id.Home:
				Finish();
				return true;

			default:
				return base.OnOptionsItemSelected(item);
			}
		}
		public void refreshList(News nn)
		{
			globNews.Insert (0, nn);
			RunOnUiThread (() => {
				lv.Adapter = new NewsAdapter (this, globNews, Assets);
			});
		}
		protected async void getFreshNews()
		{
			var pd = ProgressDialog.Show(this, "", "Nachrichten werden geladen");
			var newNews = await p.getNews ();
			pd.Dismiss ();
			globNews.AddRange (newNews);
			RunOnUiThread (() => {
				lv.Adapter = new NewsAdapter (this, globNews, Assets);
				fetcher = new Fetcher (refreshList, groupn);
			});
		}
	}
}

