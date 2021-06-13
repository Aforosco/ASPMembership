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
using Memberships.Areas.Admin.Extensions;
using Memberships.Areas.Admin.Models;

namespace Memberships.Areas.Admin.Controllers
{

    [Authorize(Roles = "Admin")]
    public class SubscriptionProductController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/SubscriptionProduct
        public async Task<ActionResult> Index()
        {
            var SubscriptionProduct = await db.SubscriptionProducts.Convert(db);
            return View(SubscriptionProduct);
        }

        // GET: Admin/SubscriptionProduct/Details/5
        public async Task<ActionResult> Details(int? Subscriptionid, int? ProductId)
        {
            if (Subscriptionid == null || ProductId== null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubscriptionProduct subscriptionProduct =  await GetSubscriptionProduct(Subscriptionid, ProductId);
            if (subscriptionProduct == null)
            {
                return HttpNotFound();
            }

            return View( await subscriptionProduct.ConvertD(db));
        }

        // GET: Admin/SubscriptionProduct/Create
        public async Task<ActionResult> Create()
        {
            var model = new SubscriptionProductModel
            {
                products =  await db.products.ToListAsync(),
                subscriptions = await db.subscriptions.ToListAsync()
               };
            return View(model);
        }

        // POST: Admin/SubscriptionProduct/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ProductId,SubscriptionId")] SubscriptionProduct subscriptionProduct)
        {
            if (ModelState.IsValid)
            {
                db.SubscriptionProducts.Add(subscriptionProduct);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(subscriptionProduct);
        }

        // GET: Admin/SubscriptionProduct/Edit/5
        public async Task<ActionResult> Edit(int? subscriptionId, int? productId)
        {
            if (subscriptionId  == null || productId==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubscriptionProduct subscriptionProduct = await GetSubscriptionProduct(subscriptionId,productId);
            if (subscriptionProduct == null)
            {
                return HttpNotFound();
            }
            return View( await subscriptionProduct.Convert(db));
        }

        // POST: Admin/SubscriptionProduct/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ProductId,SubscriptionId,OldSubscriptionId,OldProductId")] SubscriptionProduct subscriptionProduct)
        {
            if (ModelState.IsValid)
            {
                var canchange = await subscriptionProduct.CanChange(db);   
                if (canchange)
                    await subscriptionProduct.Change(db);
                return RedirectToAction("Index");
            };
            return View(subscriptionProduct);
        }

        // GET: Admin/SubscriptionProduct/Delete/5
        public async Task<ActionResult> Delete(int? SubscriptionId, int? ProductId)
        {
            if (SubscriptionId == null || ProductId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubscriptionProduct subscriptionProduct = await GetSubscriptionProduct(SubscriptionId, ProductId);
            if (subscriptionProduct == null)
            {
                return HttpNotFound();
            }
            return View( await subscriptionProduct.ConvertD(db));
        }

        // POST: Admin/SubscriptionProduct/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            SubscriptionProduct subscriptionProduct = await db.SubscriptionProducts.FindAsync(id);
            db.SubscriptionProducts.Remove(subscriptionProduct);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private async Task <SubscriptionProduct> GetSubscriptionProduct(int? SubscriptionId, int? ProductId)
        {
            int prodId = 0, subId = 0;

            int.TryParse(SubscriptionId.ToString(), out subId);

            int.TryParse(ProductId.ToString(), out prodId);

            var SubscriptionProduct = await db.SubscriptionProducts.FirstOrDefaultAsync
                (s => s.SubscriptionId.Equals(subId) && s.ProductId.Equals(prodId));
            return SubscriptionProduct;
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
