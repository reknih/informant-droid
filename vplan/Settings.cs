using System;
using Android.Content;

namespace vplan
{
	public class Settings
	{
		private string file = "vplan";
		private ISharedPreferences sCont; 
		private ISharedPreferencesEditor sEdit;
		public Settings (Context c)
		{
			sCont = c.GetSharedPreferences (file, 0);
			sEdit = sCont.Edit ();
		}
		public object read(string key){
			object r = null;
			if (sCont.Contains (key)) {
				r = sCont.GetInt (key, 0);
			}
			return r;	
		}
		public void write(string key, int value){
			sEdit.PutInt (key, value);
			sEdit.Commit ();
		}
	}

}


