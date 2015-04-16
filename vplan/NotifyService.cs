using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using UntisExp;
using UntisExp.Containers;

namespace vplan
{
	[Service]
	public class NotifyService : Service
	{
		private Fetcher _fetcher;
        private Settings _settings;
        private int _group;
        private int _firstStart = 1;
        private int _lastState;

	    public override void OnCreate ()
		{
			base.OnCreate ();
			_settings = new Settings (this);
			_fetcher = new Fetcher (2);
	        _fetcher.RaiseErrorMessage += (sender, args) =>
	        {
	            StopSelf();
	        };

	        _fetcher.RaiseRetreivedScheduleItems += (sender, args) =>
	        {
	            OnReceive(args.Schedule);
	        };

			try
			{
				_group = (int)_settings.Read ("group");
			} catch {
				StopSelf ();
				return;
			}
			try {
                _firstStart = (int)_settings.Read("firstStart");
                _lastState = (int)_settings.Read("lastState");
			}
			catch
			{
			    // ignored
			}
		}
		public override IBinder OnBind(Intent i) {
			return null;
		}
		public override StartCommandResult OnStartCommand (Intent intent, StartCommandFlags flags, int startId)
		{
			if (intent.Action == "setup") {
				RegisterAlarm ();
				if (_firstStart == 1) {
					Notify ("Jetzt auch mit Benachrichtigungen!");
					_firstStart = 0;
					_settings.Write("firstStart", 0);
				}
				StopSelf();
            }
            else if (_settings.Read("group") != null)
            {
				_fetcher.GetTimes (_group);
			} else {
				if (IsAlarmSet ()) {
					return StartCommandResult.NotSticky;
				}
			    RegisterAlarm ();
			    return StartCommandResult.NotSticky;
			}
			return StartCommandResult.NotSticky;
		}

	    private void OnReceive(List<Data> l)
		{
			if (l.Count > 0 && l.Count != _lastState) {
				string notTxt;
				if (l.Count == 1) {
					notTxt = "Es gibt eine neue Vertretung";
				} else  {
					notTxt = "Es gibt " + l.Count + " neue Vertretungen";
				}
				_settings.Write ("lastState", l.Count);
				Notify (notTxt);
			}
		}

	    private void Notify(string notTxt)
		{
			var nMgr = (NotificationManager)GetSystemService (NotificationService);
	        var pendingIntent = PendingIntent.GetActivity (this, 0, new Intent (this, typeof(MainActivity)), 0);
			NotificationCompat.Builder builder = new NotificationCompat.Builder (this)
				.SetContentTitle ("CWS Informant")
				.SetContentText (notTxt)
				.SetSmallIcon (Resource.Drawable.notifications)
				.SetContentIntent(pendingIntent);
			var notification = builder.Build();
			nMgr.Notify (0, notification);
			if (IsAlarmSet ()) {
			} else {
				RegisterAlarm ();
			}
		}

	    private bool IsAlarmSet ()
		{
			return ((PendingIntent.GetBroadcast (this, 0, new Intent (this, typeof(NotifyService)), PendingIntentFlags.NoCreate)) != null);
		}

	    private void RegisterAlarm ()
		{
			# if DEBUG
			const long iv = (AlarmManager.IntervalFifteenMinutes / 15) * 2;
			# else
			Random rnd = new Random ();
			var iv = AlarmManager.IntervalDay / 4;
			iv += rnd.Next(1200000);
			# endif
				
			AlarmManager alm = (AlarmManager)GetSystemService(AlarmService);

			alm.SetInexactRepeating(AlarmType.ElapsedRealtimeWakeup, iv, iv, PendingIntent.GetBroadcast(this, 0, new Intent (this, typeof(NotifyService)), 0));
		}
	}
}

