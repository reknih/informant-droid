using System;
using System.Collections.Generic;
using System.Text;
using Android.OS;
using Android.App;
using Android.Views;
using Android.Widget;

namespace vplan
{
	public class DataAdapter : BaseAdapter<Data>
	{
		Activity mContext;
		List<Data> data = null;
		public DataAdapter(Activity amContext, List<Data> aData) : base(){
			mContext = amContext;
			data = aData;
		}
		public override long GetItemId(int position)
		{
			return position;
		}
		public override Data this[int position] {  
			get { return data[position]; }
		}
		public override int Count {
			get { return data.Count; }
		}
		public override View GetView(int position, View convertView, ViewGroup parent){
			if (convertView==null) {
				convertView = mContext.LayoutInflater.Inflate(Resource.Layout.list_row, null);
			}
			Data dataEntry = data [position];

			TextView l1 = (TextView)convertView.FindViewById (Resource.Id.firstLine);
			if (dataEntry.Head == true) {
				l1.SetTypeface (Android.Graphics.Typeface.Default, Android.Graphics.TypefaceStyle.Bold);
			}
			if (Build.VERSION.SdkInt >= BuildVersionCodes.Honeycomb) {
				if (dataEntry.Entfall == true) {
					l1.SetTextColor (Android.Graphics.Color.DarkRed);
				} else if (dataEntry.Mitbetreung == true) {
					l1.SetTextColor (Android.Graphics.Color.DarkOrange);
				} else if (dataEntry.Veranstaltung == true) {
					l1.SetTextColor (Android.Graphics.Color.DarkGreen);
				} else if (dataEntry.Lehrer != dataEntry.Vertreter) {
					l1.SetTextColor (Android.Graphics.Color.DarkBlue);
				}
				if (dataEntry.Head == true) { l1.SetTextColor (Android.Graphics.Color.Black); }
			}

			TextView l2 = (TextView)convertView.FindViewById (Resource.Id.secondLine);
			l1.Text = dataEntry.Line1;
			l2.Text = dataEntry.Line2;
			return convertView;
		}
	}
}

