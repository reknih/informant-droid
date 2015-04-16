using System.Collections.Generic;
using Android.Content.Res;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using UrlImageViewHelper;
using UntisExp.Containers;

namespace vplan
{
	public class NewsAdapter : BaseAdapter<News>
	{
	    readonly Android.App.Activity _mContext;
	    List<News> _data = new List<News>();
	    readonly Typeface _type;
	    readonly Typeface _light;
		public NewsAdapter(Android.App.Activity amContext, List<News> aData, AssetManager asset)
		{
			_mContext = amContext;
			_data.AddRange (aData);
			_type  = Typeface.CreateFromAsset (asset, "SourceSansPro-Regular.ttf");
			_light = Typeface.CreateFromAsset (asset, "SourceSansPro-Light.ttf");
		}
		public override long GetItemId(int position)
		{
			return position;
		}
		public override News this[int position] {  
			get { return _data[position]; }
		}
		public override int Count {
			get { return _data.Count; }
		}
		public override View GetView(int position, View convertView, ViewGroup parent){
			if (convertView==null) {
				convertView = _mContext.LayoutInflater.Inflate(Resource.Layout.news_row, null);
			}
			News dataEntry = _data [position];

			TextView l1 = (TextView)convertView.FindViewById (Resource.Id.firstLine);
			l1.SetTypeface (_type, TypefaceStyle.Normal);
			TextView l2 = (TextView)convertView.FindViewById (Resource.Id.secondLine);
			ImageView img = (ImageView)convertView.FindViewById (Resource.Id.Image);
			l2.SetTypeface (_light, TypefaceStyle.Normal);
			l1.Text = dataEntry.Title;
			l2.Text = dataEntry.Summary;
			img.SetUrlDrawable (dataEntry.Image);
			return convertView;
		}
	}
}

