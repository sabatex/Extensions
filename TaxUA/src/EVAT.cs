using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace sabatex.TaxUA
{
    public enum EVAT
    {
        [Description("20%")]
        Default = 20,
        [Description("7%")]
        Country = 7,
        [Description("Export 0%")]
        Export = 901,
        [Description("Import 0%")]
        Import = 902,
        [Description("Free VAT")]
        FreeVAT = 903
    }

}
