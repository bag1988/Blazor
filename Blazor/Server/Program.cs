using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Microsoft.AspNetCore.ResponseCompression;
using ServiceGrpc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddScoped(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();

    string serverUrl;
    serverUrl = config["ServerUrl"];
    if (Directory.Exists("wwwroot") && System.IO.File.Exists("wwwroot/ip.txt"))
    {
        serverUrl = System.IO.File.ReadAllText("wwwroot/ip.txt");
    }

    var channel = GrpcChannel.ForAddress(serverUrl, new GrpcChannelOptions
    {
        HttpHandler = new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler())
    });

    var client = new MessageContext.MessageContextClient(channel);

    return client;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
