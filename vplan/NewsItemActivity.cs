using System;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Android.Support.V7;
using Android.Support.V7.App;
using Toolbar = Android.Support.V7.Widget.Toolbar;

using UrlImageViewHelper;
using UntisExp;

namespace vplan
{
	[Activity (Label = "Nachricht")]		
	public class NewsItemActivity : ActionBarActivity
	{
		protected Settings settings;
		protected News thatThing;
		protected TextView main;
		protected TextView title;
		protected ImageView icon;
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
			settings = new Settings (this);
			thatThing = settings.readNews ("lastClick");

			var type  = Typeface.CreateFromAsset (Assets, "SourceSansPro-Regular.ttf");
			var bold  = Typeface.CreateFromAsset (Assets, "SourceSansPro-Bold.ttf");

			Title = thatThing.SourcePrint;

			main = (TextView)FindViewById<TextView> (Resource.Id.mainText);
			title = (TextView)FindViewById<TextView> (Resource.Id.title);
			icon = (ImageView)FindViewById<ImageView> (Resource.Id.icon);
			main.Text = thatThing.Content;
			main.Typeface = type;
			title.Text = thatThing.Title;
			title.Typeface = bold;
			icon.SetUrlDrawable (thatThing.Image, Resource.Drawable.notifications);
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
				string url = thatThing.Source.AbsoluteUri;
				if (url.IndexOf ("http://") == -1 && url.IndexOf ("https://") == -1)
					url = "http://" + url;
				Intent browserIntent = new Intent (Intent.ActionView, Android.Net.Uri.Parse (url));
				StartActivity (browserIntent);
				return true;
				break;
			case Android.Resource.Id.Home:
				Finish();
				return true;

			default:
				return base.OnOptionsItemSelected(item);
			}
		}
	}
}

