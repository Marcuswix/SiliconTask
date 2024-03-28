using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WebApi.Configurations
{
    //Static gör att det inte går att skapa instanse av denna klass... 
    public static class JwtConfiguration
    {
        //En Jwt är viktig för att säker kommunitkation och authensiserign, som ett digitalt Id-kort...

        //IServiceCollection samlar alla appens tjänster ex. databasanrop och autentisering...

        //IConfiguration gör så att man kan komma åt API-nyvcklar, databaser och anslutninsgsträngar...

        //this gör så att du lägger till din funktion i IserviceCollection
        public static void RegisterJwt(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(x =>
             {
                 x.TokenValidationParameters = new TokenValidationParameters
                 {
                     //Vem är utgivaren
                     ValidateIssuer = true,
                     ValidIssuer = configuration["Jwt:Issuer"],

                     //Vem är mottagaren
                     ValidateAudience = true,
                     ValidAudience = configuration["Jwt:Audience"],

                     ValidateLifetime = true,

                     ValidateIssuerSigningKey = true,
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]!)),

                     ClockSkew = TimeSpan.Zero
                 };
             });
        }
    }
}
