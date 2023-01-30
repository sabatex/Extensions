using sabatex.Extensions;
using sabatex.Extensions.DateTimeExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace WPF.Controls.Demo.Models
{
    public class MainWindowModel:ObservableObject
    {
        public BindingList<DemoModel> ComboBoxItems { get; set; } = new BindingList<DemoModel>();
        public DemoModel SelectedItem {
            get=>selectedItem;
            set=>SetProperty(ref selectedItem,value); }

        string test;
        public string FolderPath {
            get=>test;
            set=>SetProperty(ref test,value);
        }


        public Period? DatePeriod {
            get=>period;
            set=>SetProperty(ref period,value);
        } 
        private Period? period;

        public MainWindowModel()
        {
            ComboBoxItems = new BindingList<DemoModel>
            {
                new DemoModel{Value= "Test 1" },
                new DemoModel{Value= "Test 2" },
                new DemoModel{Value= "Test 3" },
                new DemoModel{Value= "Test 4" }

            };
            SelectedItem = ComboBoxItems[0];
            DatePeriod = new Period(new DateTime?(),new DateTime?());
            FolderPath = System.Environment.CurrentDirectory;
        }

        public static MainWindowModel TestModel { get; set; } = new MainWindowModel();

        private DemoModel selectedItem;

        private string selectedFolder;

    }
}
