using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Azure.Security.KeyVault.Secrets;
using Azure.Identity;

namespace ApiCubosExamenAzureMacarena.Helpers
{
    public class HelperActionServicesOAuth
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecretKey { get; set; }

        public HelperActionServicesOAuth(IConfiguration configuration)
        {
            /*this.Issuer =
                configuration.GetValue<string>("ApiOAuth:Issuer");
            this.Audience =
                configuration.GetValue<string>("ApiOAuth:Audience");
            this.SecretKey =
                configuration.GetValue<string>("ApiOAuth:SecretKey");*/

            var KeyVaultUri = configuration.GetValue<string>("KeyVault:VaultUri");
            var secretClient = new SecretClient(new Uri(KeyVaultUri), new DefaultAzureCredential());
            this.Issuer = GetSecretValue(secretClient, "secretoissuer");
            this.Audience = GetSecretValue(secretClient, "secretoaudience");
            this.SecretKey = GetSecretValue(secretClient, "secretosecretkey");

        }

        private string GetSecretValue(SecretClient secretClient, string secretName)
        {
            try
            {
                KeyVaultSecret secret = secretClient.GetSecret(secretName);
                return secret.Value;
            }catch (Exception ex)
            {
                throw new Exception("No se pudo obtener el secreto " + secretName + " del key vault." + ex.Message);

            }
        }


        public SymmetricSecurityKey GetKeyToken()
        {
            byte[] data =
                Encoding.UTF8.GetBytes(this.SecretKey);
            return new SymmetricSecurityKey(data);
        }

        public Action<JwtBearerOptions> GetJwtBearerOptions()
        {
            Action<JwtBearerOptions> options =
                new Action<JwtBearerOptions>(options =>
                {
                    options.TokenValidationParameters =
                    new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = this.Issuer,
                        ValidAudience = this.Audience,
                        IssuerSigningKey = this.GetKeyToken()
                    };
                });
            return options;
        }

        public Action<AuthenticationOptions>
            GetAuthenticateSchema()
        {
            Action<AuthenticationOptions> options =
                new Action<AuthenticationOptions>(options =>
                {
                    options.DefaultScheme =
                    JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultAuthenticateScheme =
                    JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme =
                    JwtBearerDefaults.AuthenticationScheme;
                });
            return options;
        }

    }
}
