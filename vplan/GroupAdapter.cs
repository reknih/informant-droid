using System.Collections.Generic;
using Android.Content.Res;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using UntisExp.Containers;

namespace vplan
{
	public class GroupAdapter : BaseAdapter<Group>
	{
	    readonly Android.App.Activity _mContext;
		List<Group> _data;
	    readonly Typeface _type;
		public GroupAdapter(Android.App.Activity amContext, List<Group> aData, AssetManager asset)
		{
			_type  = Typeface.CreateFromAsset (asset, "SourceSansPro-Regular.ttf");
			_mContext = amContext;
			_data = aData;
		}
		public override long GetItemId(int position)
		{
			return position;
		}
		public override Group this[int position] {  
			get { return _data[position]; }
		}
		public override int Count {
			get { return _data.Count; }
		}
		public override View GetView(int position, View convertView, ViewGroup parent){
			if (convertView==null) {
				convertView = _mContext.LayoutInflater.Inflate(Resource.Layout.list_row, null);
			}
			Group dataEntry = _data [position];
			_mContext.SetTheme (Resource.Drawable.btn);
			TextView l1 = (TextView)convertView.FindViewById (Resource.Id.firstLine);
			l1.SetTypeface (_type, TypefaceStyle.Normal);
			TextView l2 = (TextView)convertView.FindViewById (Resource.Id.secondLine);
			l1.Text = dataEntry.ClassName;
			l2.Visibility = ViewStates.Invisible;
			convertView.Click += (sender, e) => {
				Settings s = new Settings(_mContext);
				s.Write("group", dataEntry.Id + 1);
				_mContext.OnBackPressed();
			};
			return convertView;
		}

	}
}

