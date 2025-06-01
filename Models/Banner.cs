using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tennis.Models;

public class Banner
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int BannerId { get; set; }

	[DisplayName("Tên")]
	public string? BannerName { get; set; }

	[DisplayName("Khuyến mãi")]
    [Range(0, 100)]
    public int? ProductDiscount { get; set; }


	[DisplayName("Giá")] public decimal? BannerPrice { get; set; }

	[DisplayName("Nội dung")]
	public string? BannerDescription { get; set; }

	[StringLength(50)]
	[DisplayName("Hình ảnh")]
	public string? BannerImage { get; set; }

	[DisplayName("Ngày thêm")]
	[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
	public DateTime BannerDateCreated { get; set; } = DateTime.Now;
}