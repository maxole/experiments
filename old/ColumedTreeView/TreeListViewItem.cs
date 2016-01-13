using System.Windows;
using System.Windows.Controls;

namespace Core.UI
{
    public class TreeListViewItem : TreeViewItem
    {
        /// <summary>
        /// Item's hierarchy in the tree
        /// </summary>
        public int Level
        {
            get
            {
                if (_level != -1) return _level;
                var parent = ItemsControlFromItemContainer(this) as TreeListViewItem;
                _level = (parent != null) ? parent.Level + 1 : 0;
                return _level;
            }
        }


        protected override DependencyObject GetContainerForItemOverride()
        {
            return new TreeListViewItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is TreeListViewItem;
        }

        private int _level = -1;

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof (object),
            typeof (TreeListViewItem));

        public object Value
        {
            get { return GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
    }
}