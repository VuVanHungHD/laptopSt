using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PictureStore.Models
{
    [Table("Picture")]
    public class Picture
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [DisplayName("Mã tác phẩm")]
        public int id { get; set; }

        [Required(ErrorMessage = "Tên tác phẩm không được trống!")]
        [StringLength(50)]
        [DisplayName("Tên tác phẩm")]
        public string name { get; set; }
        [StringLength(250)]

        public Category category { get; set; }

        [Required(ErrorMessage = "Loại tranh không được để trống!")]
        [DisplayName("Loại tranh")]
        public int categoryId { get; set; }

        [DefaultValue(0)]
        [DisplayName("Giá")]
        public int price { get; set; }

        [DefaultValue(0)]
        [DisplayName("Giá khuyến mãi")]
        public int? promotionPrice { get; set; }

        [DisplayName("Ngày nhập")]
        public DateTime createDate { get; set; }

        [DisplayName("Mô tả")]
        public string descript { get; set; }

        [DisplayName("Chất liệu")]
        public string material { get; set; }

        [RegularExpression("[0-9]+x[0-9]+", ErrorMessage = "Định dạng đúng là: LxW (cm)")]
        [DisplayName("Kích thước (định dạng chiều-dàixchiều-rộng LxW (cm)) ")]
        public string size { get; set; }

        [DefaultValue("Khuyết danh")]
        [DisplayName("Tác giả (mặc định: Khuyết danh)")]
        public string author { get; set; }

        [DefaultValue(0)]
        [DisplayName("Lượt thích")]
        public int likeCount { get; set; }

        public string imageUrl { get; set; }

        [DefaultValue("Còn hàng")]
        [DisplayName("Trạng thái")]
        public string status { get; set; }
        [DisplayName("Mô tả ngắn")]

        [StringLength(250)]
        public string shortDescript { get; set; }
    }
}
