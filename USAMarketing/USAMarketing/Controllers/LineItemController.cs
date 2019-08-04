using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using USAMarketing.Repositary;

namespace USAMarketing.Controllers
{
    public class LineItemController : Controller
    {
        private USAMarket reposUSA = new USAMarket();
        
        //
        // GET: /LineItem/
        public ActionResult Index()
        {
            var model = reposUSA.GetLineItemModel();
            return View(model);
        }
		
		// Get: /LineItem/Create
        [HttpGet]
        public ActionResult Create()
        {
           // gets an instance of the Item object
            var model = reposUSA.GetItemModel();  // Here GetItemModel() is one of our two overloaded methods which doesnot filter the ItemList based on id
            
            // creates a selectlist
            List<SelectListItem> ItemList = new List<SelectListItem>();

            ItemList.Add(new SelectListItem() { Text = "Select an Item",  // DropDownList for selecting an ItemID on the create view of LineItem Model
                                                Value = "0", 
                                                Selected = true });

            foreach (var item in model.ItemCollection)
            {
                ItemList.Add(new SelectListItem() { Text = item.ItemNumber,
                                                    Value = item.ItemID.ToString(), 
                                                    Selected = false });
            }
            ViewBag.ItemList = ItemList;
            return View();
        }

        // Post: /LineItem/Create
        [HttpPost]
        public ActionResult Create(USAMarketing.Models.LineItemModels LineItemModel, FormCollection FormCollection)
        {
           if (FormCollection["ItemList"] == "0")
           {
               return RedirectToAction("Create");
           }

            LineItemModel.LineItemCollection[0].ItemID = Convert.ToInt32(FormCollection["ItemList"]);   // This will retrieve the Selected value (ItemID) from FormCollection.ItemList 
            
            reposUSA.AddLineItem(LineItemModel);
           
            return RedirectToAction("Index");    // This will redirect the page to list(Index)
            //return View();                     // This will just return the view
        }


        //Get: /LineItem/Edit/2000 (LineItemID)
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            //Retrieve current LineItem passed as id
          // var model = reposUSA.GetLineItemModel(id);
            
            //Retrieve ItemModel
           // var ItemCollection = reposUSA.GetItemModel().ItemCollection;

            //The selected value will be the Foreign key of the LineItem table
            var LineItemModel = reposUSA.GetLineItemModel();
            reposUSA.FindLineItemModelByItem(id, LineItemModel);

            var ItemModel = reposUSA.GetItemModel();
            ViewBag.ItemList = new SelectList(ItemModel.ItemCollection,
                                               "ItemID",                                       // Value
                                               "ItemNumber",                                   // Text 
                                               LineItemModel.LineItemCollection[0].ItemID);    // Selected value
         

           // var LineItemModel.id;
            return View(LineItemModel);

        }

        //Post: /LineItem/Edit/2000 (LineItemID)
        [HttpPost]
        public ActionResult Edit(USAMarketing.Models.LineItemModels model,
                                      FormCollection FormCollection)
        {
             
            model.LineItemCollection[0].ItemID = Convert.ToInt32(FormCollection["ItemList"]); // this will retrieve the selected value from the drop down list
                                                                                              //that returned in the POST and then UPDATE the model to set the selected value(ItemID here)

            reposUSA.UpdateLineItemModel(model);

            return RedirectToAction("Index");    // This will invoke the Edit() method in the repositary
        }

        //Get: /LineItem/Delete/2000 (LineItemID)
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            //Retrieve current Subcategory passed as id
            var model = reposUSA.GetLineItemModel(id);

            return View(model);
        }


        //Post: /LineItem/Delete/2000 (LineItemID)
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteItem(int? id)
        {
            reposUSA.DeleteLineItemModel(id);
            return RedirectToAction("Index");
        }

    }
}