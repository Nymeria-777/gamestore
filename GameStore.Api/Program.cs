using GameStore.Api;
using GameStore.Api.Dtos;
using GameStore.Api.Endpoints;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);


var app = builder.Build();


app.MapGamesEndpoints();  

app.Run();
 