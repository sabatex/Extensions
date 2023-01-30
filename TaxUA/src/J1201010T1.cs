using System;
using System.Collections.Generic;
using System.Text;
using sabatex.Extensions;

namespace sabatex.TaxUA
{
    /// <summary>
    /// J1201010
    /// </summary>
    public  class J1201010T1 : ObservableObject
    {
        /// <xs:element name="RXXXXG3S" type="StrColumn" nillable="true" maxOccurs="9999"/>
        /// <summary>
        /// Найменування ТМЦ Номенклатура поставки товарів 
        /// </summary>
        public string RXXXXG3S { get; set; }
        /// <summary>
        /// Ознака імпортованого товару 
        /// <xs:element name="RXXXXG32" type="ChkColumn" nillable="true" minOccurs="0" maxOccurs="9999"/>
        /// </summary>
        public uint? RXXXXG32 { get; set; }
        /// <summary>
        /// Послуги згідно з ДКПП
        /// <xs:element name="RXXXXG33" type="DKPPColumn" nillable="true" minOccurs="0" maxOccurs="9999"/>
        /// </summary>
        public string RXXXXG33 { get; set; }
        /// <summary>
        /// Код товару згідно з УКТ ЗЕД
        /// <xs:element name="RXXXXG4" type="UKTZEDColumn" nillable="true" minOccurs="0" maxOccurs="9999"/>
        /// </summary>
        public string RXXXXG4 { get; set; }
        /// <summary>
        /// Одиниця виміру товару/послуги (умовне позначення (українське) 
        /// <xs:element name="RXXXXG4S" type="StrColumn" nillable="true" minOccurs="0" maxOccurs="9999"/>
        /// </summary>
        public string RXXXXG4S { get; set; }
        /// <summary>
        /// Одиниця виміру товару/послуги (код)
        /// DeclarContent.ParseXML(ColumnName, i + 1, DataValue[i], "0000")
        ///<xs:element name="RXXXXG105_2S" type="DGI4lzColumn" nillable="true" minOccurs="0" maxOccurs="9999"/> 
        /// </summary>
        public uint? RXXXXG105_2S { get; set; }
        /// <summary>
        /// Кількість
        /// DeclarContent.ParseXML(ColumnName, i + 1, DataValue[i], "0.000000")
        /// <xs:element name="RXXXXG5" type="Decimal12Column_R" nillable="true" minOccurs="0" maxOccurs="9999"/>
        /// </summary>
        public decimal? RXXXXG5 { get; set; }
        /// <summary>
        /// Ціна постачання одиниці товару\послуги
        /// DeclarContent.ParseXML(ColumnName, i + 1, DataValue[i], "0.000000000000")
        /// <xs:element name="RXXXXG5" type="Decimal12Column_R" nillable="true" minOccurs="0" maxOccurs="9999"/>
        /// </summary>
        public decimal? RXXXXG6 { get; set; }
        /// <summary>
        /// Код ставки 
        /// <xs:element name="RXXXXG008" type="DGI3nomColumn" nillable="true" minOccurs="0" maxOccurs="9999"/>
        /// </summary>
        public EVAT RXXXXG008 { get; set; } = EVAT.Default;
        /// <summary>
        /// Код пільги
        /// DeclarContent.ParseXML(ColumnName, i + 1, DataValue[i], "0000000")
        /// <xs:element name="RXXXXG009" type="CodPilgColumn" nillable="true" minOccurs="0" maxOccurs="9999"/>
        /// </summary>
        public uint? RXXXXG009 { get; set; }
        /// <summary>
        /// Обсяги постачання (база оподаткування) без урахування податку на додану вартість 
        /// DeclarContent.ParseXML(ColumnName, i + 1, DataValue[i], "0.00")
        /// <xs:element name="RXXXXG010" type="Decimal2Column_P" nillable="true" minOccurs="0" maxOccurs="9999"/>
        /// </summary>
        public decimal? RXXXXG010 { get; set; }
        /// <summary>
        /// Сума податку на додану вартість 
        /// DeclarContent.ParseXML(ColumnName, i + 1, DataValue[i], "0.000000")
        /// <xs:element name="RXXXXG11_10" type="Decimal6Column_R" nillable="true" minOccurs="0" maxOccurs="9999"/>
        /// </summary>
        public decimal? RXXXXG11_10
        {
            get
            {
                if (rXXXXG11_10 != null) return rXXXXG11_10;
                if (RXXXXG010 == null) return null;
                switch (RXXXXG008)
                {
                   case EVAT.Default:
                           return RXXXXG010!=null?  Math.Round(RXXXXG010.Value * 0.2m, 3): new decimal?();
                   case EVAT.Country:
                           return RXXXXG010 != null ? Math.Round(RXXXXG010.Value * 0.07m,3) : new decimal?();
                   default:
                        return new decimal?();
                }
            }
            set => SetProperty(ref rXXXXG11_10, value);
        }
        /// <summary>
        /// Код виду діяльності сільськогосподарського товаровиробника 
        /// DeclarContent.ParseXML(ColumnName, i + 1, DataValue[i], "0")
        /// <xs:element name="RXXXXG011" type="DGI3nomColumn" nillable="true" minOccurs="0" maxOccurs="9999"/>
        /// </summary>
        public uint? RXXXXG011 { get; set; }
        decimal? rXXXXG11_10 = new decimal?();

        public StringBuilder AppendAsXML(StringBuilder st, int line, int indent)
        {
            st.Append(' ', indent).AddValue(nameof(RXXXXG3S), RXXXXG3S, line);
            st.Append(' ', indent).AddValue(nameof(RXXXXG32), RXXXXG32, line);
            st.Append(' ', indent).AddValue(nameof(RXXXXG33), RXXXXG33, line);
            st.Append(' ', indent).AddValue(nameof(RXXXXG4), RXXXXG4, line);
            st.Append(' ', indent).AddValue(nameof(RXXXXG4S), RXXXXG4S, line);
            st.Append(' ', indent).AddValue(nameof(RXXXXG5), RXXXXG5, line);
            st.Append(' ', indent).AddValue(nameof(RXXXXG6), RXXXXG6, line);
            st.Append(' ', indent).AddValue(nameof(RXXXXG008), RXXXXG008, line);
            st.Append(' ', indent).AddValue(nameof(RXXXXG009), RXXXXG009, line);
            st.Append(' ', indent).AddValue(nameof(RXXXXG010), RXXXXG010, line);
            st.Append(' ', indent).AddValue(nameof(RXXXXG11_10), RXXXXG11_10, line);
            st.Append(' ', indent).AddValue(nameof(RXXXXG011), RXXXXG011, line);
            return st;
        }



    }

}
