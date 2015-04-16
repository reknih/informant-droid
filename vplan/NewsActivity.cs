using System;
using System.Collections.Generic;
using System.Linq;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

using Android.Support.V7.App;
using Toolbar = Android.Support.V7.Widget.Toolbar;

using UntisExp;
using UntisExp.Containers;
using Activity = UntisExp.Activity;

namespace vplan
{
	[Activity (Label = "Nachrichten-Liste")]			
	public class NewsActivity : ActionBarActivity
	{
	    private Fetcher _fetcher;
        private readonly Press _p = new Press();
        private readonly List<News> _globNews = new List<News>();
        private ListView _lv;
        private int _groupn;
        private Settings _settings;
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			_settings = new Settings (this);
			SetContentView (Resource.Layout.News);
			var toolbar = FindViewById<Toolbar> (Resource.Id.toolbar);
			//Toolbar will now take on default actionbar characteristics
			SetSupportActionBar (toolbar);
			SupportActionBar.Title = "Nachrichten";
			SupportActionBar.SetDisplayHomeAsUpEnabled (true);
			SupportActionBar.SetHomeButtonEnabled (true);
			_lv = FindViewById<ListView>(Resource.Id.lv);
			_lv.ItemClick += delegate(object sender, AdapterView.ItemClickEventArgs e) {
				var t = _globNews.ElementAt(e.Position);
				_settings.WriteNews ("lastClick", t);
				var set = new Intent(this, typeof(NewsItemActivity));
				StartActivity(set);
			};
		    try
		    {
		        _groupn = (int) _settings.Read("group");
		    }
		    catch (Exception e)
		    {
		        if (e is InvalidCastException || e is NullReferenceException)
		            _groupn = 7;
		        throw;
		    }
		    GetFreshNews ();

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

	    private void RefreshList(News nn)
		{
			_globNews.Insert (0, nn);
			RunOnUiThread (() => {
				_lv.Adapter = new NewsAdapter (this, _globNews, Assets);
			});
		}

	    private async void GetFreshNews()
		{
			var pd = ProgressDialog.Show(this, "", "Nachrichten werden geladen");
			var newNews = await _p.GetNews ();
			pd.Dismiss ();
			_globNews.AddRange (newNews);
			RunOnUiThread (() =>
			{
			    _lv.Adapter = new NewsAdapter(this, _globNews, Assets);
				_fetcher = new Fetcher ();
			    _fetcher.RaiseRetreivedNewsItem += (sender, args) =>
			    {
			        RefreshList(args.News);
			    };
			    _fetcher.GetTimes(_groupn, Activity.GetNews);
			});
		}
	}
}

