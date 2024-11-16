using ActivelyServer;
using ActivelyServer.Services;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


BazaDanych baza = new BazaDanych();




builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder
            .AllowAnyOrigin()    
            .AllowAnyMethod()    
            .AllowAnyHeader());  
});

builder.Services.AddSingleton<ICredentialService, CredentialService>();



var app = builder.Build();

app.UseCors("AllowAll"); 


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "uploads")),
    RequestPath = "/uploads"
});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();
