using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Content;

using Android.Support.V7.App;
using Toolbar = Android.Support.V7.Widget.Toolbar;

using UntisExp;
using UntisExp.Containers;

namespace vplan
{
	[Activity (Label = "Einstellungen", ParentActivity = typeof(MainActivity))]			
	public class SettingsActivity : ActionBarActivity
	{
		private ListView _lv;
		private Fetcher _fetcher;
		private ProgressDialog _pd;
		private Settings _setti;
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.Settings);
			_setti = new Settings (this);
			var toolbar = FindViewById<Toolbar> (Resource.Id.toolbar);
			//Toolbar will now take on default actionbar characteristics
			SetSupportActionBar (toolbar);
			SupportActionBar.Title = "Klasse / Kurs";
			if (_setti.Read ("group") != null) {
				SupportActionBar.SetHomeButtonEnabled (true);
				SupportActionBar.SetDefaultDisplayHomeAsUpEnabled (true);
			}
			_lv = FindViewById<ListView>(Resource.Id.lv);

			_fetcher = new Fetcher ();
		    _fetcher.RaiseErrorMessage += (sender, args) =>
		    {
		        Toast(args.MessageBody);
		    };
		    _fetcher.RaiseRetreivedGroupItems += (sender, args) =>
		    {
                Refresh(args.Groups);
		    };
			#if LEHRER
			try {
				if ((bool)_setti.Read("Teacher") == false)
					throw new NullReferenceException();
				_pd = new ProgressDialog (this);
				_pd.SetMessage("Klassen werden geladen");
				_pd.Show ();

				_fetcher.GetClasses();
			} catch (NullReferenceException) {
				ShowAuthDialogue();
			}
			#endif
			#if !LEHRER
			_pd = new ProgressDialog (this);
			_pd.SetMessage("Klassen werden geladen");
			_pd.Show ();

			_fetcher.GetClasses();
			#endif
		}
		private void ShowAuthDialogue(bool wrongPw = false) {
			var builder = new AlertDialog.Builder(this);
			builder.SetTitle("Passwort");
			var view = LayoutInflater.From(this).Inflate(Resource.Layout.dialog_pw, null);
			if (wrongPw)
				view.FindViewById<TextView>(Resource.Id.description).Text = "Ungültiges Passwort. Versuchen Sie es noch einmal.";
			else
				view.FindViewById<TextView>(Resource.Id.description).Text = VConfig.EnterPw;
			builder.SetView(view);
			builder.SetPositiveButton("Prüfen", ((sender, e) => {
				if (view.FindViewById<EditText>(Resource.Id.password).Text == VConfig.Password)
				{
					_setti.Write("Teacher", true);
					((AlertDialog)sender).Dismiss();
					_pd = new ProgressDialog (this);
					_pd.SetMessage("Klassen werden geladen");
					_pd.Show ();

					_fetcher.GetClasses();
				} else {
					ShowAuthDialogue(true);
				}
			}));
			builder.SetCancelable(false);

			var dia = builder.Create();

			dia.Show();

		}
		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			if (_setti.Read ("group") != null) {
				MenuInflater.Inflate (Resource.Menu.groups, menu);
			}
			return base.OnCreateOptionsMenu (menu);
		}
		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId)
			{
			case Resource.Id.buttonBack:
				if (_setti.Read ("group") != null) {
					var set = new Intent (this, typeof(MainActivity));
					StartActivity (set);
				}
				return true;

			default:
				return base.OnOptionsItemSelected(item);
			}
		}

	    private void Refresh(List<Group> v1) {
			RunOnUiThread(() => 
				{
					_lv.Adapter = new GroupAdapter (this, v1, Assets);
					_pd.Dismiss();
				});
		}

	    // ReSharper disable once UnusedMember.Global
		protected void OnListItemClick(ListView l, View v, int position, long id)
		{
			_setti.Write ("group", position - 1);
			var set = new Intent (this, typeof(MainActivity));
			Intent.PutExtra ("group", position - 1);
			SetResult (Result.Ok);
			StartActivity (set);
		}

	    private void Toast(string str) {
			RunOnUiThread (() => {
				Android.Widget.Toast.MakeText (this, str, ToastLength.Long).Show ();
				_pd.Dismiss ();
			});
		}

	}
}

