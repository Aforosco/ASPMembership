using Memberships.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Memberships.Areas.Admin.Models
{
    public class ProductModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        [MaxLength(2048)]
        public string Description { get; set; }

        [DisplayName("Image Url")]
        [MaxLength(1024)]
        public string ImageUrl { get; set; }

        public int ProductLinkTextId { get; set; }

        public int ProductTypeId { get; set; }
        [DisplayName("Product Type")]
        public ICollection<ProductType> producttypes { get; set; }
        [DisplayName("Product Link Text")]
        public ICollection<ProductLinkText> ProductLinkTexts { get; set; }

        [DisplayName("Product Type")]
        public string producttype { get {

                return
                     producttypes == null || producttypes.Count.Equals(0) ? 
                      string.Empty : producttypes.First().Title;
                    } }
        [DisplayName("Product Link Text")]
        public string ProductLinkText
        {
            get {  return
                   ProductLinkTexts == null || ProductLinkTexts.Count.Equals(0) ?
                   string.Empty : ProductLinkTexts.First().Title;
               }
        }
    }
}