using sabatex.WPF.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using WPF.Controls.Demo.Views;

namespace WPF.Controls.Demo.Models
{
    public class DemoReference : IReference<DemoModel>
    {
        public IEnumerable<DemoModel> GetItems()
        {
            yield return new DemoModel() { Value = "Item 1" };
            yield return new DemoModel() { Value = "Item 1" };
            yield return new DemoModel() { Value = "Item 1" };
            yield return new DemoModel() { Value = "Item 1" };
            yield return new DemoModel() { Value = "Item 1" };
        }

        public Type GetSelectedForm()
        {
            return typeof(DemoSelectItem);
        }
    }
}
