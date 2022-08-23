using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AioCore.Web.Pages.SettingPages.EntityPages.ViewModels;

public class EntityDetailModel
{
    public Guid Id { get; set; }
    
    [Required, DisplayName("Tên")]
    public string Name { get; set; } = default!;
}