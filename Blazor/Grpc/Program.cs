using BlazorApp.Grpc.Data;
using BlazorApp.Grpc.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MessagesDbContext>();

builder.Services.AddScoped<IMessageInterface, MessageRelease>();
builder.Services.AddGrpc();

builder.Services.AddCors(o => o.AddPolicy("AllowAll", builder =>
{
    builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
        .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
}));



var app = builder.Build();

app.UseStaticFiles();
app.UseCors();
app.UseGrpcWeb();
// Configure the HTTP request pipeline.
app.MapGrpcService<MessageService>().RequireCors("AllowAll").EnableGrpcWeb();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
