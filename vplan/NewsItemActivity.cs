using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using UntisExp.Containers;
using UrlImageViewHelper;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Uri = Android.Net.Uri;

namespace vplan
{
	[Activity (Label = "Nachricht")]		
	public class NewsItemActivity : ActionBarActivity
	{
        private Settings _settings;
        private News _thatThing;
        private TextView _main;
        private TextView _title;
		private ImageView _icon;
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.NewsItem);
			var toolbar = FindViewById<Toolbar> (Resource.Id.toolbar);
			//Toolbar will now take on default actionbar characteristics
			SetSupportActionBar (toolbar);
			SupportActionBar.Title = "Nachrichten";
			SupportActionBar.SetDisplayHomeAsUpEnabled (true);
			SupportActionBar.SetHomeButtonEnabled (true);
			_settings = new Settings (this);
			_thatThing = _settings.ReadNews ("lastClick");

			var type  = Typeface.CreateFromAsset (Assets, "SourceSansPro-Regular.ttf");
			var bold  = Typeface.CreateFromAsset (Assets, "SourceSansPro-Bold.ttf");

			Title = _thatThing.SourcePrint;

			_main = FindViewById<TextView> (Resource.Id.mainText);
			_title = FindViewById<TextView> (Resource.Id.title);
			_icon = FindViewById<ImageView> (Resource.Id.icon);
			_main.Text = _thatThing.Content;
			_main.Typeface = type;
			_title.Text = _thatThing.Title;
			_title.Typeface = bold;
			_icon.SetUrlDrawable (_thatThing.Image, Resource.Drawable.notifications);
		}
		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			MenuInflater.Inflate (Resource.Menu.news, menu);
			return base.OnCreateOptionsMenu (menu);
		}
		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId)
			{
			case Resource.Id.web:
				string url = _thatThing.Source.AbsoluteUri;
				if (url.IndexOf("http://", StringComparison.Ordinal) == -1 && url.IndexOf("https://", StringComparison.Ordinal) == -1)
					url = "http://" + url;
				Intent browserIntent = new Intent (Intent.ActionView, Uri.Parse (url));
				StartActivity (browserIntent);
				return true;
			case Android.Resource.Id.Home:
				Finish();
				return true;

			default:
				return base.OnOptionsItemSelected(item);
			}
		}
	}
}

