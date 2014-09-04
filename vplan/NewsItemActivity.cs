using System;

using Android.OS;
using Android.App;
using Android.Widget;
using Android.Graphics;
using Android.Content;

using UrlImageViewHelper;
using UntisExp;

namespace vplan
{
	[Activity (Label = "Nachricht")]		
	public class NewsItemActivity : Android.App.Activity
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

			ImageButton bt = FindViewById<ImageButton>(Resource.Id.backbtn);
			bt.Click += (sender, e) => {
				this.OnBackPressed ();
			};
			ImageButton br = FindViewById<ImageButton>(Resource.Id.browsebtn);
			br.Click += (sender, e) => {
				string url = thatThing.Source.AbsoluteUri;
				if (url.IndexOf("http://") == -1 && url.IndexOf("https://") == -1)
					url = "http://" + url;
				Intent browserIntent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(url));
				StartActivity(browserIntent);
			};
			try {
				ActionBar.SetBackgroundDrawable(new Android.Graphics.Drawables.ColorDrawable(Android.Graphics.Color.Rgb(0,31,63)));
			} catch {
			}
		}
	}
}

