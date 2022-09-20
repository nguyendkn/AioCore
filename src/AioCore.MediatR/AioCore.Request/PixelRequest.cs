using System.Drawing;
using System.Drawing.Imaging;
using AioCore.Domain.DatabaseContexts;
using AioCore.Domain.DynamicAggregate;
using AioCore.Shared.Common.Constants;
using AioCore.Shared.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace AioCore.Request;

public class PixelRequest : IRequest<byte[]>
{
    public Guid Tenant { get; set; }

    public string Event { get; set; } = default!;

    internal class Handler : IRequestHandler<PixelRequest, byte[]>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DynamicContext _dynamicContext;

        public Handler(IHttpContextAccessor httpContextAccessor, 
            DynamicContext dynamicContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _dynamicContext = dynamicContext;
        }

        public async Task<byte[]> Handle(PixelRequest request, CancellationToken cancellationToken)
        {
            var httpRequest = _httpContextAccessor.HttpContext?.Request;
            _ = Task.Run(async () =>
            {
                await _dynamicContext.Requests.AddAsync(new DynamicRequest
                {
                    Tenant = request.Tenant,
                    Url = httpRequest?.Path.Value,
                    IP = httpRequest?.Headers[RequestHeader.XForwardedFor],
                    IPLong = httpRequest?.Headers[RequestHeader.XForwardedFor].ToString().IpToLong(),
                    Country = string.Empty,
                    Province = string.Empty,
                    Latitude = 0,
                    Longitude = 0
                });
            }, cancellationToken);
            return await Task.FromResult(Pixel());
        }

        private static byte[] Pixel()
        {
            var image = new Bitmap(1, 1);
            return image.ImageToByte2(ImageFormat.Png);
        }
    }
}