using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace vplan
{
	public class GroupAdapter : BaseAdapter<Group>
	{
		Activity mContext;
		List<Group> data = null;
		public GroupAdapter(Activity amContext, List<Group> aData) : base(){
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

