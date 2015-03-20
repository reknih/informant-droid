using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V7.App;

using UntisExp;

namespace vplan
{
	[Service]
	public class NotifyService : Service
	{
		protected Fetcher fetcher;
		protected Settings settings;
		protected int group;
		protected int firstStart = 1;
		protected int lastState = 0;
		public NotifyService ()
		{
		}
		public override void OnCreate ()
		{
			base.OnCreate ();
			settings = new Settings (this);
			fetcher = new Fetcher (StopSelf, OnReceive, 2);
			try
			{
				group = (int)settings.read ("group");
			} catch {
				StopSelf ();
				return;
			}
			try {
				firstStart = (int)settings.read ("firstStart");
				lastState = (int)settings.read ("lastState");
			} catch {
			}
		}
		public override IBinder OnBind(Intent i) {
			return null;
		}
		public override StartCommandResult OnStartCommand (Android.Content.Intent intent, StartCommandFlags flags, int startId)
		{
			if (intent.Action == "setup") {
				registerAlarm ();
				if (firstStart == 1) {
					notify ("Jetzt auch mit Benachrichtigungen!");
					firstStart = 0;
					settings.write ("firstStart", 0);
				}
				StopSelf();
			} else {
				fetcher.getTimes (group, UntisExp.Activity.ParseFirstSchedule, 30);
			}
			return StartCommandResult.NotSticky;
		}
		public void OnReceive(List<Data> l)
		{
			if (l.Count > 0 && l.Count != lastState) {
				string notTxt;
				if (l.Count == 1) {
					notTxt = "Es gibt eine neue Vertretung";
				} else {
					notTxt = "Es gibt " + l.Count + " neue Vertretungen";
				}
				settings.write ("lastState", l.Count);
				notify (notTxt);
			}
		}
		public override void OnDestroy ()
		{
			base.OnDestroy ();
		}
		protected void notify(string notTxt)
		{
			var nMgr = (NotificationManager)GetSystemService (NotificationService);
			var notification = new Notification (Resource.Drawable.notifications, notTxt);
			var pendingIntent = PendingIntent.GetActivity (this, 0, new Intent (this, typeof(MainActivity)), 0);
			NotificationCompat.Builder builder = new NotificationCompat.Builder (this)
				.SetContentTitle ("CWS Informant")
				.SetContentText (notTxt)
				.SetSmallIcon (Resource.Drawable.notifications)
				.SetContentIntent(pendingIntent);
			notification = builder.Build();
			nMgr.Notify (0, notification);
			if (IsAlarmSet())
				return;
		}
		protected bool IsAlarmSet ()
		{
			return PendingIntent.GetBroadcast (this, 0, new Intent (this, typeof(MainActivity)), PendingIntentFlags.NoCreate) != null;
		}
		protected void registerAlarm ()
		{
			Random rnd = new Random ();
			var iv = 120000;
			AlarmManager alm = (AlarmManager)GetSystemService(AlarmService);

			alm.SetInexactRepeating(AlarmType.ElapsedRealtimeWakeup, iv, iv, PendingIntent.GetService(this, 0, new Intent (this, typeof(NotifyService)), 0));
		}
	}
}

