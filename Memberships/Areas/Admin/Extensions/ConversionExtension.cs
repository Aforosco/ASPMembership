using Memberships.Areas.Admin.Models;
using Memberships.Entities;
using Memberships.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;

namespace Memberships.Areas.Admin.Extensions
{
    public static class ConversionExtension
    {
        public static async Task<IEnumerable<ProductModel>> Convert(this IEnumerable<Product> products, ApplicationDbContext db)
        {
            if (products.Count().Equals(0)) return new List<ProductModel>();
            var texts = await db.productlinktexts.ToListAsync();
            var types = await db.producttypes.ToListAsync();

            return from p in products
                   select new ProductModel
                   {
                       Description = p.Description,
                       Id = p.Id,
                       ImageUrl = p.ImageUrl,
                       Title = p.Title,
                       ProductTypeId = p.ProductTypeId,
                       ProductLinkTextId = p.ProductLinkTextId,
                       ProductLinkTexts = texts,
                       producttypes = types

                   };

        }
        public static async Task<ProductModel> Convert(this Product products, ApplicationDbContext db)
        {

            var texts = await db.productlinktexts.FirstOrDefaultAsync(p => p.Id.Equals(products.ProductLinkTextId));
            var types = await db.producttypes.FirstOrDefaultAsync(p => p.Id.Equals(products.ProductTypeId));

            var model = new ProductModel
            {
                Description = products.Description,
                Id = products.Id,
                ImageUrl = products.ImageUrl,
                Title = products.Title,
                ProductTypeId = products.ProductTypeId,
                ProductLinkTextId = products.ProductLinkTextId,
                ProductLinkTexts = new List<ProductLinkText>(),
                producttypes = new List<ProductType>(),

            };
            model.ProductLinkTexts.Add(texts);
            model.producttypes.Add(types);
            return model;
        }
        public static async Task<IEnumerable<ProductItemModel>> Convert(this IQueryable<ProductItem> productItems, ApplicationDbContext db)
        {
            if (productItems.Count().Equals(0)) return new List<ProductItemModel>();

            return await (from pi in productItems
                          select new ProductItemModel
                          {
                              ItemId = pi.ItemId,
                              ProductId = pi.ProductId,
                              ItemTitle = db.Items.FirstOrDefault
                              (i => i.Id.Equals(pi.ItemId)).Title,
                              ProductTitle = db.products.FirstOrDefault
                              (p => p.Id.Equals(pi.ProductId)).Title

                          }).ToListAsync();

        }
        public static async Task<ProductItemModel> Convert(this ProductItem productItems, ApplicationDbContext db)
        {

            var model = new ProductItemModel
            {
                ItemId = productItems.ItemId,
                ProductId = productItems.ProductId,
                Items = await db.Items.ToListAsync(),
                products = await db.products.ToListAsync(),
            };

            return model;
        }
        public static async Task<bool> CanChange(this ProductItem productitems, ApplicationDbContext db)
        {
            var oldPI = await db.ProductItems.CountAsync(pi => pi.ItemId.Equals(productitems.OldItemId)
             && pi.ProductId.Equals(productitems.OldProductId));

            var newPI = await db.ProductItems.CountAsync(pi => pi.ItemId.Equals(productitems.ItemId)
            && pi.ProductId.Equals(productitems.ProductId));

            return oldPI.Equals(1) && newPI.Equals(0);

        }
        public static async Task Change(this ProductItem productitems, ApplicationDbContext db)
        {
            var oldProductItem = await db.ProductItems.FirstOrDefaultAsync(pi => pi.ItemId.Equals(productitems.OldItemId)
             && pi.ProductId.Equals(productitems.OldProductId));

            var newProductItem = await db.ProductItems.FirstOrDefaultAsync(pi => pi.ItemId.Equals(productitems.ItemId)
            && pi.ProductId.Equals(productitems.ProductId));

            if (oldProductItem != null && newProductItem == null)
            {
                newProductItem = new ProductItem
                {
                    ItemId = productitems.ItemId,
                    ProductId = productitems.ProductId

                };



                using (var transaction = new TransactionScope((TransactionScopeAsyncFlowOption.Enabled)))
                {


                    try
                    {

                        db.ProductItems.Remove(oldProductItem);
                        db.ProductItems.Add(newProductItem);
                        await db.SaveChangesAsync();
                        transaction.Complete();

                    }


                    catch { transaction.Dispose(); }
                }
            }
        }
        public static async Task<ProductItemModel> ConvertD(this ProductItem productItems, ApplicationDbContext db, bool AddListData= true)
        {

            var model = new ProductItemModel
            {
                ItemId = productItems.ItemId,
                ProductId = productItems.ProductId,
                Items = AddListData? await db.Items.ToListAsync() : null,            
                products = AddListData? await db.products.ToListAsync(): null,
                ItemTitle =(await db.Items.FirstOrDefaultAsync(i=> i.Id.Equals(productItems.ItemId))).Title,
                ProductTitle = (await db.products.FirstOrDefaultAsync(p => p.Id.Equals(productItems.ProductId))).Title,
            };

            return model;
        }
        public static async Task<IEnumerable<SubscriptionProductModel>> Convert(this IQueryable<SubscriptionProduct> subscriptionproducts, ApplicationDbContext db)
        {
            if (subscriptionproducts.Count().Equals(0)) return new List<SubscriptionProductModel>();

            return await (from pi in subscriptionproducts
                          select new SubscriptionProductModel
                          {
                              SubscriptionId = pi.SubscriptionId,
                              ProductId = pi.ProductId,
                              SubscriptionTitle = db.Items.FirstOrDefault
                              (i => i.Id.Equals(pi.SubscriptionId)).Title,
                              ProductTitle = db.products.FirstOrDefault
                              (p => p.Id.Equals(pi.ProductId)).Title

                          }).ToListAsync();

        }
        public static async Task<SubscriptionProductModel> Convert(this SubscriptionProduct subscriptionproducts, ApplicationDbContext db)
        {

            var model = new SubscriptionProductModel
            {
                SubscriptionId = subscriptionproducts.SubscriptionId,
                ProductId = subscriptionproducts.ProductId,
                subscriptions = await db.subscriptions.ToListAsync(),
                products = await db.products.ToListAsync(),
            };

            return model;
        }
        public static async Task<bool> CanChange(this SubscriptionProduct subcriptionproducts, ApplicationDbContext db)
        {
            var oldSp = await db.SubscriptionProducts.CountAsync(sp => sp.SubscriptionId.Equals(subcriptionproducts.OldSubscriptionId)
             && sp.ProductId.Equals(subcriptionproducts.OldProductId));

            var newSp = await db.SubscriptionProducts.CountAsync(sp => sp.SubscriptionId.Equals(subcriptionproducts.SubscriptionId)
            && sp.ProductId.Equals(subcriptionproducts.ProductId));

            return oldSp.Equals(1) && newSp.Equals(0);

        }
        public static async Task Change(this SubscriptionProduct subscriptionproducts, ApplicationDbContext db)
        {
            var oldSubscriptionProduct = await db.SubscriptionProducts.FirstOrDefaultAsync(sp => sp.SubscriptionId.Equals(subscriptionproducts.OldSubscriptionId)
             && sp.ProductId.Equals(subscriptionproducts.OldProductId));

            var newSubscriptionProduct = await db.SubscriptionProducts.FirstOrDefaultAsync(sp => sp.SubscriptionId.Equals(subscriptionproducts.SubscriptionId)
            && sp.ProductId.Equals(subscriptionproducts.ProductId));

            if (oldSubscriptionProduct != null && newSubscriptionProduct == null)
            {
                newSubscriptionProduct = new SubscriptionProduct
                {
                    SubscriptionId = subscriptionproducts.SubscriptionId,
                    ProductId = subscriptionproducts.ProductId

                };



                using (var transaction = new TransactionScope((TransactionScopeAsyncFlowOption.Enabled)))
                {


                    try
                    {

                        db.SubscriptionProducts.Remove(oldSubscriptionProduct);
                        db.SubscriptionProducts.Add(newSubscriptionProduct);
                        await db.SaveChangesAsync();
                        transaction.Complete();

                    }


                    catch { transaction.Dispose(); }
                }
            }
        }
        public static async Task<SubscriptionProductModel> ConvertD(this SubscriptionProduct subscriptionProducts, ApplicationDbContext db, bool AddListData = true)
        {

            var model = new SubscriptionProductModel
            {
                SubscriptionId = subscriptionProducts.SubscriptionId,
                ProductId = subscriptionProducts.ProductId,
                subscriptions = AddListData ? await db.subscriptions.ToListAsync() : null,
                products = AddListData ? await db.products.ToListAsync() : null,
                SubscriptionTitle = (await db.Items.FirstOrDefaultAsync(i => i.Id.Equals(subscriptionProducts.SubscriptionId))).Title,
                ProductTitle = (await db.products.FirstOrDefaultAsync(p => p.Id.Equals(subscriptionProducts.ProductId))).Title,
            };

            return model;
        }

    }
}



    
