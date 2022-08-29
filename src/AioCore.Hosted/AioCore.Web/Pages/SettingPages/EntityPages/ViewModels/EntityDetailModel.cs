using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using AioCore.Domain.SettingAggregate;

namespace AioCore.Web.Pages.SettingPages.EntityPages.ViewModels;

public class EntityDetailModel
{
    public Guid Id { get; set; }
    
    [Required, DisplayName("Tên")]
    public string Name { get; set; } = default!;

    [Required, DisplayName("Nguồn dữ liệu")]
    public DataSource DataSource { get; set; }
    
    [DisplayName("Đường dẫn nguồn dữ liệu")]
    public string? SourcePath { get; set; }
}