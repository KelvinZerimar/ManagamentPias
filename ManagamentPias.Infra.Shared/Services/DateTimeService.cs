using ManagementPias.App.Interfaces;

namespace ManagementPias.Infra.Shared.Services;

public class DateTimeService : IDateTimeService
{
    public DateTime NowUtc => DateTime.UtcNow;
}
