using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PictureStore.Models
{
    [Table("Bill")]
    public class Bill
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [DisplayName("Mã đơn hàng")]
        public int id { get; set; }
        
        public User user { get; set; }
        [Required(ErrorMessage = "Người đặt hàng không được trống")]
        [DisplayName("Người đặt hàng")]
        public int userId { get; set; }

        [DefaultValue(0)]
        [DisplayName("Tổng tiền")]
        public int total { get; set; }

        [DisplayName("Ngày đặt")]
        public DateTime dateCreate { get; set; }

        [DisplayName("Trạng thái")]
        public string status { get; set; } //đang giao, đã giao, đã hủy
    }
}