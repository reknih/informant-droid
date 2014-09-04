using System;
using System.Collections.Generic;
using System.Text;
using Android.OS;
using Android.Content.Res;
using Android.App;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Net;
using UrlImageViewHelper;
using UntisExp;

namespace vplan
{
	public class NewsAdapter : BaseAdapter<News>
	{
		Android.App.Activity mContext;
		List<News> data = new List<News>();
		Typeface type;
		Typeface light;
		public NewsAdapter(Android.App.Activity amContext, List<News> aData, AssetManager asset) : base(){
			mContext = amContext;
			data.AddRange (aData);
			type  = Typeface.CreateFromAsset (asset, "SourceSansPro-Regular.ttf");
			light = Typeface.CreateFromAsset (asset, "SourceSansPro-Light.ttf");
		}
		public override long GetItemId(int position)
		{
			return position;
		}
		public override News this[int position] {  
			get { return data[position]; }
		}
		public override int Count {
			get { return data.Count; }
		}
		public override View GetView(int position, View convertView, ViewGroup parent){
			if (convertView==null) {
				convertView = mContext.LayoutInflater.Inflate(Resource.Layout.news_row, null);
			}
			News dataEntry = data [position];

			TextView l1 = (TextView)convertView.FindViewById (Resource.Id.firstLine);
			l1.SetTypeface (type, TypefaceStyle.Normal);
			TextView l2 = (TextView)convertView.FindViewById (Resource.Id.secondLine);
			ImageView img = (ImageView)convertView.FindViewById (Resource.Id.Image);
			l2.SetTypeface (light, TypefaceStyle.Normal);
			l1.Text = dataEntry.Title;
			l2.Text = dataEntry.Summary;
			img.SetUrlDrawable (dataEntry.Image);
			return convertView;
		}
	}
}

