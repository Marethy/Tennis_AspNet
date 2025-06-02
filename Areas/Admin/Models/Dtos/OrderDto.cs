using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tennis.Areas.Admin.Models.Dtos
{
    public class OrderDto
    {
        [Key]
        [DisplayName("Mã đơn đặt hàng")]
        public int OrderId { get; set; }

        [DisplayName("Ngày đặt hàng")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Vui lòng chọn ngày đặt hàng.")]
        public DateTime DayOrder { get; set; }

        [DisplayName("Ngày vận chuyển")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Vui lòng chọn ngày vận chuyển.")]
        public DateTime DayDelivery { get; set; }

        [DisplayName("Đã trả tiền")]
        public bool PaidState { get; set; }

        [DisplayName("Trạng thái đơn hàng")]
        public bool DeliveryState { get; set; }

        [DisplayName("Tổng tiền")]
        [Range(0, double.MaxValue, ErrorMessage = "Tổng tiền phải lớn hơn hoặc bằng 0.")]
        public decimal TotalMoney { get; set; }

        [DisplayName("Khách hàng")]
        [Required(ErrorMessage = "Vui lòng chọn khách hàng.")]
        public int CustomerId { get; set; }
    }
}
