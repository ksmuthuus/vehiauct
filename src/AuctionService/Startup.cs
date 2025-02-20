using AuctionService.Data;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

namespace AuctionService;

public class Startup(IConfiguration config)
{
  public void ConfigureServices(IServiceCollection services)
  {
    // Add services to the container.
    services.AddControllers();
    services.AddDbContext<AuctionDbContext>(options =>
        options.UseNpgsql(config.GetConnectionString("DefaultConnection")));
    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    services.AddMassTransit(x =>
    {
      x.AddEntityFrameworkOutbox<AuctionDbContext>(o =>
      {
        o.QueryDelay = TimeSpan.FromSeconds(60);
        o.UsePostgres();
        o.UseBusOutbox();
      });
      x.UsingRabbitMq((context, cfg) =>
      {
        cfg.ConfigureEndpoints(context);
      });
    });
    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
          options.Authority = config["IdentityService:IssuerUri"];
          options.RequireHttpsMetadata = false;
          options.TokenValidationParameters.ValidateAudience = false;
          options.TokenValidationParameters.NameClaimType = "username";
        });
  }

  public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
  {
    // Configure the HTTP request pipeline.
    if (env.IsDevelopment())
    {
      app.UseDeveloperExceptionPage();
    }

    // app.UseHttpsRedirection();

    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
      endpoints.MapControllers();
    });
  }
}
