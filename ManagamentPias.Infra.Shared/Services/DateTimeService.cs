using ManagamentPias.App.Interfaces;

namespace ManagamentPias.Infra.Shared.Services;

public class DateTimeService : IDateTimeService
{
    public DateTime NowUtc => DateTime.UtcNow;
}
