using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using USAMarketing.Models;
using USAMarketing.ViewModels;
using USAMarketing.DataAccessLayer;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.Mvc;

namespace USAMarketing.Repositary
{
    public class USAMarket
    {
      public USAMarket()      // this is the constructor method of our project which will initializes the connection string nad gets the instance if our data access layer at one time
      {
         ConnectionString = WebConfigurationManager.ConnectionStrings["USAMarketing"].ConnectionString;     // it will build our connection string 
         DAL = new daUSAMarketing();        // it will instantiate the copy of our data access layer
      }
     
      private string ConnectionString;

      private daUSAMarketing DAL;
        
      public ItemModels GetItemModel()
        {
            //ItemModels model = new ItemModels();
            //model.ItemCollection.Add(new ItemModels.tblItem { ItemID = 1500, ItemNumber = "1001M", Description = "GO JAG T SHIRTS---", UnitPrice = "4.99" });
            //model.ItemCollection.Add(new ItemModels.tblItem { ItemID = 1504, ItemNumber = "1001L", Description = "GO JAG RED T SHIRTS---", UnitPrice = "4.99" });

            //return model;
           return GetItemModel(null);           
         }
      
      public ItemModels GetItemModel(int? ItemID)
      {
          ItemModels model = new ItemModels();
          DataTable dtItem = DAL.GetItemModel(ConnectionString);

          foreach (DataRow row in dtItem.Rows)
          {
              model.ItemCollection.Add(new ItemModels.tblItem
              {
                  ItemID = Convert.ToInt32(row["ItemID"]),
                  ItemNumber = row["ItemNumber"].ToString(),
                  Description = row["Description"].ToString(),
                  UnitPrice = row["UnitPrice"].ToString()       });
          }
          // will be used for regression testing...
          //model.ItemCollection.Add(new ItemModels.tblItem { ItemID = 1500, ItemNumber = "1001M", Description = "GO JAG T SHIRTS---", UnitPrice = "4.99" });
          //model.ItemCollection.Add(new ItemModels.tblItem { ItemID = 1504, ItemNumber = "1001L", Description = "GO JAG RED T SHIRTS---", UnitPrice = "4.99" });

          model.ItemCollection.RemoveAll(item => (item.ItemID != ItemID && ItemID != null));
          return model;
     }

      public LineItemModels GetLineItemModel()
      {
         return GetLineItemModel(null);
      
      }

      public LineItemModels GetLineItemModel(int? LineItemID)
      {
          LineItemModels model = new LineItemModels();
          DataTable dtLineItem = DAL.GetLineItemModel(ConnectionString);

          foreach (DataRow row in dtLineItem.Rows)
          {
              model.LineItemCollection.Add(new LineItemModels.tblLineItem
              {
                  LineItemID = Convert.ToInt32(row["LineItemID"]),
                  Quantity = Convert.ToInt32(row["Quantity"]),
                  UnitPrice = row["UnitPrice"].ToString(),
                  Amount = row["Amount"].ToString(),
                  InvoiceID = Convert.ToInt32("1003"),
                  ItemID = Convert.ToInt32(row["ItemID"])
              });
          }

          //Will be used for regression testing..
          //model.LineItemCollection.Add(new LineItemModels.tblLineItem { LineItemID = 2000, Quantity = "200", UnitPrice = "4.99", Amount = "998", InvoiceID=1003, ItemID = 1500 });
          //model.LineItemCollection.Add(new LineItemModels.tblLineItem { LineItemID = 2005, Quantity = "200", UnitPrice = "4.99", Amount = "998", InvoiceID=1009, ItemID = 1500 });
          //model.LineItemCollection.Add(new LineItemModels.tblLineItem { LineItemID = 2010, Quantity = "350", UnitPrice = "6.99", Amount = "2446.50",InvoiceID=1000, ItemID = 1520 });
          //model.LineItemCollection.Add(new LineItemModels.tblLineItem { LineItemID = 2020, Quantity = "100", UnitPrice = "8.99", Amount = "899.00",InvoiceID=1003, ItemID = 1504 });
          //model.LineItemCollection.Add(new LineItemModels.tblLineItem { LineItemID = 2012, Quantity = "50", UnitPrice = "0.99", Amount = "46.00",InvoiceID=1009, ItemID = 1504 });
          //model.LineItemCollection.Add(new LineItemModels.tblLineItem { LineItemID = 2013, Quantity = "30", UnitPrice = "6", Amount = "180",InvoiceID=1003, ItemID = 1504 });
          //model.LineItemCollection.Add(new LineItemModels.tblLineItem { LineItemID = 2014, Quantity = "50", UnitPrice = "9", Amount = "450",InvoiceID=1000, ItemID = 1504 });
          //model.LineItemCollection.Add(new LineItemModels.tblLineItem { LineItemID = 2015, Quantity = "35", UnitPrice = "10", Amount = "350",InvoiceID=1000, ItemID = 1504 });
          
          model.LineItemCollection.RemoveAll(item => (item.LineItemID != LineItemID && LineItemID != null));

          return model;

      }

      public void AddItem(ItemModels ItemModel)
      {
          // This Method will invoke on DAL to pass data for new record of Master 
          // data of ItemModel
          DAL.AddItem(ItemModel.ItemCollection[0].ItemNumber,
                      ItemModel.ItemCollection[0].Description,
                      ItemModel.ItemCollection[0].UnitPrice,
                       ConnectionString);
      }

      public void AddLineItem(LineItemModels LineItemModel)
      {
          // This Method will invoke on DAL to pass data for new record of Master 
          // data of LineItemModel
          DAL.AddLineItem(LineItemModel.LineItemCollection[0].Quantity,
                          LineItemModel.LineItemCollection[0].UnitPrice,
                          LineItemModel.LineItemCollection[0].Amount,
                          LineItemModel.LineItemCollection[0].InvoiceID,
                          LineItemModel.LineItemCollection[0].ItemID,
                          ConnectionString);
      }
      
      public ItemLineItem GetItemLineItemViewModel()
      {
          ItemLineItem model = new ItemLineItem();

          model.ItemViewModel.ItemCollection = GetItemModel().ItemCollection;
          model.LineItemViewModel.LineItemCollection = GetLineItemModel().LineItemCollection;

          return model;
      }
      
      public ItemLineItem FindItemViewModel(int? id, ItemLineItem model)
      {
          model.ItemViewModel.ItemCollection.RemoveAll(item => item.ItemID != id);
          
       ////
        //  foreach (ItemModels.tblItem Item in model.ItemViewModel.ItemCollection)
        //  {
        //    if(Item.ItemID != id)
        //    {
        //        model.ItemViewModel.ItemCollection.Remove(Item);
        //    }
        //  }
        ////  
          return model;
      }
      
      public ItemLineItem FindLineItemViewModel(int? ItemID, ItemLineItem model)
      {
          model.LineItemViewModel.LineItemCollection.RemoveAll(item => item.ItemID != ItemID);
            ////
            // foreach (LineItemModels.tblLineItem item in model.LineItemViewModel.LineItemCollection)
            //{
            //  if(item.LineItemID != id)
            //{
            //  model.LineItemViewModel.LineItemCollection.Remove(item);
            //     }
            // }
            //// 
            return model;
      }

      public LineItemModels FindLineItemModelByItem(int? ItemID, LineItemModels model)
      {
          model.LineItemCollection.RemoveAll(item => item.ItemID != ItemID);
         
          return model;
      }
      
      public void UpdateItemViewModel(ItemLineItem ItemModel)
      {
          //This will invoke the DAL to edit the tblItem
          DAL.UpdateItem(Convert.ToInt32(ItemModel.ItemViewModel.ItemCollection[0].ItemID),
                         ItemModel.ItemViewModel.ItemCollection[0].ItemNumber,
                         ItemModel.ItemViewModel.ItemCollection[0].Description,
                         ItemModel.ItemViewModel.ItemCollection[0].UnitPrice,
                         ConnectionString);
      }

      public void UpdateLineItemModel(LineItemModels LineItemModel)
      {
          // this will invoke DAL method to Update the LineItemModel
          DAL.UpdateLineItem(Convert.ToInt32(LineItemModel.LineItemCollection[0].LineItemID),
                             Convert.ToInt32(LineItemModel.LineItemCollection[0].Quantity),
                             LineItemModel.LineItemCollection[0].UnitPrice,
                             LineItemModel.LineItemCollection[0].Amount,
                             LineItemModel.LineItemCollection[0].InvoiceID, 
                             Convert.ToInt32(LineItemModel.LineItemCollection[0].ItemID),
                             ConnectionString);
      }

      public void DeleteItemModel(int? ItemID)
      {
          //This will invoke the DAL to delete the tblItem
          DAL.DeleteItem(Convert.ToInt32(ItemID), ConnectionString);
      }

      public void DeleteLineItemModel(int? LineItemID)
      {
          //This will invoke the DAL to delete the tblItem
          DAL.DeleteLineItem(Convert.ToInt32(LineItemID), ConnectionString);
      }

      public List<SelectListItem> GetItemListSelectListItem()
      {
          return GetItemListSelectListItem(null);
      }
        
      public List<SelectListItem> GetItemListSelectListItem(int? ItemID)
      {
          ItemModels model = GetItemModel();
          List<SelectListItem> ItemList = new List<SelectListItem>();
          foreach (var item in model.ItemCollection)
          {
              ItemList.Add(new SelectListItem()
              {
                  Text = item.ItemNumber,
                  Value = item.ItemID.ToString(),
                 //True (TT) if Itemid is not null and value match
                  Selected = (ItemID != null && item.ItemID == ItemID)
              });
          }
          return ItemList;
      }
    }
}