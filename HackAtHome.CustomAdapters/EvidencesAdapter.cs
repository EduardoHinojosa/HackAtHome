using Android.Widget;
using HackAtHome.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Views;
using Android.App;

namespace HackAtHome.CustomAdapters
{
    public class EvidencesAdapter : BaseAdapter<Evidence>
    {
        List<Evidence> Items;
        Activity Context;
        int ItemLayoutTemplate;
        int EvidenceTitleViewID;
        int EvidenceStatusViewID;

        public EvidencesAdapter(Activity context, List<Evidence> evidences, int itemLayoutTemplate,
            int evidenceTitleViewID, int evidenceStatusViewID)
        {
            this.Context = context;
            this.Items = evidences;
            this.ItemLayoutTemplate = itemLayoutTemplate;
            this.EvidenceTitleViewID = evidenceTitleViewID;
            this.EvidenceStatusViewID = evidenceStatusViewID;
        }

        public override Evidence this[int position]
        {
            get { return Items[position]; }
        }

        public override int Count
        {
            get { return Items.Count; }
        }

        public override long GetItemId(int position)
        {
            return Items[position].EvidenceID;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var Item = Items[position];
            View ItemView;
            if(convertView == null)
            {
                ItemView = Context.LayoutInflater.Inflate(ItemLayoutTemplate, null);
            }
            else
            {
                ItemView = convertView;
            }

            ItemView.FindViewById<TextView>(EvidenceTitleViewID).Text = Item.Title;
            ItemView.FindViewById<TextView>(EvidenceStatusViewID).Text = Item.Status;

            return ItemView;
        }
    }
}
