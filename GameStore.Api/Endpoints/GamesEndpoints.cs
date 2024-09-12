using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Entities;
using GameStore.Api.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
const string GetGameEndpointName = "GetGame";
private static readonly List<GameSummaryDto> games =[
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
        return game is null ? Results.NotFound() :Results.Ok(game.ToGameDetailsDto());
    })
    .WithName(GetGameEndpointName);
    
//POST/games
group.MapPost("/", (CreateGameDto newGame, GameStoreContext dbContext) =>
{
    
    Game game =newGame.ToEntity();
    game.Genre =dbContext.Genres.Find(newGame.GenreId);
 

    dbContext.Games.Add(game);   
    dbContext.SaveChanges();

   
    return Results.CreatedAtRoute(
        GetGameEndpointName, 
        new { id = game.Id}, 
        game.ToGameDetailsDto());
}); 

// PUT/games
group.MapPut("/{id}", (int id, UpdateGameDto updatedGame, GameStoreContext dbContext) =>
{
    // var index = games.FindIndex(game => game.Id == id);
    var existingGame =dbContext.Games.Find(id);
    if (existingGame is null)
    {
        return Results.NotFound();
    }
   dbContext.Entry(existingGame)
                    .CurrentValues
                    .SetValues(updatedGame.ToEntity(id));

    dbContext.SaveChanges();                
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
