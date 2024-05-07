using Hangfire;

namespace SlowlySimulate.Api.Services;

public class BirthdayJobScheduler
{
    private readonly IRecurringJobManager _recurringJobManager;

    public BirthdayJobScheduler(IRecurringJobManager recurringJobManager)
    {
        _recurringJobManager = recurringJobManager;
    }

    public void ScheduleBirthdayCheckJob()
    {
        _recurringJobManager.AddOrUpdate<IBirthdayNotificationService>("BirthdayCheck", x => x.SendBirthdayNotificationToAll(), Cron.Daily);
        //RecurringJob.AddOrUpdate<IBirthdayNotificationService>("daily-birthday-notifications", x => x.SendBirthdayNotificationToAll(), Cron.Daily);
    }
}