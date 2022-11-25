using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanCtrl
{
    public class OSDGroup
    {
        public string Name { get; set; }

        public bool IsColor { get; set; } = false;

        public Color Color { get; set; } = Color.White;

        public int Digit { get; set; } = 5;

        private List<OSDItem> mItemList = new List<OSDItem>();
        public List<OSDItem> ItemList
        {
            get { return mItemList; }
        }

        public OSDGroup()
        {

        }

        public string getOSDString(int maxNameLength)
        {
            var osdString = new StringBuilder();

            // Color prefix
            if (IsColor == true)
            {
                osdString.Append("<C=");

                var color = new byte[3];
                color[0] = Color.R;
                color[1] = Color.G;
                color[2] = Color.B;
                osdString.Append(Util.getHexString(color));

                osdString.Append(">");
            }

            // Name
            string name = Name;
            for (int i = name.Length; i < maxNameLength; i++)
            {
                name += " ";
            }

            osdString.Append(name);

            // Color postfix
            if (IsColor == true)
            {
                osdString.Append("<C>");
            }

            // item list
            for (int i = 0; i < mItemList.Count; i++)
            {
                var item = mItemList[i];
                osdString.Append(item.getOSDString(Digit));                
            }

            osdString.Append("\n");
            return osdString.ToString();
        }

        public OSDGroup clone()
        {
            var group = new OSDGroup();
            group.Name = this.Name;
            group.IsColor = this.IsColor;
            group.Color = Color.FromArgb(this.Color.R, this.Color.G, this.Color.B);
            group.Digit = this.Digit;

            for (int i = 0; i < mItemList.Count; i++)
                group.ItemList.Add(mItemList[i].clone());

            return group;
        }
    }
}
