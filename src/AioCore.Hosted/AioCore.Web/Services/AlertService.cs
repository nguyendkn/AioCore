using AntDesign;

namespace AioCore.Web.Services;

public interface IAlertService
{
    Task Show(string? description, NotificationType type);
    
    Task Show(string message, string description, NotificationType type);
    
    Task Success(string? description);
    
    Task Success(string message, string description);
    
    Task Warning(string? description);
    
    Task Warning(string message, string description);
    
    Task Error(string? description);
    
    Task Error(string message, string description);
}

public class AlertService : IAlertService
{
    private readonly NotificationService _notice;

    public AlertService(NotificationService notice)
    {
        _notice = notice;
    }

    public async Task Show(string? description, NotificationType type)
    {
        await _notice.Open(new NotificationConfig
        {
            Placement = NotificationPlacement.TopRight,
            NotificationType = type,
            Message = "Thông báo",
            Description = description
        });
    }

    public async Task Show(string message, string description, NotificationType type)
    {
        await _notice.Open(new NotificationConfig
        {
            Placement = NotificationPlacement.TopRight,
            NotificationType = type,
            Message = message,
            Description = description
        });
    }

    public async Task Success(string? description)
    {
        await _notice.Open(new NotificationConfig
        {
            Placement = NotificationPlacement.TopRight,
            NotificationType = NotificationType.Success,
            Message = "Thông báo",
            Description = description
        });
    }

    public async Task Success(string message, string description)
    {
        await _notice.Open(new NotificationConfig
        {
            Placement = NotificationPlacement.TopRight,
            NotificationType = NotificationType.Success,
            Message = message,
            Description = description
        });
    }

    public async Task Warning(string? description)
    {
        await _notice.Open(new NotificationConfig
        {
            Placement = NotificationPlacement.TopRight,
            NotificationType = NotificationType.Warning,
            Message = "Thông báo",
            Description = description
        });
    }

    public async Task Warning(string message, string description)
    {
        await _notice.Open(new NotificationConfig
        {
            Placement = NotificationPlacement.TopRight,
            NotificationType = NotificationType.Warning,
            Message = message,
            Description = description
        });
    }

    public async Task Error(string? description)
    {
        await _notice.Open(new NotificationConfig
        {
            Placement = NotificationPlacement.TopRight,
            NotificationType = NotificationType.Error,
            Message = "Thông báo",
            Description = description
        });
    }

    public async Task Error(string message, string description)
    {
        await _notice.Open(new NotificationConfig
        {
            Placement = NotificationPlacement.TopRight,
            NotificationType = NotificationType.Error,
            Message = message,
            Description = description
        });
    }
}