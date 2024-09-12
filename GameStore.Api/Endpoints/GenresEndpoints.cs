using GameStore.Api.Data;

namespace GameStore.Api.Endpoints;
using GameStore.Api.Entities;
using GameStore.Api.Mapping;
using Microsoft.EntityFrameworkCore;

public static class GenresEndpoints
{
    public static RouteGroupBuilder MapGenresEndpoints(this WebApplication app)
    {
        var group =app.MapGroup("genres");
        group.MapGet("/", async (GameStoreContext dbContext) =>
            await dbContext.Genres
                            .Select(genre => genre.ToDto())
                            .AsNoTracking()
                            .ToListAsync());

        return group;        
    }
}
