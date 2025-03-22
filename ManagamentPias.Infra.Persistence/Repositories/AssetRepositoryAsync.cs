using LinqKit;
using ManagementPias.App.Features.Assets.Queries.GetAssets;
using ManagementPias.App.Features.Assets.Queries.GetAssetsGroupedByDateSituation;
using ManagementPias.App.Interfaces.Repositories;
using ManagementPias.App.Parameters;
using ManagementPias.Domain.Entities;
using ManagementPias.Domain.Enums;
using ManagementPias.Infra.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Dynamic.Core;

namespace ManagementPias.Infra.Persistence.Repositories;

public class AssetRepositoryAsync : GenericRepositoryAsync<Asset>, IAssetRepositoryAsync
{
    private readonly DbSet<Asset> _repository;

    public AssetRepositoryAsync(ApplicationDbContext dbContext,
            ILogger<AssetRepositoryAsync> logger) : base(dbContext)
    {
        _repository = dbContext.Set<Asset>();
    }


    public async Task<(IEnumerable<AssetDetailsDto> data, RecordsCount recordsCount)> GetPagedAssetResponseAsync(GetAssetsQuery requestParameters)
    {
        var description = requestParameters.Description;

        var pageNumber = requestParameters.PageNumber;
        var pageSize = requestParameters.PageSize;
        var orderBy = requestParameters.OrderBy;
        var fields = requestParameters.Fields;

        int recordsTotal, recordsFiltered;

        // Setup IQueryable
        var result = _repository
            .AsNoTracking()
            .AsExpandable();

        // Count records total
        recordsTotal = await result.CountAsync();

        // filter data
        FilterByColumn(ref result, description);

        // Count records after filter
        recordsFiltered = await result.CountAsync();

        //set Record counts
        var recordsCount = new RecordsCount
        {
            RecordsFiltered = recordsFiltered,
            RecordsTotal = recordsTotal
        };

        // set order by
        if (!string.IsNullOrWhiteSpace(orderBy))
        {
            result = result.OrderBy(orderBy);
        }
        // projection
        result = result.Include(asset => asset.Rating); //.ThenInclude(rating => rating.Portfolio);

        // select columns
        if (!string.IsNullOrWhiteSpace(fields))
        {
            result = result.Select<Asset>("new(" + fields + ")");
        }

        // paging
        result = result
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        // retrieve data to list
        var resultData = await result.ToListAsync();

        // shape data
        var shapeData = resultData.Select(rd => new AssetDetailsDto()
        {
            Description = rd.Rating.Portfolio.GetDescription(),
            ValuePatrimony = rd.ValuePatrimony,
            NumUnit = rd.NumUnit,
            ValuationRating = rd.Rating.Valuation
        }).OrderByDescending(a => a.ValuePatrimony); //_dataShaper.ShapeData(resultData, fields);

        return (shapeData, recordsCount);
    }

    private void FilterByColumn(ref IQueryable<Asset> query, string? description)
    {
        if (!query.Any())
            return;

        if (string.IsNullOrEmpty(description))
            return;

        var predicate = PredicateBuilder.New<Asset>();

        if (!string.IsNullOrEmpty(description))
            predicate = predicate.Or(p => p.Rating.Portfolio.GetDescription().Contains(description.Trim()));

        query = query.Where(predicate);
    }

    public async Task<IEnumerable<Asset>> GetAssetByDateSituationAsync()
    {
        var result = _repository
            .AsNoTracking()
            .AsExpandable();

        var dateSituationUtc = _repository.Max(asset => asset.Rating.DateSituation).ToUniversalTime();

        result = result
            .Include(asset => asset.Rating)
            .Where(asset => asset.Rating.DateSituation >= dateSituationUtc.Date &&
                        asset.Rating.DateSituation < dateSituationUtc.Date.AddDays(1));
        var resultData = await result.ToListAsync();
        return resultData;
    }

    //public async Task<IEnumerable<decimal>> GetValuePatrimonyByMonthAsync(int year)
    //{
    //    var result = _repository
    //        .AsNoTracking()
    //        .AsExpandable();

    //    var valuePatrimonyByMonth = await result
    //        .Where(asset => asset.Rating.DateSituation.Year == year)
    //        .GroupBy(asset => asset.Rating.DateSituation.Month)
    //        .Select(group => group.Sum(asset => asset.ValuePatrimony))
    //        .ToListAsync();

    //    return valuePatrimonyByMonth;
    //}

    public async Task<IEnumerable<AssetChartDto>> GetValuePatrimonyByPortfolioMonthlyAsync(int year)
    {
        var result = _repository
            .AsNoTracking()
            .AsExpandable();

        var valuePatrimonyByPortfolioMonthly = await result
            .Include(asset => asset.Rating)
            .Where(asset => asset.Rating.DateSituation.Year >= year)
            .GroupBy(asset => new { asset.Rating.Portfolio })
            .Select(group => new AssetChartDto
            {
                Portfolio = group.Key.Portfolio,
                Assets = group.Select(asset => new AssetValue(
                    asset.Rating.DateSituation.Year,
                    asset.Rating.DateSituation.Month,
                    asset.ValuePatrimony)).ToArray()
            }).ToArrayAsync();

        return valuePatrimonyByPortfolioMonthly;
    }
}
