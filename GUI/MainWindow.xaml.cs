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

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string fileName = "test_cs_rule.aki";

        public MainWindow()
        {
            InitializeComponent();
            treeView1.Load(fileName);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            treeView1.Save();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            treeView1.Add();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            akira.akira a = new akira.akira();
            a.RunXml(fileName);
            a.Save("result.xml");
            treeView2.Load("result.xml");
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            treeView1.DeleteSelected();
        }
    }
}
