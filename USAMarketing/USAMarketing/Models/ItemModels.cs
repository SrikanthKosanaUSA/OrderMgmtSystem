using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace USAMarketing.Models
{
    public class ItemModels    // This is a nested class, having another class tblItem inside it..
    {

		// A constructor for instantiating our ItemModels class

		public ItemModels()
        {
            ItemCollection = new List<tblItem>();
            ItemSelectList = new List<SelectListItem>();
            SelectItemID = new Int32();
        }
        
		// representing tblItem as List<T> object where its type is Class
        public List<tblItem> ItemCollection { get; set; }

		public List<SelectListItem> ItemSelectList { get; set; }

		public Int32 SelectItemID { get; set; }

		// This is the Class representation of DataTable in our data tier
		public class tblItem
    {        
       [Display(Name="ItemID")]     
        public Int32 ItemID { get; set; }
       [Display(Name="ItemNumber")]
        public string ItemNumber { get; set; }
       [Display(Name="Description")]
        public string Description { get; set; }
       [Display(Name="UnitPrice")]
        public string UnitPrice { get; set; }
    }
  }
}