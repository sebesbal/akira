using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace GUI
{
    /// <summary>
    /// Interaction logic for AkiTreeView.xaml
    /// </summary>
    public partial class AkiTreeView : UserControl
    {
        public AkiTreeView()
        {
            InitializeComponent();
        }

        string file_xml;
        XDocument doc;
        public void Load(string file_xml)
        {
            this.file_xml = file_xml;
            doc = XDocument.Load(file_xml);
            var item = CreateItem(doc.Root);
            treeView.Items.Clear();
            treeView.Items.Add(item);
        }

        public TreeViewItem CreateItem(XElement e)
        {
            var item = new TreeViewItem();
            item.DataContext = e;
            item.Header = e.Name;
            item.IsExpanded = true;

            foreach (var child_element in e.Elements())
            {
                var child_item = CreateItem(child_element);
                item.Items.Add(child_item);
            }

            return item;
        }

        //private void treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        //{
        //    attributes.Items.Clear();
        //    var item = (TreeViewItem)e.NewValue;
        //    var element = (XElement)item.DataContext;
        //    codeView.DataContext = null;
        //    foreach (var attribute in element.Attributes())
        //    {
        //        if (attribute.Name == "code")
        //        {
        //            codeView.DataContext = attribute;
        //        }
        //        else
        //        {
        //            attributes.Items.Add(attribute);
        //        }
        //    }
        //}

        //private void treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        //{
        //    attributes.Items.Clear();
        //    var item = (TreeViewItem)e.NewValue;
        //    var element = (XElement)item.DataContext;
        //    codeView.DataContext = null;
        //    foreach (var attribute in element.Attributes())
        //    {
        //        if (attribute.Name == "code")
        //        {
        //            codeView.DataContext = attribute;
        //        }
        //        else
        //        {
        //            attributes.Items.Add(attribute);
        //        }
        //    }
        //}


        
        private void treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            nodeView.DataContext = ((TreeViewItem)e.NewValue).DataContext;
        }

        public void Save()
        {
            doc.Save(file_xml);
        }

        public void Add()
        {
            var item = (TreeViewItem)treeView.SelectedItem;
            var element = (XElement)item.DataContext;
            var new_element = new XElement("fos");
            element.Add(new_element);
            var new_item = CreateItem(new_element);
            item.Items.Add(new_item);
            new_item.IsSelected = true;
        }
    }
}
