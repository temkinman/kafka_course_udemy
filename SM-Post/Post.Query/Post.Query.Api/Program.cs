using Microsoft.EntityFrameworkCore;
using Post.Query.Infrastructure.DataAccess;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddQueryInfrastructure(builder.Configuration.GetConnectionString("SqlServer")!, builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    db.Database.EnsureCreated();
}


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
