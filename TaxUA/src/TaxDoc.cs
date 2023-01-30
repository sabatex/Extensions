using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using sabatex.Extensions.ClassExtensions;
using sabatex.Extensions;

namespace sabatex.TaxUA
{
    public abstract class TaxDoc:ObservableObject
    {

        /// <summary>
        /// позиції з 5 по 14 включно містять податковий номер 
        /// або реєстраційний номер облікової картки платника податків
        /// (серія та номер паспорта (для фізичних осіб,
        /// які через свої релігійні переконання відмовляються від прийняття реєстраційного
        /// номера облікової картки платника податків та офіційно повідомили
        /// про це відповідний контролюючий орган і мають відмітку у паспорті))
        /// (значення елемента TIN, доповненого зліва нулями до 10 символів);
        /// </summary>
        [StringLength(10)]
        public string TIN { get=>_tin; set=>SetProperty(ref _tin,value.PadLeft(10,'0')); }
        string _tin;

        /// <summary>
        /// Код документа
        /// Соответствует значению элемента C_DOC справочника отчётных документов(SPR_DOC.XML)
        /// </summary>
        [StringLength(3)]
        [Display(
            Name = "Код документа",
            Description = "Соответствует значению элемента C_DOC справочника отчётных документов(SPR_DOC.XML)")]
        public abstract string C_DOC { get;}
        /// <summary>
        /// Подтип документа
        /// Соответствует значению элемента C_DOC_SUB справочника отчётных документов(SPR_DOC.XML)
        /// </summary>
        [StringLength(3)]
        [Display(
            Name = "Код документа",
            Description = "Соответствует значению элемента C_DOC_SUB справочника отчётных документов(SPR_DOC.XML)"
            )]
        public abstract string C_DOC_SUB { get;}
        /// <summary>
        /// Номер версии документа
        /// Соответствует значению элемента C_DOC_VER справочника отчётных документов(SPR_DOC.XML)
        /// </summary>
        public abstract string C_DOC_VER { get;}
        /// <summary>
        /// Номер нового отчётного(уточняющего) документа
        /// Для первого поданного(отчётного) документа значение данного элемента равняется 0. Для каждого последующего нового отчётного(уточняющего) документа этого же типа для данного отчётного периода значение увеличивается на единицу
        /// </summary>
        public int C_DOC_TYPE { get; set; }
        /// <summary>
        /// порядковий номер документа, що може подаватись декілька разів в одному
        /// звітному періоді (значення елемента C_DOC_CNT, доповненого зліва нулями
        /// до 7 символів). Якщо звіт подається лише один раз, то позиції 26 − 32
        /// міститимуть значення 0000001;
        /// </summary>
        ulong c_DOC_CNT;
        public ulong C_DOC_CNT
        {
            get => c_DOC_CNT;
            set
            {
                SetProperty(ref c_DOC_CNT, value);
            }
        }

        /// <summary>
        /// Код области
        /// Код области, на территории которой расположена налоговая инспекция, в которую подаётся оригинал либо копия документа. Заполняется согласно справочнику SPR_STI.XML.
        /// </summary>
        public int C_REG { get; set; }
        /// <summary>
        /// Код района
        /// Код района, на территории которого расположена налоговая инспекция, в которую подаётся оригинал либо копия документа. Заполняется согласно справочнику SPR_STI.XML.
        /// </summary>
        public int C_RAJ { get; set; }
        /// <summary>
        /// Отчётный месяц
        /// Отчётным месяцем считается последний месяц в отчётном периоде (для месяцев - порядковый номер месяца, для квартала - 3,6,9,12 месяц, полугодия - 6 и 12, для года - 12)я 9 місяців – 9, для року – 12)
        /// </summary>
        public int PERIOD_MONTH { get; set; }
        /// <summary>
        /// Тип отчётного периода
        /// 1-месяц, 2-квартал, 3-полугодие, 4-девять мес., 5-год
        /// </summary>
        public int PERIOD_TYPE { get; set; }
        /// <summary>
        /// Отчётный год
        /// Формат YYYY
        /// </summary>
        public int PERIOD_YEAR { get; set; }
        /// <summary>
        /// Код инспекции, в которую подаётся оригинал документа
        /// Код выбирается из справочника инспекций. вычисляется по формуле: C_REG*100+C_RAJ.
        /// </summary>
        public int C_STI_ORIG
        {
            get
            {
                return this.C_REG * 100 + C_RAJ;
            }
        }
        /// <summary>
        /// Состояние документа
        /// 1-отчётный документ, 2-новый отчётный документ ,3-уточняющий документ
        /// </summary>
        public int C_DOC_STAN { get; set; }
        /// <summary>
        /// Перечень связанных документов. Элемент является узловым, в себе содержит элементы DOC
        /// Для основного документа содержит ссылку на дополнение, для дополнения - ссылку на основной.
        /// </summary>
        public DHeadDOC[] LINKED_DOCS { get; set; }
        /// <summary>
        /// Дата заполнения документа
        /// Формат ddmmyyyy
        /// </summary>
        public DateTime D_FILL { get; set; }
        /// <summary>
        /// Сигнатура программного обеспечения
        /// Идентификатор ПО, с помощью которого сформирован отчёт
        /// </summary>
        public string SOFTWARE { get; set; }
  
        public string GetAsXML()
        {
            StringBuilder st = new StringBuilder();
            st.AppendLine("<?xml version=\"1.0\" encoding=\"windows-1251\" ?>");
            st.AppendFormat("<DECLAR xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:noNamespaceSchemaLocation=\"{0}\">\n",GetXSDFileName());
            st.Append(' ', 4).AppendLine("<DECLARHEAD>");
            st.Append(' ', 8).AddValue(nameof(TIN), TIN);
            st.Append(' ', 8).AddValue(nameof(C_DOC), C_DOC);
            st.Append(' ', 8).AddValue(nameof(C_DOC_SUB), C_DOC_SUB);
            st.Append(' ', 8).AddValue(nameof(C_DOC_VER), C_DOC_VER);
            st.Append(' ', 8).AddValue(nameof(C_DOC_TYPE), C_DOC_TYPE);
            st.Append(' ', 8).AddValue(nameof(C_DOC_CNT), C_DOC_CNT);
            st.Append(' ', 8).AddValue(nameof(C_REG), C_REG,"00");
            st.Append(' ', 8).AddValue(nameof(C_RAJ), C_RAJ,"00");
            st.Append(' ', 8).AddValue(nameof(PERIOD_MONTH), PERIOD_MONTH);
            st.Append(' ', 8).AddValue(nameof(PERIOD_TYPE), PERIOD_TYPE);
            st.Append(' ', 8).AddValue(nameof(PERIOD_YEAR), PERIOD_YEAR);
            st.Append(' ', 8).AddValue(nameof(C_STI_ORIG), C_STI_ORIG);
            st.Append(' ', 8).AddValue(nameof(C_DOC_STAN), C_DOC_STAN);
            st.Append(' ', 8).AddValue("LINKED_DOCS", (string)null);
            st.Append(' ', 8).AddValue(nameof(D_FILL), D_FILL);
            st.Append(' ', 8).AddValue(nameof(SOFTWARE), SOFTWARE);
            st.Append(' ', 4).AppendLine("</DECLARHEAD>");
            st.Append(' ', 4).AppendLine("<DECLARBODY>");
            DoGetXmlBody(st, 8);
            st.Append(' ', 4).AppendLine("</DECLARBODY>");
            st.AppendLine("</DECLAR>");
            return st.ToString();
        }

        protected abstract StringBuilder DoGetXmlBody(StringBuilder st, int indent);
        protected abstract bool DoSetFromFile(XmlDocument doc);

        public string GetXMLFileName()
        {
            return C_REG.ToString().PadLeft(2, '0') +      // 1..2   код області
                   C_RAJ.ToString().PadLeft(2, '0') +      // 3..4   код адміністративного району
                   TIN.PadLeft(10, '0') +                  // 5..14  код ЄДРПОУ 
                   C_DOC.PadLeft(3, '0') +                 // 15..17 код документа
                   C_DOC_SUB.PadLeft(3, '0') +             // 18..20 підтип документа 
                   C_DOC_VER.PadLeft(2, '0') +             // 21..22 номер версії документа
                   C_DOC_STAN.ToString() +            // 23 Код Стан документа (1, 2, 3) (1-звітний документ 2-новий звітний документ 3-уточнюючий документ)  
                   C_DOC_TYPE.ToString().PadLeft(4, '0') + // 24..27
                   C_DOC_CNT.ToString().PadLeft(7,'0') +             // 28..32 тризначний порядковий номер (доповненого зліва нулями до 5 знаків)
                   PERIOD_TYPE.ToString() +           // 33
                   PERIOD_MONTH.ToString().PadLeft(2, '0') +          // 34..35 звітного місяця
                   PERIOD_YEAR.ToString().PadLeft(4, '0') +           // 36..39 year
                   C_STI_ORIG.ToString().PadLeft(4, '0') + ".xml";    // 40..43 year
        }
        public string GetXSDFileName()
        {
            return C_DOC + C_DOC_SUB + C_DOC_VER.PadLeft(2, '0') + ".xsd";
        }
        public bool SetFromFile(XmlDocument doc)
        {
            try
            {
                XmlNode DB = doc.SelectSingleNode("/DECLAR/DECLARHEAD");
                TIN = DB["TIN"].InnerText;

                int c_doc_type;
                if (int.TryParse(DB["C_DOC_TYPE"].InnerText, out c_doc_type))
                    C_DOC_TYPE = c_doc_type;
                else
                    return false;

                string st = DB["C_DOC_CNT"].InnerText;
                st = st.TrimStart('0');

                C_DOC_CNT = ulong.Parse(st);


                int c_reg;
                if (int.TryParse(DB["C_REG"].InnerText, out c_reg))
                    C_REG = c_reg;
                else
                    return false;

                int c_raj;
                if (int.TryParse(DB["C_RAJ"].InnerText, out c_raj))
                    C_RAJ = c_raj;
                else
                    return false;

                int period_month;
                if (int.TryParse(DB["PERIOD_MONTH"].InnerText, out period_month))
                    PERIOD_MONTH = period_month;
                else
                    return false;

                int period_type;
                if (int.TryParse(DB["PERIOD_MONTH"].InnerText, out period_type))
                    PERIOD_TYPE = period_type;
                else
                    return false;


                int period_year;
                if (int.TryParse(DB["PERIOD_MONTH"].InnerText, out period_year))
                    PERIOD_YEAR = period_year;
                else
                    return false;

                int c_doc_stan;
                if (DB["C_DOC_STAN"] != null)
                {
                    if (int.TryParse(DB["C_DOC_STAN"].InnerText, out c_doc_stan))
                        C_DOC_STAN = c_doc_stan;
                    else
                        return false;
                }

                D_FILL = DB["D_FILL"].InnerText.GetTAXDate().Value;
            }
            catch (Exception e)
            {
                Trace.Write(e.Message);
                return false;
            }

            return true;
        }

        public static TaxDoc OpenXMLFile(string FileName, string C_DOC, string C_DOC_SUB, string KODE, DateTime? StartDate, DateTime? EndDate, out XmlDocument doc)
        {
            doc = new XmlDocument();
            try
            {
                doc.Load(FileName);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
                return null;
            }
            string c_DOC_VER;
            try
            {
                XmlNode DH = doc.SelectSingleNode("/DECLAR/DECLARHEAD");
                if (DH["C_DOC"].InnerText != C_DOC || DH["C_DOC_SUB"].InnerText != C_DOC_SUB) return null;
                c_DOC_VER = DH["C_DOC_VER"].InnerText;
                XmlNode DB = doc.SelectSingleNode("/DECLAR/DECLARBODY");
                DateTime HFILL = DB["HFILL"].InnerText.GetTAXDate().Value;
                if ((StartDate != null &&  HFILL < StartDate) || (EndDate != null && HFILL > EndDate)) return null;
                if (DB["HKBUY"].InnerText != KODE) return null;
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
                return null;
            }

            TaxDoc taxDoc;
            switch (C_DOC + C_DOC_SUB + c_DOC_VER)
            {
                case nameof(J1201010):
                    taxDoc = new J1201010();
                    return taxDoc.SetFromFile(doc)?taxDoc:null;
                default:
                    return null;

            }
       }

    }
}
