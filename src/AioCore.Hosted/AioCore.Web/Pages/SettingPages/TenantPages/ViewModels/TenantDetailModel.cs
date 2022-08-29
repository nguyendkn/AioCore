using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AioCore.Web.Pages.SettingPages.TenantPages.ViewModels;

public class TenantDetailModel
{
    public Guid Id { get; set; }
    
    [Required, DisplayName("Tên")]
    public string Name { get; set; } = default!;

    [Required, DisplayName("Tên miền")]
    public string Domain { get; set; } = default!;

    [Required, DisplayName("Tiêu đề")]
    public string Title { get; set; } = default!;

    [DisplayName("Từ khóa chính")]
    public string? Keyword { get; set; }

    [DisplayName("Nhóm thuê bao")]
    public Guid GroupId { get; set; }
}