using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sabatex.TaxUA
{
    public interface IOrganization
    {
        string FullName { get; set; }
        bool IS_NP { get; set; }
        //string KODE { get; set; }
        int C_REG { get; set; }
        int C_RAJ { get; set; }
        string FilialNumber { get; set; }
        string Manager { get; set; }
        string ManagerIPN { get; set; }
        string J12010_OutPath { get; set; }
        int J12010_StartNumber { get; set; }
        bool AutoPDV { get; set; }
        string FirmCode { get; set; }
    }
}
