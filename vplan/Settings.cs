using System;
using Android.Content;
using UntisExp;

namespace vplan
{
	public class Settings : ISettings
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
				r = sCont.All [key];
			}
			return r;
		}

		public void write(string key, object value){
			if (value.GetType() == typeof(int)) {
				sEdit.PutInt (key, (int)value);
			} else if (value.GetType() == typeof(string)) {
				sEdit.PutString (key, (string)value);
			} else if (value.GetType() == typeof(float)) {
				sEdit.PutFloat (key, (float)value);
			} else if (value.GetType() == typeof(long)) {
				sEdit.PutLong (key, (long)value);
			} else if (value.GetType() == typeof(bool)) {
				sEdit.PutBoolean (key, (bool)value);
			} else if (value.GetType() == typeof(System.Collections.Generic.ICollection<string>)) {
				sEdit.PutStringSet (key, (System.Collections.Generic.ICollection<string>)value);
			}
			sEdit.Commit ();
		}

		/// <summary>
		/// LOSSY!
		/// Writes the most important elements of a <c>UntisExp.News</c> object to a string and saves it in the preferences dictionary for later serialisation.
		/// </summary>
		/// <param name="key">The key for the news entry</param>
		/// <param name="value">The <c>News</c>-Object to be stringified and written to the dictionary</param>
		public void writeNews(string key, News value){
			string keyp = value.Title + "^" + value.Image + "^" + value.Content + "^" + value.Source.AbsoluteUri;
			sEdit.PutString (key, keyp);
			sEdit.Commit ();
		}

		/// <summary>
		/// LOSSY!
		/// Reads a stringified <c>UntisExp.News</c> object out of the preferences dictionary and does the heavy-lifting of reserializing it.
		/// </summary>
		/// <returns>The essence of the saved <c>News</c> object. Will return empty <c>News</c> if key is not assigned a value.</returns>
		/// <param name="key">Key to retreive.</param>
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


