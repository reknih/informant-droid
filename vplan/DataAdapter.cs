using System;
using System.Collections.Generic;
using System.Text;
using Android.OS;
using Android.Content.Res;
using Android.App;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using UntisExp;

namespace vplan
{
	public class DataAdapter : BaseAdapter<Data>
	{
		Activity mContext;
		List<Data> data = null;
		//Typeface type;
		//Typeface bold;
		//Typeface light;
		public DataAdapter(Activity amContext, List<Data> aData, AssetManager asset) : base(){
			mContext = amContext;
			data = aData;
			//type  = Typeface.CreateFromAsset (asset, "SourceSansPro-Regular.ttf");
			//bold  = Typeface.CreateFromAsset (asset, "SourceSansPro-Bold.ttf");
			//light = Typeface.CreateFromAsset (asset, "SourceSansPro-Light.ttf");
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
			//l1.SetTypeface (type, TypefaceStyle.Normal);

			if (dataEntry.Head == true) {
				//l1.SetTypeface (bold, TypefaceStyle.Bold);
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
			//l2.SetTypeface (light, TypefaceStyle.Normal);
			l1.Text = dataEntry.Line1;
			l2.Text = dataEntry.Line2;
			return convertView;
		}
	}
}

