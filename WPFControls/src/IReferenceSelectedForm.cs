using System;
using System.Collections.Generic;
using System.Text;

namespace sabatex.WPF.Controls
{
    public interface IReferenceSelectedForm
    {
        /// <summary>
        /// Get selected item
        /// </summary>
        /// <returns> Selected item or null </returns>
        IReferenceItem GetSelectedReferenceItem();
    }
}
