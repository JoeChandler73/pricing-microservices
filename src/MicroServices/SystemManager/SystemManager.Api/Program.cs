using SystemManager.Api.Extensions;

var builder = WebApplication.CreateBuilder(args)
    .AddServices();
    
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

await app.RunAsync();