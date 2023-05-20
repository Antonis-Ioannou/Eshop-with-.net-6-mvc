using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CirceBook.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
		[MaxLength(100)]
		public string Title { get; set; }

        [Required]
		[MaxLength(200)]
		public string Description { get; set; }

        [Required]
		[MaxLength(100)]
		public string ISBN { get; set; }

        [Required]
		[MaxLength(100)]
		public string Author { get; set; }

        [Required]
        [Range(1,10000)]
		[Display(Name = "List Price")]
		public double ListPrice { get; set; }

        [Required]
        [Range(1, 10000)]
		[Display(Name = "Price for 1-50")]
		public double Price { get; set; }

        [Required]
        [Range(1, 10000)]
		[Display(Name = "Price for 51-100")]
		public double Price50 { get; set; }

        [Required]
        [Range(1, 10000)]
		[Display(Name = "Price for 100+")]
		public double Price100 { get; set; }

		[ValidateNever]
		[MaxLength(200)]
		public string ImageUrl { get; set; }

        // db mappings //
        [Required]
		[Display(Name = "Category")]
		public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; }

        [Required]
        [Display(Name ="Cover Type")]
        public int CoverTypeId { get; set; }

        [ForeignKey("CoverTypeId")]
		[ValidateNever]
		public CoverType CoverType { get; set; }
    }
}
