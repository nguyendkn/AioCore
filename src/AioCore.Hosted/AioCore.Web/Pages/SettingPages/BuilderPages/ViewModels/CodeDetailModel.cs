using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AioCore.Web.Pages.SettingPages.BuilderPages.ViewModels;

public class CodeDetailModel
{
    [Required, DisplayName("Tên tập tin")]
    public string Name { get; set; } = default!;
}