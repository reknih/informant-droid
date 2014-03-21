using System;
using Android.App;
using Android.Util;
using System.Threading;

namespace NotifyService
{
	[Service]
	public class NotifyService : Service
	{
		System.Threading.Timer _timer;

		public override void OnStart (Android.Content.Intent intent, int startId)
		{
			base.OnStart (intent, startId);
			#if DEBUG
			Log.Debug ("SimpleService", "SimpleService started");
			#endif
			DoStuff ();
		}

		public override void OnDestroy ()
		{
			base.OnDestroy ();

			_timer.Dispose ();

			Log.Debug ("SimpleService", "SimpleService stopped");       
		}

		public void DoStuff ()
		{
			_timer = new System.Threading.Timer ((o) => {
				Log.Debug ("SimpleService", "hello from simple service");}
				, null, 0, 4000);
		}

		public override Android.OS.IBinder OnBind (Android.Content.Intent intent)
		{
			throw new NotImplementedException ();
		}
	}

}

