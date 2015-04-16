using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Android.OS;

using Android.Support.V7.App;
using Toolbar = Android.Support.V7.Widget.Toolbar;

using UntisExp;
using UntisExp.Interfaces;
using UntisExp.Containers;

using com.refractored.fab;

namespace vplan
{
	[Activity (Label = "CWS Informant", MainLauncher = true)]
	public class MainActivity : ActionBarActivity
	{

		private Fetcher _fetcher;
		private bool _fetching;
		private ListView _lv;
		private readonly List<Data> _list = new List<Data>();
		private ProgressDialog _pd;
		private ISettings _settings;
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			//Typeface.Default = Typeface.CreateFromAsset (Assets, "SourceSansPro-Regular.ttf");
			//Typeface.DefaultBold = Typeface.CreateFromAsset (Assets, "SourceSansPro-Bold.ttf");
			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);
            _fetcher = new Fetcher();
		    _fetcher.RaiseErrorMessage += (sender, args) =>
		    {
                Toast(args.MessageBody);
		    };
            _fetcher.RaiseReadyToClearView += (sender, args) =>
            {
                Clear();
            };
            _fetcher.RaiseRetreivedScheduleItems += (sender, args) =>
            {
                Refresh(args.Schedule);
            };
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
			//Toolbar will now take on default actionbar characteristics
			SetSupportActionBar (toolbar);
			SupportActionBar.Title = "CWS Informant";
			_pd = ProgressDialog.Show (this, "", "Vertretungen werden geladen" );
			_lv = FindViewById<ListView>(Resource.Id.lv);
			var fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
			fab.Click += (sender, e) => {
				var set1 = new Intent(this, typeof(NewsActivity));
				StartActivity(set1);
			};


			fab.AttachToListView (_lv);

			_settings = new Settings (this);
			if (_settings.Read("notifies") == null) {
				StartService (new Intent ("setup", Android.Net.Uri.Parse (VConfig.Url), this, typeof(NotifyService)));
				_settings.Write ("notifies", 1);
			}
			try {
				int group = (int)_settings.Read ("group");
				if (!_fetching) {
					_fetcher.GetTimes (group);
					_fetching = true;
				}
				_list.Clear();
			} catch {
				var set = new Intent(this, typeof(SettingsActivity));
				StartActivityForResult(set, 0);

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
				if (!_fetching) {
					_fetcher.GetTimes ((int)_settings.Read("group"));
					_fetching = true;
					_list.Clear();
					_pd = ProgressDialog.Show(this, "", "Vertretungen werden geladen" );
				}

				break;
			}
			return true;
		}
	    // ReSharper disable once RedundantOverridenMember
		protected override void OnActivityResult (int requestCode, Result resultCode, Intent data) {
			base.OnActivityResult(requestCode, resultCode, data);
		}
		protected override void OnResume ()
		{
			base.OnResume ();
			try {
				int group = (int)_settings.Read("group");
				if (!_fetching) {
					_fetcher.GetTimes (group);
					_fetching = true;
				}
				_list.Clear();
			}
			catch
			{
			    // ignored
			}
		}

	    private void Refresh(List<Data> v1) {
			RunOnUiThread(() => 
				{
					_list.AddRange(v1);
					_pd.Dismiss();
					try {
						if (_list.Count == 0) {
							_list.Add(new Data());
						} else if (v1[0].Line1 == _list[0].Line1 && _list[0].Line1 == "Keine Vertretungen."){
							return;
						} else if (_list[0].Line1 == "Keine Vertretungen." && v1.Count > 0) {
							_list.RemoveAt(0);
						}
					}
					catch
					{
					    // ignored
					}
				    _fetching = false;
					_lv.Adapter = new DataAdapter (this, _list, Assets);
					_lv.Invalidate();
					//FindViewById<ImageButton> (Resource.Id.button2).Clickable = true;
				});
		}

	    private void Clear() {
			RunOnUiThread(() => 
				{
					_list.Clear();

					_lv.Adapter = new DataAdapter (this, _list, Assets);
				});
		}

	    private void Toast(string str) {
			RunOnUiThread (() => {
				_pd.Dismiss ();
				Android.Widget.Toast.MakeText(this, str, ToastLength.Long).Show ();
			});
		}

	}
}


