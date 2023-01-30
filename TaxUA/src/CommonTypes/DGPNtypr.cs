using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace sabatex.TaxUA.CommonTypes
{
    /// <summary>
    /// ПОРЯДОК заповнення податкової накладної (Наказ Міністерства фінансів України від 01.11.11 № 1379)
    /// п.8. Усі примірники податкових накладних, окремі особливості заповнення яких викладені в підпунктах 8.1-8.4 цього пункту, залишаються в особи, що їх виписала, і зберігаються відповідно до викладеного в підпунктах 6.1 та 6.4 пункту 6 цього Порядку.
    /// Можливі значення значення 01, 02, ..., 14
    /// </summary>
    public enum DGPNtypr : int
    {

        [Display(Name = "01 Виписана на суму перевищення звичайної ціни над фактичною")]
        [XmlEnumAttribute("01")]
        Item01 = 1,

        [Display(Name = "02 Постачання неплатнику податку")]
        [XmlEnumAttribute("02")]
        Item02,

        [Display(Name = "03 Натуральна виплата в рахунок оплати праці фізичним особам")]
        [System.Xml.Serialization.XmlEnumAttribute("03")]
        Item03,

        [Display(Name = "04 Постачання у межах балансу для невиробничого використанн")]
        [System.Xml.Serialization.XmlEnumAttribute("04")]
        Item04,

        [Display(Name = "05 Ліквідація основних засобів за самостійним рішенням платника податку")]
        [System.Xml.Serialization.XmlEnumAttribute("05")]
        Item05,

        [Display(Name = "06 Переведення виробничих основних засобів до складу невиробничих")]
        [System.Xml.Serialization.XmlEnumAttribute("06")]
        Item06,

        [Display(Name = "07 Експортні постачанн")]
        [System.Xml.Serialization.XmlEnumAttribute("07")]
        Item07,

        [Display(Name = "08 Постачання для операцій, які не є об'єктом оподаткування податком на додану вартість")]
        [System.Xml.Serialization.XmlEnumAttribute("08")]
        Item08,

        [Display(Name = "09 Постачання для операцій, які звільнені від оподаткування податком на додану вартість")]
        [System.Xml.Serialization.XmlEnumAttribute("09")]
        Item09,

        [Display(Name = "10 Визнання умовного постачання товарних залишків та/або необоротних активів, що перебувають в обліку платника податку на день анулювання його реєстрації як платника податку на додану вартість, щодо яких був нарахований податковий кредит у минулих або поточному податкових періодах при анулюванні реєстрації платника податку на додану вартість.")]
        [System.Xml.Serialization.XmlEnumAttribute("10")]
        Item10,

        [Display(Name = "11 Виписана за щоденними підсумками операцій")]
        [System.Xml.Serialization.XmlEnumAttribute("11")]
        Item11,

        [Display(Name = "12 Виписана на вартість безоплатно поставлених товарів/послуг, обчислену виходячи з рівня звичайних цін")]
        [System.Xml.Serialization.XmlEnumAttribute("12")]
        Item12,

        [Display(Name = "13 Використання виробничих або невиробничих засобів, інших товарів/послуг не у господарській діяльності")]
        [System.Xml.Serialization.XmlEnumAttribute("13")]
        Item13,

        [Display(Name = "14 Виписана покупцем (отримувачем) послуг від нерезидента")]
        [System.Xml.Serialization.XmlEnumAttribute("14")]
        Item14,

        [Display(Name = "15 Складена на суму перевищення ціни придбання товарів/послуг над фактичною ціною їх постачання")]
        [System.Xml.Serialization.XmlEnumAttribute("15")]
        Item15,

        [Display(Name = "16 Складена на суму перевищення балансової (залишкової) вартості необоротних активів над фактичною ціною їх постачання")]
        [System.Xml.Serialization.XmlEnumAttribute("16")]
        Item16,
    }
}
