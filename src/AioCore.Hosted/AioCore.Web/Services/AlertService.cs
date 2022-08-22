using AntDesign;

namespace AioCore.Web.Services;

public interface IAlertService
{
    Task Show(string? message, NotificationType type);
    
    Task Show(string title, string message, NotificationType type);
    
    Task Success(string? title);
    
    Task Success(string title, string message);
    
    Task Warning(string? title);
    
    Task Warning(string title, string message);
    
    Task Error(string? title);
    
    Task Error(string title, string message);
}

public class AlertService : IAlertService
{
    private readonly NotificationService _notice;

    public AlertService(NotificationService notice)
    {
        _notice = notice;
    }

    public async Task Show(string? message, NotificationType type)
    {
        await _notice.Open(new NotificationConfig
        {
            Placement = NotificationPlacement.TopRight,
            NotificationType = type,
            Message = "Thông báo",
            Description = message
        });
    }

    public async Task Show(string title, string message, NotificationType type)
    {
        await _notice.Open(new NotificationConfig
        {
            Placement = NotificationPlacement.TopRight,
            NotificationType = type,
            Message = title,
            Description = message
        });
    }

    public async Task Success(string? title)
    {
        await _notice.Open(new NotificationConfig
        {
            Placement = NotificationPlacement.TopRight,
            NotificationType = NotificationType.Success,
            Message = "Thông báo",
            Description = title
        });
    }

    public async Task Success(string title, string message)
    {
        await _notice.Open(new NotificationConfig
        {
            Placement = NotificationPlacement.TopRight,
            NotificationType = NotificationType.Success,
            Message = title,
            Description = message
        });
    }

    public async Task Warning(string? title)
    {
        await _notice.Open(new NotificationConfig
        {
            Placement = NotificationPlacement.TopRight,
            NotificationType = NotificationType.Warning,
            Message = "Thông báo",
            Description = title
        });
    }

    public async Task Warning(string title, string message)
    {
        await _notice.Open(new NotificationConfig
        {
            Placement = NotificationPlacement.TopRight,
            NotificationType = NotificationType.Warning,
            Message = title,
            Description = message
        });
    }

    public async Task Error(string? title)
    {
        await _notice.Open(new NotificationConfig
        {
            Placement = NotificationPlacement.TopRight,
            NotificationType = NotificationType.Error,
            Message = "Thông báo",
            Description = title
        });
    }

    public async Task Error(string title, string message)
    {
        await _notice.Open(new NotificationConfig
        {
            Placement = NotificationPlacement.TopRight,
            NotificationType = NotificationType.Error,
            Message = title,
            Description = message
        });
    }
}