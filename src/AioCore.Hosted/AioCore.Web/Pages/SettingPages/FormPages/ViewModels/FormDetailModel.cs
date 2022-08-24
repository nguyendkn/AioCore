using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AioCore.Web.Pages.SettingPages.FormPages.ViewModels;

public class FormDetailModel
{
    public Guid Id { get; set; }

    public Guid EntityId { get; set; }
    
    [Required, DisplayName("Tên")]
    public string Name { get; set; } = default!;
}