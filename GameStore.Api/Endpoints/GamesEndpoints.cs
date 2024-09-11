using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Entities;
using GameStore.Api.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
const string GetGameEndpointName = "GetGame";
private static readonly List<GameDto> games =[
    new(
        1,
        "Street Fighter",
        "Fighting",
        19.9M,
        new DateOnly(1992, 7, 15)),
    new(
        2,
        "Final Fantasy",
        "Roleplaying",
        59.9M,
        new DateOnly(2001, 9, 15)),
    new(
        3,
        "FIFA 2024",
        "Sports",
        59.9M,
        new DateOnly(2023, 11, 15))     
    
];

public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app) 
{
    var group = app.MapGroup("games").WithParameterValidation();
        // GET/games
group.MapGet("/", () => games);

    // GET/games/1
    group.MapGet ("/{id}", (int id, GameStoreContext  dbContext) =>
    {
        Game?  game= dbContext.Games.Find(id);
        return game is null ? Results.NotFound() :Results.Ok(game);
    })
    .WithName(GetGameEndpointName);
    
//POST/games
group.MapPost("/", (CreateGameDto newGame, GameStoreContext dbContext) =>
{
    
    Game game =newGame.ToEntity();
    game.Genre =dbContext.Genres.Find(newGame.GenreId);
 

    dbContext.Games.Add(game);   
    dbContext.SaveChanges();

   
    return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id}, game.ToDto());
});

// PUT/games
group.MapPut("/{id}", (int id, UpdateGameDto updatedGame) =>
{
    var index = games.FindIndex(game => game.Id == id);
    if (index== -1)
    {
        return Results.NotFound();
    }
    games[index] =new GameDto( 
        id,
        updatedGame.Name,
        updatedGame.Genre,
        updatedGame.Price,
        updatedGame.ReleaseDate
    );
    return Results.NoContent(); 
});

//Delete/games/1

group.MapDelete("/{id}", (int id)=>
{
 games.RemoveAll(game => game.Id == id);
 return Results.NoContent(); 
});

return group;
}
}
