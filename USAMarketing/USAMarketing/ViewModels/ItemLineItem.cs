using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using USAMarketing.Models;

namespace USAMarketing.ViewModels
{
    public class ItemLineItem
    {
       public ItemLineItem()
        {
            ItemViewModel = new ItemModels();
            LineItemViewModel = new LineItemModels();
        }

        //Master Record
        public ItemModels ItemViewModel { get; set; }

        //Details Records for it's Master
        public LineItemModels LineItemViewModel { get; set; }

    }
}