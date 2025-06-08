using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Influencerhub.DAL.Data;
using Influencerhub.Common.Enum;

public class JobStatusCheckerService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public JobStatusCheckerService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<InfluencerhubDBContext>();

                var now = DateTime.UtcNow;
                // So sánh theo ngày, không tính giờ
                var jobs = await dbContext.Jobs
                    .Where(j => j.Status == JobStatus.Available
                                && j.StartTime != null
                                && j.StartTime.Value.Date <= now.Date)
                    .ToListAsync(stoppingToken);

                if (jobs.Any())
                {
                    foreach (var job in jobs)
                    {
                        job.Status = JobStatus.RegistrationExpired;
                    }
                    await dbContext.SaveChangesAsync(stoppingToken);
                }
            }

            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }
}
