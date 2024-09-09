﻿using GameStore.Api.Dtos;

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
    group.MapGet ("/{id}", (int id) =>
    {
        GameDto?  game= games.Find(game => game.Id == id);
        return game is null ? Results.NotFound() :Results.Ok(game);
    })
    .WithName(GetGameEndpointName);
    
//POST/games
group.MapPost("/", (CreateGameDto newGame) =>
{
    GameDto game =new(
        games.Count +1,
        newGame.Name,
        newGame.Genre,
        newGame.Price,
        newGame.ReleaseDate);
    games.Add(game);    
    
    return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id}, game);
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
