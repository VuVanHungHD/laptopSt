using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PictureStore.Models
{
    [Table("Category")]
    public class Category
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [DisplayName("Mã danh mục")]
        public int id { get; set; }

        [Required(ErrorMessage = "Tên danh mục không được trống!")]
        [StringLength(255)]
        [DisplayName("Tên danh mục")]
        public string name { get; set; }

        [Required(ErrorMessage = "Mô tả không được trống!")]
        [DisplayName("Mô tả về danh mục")]
        public string descript { get; set; }

        [DisplayName("Danh mục cha")]
        public int? parentId { get; set; }
    }
}