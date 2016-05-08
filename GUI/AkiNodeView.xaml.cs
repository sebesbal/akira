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
    public partial class AkiNodeView : UserControl
    {
        public AkiNodeView()
        {
            InitializeComponent();
        }

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            attributes.Items.Clear();
            var element = (XElement)DataContext;
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
