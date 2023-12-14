using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Naomi.marketing_service.Configurations;
using Naomi.marketing_service.Models.Contexts;
using Naomi.marketing_service.Models.Response;
using Naomi.marketing_service.Services.ApprovalService;
using Naomi.marketing_service.Services.EntertainService;
using Naomi.marketing_service.Services.MembershipService;
using Naomi.marketing_service.Services.PromoAppService;
using Naomi.marketing_service.Services.PromoChannelService;
using Naomi.marketing_service.Services.PromoClassService;
using Naomi.marketing_service.Services.PromoMaterialService;
using Naomi.marketing_service.Services.PromoStatusService;
using Naomi.marketing_service.Services.PromotionService;
using Naomi.marketing_service.Services.PromoTypeService;
using Naomi.marketing_service.Services.PubService;
using Naomi.marketing_service.Services.S3Service;
using Naomi.marketing_service.Services.SapService;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using VaultSharp.Extensions.Configuration;

//config App
var builder = WebApplication.CreateBuilder(args);

//Config Vault
VaultConfig? vaultConfig = builder.Configuration.GetSection("Vault").Get<VaultConfig>();
builder.Configuration.AddJsonFile("appsettings.json").AddVaultConfiguration(() =>
    new VaultOptions(vaultAddress: vaultConfig.Link!, vaultToken: vaultConfig.Token), vaultConfig.BasePath!, vaultConfig.MountPoint!);

//Config Env
builder.Services.Configure<AppConfig>(builder.Configuration);
AppConfig? appConfig = builder.Configuration.Get<AppConfig>();

//Config DB Postgres
builder.Services.AddDbContext<DataDbContext>(options => {
    options.UseNpgsql(appConfig.PostgreSqlConnectionString!);
});

//Config Cap Kafka
builder.Services.AddCap(x =>
{
    x.UsePostgreSql(opt =>
    {
        opt.Schema = "cap";
        opt.ConnectionString = appConfig.PostgreSqlConnectionString!;
    });

    x.UseKafka(opt =>
    {
        opt.Servers = appConfig.KafkaConnectionString!;
        opt.CustomHeaders = kafkaResult => new List<KeyValuePair<string, string>>
        {
            new ("cap-msg-id", Guid.NewGuid().ToString()),
            new ("cap-msg-name", kafkaResult.Topic)
        };
    });
    x.UseDashboard();
});

//Dependency Injection
builder.Services.AddScoped<IApprovalService, ApprovalService>();
builder.Services.AddScoped<IEntertainService, EntertainService>();
builder.Services.AddScoped<IMembershipService, MembershipService>();
builder.Services.AddScoped<IPromoAppService, PromoAppService>();
builder.Services.AddScoped<IPromoChannelService, PromoChannelService>();
builder.Services.AddScoped<IPromoClassService, PromoClassService>();
builder.Services.AddScoped<IPromoMaterialService, PromoMaterialService>();
builder.Services.AddScoped<IPromoStatusService, PromoStatusService>();
builder.Services.AddScoped<IPromotionService, PromotionService>();
builder.Services.AddScoped<IPromoTypeService, PromoTypeService>();
builder.Services.AddScoped<IPubService, PubService>();
builder.Services.AddScoped<IS3Service, S3Service>();
builder.Services.AddScoped<ISapService, SapService>();

//Background Job
builder.Services.AddHostedService<EntertainJob>();
builder.Services.AddHostedService<PromotionStatusJob>();

//Config Automapper
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

//Config Jwt Auth
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    var rsa = RSA.Create();
    rsa.ImportFromPem(appConfig.PublicKey!);

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = false,
        ValidateIssuer = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new RsaSecurityKey(rsa)
    };
});

//Config Filter Validation
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

//Config Controller
builder.Services.AddControllers(option =>
                {
                    option.Filters.Add(typeof(ValidateModelResponse));
                })
                .AddFluentValidation(v =>
                {
                    v.ImplicitlyValidateChildProperties = true;
                    v.ImplicitlyValidateRootCollectionElements = true;
                    v.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
                });

//Config Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standart Authorize Bearer",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    c.OperationFilter<SecurityRequirementsOperationFilter>();
});

var app = builder.Build();

//Apply MigrateDb
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DataDbContext>();
    db.Database.Migrate();
}

//Run Swagger
app.UseSwagger();
app.UseSwaggerUI(c => {
    c.DefaultModelsExpandDepth(-1);
});

//Run Https
app.UseHttpsRedirection();

//Run Auth
app.UseAuthentication();
app.UseAuthorization();

//Run Contoller
app.MapControllers();

//Run App
app.Run();
