using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace USAMarketing.Models
{
    public class LineItemModels
    {
       // Instatntiation of LineItemCollection list
       public LineItemModels()
        {
            LineItemCollection = new List<tblLineItem>();
        }
        
        public List<tblLineItem> LineItemCollection { get; set; }
   
        public class tblLineItem
    {
        [Display(Name="LineItemID")]
            public Int32 LineItemID { get; set; }
        [Display(Name = "Quantity")]
            public Int32 Quantity { get; set; }
        [Display(Name = "UnitPrice")]
            public string UnitPrice { get; set; }
        [Display(Name = "Amount")]
            public string Amount { get; set; }
        [Display(Name = "InvoiceID")]
            public Int32 InvoiceID { get; set; }
        [Display(Name = "ItemID")]
            public Int32 ItemID { get; set; }

            //public string Name { get; set; }

    }

    }
}