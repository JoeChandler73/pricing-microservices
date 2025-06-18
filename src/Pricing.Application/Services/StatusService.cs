using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pricing.Application.Configuration;
using Pricing.Application.Events;
using Pricing.Application.Messaging;

namespace Pricing.Application.Services;

public class StatusService(
    IMessageBus messageBus, 
    IOptions<StatusOptions> _options,
    ILogger<StatusService> _logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("StatusService is running.");
        
        using var timer = new PeriodicTimer(_options.Value.Interval);

        while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                _logger.LogInformation("Sending status message.");
                
                await messageBus.Publish(CreateStatusEvent());
            }
            catch (Exception e)
            {
                _logger.LogError("StatusService failed to send status message: {0}", e.Message);
            }
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("StatusService is stopping.");

        await messageBus.DisposeAsync();
        await base.StopAsync(cancellationToken);
    }

    private StatusEvent CreateStatusEvent()
    {
        return new StatusEvent(
            Environment.MachineName,
            Path.GetFileName(Environment.GetCommandLineArgs()[0]),
            Status.Active);
    }
}