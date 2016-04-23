using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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

        public void Load(string file_xml)
        {
            var doc = XDocument.Load(file_xml);
            XElement e = doc.Root;
            var item = CreateItem(e);
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
        
        private void treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            attributes.Items.Clear();
            var item = (TreeViewItem)e.NewValue;
            var element = (XElement)item.DataContext;
            codeView.DataContext = null;
            foreach (var attribute in element.Attributes())
            {
                if (attribute.Name == "code")
                {
                    codeView.DataContext = attribute;
                }
                else
                {
                    attributes.Items.Add(attribute);
                }
            }
        }
    }
}
