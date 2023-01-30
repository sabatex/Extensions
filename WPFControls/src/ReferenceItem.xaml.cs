using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace sabatex.WPF.Controls
{
    /// <summary>
    /// Interaction logic for ReferenceSelectorControl.xaml
    /// </summary>
    public partial class ReferenceItem : UserControl
    {
        public ReferenceItem()
        {
            InitializeComponent();
        }

        public IReferenceItem Item { get; set; }

        object reference;
        public object Reference
        { 
            get=>reference;
            set
            {
                var t = value.GetType();


            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
