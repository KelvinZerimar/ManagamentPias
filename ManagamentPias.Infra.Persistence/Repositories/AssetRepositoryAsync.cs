using LinqKit;
using ManagamentPias.App.Features.Assets.Queries.GetAssets;
using ManagamentPias.App.Interfaces;
using ManagamentPias.App.Interfaces.Repositories;
using ManagamentPias.App.Parameters;
using ManagamentPias.Domain.Entities;
using ManagamentPias.Infra.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Dynamic.Core;

namespace ManagamentPias.Infra.Persistence.Repositories;

public class AssetRepositoryAsync : GenericRepositoryAsync<Asset>, IAssetRepositoryAsync
{
    private readonly DbSet<Asset> _repository;
    private readonly IDataShapeHelper<Asset> _dataShaper;

    public AssetRepositoryAsync(ApplicationDbContext dbContext,
            IDataShapeHelper<Asset> dataShaper,
            ILogger<AssetRepositoryAsync> logger) : base(dbContext)
    {
        _repository = dbContext.Set<Asset>();
        _dataShaper = dataShaper;
    }

    public Task<bool> Create(Asset asset)
    {
        throw new NotImplementedException();
    }

    public Task<Asset> GetAsync(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<(IEnumerable<Entity> data, RecordsCount recordsCount)> GetPagedAssetReponseAsync(GetAssetsQuery requestParameters)
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
        result = result.Include(asset => asset.Rating).ThenInclude(rating => rating.Portfolio);

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
        var shapeData = _dataShaper.ShapeData(resultData, fields);

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
            predicate = predicate.Or(p => p.Rating.Portfolio.Description.Contains(description.Trim()));

        query = query.Where(predicate);
    }
}
