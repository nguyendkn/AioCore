using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AioCore.Web.Pages.SettingPages.BuilderPages.ViewModels;

public class CodeDetailModel
{
    public Guid Id { get; set; }
    
    [Required, DisplayName("Tên tập tin")]
    public string Name { get; set; } = default!;

    [DisplayName("Mã nguồn")] public string Code { get; set; } = "// Code Empty";
}