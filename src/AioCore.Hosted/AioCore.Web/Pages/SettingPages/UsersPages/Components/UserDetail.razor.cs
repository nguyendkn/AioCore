using System.Text.Json;
using AntDesign;
using Microsoft.AspNetCore.Components.Forms;

namespace AioCore.Web.Pages.SettingPages.UsersPages.Components;

public class Model
{
    public string Layout { get; set; } = FormLayout.Horizontal;
    public string FieldA { get; set; }
    public string FieldB { get; set; }
}

public partial class UserDetail
{
    

    private Model model = new Model();

    private ColLayoutParam GetFormLabelCol()
    {
        return model.Layout == FormLayout.Horizontal ? new ColLayoutParam { Span = "4" } : null;
    }

    private ColLayoutParam GetFormWrapperCol()
    {
        return model.Layout == FormLayout.Horizontal ? new ColLayoutParam { Span = "14" } : null;
    }

    private ColLayoutParam GetButtonItemLayout()
    {
        return model.Layout == FormLayout.Horizontal ? new ColLayoutParam { Span = "14", Offset = "4" } : null;
    }

    private void OnFinish(EditContext editContext)
    {
        Console.WriteLine($"Success:{JsonSerializer.Serialize(model)}");
    }

    private void OnFinishFailed(EditContext editContext)
    {
        Console.WriteLine($"Failed:{JsonSerializer.Serialize(model)}");
    }
}