using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PictureStore.Models
{
    [Table("Blog")]
    public class Blog
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [DisplayName("Mã bài viết")]
        public int id { get; set; }

        [Required(ErrorMessage = "Tiêu đề bài viết không được trống!")]
        [StringLength(255)]
        [DisplayName("Tiêu đề")]
        public string name { get; set; }

        [Required(ErrorMessage = "Nội dung không được trống!")]
        [DisplayName("Nội dung bài viết")]
        public string content { get; set; }

        [DefaultValue("Khuyết danh")]
        [Required(ErrorMessage = "Tác giả không được trống!")]
        [DisplayName("Tác giả bài viết")]
        public string author { get; set; }

        [DisplayName("Ngày viết")]
        public DateTime dateCreate { get; set; }
    }
}