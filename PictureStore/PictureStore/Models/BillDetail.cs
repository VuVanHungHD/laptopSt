using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PictureStore.Models
{
    [Table("BillDetail")]
    public class BillDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int id { get; set; }
        public Picture picture { get; set; }
        [Required(ErrorMessage = "Sản phẩm mua không được trống")]
        [DisplayName("Sản phẩm mua")]
        public int pictureId { get; set; }
        public Bill bill { get; set; }
        [Required(ErrorMessage = "Đơn hàng không được trống")]
        [DisplayName("Đơn hàng")]
        public int billId { get; set; }

        [DefaultValue(0)]
        public int count { get; set; }
    }
}