using Avalon.Windows.Dialogs;
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

namespace sabatex.WPF.Controls
{
    /// <summary>
    /// Interaction logic for FolderPathWPF.xaml
    /// </summary>
    public partial class FolderPath : UserControl
    {
        public FolderPath()
        {
            InitializeComponent();
        }

        public readonly static DependencyProperty SelectedFolderProperty =
            DependencyProperty.Register(
                nameof(SelectedFolder),
                typeof(string),
                typeof(FolderPath),
                new FrameworkPropertyMetadata(
                    @"C:\temp",
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(OnFolderPathChanged)));
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            
            
            var d = new FolderBrowserDialog();
            // Показываем надпись в наверху диалога.
            d.Title = "Вкажіть каталог для вивантаження податкових";
            //d.Description = Description;
            // Выбираем первоначальную папку.
            d.SelectedPath = SelectedFolder;
            // Показываем диалог.
            if (d.ShowDialog() == true)
            {
                // Изменяем залоговок окна на выбранную папку.
                SelectedFolder = d.SelectedPath;
                //    if (OnDirectoryChange != null)
                //        OnDirectoryChange(this, new EventArgs());
                //
            }

        }


        private static void OnFolderPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FolderPath folderPathWPF = (FolderPath)d;
        }


        public string SelectedFolder
        {
            get =>(string)GetValue(SelectedFolderProperty);
            set =>SetValue(SelectedFolderProperty, value);
        }


    }
}
