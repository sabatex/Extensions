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
using System.Windows.Shapes;
using WPF.Controls.Demo.Models;

namespace WPF.Controls.Demo.Views
{
    /// <summary>
    /// Interaction logic for EditComboboxItems.xaml
    /// </summary>
    public partial class EditComboboxItems : Window
    {
        
        public EditComboboxItems()
        {
            InitializeComponent();
        }
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            DataContext = Models.MainWindowModel.TestModel;
        }

        private void ListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Insert)
            {
                Models.MainWindowModel.TestModel.ComboBoxItems.Add(new Models.DemoModel());

            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var value = new DemoModel();
            value.Value = $"New item {value.Id}";
            Models.MainWindowModel.TestModel.ComboBoxItems.Add(value);
            DG.SelectedItem = value;
            DG.ScrollIntoView(value);
            DG.Focus();

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var item = DG.SelectedItem as DemoModel;
            if (item != null)
            {
                Models.MainWindowModel.TestModel.ComboBoxItems.Remove(item);
                if (Models.MainWindowModel.TestModel.SelectedItem == item)
                    Models.MainWindowModel.TestModel.SelectedItem = null;
            }

        }
    }
}
