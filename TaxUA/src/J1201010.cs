using sabatex.TaxUA.CommonTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security;
using System.Text;
using System.Xml;

namespace sabatex.TaxUA
{


    public partial class J1201010 : TaxDoc
    {
        public J1201010()
        {
            PERIOD_TYPE = 1;
            R01G1 = null;
            HORIG1 = null;
            HTYPR = null;
            HNUM1 = null;
        }

        public override string C_DOC => "J12";
        public override string C_DOC_SUB => "010";
        public override string C_DOC_VER => "10";



        #region HEAD
        /// <summary>
        /// Зведена податкова накладна (null,0) - no 1 - yes
        /// </summary>
        public int? R01G1 {get;set;}

        /// <summary>
        /// Складена на операції, звільнені від оподаткування
        /// </summary>
        public string R03G10S { get; set; }

        /// <summary>
        /// Не підлягає виданню отримувачу (покупцю) з причини 
        /// </summary>
        public int? HORIG1 { get; set; }

        /// <summary>
        /// Зазначається відповідний тип причини 
        /// </summary>
        public DGPNtypr? HTYPR { get; set; }

        /// <summary>
        /// Дата виписки ПН
        /// </summary>
        public string HFILL { get; set; }

         /// <summary>
        /// Порядковий номер ПН 
        /// </summary>
        public ulong HNUM { get; set; }

        /// <summary>
        /// Порядковий номер ПН (код діяльності)
        /// </summary>
        public ulong? HNUM1 { get; set; }
       
        /// <summary>
        /// Постачальник (продавець) (найменування)
        /// </summary>
        public string HNAMESEL { get; set; }

        /// <summary>
        /// Отримувач (покупець) (найменування) 
        /// </summary>
        public string HNAMEBUY { get; set; }
        
        /// <summary>
        /// ІПН підприємства 
        /// </summary>
        public string HKSEL { get; set; }

        /// <summary>
        /// Числовий номер філії продавця 
        /// </summary>
        public string HNUM2 { get; set; }

        /// <summary>
        /// Податковий номер платника податку або серія та/або номер паспорта (постачальник)
        /// 8-розрядний код ЄДРПОУ — для юросіб — платників податків, включених до Єдиного державного реєстру підприємств та організацій України;
        /// 10-розрядний реєстраційний номер облікової картки платника податків — для фізосіб. Якщо фізособа (покупець або продавець) через свої релігійні переконання відмовилася від прийняття реєстраційного номера облікової картки платника податків і має про це відмітку в паспорті, для неї в новому рядку вказують серію (за наявності) та номер паспорта;
        /// 9-розрядний реєстраційний (обліковий) номер платника податків, наданий контролюючим органом (п. 2.4 Порядку № 1588): 
        ///  - уповноваженим особам договорів про спільну діяльність на території України без створення юрособи;
        ///  - управителям майна в разі взяття на облік договорів управління майном;
        ///  - інвесторам(операторам) за угодами про розподіл продукції;
        ///  - виконавцям(юрособам-нерезидентам) проектів(програм) міжнародної технічної допомоги;
        ///  - іноземним дипломатичним представництвам та консульським установам, представництвам міжнародних організацій в Україні;
        ///  - нерезидентам і постійним представництвам нерезидентів на території України в разі взяття їх на облік.
        /// </summary>
        public string HTINSEL { get; set; }
        
        public string HKS { get; set; } // add 12 version
        
        /// <summary>
        /// ІПН покупця 
        /// </summary>
        public string HKBUY { get; set; }
        /// <summary>
        /// Код філії покупця
        /// </summary>
        public string HFBUY { get; set; }
        /// <summary>
        /// Податковий номер платника податку або серія та/або номер паспорта (покупець)
        /// 8-розрядний код ЄДРПОУ — для юросіб — платників податків, включених до Єдиного державного реєстру підприємств та організацій України;
        /// 10-розрядний реєстраційний номер облікової картки платника податків — для фізосіб. Якщо фізособа (покупець або продавець) через свої релігійні переконання відмовилася від прийняття реєстраційного номера облікової картки платника податків і має про це відмітку в паспорті, для неї в новому рядку вказують серію (за наявності) та номер паспорта;
        /// 9-розрядний реєстраційний (обліковий) номер платника податків, наданий контролюючим органом (п. 2.4 Порядку № 1588): 
        ///  - уповноваженим особам договорів про спільну діяльність на території України без створення юрособи;
        ///  - управителям майна в разі взяття на облік договорів управління майном;
        ///  - інвесторам(операторам) за угодами про розподіл продукції;
        ///  - виконавцям(юрособам-нерезидентам) проектів(програм) міжнародної технічної допомоги;
        ///  - іноземним дипломатичним представництвам та консульським установам, представництвам міжнародних організацій в Україні;
        ///  - нерезидентам і постійним представництвам нерезидентів на території України в разі взяття їх на облік.
        /// </summary>
        public string HTINBUY { get; set; }
        
        public string HKB { get; set; } // add 12 version
        #endregion HEAD

        #region BODY
        /// <summary>
        /// Усього обсяги постачання за основною ставкою (код ставки 20) 
        /// <xs:element name="R01G7" type="DGdecimal2_P" nillable="true" minOccurs="0"/>
        /// </summary>
        public decimal? R01G7 { get=>r01G7 != null?r01G7:T1.Where(w=>w.RXXXXG008 == EVAT.Default).Sum(s=>s.RXXXXG010); set=>SetProperty(ref r01G7,value); }
        /// <summary>
        /// Усього обсяги постачання за ставкою            7 % (код ставки 7) 
        /// <xs:element name="R03G109" type="DGdecimal2_P" nillable="true" minOccurs="0"/>
        /// </summary>
        public decimal? R01G109 { get => r01G109 != null ? r01G109 : T1.Where(w => w.RXXXXG008 == EVAT.Country).Sum(s => s.RXXXXG010); set => SetProperty(ref r01G109, value); }
        /// <summary>
        /// Усього обсяги постачання при експорті товарів за ставкою 0% (код ставки 901) 
        /// <xs:element name="R01G9" type="DGdecimal2_P" nillable="true" minOccurs="0"/>
        /// </summary>
        public decimal? R01G9 { get => r01G9 != null ? r01G9 : T1.Where(w => w.RXXXXG008 == EVAT.Export).Sum(s => s.RXXXXG010); set => SetProperty(ref r01G9, value); }
        /// <summary>
        /// Усього обсяги постачання на митній території України за ставкою 0% (код ставки 902) 
        ///	<xs:element name="R01G8" type="DGdecimal2_P" nillable="true" minOccurs="0"/> 
        /// </summary>
        public decimal? R01G8 { get => r01G8 != null ? r01G8 : T1.Where(w => w.RXXXXG008 == EVAT.Import).Sum(s => s.RXXXXG010); set => SetProperty(ref r01G8, value); }
        /// <summary>
        /// Усього обсяги операцій, звільнених від оподаткування (код ставки 903) 
        /// <xs:element name = "R01G10" type="DGdecimal2_P" nillable="true" minOccurs="0"/>
        /// </summary>
        public decimal? R01G10 { get => r01G10 != null ? r01G10 : T1.Where(w => w.RXXXXG008 == EVAT.FreeVAT).Sum(s => s.RXXXXG010); set => SetProperty(ref r01G10, value); }
        /// <summary>
        /// Дані щодо зворотної (заставної) тари
        /// <xs:element name="R02G11" type="DGdecimal2_P" nillable="true" minOccurs="0"/>
        /// </summary>
        public System.Nullable<decimal> R02G11 { get; set; }
        /// <summary>
        /// Загальна сума податку на додану вартість за основною ставкою 
        /// <xs:element name="R03G7" type="DGdecimal2_P" nillable="true" minOccurs="0"/>
        /// </summary>
        public decimal? R03G7 
        {
            get => r03G7 != null ? r03G7 : T1.Where(w => w.RXXXXG008 == EVAT.Default).Sum(s => s.RXXXXG11_10);
            set => SetProperty(ref r03G7, value);
        }
        /// <summary>
        /// Загальна сума податку на додану вартість за ставкою 7%
        /// <xs:element name="R01G109" type="DGdecimal2_P" nillable="true" minOccurs="0"/>
        /// </summary>
        public decimal? R03G109 { get => r03G109 != null ? r03G109 : T1.Where(w => w.RXXXXG008 == EVAT.Country).Sum(s => s.RXXXXG11_10); set => SetProperty(ref r03G109, value); }
        /// <summary>
        /// Загальна сума податку на додану вартість, у тому числі: 
        /// <xs:element name="R03G11" type="DGdecimal2_P" nillable="true" minOccurs="0"/>
        /// </summary>
        public decimal? R03G11
        {
            get => r03G11 != null ? r03G11 : T1.Sum(s => s.RXXXXG11_10);
            set => SetProperty(ref r03G11, value);
        }
        /// <summary>
        /// Загальна сума коштів, що підлягають сплаті з урахуванням податку на додану вартість 
        /// <xs:element name="R04G11" type="DGdecimal2_P" nillable="true" minOccurs="0"/>
        /// </summary>
        public decimal? R04G11
        {
            get => (r04G11 != null ? r04G11 : T1.Sum(s => s.RXXXXG010)) + (R03G11==null ? T1.Sum(s => s.RXXXXG11_10) : R03G11);
            set=>SetProperty(ref r04G11,value);
        }
        /// <summary>
        /// Посадова (уповноважена) особа/фізична особа (законний представник)
        /// <xs:element name="HBOS" type="DGHBOS"/>
        /// </summary>
        public string HBOS { get; set; }
        /// <summary>
        /// Реєстраційний номер облікової картки платника податку 
        /// <xs:element name="HKBOS" type="DGLong"/>
        /// </summary>
        public string HKBOS { get; set; }
        #endregion BODY

        #region TABLE №1
        public List<J1201010T1> T1 { get; set; } = new List<J1201010T1>();
        #endregion TABLE
        /// <summary>
        /// Відповідні пункти, якими передбачено звільнення від оподаткування
        /// <xs:element name="R003G10S" type="xs:string" nillable="true" minOccurs="0"/>
        /// </summary>
        public string R003G10S { get; set; }


        decimal? r04G11 = new decimal?();
        decimal? r01G7 = new decimal?();
        decimal? r01G109 = new decimal?();
        decimal? r01G9= new decimal?();
        decimal? r01G8 = new decimal?();
        decimal? r01G10 = new decimal?();
        decimal? r03G7 = new decimal?();
        decimal? r03G109 = new decimal?();
        decimal? r03G11 = new decimal?();

        protected override StringBuilder DoGetXmlBody(StringBuilder st, int indent)
        {
            st.Append(' ', indent).AddValue(nameof(R01G1), R01G1);
            st.Append(' ', indent).AddValue(nameof(R003G10S), R03G10S);
            st.Append(' ', indent).AddValue(nameof(HORIG1), HORIG1);
            st.Append(' ', indent).AddValue(nameof(HTYPR), HTYPR);
            st.Append(' ', indent).AddValue(nameof(HKSEL), HKSEL);
            st.Append(' ', indent).AddValue(nameof(HNAMESEL), HNAMESEL);
            st.Append(' ', indent).AddValue(nameof(HTINSEL), HTINSEL);
            st.Append(' ', indent).AddValue(nameof(HNUM2), HNUM2);
            st.Append(' ', indent).AddValue(nameof(HFILL),HFILL);
            st.Append(' ', indent).AddValue(nameof(HNUM), HNUM);
            st.Append(' ', indent).AddValue(nameof(HNUM1), HNUM1);
            st.Append(' ', indent).AddValue(nameof(HNAMEBUY), HNAMEBUY);
            st.Append(' ', indent).AddValue(nameof(HKBUY), HKBUY);
            st.Append(' ', indent).AddValue(nameof(HFBUY), HFBUY);
            st.Append(' ', indent).AddValue(nameof(HTINBUY), HTINBUY);
            st.Append(' ', indent).AddValue(nameof(R01G7), R01G7);
            st.Append(' ', indent).AddValue(nameof(R01G109), R01G109);
            st.Append(' ', indent).AddValue(nameof(R01G9), R01G9);
            st.Append(' ', indent).AddValue(nameof(R01G8), R01G8);
            st.Append(' ', indent).AddValue(nameof(R01G10), R01G10);
            st.Append(' ', indent).AddValue(nameof(R02G11), R02G11);
            st.Append(' ', indent).AddValue(nameof(R03G7), R03G7);
            st.Append(' ', indent).AddValue(nameof(R03G109), R03G109);
            st.Append(' ', indent).AddValue(nameof(R03G11), R03G11);
            st.Append(' ', indent).AddValue(nameof(R04G11), R04G11);
            st.Append(' ', indent).AddValue(nameof(HBOS), HBOS);
            st.Append(' ', indent).AddValue(nameof(HKBOS),HKBOS);

            int line = 1;
            foreach (var t1 in T1)
            {
                t1.AppendAsXML(st, line++, indent + 2);
            }
            st.Append(' ', indent).AddValue(nameof(R003G10S), R003G10S);

            return st;
        }


        protected override bool DoSetFromFile(XmlDocument doc)
        {
            XmlNode DB = doc.SelectSingleNode("/DECLAR/DECLARBODY");

            R01G1 = DB[nameof(R01G1)].GetAsInt();
            R03G10S = DB["R03G10S"].GetAsString();
            HORIG1 = DB["HORIG1"].GetAsInt();
            HTYPR = DB["HTYPR"].GetAsDGPNtypr();
            HKSEL = DB["HKSEL"].GetAsString();
            HNAMESEL = DB["HNAMESEL"].GetAsString();
            HTINSEL = DB[nameof(HTINSEL)].GetAsString();
            HNUM2 = DB["HNUM2"].GetAsString();
            HFILL = DB["HFILL"].GetAsString();
            HNUM = DB["HNUM"].GetAsUlong().Value;
            HNUM1 = DB["HNUM1"].GetAsUlong();
            HNAMEBUY = DB["HNAMEBUY"].GetAsString();
            HKBUY = DB["HKBUY"].GetAsString();
            HFBUY = DB["HFBUY"].GetAsString();
            HTINBUY = DB[nameof(HTINBUY)].GetAsString();

            R01G7 = DB[nameof(R01G7)].GetAsDecimal();
            R01G109 = DB[nameof(R01G109)].GetAsDecimal();
            R01G8 = DB["R01G8"].GetAsDecimal();
            R01G9 = DB["R01G9"].GetAsDecimal();
            R01G10 = DB["R01G10"].GetAsDecimal();
            R02G11 = DB["R02G11"].GetAsDecimal();
            R03G7 = DB["R03G7"].GetAsDecimal();
            R03G109 = DB["R03G109"].GetAsDecimal();
            R03G11 = DB["R03G11"].GetAsDecimal();
            R04G11 = DB["R04G11"].GetAsDecimal();
            HBOS = DB["HBOS"].GetAsString();
            HKBOS = DB["HKBOS"].GetAsString();
            R003G10S = DB["R003G10S"].GetAsString();

            // Get table
            int line = 1;
            var nodes = DB.SelectNodes("*[@ROWNUM]").Cast<XmlNode>();
            var n = nodes.Where(w => w.Attributes["ROWNUM"].InnerText==line.ToString()).ToArray();
            while (n.Length != 0)
            {
                var t1 = new J1201010T1();
                t1.RXXXXG3S = n.Single(s => s.Name == "RXXXXG3S").InnerXml;
                t1.RXXXXG008 = (EVAT)int.Parse(n.Single(s => s.Name == "RXXXXG008").InnerXml);
                t1.RXXXXG009 = n.Single(s => s.Name == "RXXXXG009").InnerXml.GetAsUint();
                t1.RXXXXG010 = n.Single(s => s.Name == "RXXXXG010").InnerXml.GetAsDecimal();
                t1.RXXXXG011 = n.Single(s => s.Name == "RXXXXG011").InnerXml.GetAsUint();
                t1.RXXXXG105_2S = n.Single(s => s.Name == "RXXXXG105_2S").InnerXml.GetAsUint();
                t1.RXXXXG11_10 = n.Single(s => s.Name == "RXXXXG11_10").InnerXml.GetAsDecimal();
                t1.RXXXXG32 = n.Single(s => s.Name == "RXXXXG32").InnerXml.GetAsUint();
                t1.RXXXXG33 = n.Single(s => s.Name == "RXXXXG33").InnerXml;
                t1.RXXXXG4 = n.Single(s => s.Name == "RXXXXG4").InnerXml;
                t1.RXXXXG4S = n.Single(s => s.Name == "RXXXXG4S").InnerXml;
                t1.RXXXXG5 = n.Single(s => s.Name == "RXXXXG5").InnerXml.GetAsDecimal();
                t1.RXXXXG6 = n.Single(s => s.Name == "RXXXXG6").InnerXml.GetAsDecimal();
                line++;
                n = nodes.Where(w => w.Attributes["ROWNUM"].InnerText == line.ToString()).ToArray();
            }
            return true;

        }
 
    }


}
