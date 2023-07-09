using Alachisoft.NCache.Caching.Distributed;
using Onion.Infrastructures.Configuration;
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
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod(); ;
                      });
});
//end

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(MyAllowSpecificOrigins);
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
