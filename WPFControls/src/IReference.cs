using System;
using System.Collections.Generic;
using System.Text;

namespace sabatex.WPF.Controls
{
    public interface IReference<out T>
    {
        IEnumerable<T> GetItems();
        Type GetSelectedForm();
    }
}
