using System.Collections.Generic;
using Android.OS;
using Android.Content.Res;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using UntisExp.Containers;

namespace vplan
{
	public class DataAdapter : BaseAdapter<Data>
	{
	    readonly Android.App.Activity _mContext;
	    List<Data> _data;
	    readonly Typeface _type;
	    readonly Typeface _bold;
	    readonly Typeface _light;
		public DataAdapter(Android.App.Activity amContext, List<Data> aData, AssetManager asset)
		{
			_mContext = amContext;
			_data = aData;
			_type  = Typeface.CreateFromAsset (asset, "SourceSansPro-Regular.ttf");
			_bold  = Typeface.CreateFromAsset (asset, "SourceSansPro-Bold.ttf");
			_light = Typeface.CreateFromAsset (asset, "SourceSansPro-Light.ttf");
		}
		public override long GetItemId(int position)
		{
			return position;
		}
		public override Data this[int position] {  
			get { return _data[position]; }
		}
		public override int Count {
			get { return _data.Count; }
		}
		public override View GetView(int position, View convertView, ViewGroup parent){
			if (convertView==null) {
				convertView = _mContext.LayoutInflater.Inflate(Resource.Layout.list_row, null);
			}
			try {
			Data dataEntry = _data [position];

			TextView l1 = (TextView)convertView.FindViewById (Resource.Id.firstLine);
			l1.SetTypeface (_type, TypefaceStyle.Normal);

			if (dataEntry.Head) {
				l1.SetTypeface (_bold, TypefaceStyle.Bold);
			}
			if (Build.VERSION.SdkInt >= BuildVersionCodes.Honeycomb) {
				if (dataEntry.Outage) {
					l1.SetTextColor (Color.DarkRed);
				} else if (dataEntry.Cared) {
					l1.SetTextColor (Color.DarkOrange);
				} else if (dataEntry.Event) {
					l1.SetTextColor (Color.DarkGreen);
				} else if (dataEntry.Teacher != dataEntry.Cover) {
					l1.SetTextColor (Color.DarkBlue);
				}
				if (dataEntry.Head) { l1.SetTextColor (Color.Black); }
			}

			TextView l2 = (TextView)convertView.FindViewById (Resource.Id.secondLine);
			l2.SetTypeface (_light, TypefaceStyle.Normal);
			l1.Text = dataEntry.Line1;
			l2.Text = dataEntry.Line2;
			}
			catch
			{
			    // ignored
			}
		    return convertView;
		}
	}
}

