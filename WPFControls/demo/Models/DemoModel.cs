using sabatex.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace WPF.Controls.Demo.Models
{
    /// <summary>
    /// Demonstrate entity 
    /// </summary>
    public class DemoModel:ObservableObject
    {
        public Guid Id { get; set; }
        string value;
        public string Value { get=>value; set=>SetProperty(ref this.value,value); }
        public DemoModel()
        {
            Id = Guid.NewGuid();
        }

        public override string ToString()
        {
            return Value;
        }

 



    }
}
