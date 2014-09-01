using System;

using Android.App;
using Android.Content;

using UntisExp;

namespace vplan
{
	[BroadcastReceiver]
	public class StartupReceiver : BroadcastReceiver
	{
		public override void OnReceive (Context context, Intent intent)
		{
			Intent serviceIntent = new Intent ("setup", Android.Net.Uri.Parse (VConfig.url), context, typeof(NotifyService));
			context.StartService(serviceIntent);
		}
	}
}

