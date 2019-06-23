using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using USAMarketing.Repositary;
using USAMarketing.ViewModels;

namespace USAMarketing.Controllers
{
    public class ItemController : Controller
    {
        private USAMarket reposUSA = new USAMarket();

        //Get: /Item/ItemDDList/1500 (ItemID)
        [HttpGet]
        public ActionResult ItemDDL(int? id = 0)      // Sets the value passed as id to default if the user didnt pass any idea which is the zero th id
        {
            var model = reposUSA.GetItemLineItemViewModel(); // This will Retrieve the entire Collection
            model.ItemViewModel.ItemSelectList = reposUSA.GetItemListSelectListItem();
            model.ItemViewModel.SelectItemID = (int)id;

            model.LineItemViewModel = reposUSA.GetLineItemModel();
            reposUSA.FindLineItemModelByItem(id, model.LineItemViewModel);

            #region "Linq formulation"
            //model.ItemViewModel.ItemSelectList.First(x => x.Value == id.ToString()).Selected = true;

            //foreach (var item in model.ItemViewModel.ItemSelectList)      // This line of code is equivalent to x => x.Value
            //{
            //    if (item.Value == id.ToString())            // this line of code equivalent to x.Value == id.ToString()
            //    {
            //        item.Selected = true;
            //        break;
            //    }
            //}
            #endregion
           
            
            return View(model);
        }

        //Post: /Item/ItemDDList/1500 (ItemID)
        [HttpPost]
        public ActionResult ItemDDL(ItemLineItem model)      // Sets the value passed as id as default if the user didnt pass any idea which is the zero th id
        {
            var ItemID = model.ItemViewModel.SelectItemID;    //this is equivalent to Iid.ItemViewModel.SelectItemID
            model.ItemViewModel.ItemSelectList = reposUSA.GetItemListSelectListItem();
           

            model.LineItemViewModel = reposUSA.GetLineItemModel();
            reposUSA.FindLineItemModelByItem(ItemID, model.LineItemViewModel);

            return View(model);
        }
        
        //Get: /Item/ItemDDList ajax/1500 (ItemID)
        [HttpGet]
        public ActionResult ItemDDLajax(int? id = 0)      // Sets the value passed as id as default if the user didnt pass any idea which is the zero th id
        {
            var model = reposUSA.GetItemLineItemViewModel(); // This will Retrieve the entire Collection
            model.ItemViewModel.ItemSelectList = reposUSA.GetItemListSelectListItem();
            model.ItemViewModel.SelectItemID = (int)id;

            model.LineItemViewModel = reposUSA.GetLineItemModel();
            reposUSA.FindLineItemModelByItem(id, model.LineItemViewModel);

            
            return View(model);
        }

        //Post: /Item/ItemDDListajax/1500 (ItemID)
        [HttpPost]
        public ActionResult ItemDDLajax(int ItemID)      // Sets the value passed as id as default if the user didnt pass any idea which is the zero th id
        {
            var model = new ItemLineItem();
            model.LineItemViewModel = reposUSA.GetLineItemModel();
            reposUSA.FindLineItemModelByItem(ItemID, model.LineItemViewModel);

            return PartialView("_LineItemDetails",model);
        }

        //Get: /Item/ItemDDList json/1500 (ItemID)
        [HttpGet]
        public ActionResult ItemDDLjson(int? id = 0)      // Sets the value passed as id as default if the user didnt pass any idea which is the zero th id
        {
            var model = reposUSA.GetItemLineItemViewModel(); // This will Retrieve the entire Collection
            model.ItemViewModel.ItemSelectList = reposUSA.GetItemListSelectListItem();
            model.ItemViewModel.SelectItemID = (int)id;

            model.LineItemViewModel = reposUSA.GetLineItemModel();
            reposUSA.FindLineItemModelByItem(id, model.LineItemViewModel);


            return View(model);
        }

        //Post: /Item/ItemDDList json/1500 (ItemID)
        [HttpPost]
        public ActionResult ItemDDLjson(ItemLineItem model)      // Sets the value passed as id as default if the user didnt pass any idea which is the zero th id
        {
            var ItemID = model.ItemViewModel.SelectItemID;
          
            model.LineItemViewModel = reposUSA.GetLineItemModel();
            reposUSA.FindLineItemModelByItem(ItemID, model.LineItemViewModel);

            return PartialView("_LineItemDetails", model);
        }
        
        // Basic CRUD operations actionresults,,,
        // GET: /Item/
        public ActionResult Index()
        {
            var model = reposUSA.GetItemModel(); 
            
            return View(model);
        }

        // Get: /item/Create
        [HttpGet]
        public ActionResult Create()
        {
           
            return View();
        }

        // Post: /item/Create
        [HttpPost]
        public ActionResult Create(USAMarketing.Models.ItemModels ItemModel)
        {
            reposUSA.AddItem(ItemModel);
            return RedirectToAction("Index");    // This will redirect the page to list(Index)
            //return View();                     // This will just return the view
        }

       
        //Get: /Item/Edit/1500 (ItemID)
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (Request.QueryString["mode"] == "redirect")
            {
                return RedirectToAction("Edit", "LineItem", new { id = id });
            }
            var model = reposUSA.GetItemLineItemViewModel(); // This will Retrieve the entire Collection

            reposUSA.FindItemViewModel(id, model); //This will Filter the Collection based on ItemID
            reposUSA.FindLineItemViewModel(id, model);

            return View(model); 
        }

        //Post: /Item/Edit/1500 (ItemID)
        [HttpPost]
        public ActionResult Edit(int? id, USAMarketing.ViewModels.ItemLineItem ViewModel )
        {
           
            reposUSA.UpdateItemViewModel(ViewModel);

            return RedirectToAction("Index");    // This will invoke the Edit() method in the repositary
        }

        //Get: /Item/Delete/1500 (ItemID)
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (Request.QueryString["mode"] == "redirect")
            {
                return RedirectToAction("Delete", "LineItem", new { id = id });
            }
            var model = reposUSA.GetItemModel(id); // This will Retrieve the entire Collection

            return View(model);
        }

        //Post: /Item/Delete/1500 (ItemID)
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteItem(int? id)
        {
            reposUSA.DeleteItemModel(id);
            return RedirectToAction("Index"); 
        }
    }
}