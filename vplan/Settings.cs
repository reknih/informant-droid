using System;
using Android.Content;
using UntisExp.Containers;
using UntisExp.Interfaces;

namespace vplan
{
	public class Settings : ISettings
	{
		private const string File = "vplan";
		private readonly ISharedPreferences _sCont; 
		private readonly ISharedPreferencesEditor _sEdit;
		public Settings (Context c)
		{
			_sCont = c.GetSharedPreferences (File, 0);
			_sEdit = _sCont.Edit ();
		}

		public object Read(string key){
			object r = null;
			if (_sCont.Contains (key)) {
				r = _sCont.All [key];
			}
			return r;
		}

		public void Write(string key, object value){
            if (value == null) return;
			if (value is int) {
				_sEdit.PutInt (key, (int)value);
			} else if (value is string) {
				_sEdit.PutString (key, (string)value);
			} else if (value is float) {
				_sEdit.PutFloat (key, (float)value);
			} else if (value is long) {
				_sEdit.PutLong (key, (long)value);
			} else if (value is bool) {
				_sEdit.PutBoolean (key, (bool)value);
			} else if (value.GetType() == typeof(System.Collections.Generic.ICollection<string>)) {
				_sEdit.PutStringSet (key, (System.Collections.Generic.ICollection<string>)value);
			}
			_sEdit.Commit ();
		}

		/// <summary>
		/// LOSSY!
		/// Writes the most important elements of a <c>UntisExp.News</c> object to a string and saves it in the preferences dictionary for later serialisation.
		/// </summary>
		/// <param name="key">The key for the news entry</param>
		/// <param name="value">The <c>News</c>-Object to be stringified and written to the dictionary</param>
		public void WriteNews(string key, News value){
			string keyp = value.Title + "^" + value.Image + "^" + value.Content + "^" + value.Source.AbsoluteUri;
			_sEdit.PutString (key, keyp);
			_sEdit.Commit ();
		}

		/// <summary>
		/// LOSSY!
		/// Reads a stringified <c>UntisExp.News</c> object out of the preferences dictionary and does the heavy-lifting of reserializing it.
		/// </summary>
		/// <returns>The essence of the saved <c>News</c> object. Will return empty <c>News</c> if key is not assigned a value.</returns>
		/// <param name="key">Key to retreive.</param>
		public News ReadNews(string key){
			News n = new News();
			string r;
			if (_sCont.Contains (key)) {
				r = _sCont.GetString (key, "");
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


