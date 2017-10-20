using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Android.Widget;
using Android.Graphics;

namespace App9
{
    class SecondActivityListView:BaseAdapter<string>
    {
        private List<ListViewData> mList;
        private Context mContext;
        private int mRowLayout;
        private int[] mAlternatingColors;

        public SecondActivityListView(Context context, int rowLayout, List<ListViewData> list)
        {
            mList = list;
            mContext = context;
            mRowLayout = rowLayout;
            mAlternatingColors = new int[] { 0xe0cba3, 0x009900 };
        }

       

        public override int Count { get { return mList.Count; }  }

        public override long GetItemId(int position)
        {
            return position;
         }

        public override string this[int position] { get { return mList[position].Id; } }

        public override View GetView(int position, View convertView,ViewGroup parent)
        {
            View row = convertView;
            if (row == null)
            {
                row = LayoutInflater.From(mContext).Inflate(mRowLayout, parent, false);
            }

            row.SetBackgroundColor(GetColorFromInteger(mAlternatingColors[position % mAlternatingColors.Length]));

            TextView txtId = row.FindViewById<TextView>(Resource.Id.txtId);
            txtId.Text = mList[position].Id;

            TextView txtName = row.FindViewById<TextView>(Resource.Id.txtName);
            txtName.Text = mList[position].Name;

            //TextView txtName3 = row.FindViewById<TextView>(Resource.Id.txtGender);
            //txtName3.Text = mList[position].Checkout;

            if ((position % 2) == 1)
            {
                //Green background, set text white
                txtId.SetTextColor(Color.White);
                txtName.SetTextColor(Color.White);
                //txtName3.SetTextColor(Color.White);
               // gender.SetTextColor(Color.White);
            }

            else
            {
                //White background, set text black
                txtId.SetTextColor(Color.Black);
                txtName.SetTextColor(Color.Black);
                //txtName3.SetTextColor(Color.Black);
               // gender.SetTextColor(Color.Black);
            }

            return row;
        }

        private Color GetColorFromInteger(int color)
        {
            return Color.Rgb(Color.GetRedComponent(color), Color.GetGreenComponent(color), Color.GetBlueComponent(color));
        }

    }
}