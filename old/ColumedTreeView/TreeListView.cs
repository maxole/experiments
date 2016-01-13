using System.Windows;
using System.Windows.Controls;

namespace Core.UI
{
    public class TreeListView : TreeView
    {
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new TreeListViewItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is TreeListViewItem;
        }

        #region Public Properties

        /// <summary> GridViewColumn List</summary>
        public GridViewColumnCollection Columns
        {
            get { return _columns ?? (_columns = new GridViewColumnCollection()); }
        }

        private GridViewColumnCollection _columns;

        #endregion
    }
}
