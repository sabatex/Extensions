using System;
using System.Collections.Generic;
using System.Text;

namespace sabatex.WPF.Controls
{
    public interface IItemsComboBoxEditor<T>
    {
        public T SelectedItem { get; set; }
    }
}
