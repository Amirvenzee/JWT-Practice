using Asp.Versioning;
using Jwt_ApiVersioning;
using Jwt_ApiVersioning.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();



builder.Services.AddDbContext<ApiContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("Man"));
});


builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerDocument>();

builder.Services.AddSwaggerGen();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion =  new ApiVersion(1, 0);
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    
}).AddApiExplorer(x =>
{
    x.GroupNameFormat = "'v'VVV";
    //  / Swagger / V1 Or v2 / swagger.json"

    //  x.GroupNameFormat = "'v'VVVV";
    // / Swagger / V1.0 Or v2.0 / swagger.json"
}); ;


#region JWT

var myKey = Encoding.ASCII.GetBytes("flkbgmboksbfbmbdf-fdbbfbSBDF-dfbbdaddafb-nflbnboibjr");

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters()
    {
        IssuerSigningKey = new SymmetricSecurityKey(myKey),
        ValidateIssuerSigningKey = true,
        ValidateAudience = true,
        ValidAudience = "MyApi",
        ValidateIssuer = true,
        ValidIssuer = "Venzee",
        ValidateLifetime = true
    };


});
#endregion





var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(x =>
    {
        var provider = app.DescribeApiVersions();


        foreach (var item in provider)
        {
            x.SwaggerEndpoint($"/Swagger/{item.GroupName}/swagger.json", item.GroupName.ToString());
        }

        //x.SwaggerEndpoint("/Swagger/VilaOpenApi/swagger.json", "Vila Open Api");
        x.RoutePrefix = "";
    });
}
        
       


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
