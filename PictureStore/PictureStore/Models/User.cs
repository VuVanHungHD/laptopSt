using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PictureStore.Models
{
    [Table("User")]
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [DisplayName("Mã người dùng")]
        public int id { get; set; }

        [Required(ErrorMessage ="Tên người dùng không được trống!")]
        [StringLength(50)]
        [DisplayName("Tên người dùng")]
        public string username { get; set; }

        [Required(ErrorMessage = "Email người dùng không được trống!")]
        [StringLength(50)]
        [DisplayName("Email")]
        public string email { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được trống!")]
        [StringLength(15)]
        [DisplayName("Số điện thoại")]
        public string phonenumber { get; set; }

        [Required(ErrorMessage = "Địa chỉ không được trống!")]
        [StringLength(200)]
        [DisplayName("Địa chỉ")]
        public string address { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được trống!")]
        [StringLength(255)]
        public string password { get; set; }

        [Required]
        [DefaultValue("USER")]
        [StringLength(10)]
        [DisplayName("Loại tài khoản")]
        public string userType { get; set; }

        [DefaultValue("Đang hoạt động")]
        [StringLength(30)]
        [DisplayName("Status")]
        public string status { get; set; }
    }
}