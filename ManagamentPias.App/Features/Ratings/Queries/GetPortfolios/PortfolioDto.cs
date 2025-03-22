namespace ManagementPias.App.Features.Ratings.Queries.GetPortfolios;

public record PortfolioDto
{
    public int Id { get; set; }
    public string Name { get; init; } = null!;
}
