using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Memberships.Entities;
using Memberships.Models;
using Memberships.Areas.Admin.Models;
using Memberships.Areas.Admin.Extensions;

namespace Memberships.Areas.Admin.Controllers
{
    public class ProductItemController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/ProductItem
        public async Task<ActionResult> Index()
        {
            var ProductItem = await db.ProductItems.Convert(db);
            return View(ProductItem);
        }

        // GET: Admin/ProductItem/Details/5
        public async Task<ActionResult> Details(int? ItemId, int? ProductId)
        {
            if (ItemId == null || ProductId== null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductItem productItem = await GetProductItem(ItemId,ProductId);
            if (productItem == null)
            {
                return HttpNotFound();
            }
            return View( await productItem.ConvertD(db));
        }

        // GET: Admin/ProductItem/Create
        public async Task< ActionResult > Create()
        {
            var model = new ProductItemModel
            {
              products = await db.products.ToListAsync(),
              Items = await db.Items.ToListAsync()
            };
            return View(model);
        }

        // POST: Admin/ProductItem/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ProductId,ItemId")] ProductItem productItem)
        {
            if (ModelState.IsValid)
            {
                db.ProductItems.Add(productItem);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(productItem);
        }

        // GET: Admin/ProductItem/Edit/5
        public async Task<ActionResult> Edit(int? ItemId , int? ProductId)
        {
            if (ItemId == null || ProductId==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductItem productItem = await GetProductItem(ItemId, ProductId);
            if (productItem == null)
            {
                return HttpNotFound();
            }
            return View( await productItem.Convert(db));
        }

        // POST: Admin/ProductItem/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ProductId,ItemId,OldProductId,OldItemId")] ProductItem productItem)
        {
            if (ModelState.IsValid)
            {

                var canchange=  await productItem.CanChange(db);
                if(canchange)
                       await productItem.Change(db);
                return RedirectToAction("Index");
            }
            return View(productItem);
        }

        // GET: Admin/ProductItem/Delete/5
        public async Task<ActionResult> Delete(int? ItemId, int? ProductId)
        {
            if (ItemId == null || ProductId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductItem productItem = await GetProductItem(ItemId, ProductId);
            if (productItem == null)
            {
                return HttpNotFound();
            }
            return View(await productItem.ConvertD(db));
        }

        // POST: Admin/ProductItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int ItemId,int ProductId)
        {
            ProductItem productItem = await GetProductItem(ItemId,ProductId);
            db.ProductItems.Remove(productItem);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private async Task<ProductItem> GetProductItem( int? ItemId, int? ProductId)
        {
            try
            {

                int ItmId = 0, ProdId = 0;
                int.TryParse(ItemId.ToString(), out ItmId);
                int.TryParse(ProductId.ToString(), out ProdId);

                var ProductItem = await db.ProductItems.FirstOrDefaultAsync
                    (pi => pi.ProductId.Equals(ProdId) && pi.ItemId.Equals(ItmId));
                return ProductItem;

            }
            catch 
            {

                return null;
            }


        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
