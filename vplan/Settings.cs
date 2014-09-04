using System;
using Android.Content;
using UntisExp;

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
		public void writeNews(string key, News value){
			string keyp = value.Title + "^" + value.Image + "^" + value.Content + "^" + value.Source.AbsoluteUri;
			sEdit.PutString (key, keyp);
			sEdit.Commit ();
		}
		public News readNews(string key){
			News n = new News();
			string r;
			if (sCont.Contains (key)) {
				r = sCont.GetString (key, "");
				string[] compute = r.Split ('^');
				n.Title = compute [0];
				n.Image = compute [1];
				n.Content = compute [2];
				n.Source = new Uri(compute[3]);
				n.Refresh ();
			}
			return n;	
		}
	}

}


