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
	public class GroupAdapter : BaseAdapter<Group>
	{
		Android.App.Activity mContext;
		List<Group> data = null;
		Typeface type;
		public GroupAdapter(Android.App.Activity amContext, List<Group> aData, AssetManager asset) : base(){
			type  = Typeface.CreateFromAsset (asset, "SourceSansPro-Regular.ttf");
			mContext = amContext;
			data = aData;
		}
		public override long GetItemId(int position)
		{
			return position;
		}
		public override Group this[int position] {  
			get { return data[position]; }
		}
		public override int Count {
			get { return data.Count; }
		}
		public override View GetView(int position, View convertView, ViewGroup parent){
			if (convertView==null) {
				convertView = mContext.LayoutInflater.Inflate(Resource.Layout.list_row, null);
			}
			Group dataEntry = data [position];
			mContext.SetTheme (Resource.Drawable.btn);
			TextView l1 = (TextView)convertView.FindViewById (Resource.Id.firstLine);
			l1.SetTypeface (type, TypefaceStyle.Normal);
			TextView l2 = (TextView)convertView.FindViewById (Resource.Id.secondLine);
			l1.Text = dataEntry.ClassName;
			l2.Visibility = ViewStates.Invisible;
			convertView.Click += (object sender, EventArgs e) => {
				Settings s = new Settings(mContext);
				s.write("group", dataEntry.ID + 1);
				mContext.OnBackPressed();
			};
			return convertView;
		}

	}
}

