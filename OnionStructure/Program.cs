using Alachisoft.NCache.Caching.Distributed;
using Onion.Domains.Middleware;
using Onion.Domains.SignalR;
using Onion.Infrastructures.Configuration;
using Onion.Services.SignalR;
using OnionStructure.Configuration;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
//start

// add dbcontext
builder.Services.AddAutoMapper(typeof(AutoMapperConfig).Assembly);
builder.Services.AddNCacheDistributedCache(configuration =>
{
    configuration.CacheName = "myReplicatedCache";
    configuration.EnableLogs = true;
    configuration.ExceptionsEnabled = true;
});
builder.Services.RegisterDbContext(builder.Configuration);
//add dependecy inject
builder.Services.RegisterDI();
//add config JWT token
builder.Services.AddTokenBear(builder.Configuration);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();
var MyAllowSpecificOrigins = "CorsApi";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name:MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                      });
});
builder.Services.AddSignalR(e =>
{
    e.EnableDetailedErrors = true;
});
//end

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionMiddleware>();
app.UseRouting();
app.UseCors(MyAllowSpecificOrigins);
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<PresenceHub>("hubs/presence");
    endpoints.MapHub<ChatHub>("hubs/chathub");
    endpoints.MapControllers();
    //endpoints.MapFallbackToController("Index", "Fallback");//publish
});

app.Run();
