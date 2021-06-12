using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Memberships.Entities
{
    [Table("UserSubscription")]
    public class UserSubscription
    {
        [Key, Column(Order =1)]
        [Required]
        public int subscriptionId { get; set; }

        [Key, Column(Order = 2)]
        [Required]
        [MaxLength(128)]
        public string userId { get; set; }

        public DateTime? StateDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}