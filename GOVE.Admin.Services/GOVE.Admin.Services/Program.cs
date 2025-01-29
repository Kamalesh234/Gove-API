using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Serilog;
using GOVE.Admin.Services.Middlewares;
using GOVE.Repository.Interfaces;
using GOVE.Repository.Implementors;
using GOVE.Infrastructure.Queries;
using GOVE.Models.Entities;
using IdentityServer4.AccessTokenValidation;
using GOVE.Models.Constants;
using GOVE.Models.Requests;
using GOVE.Infrastructure.Commands;
using GOVE.Infrastructure.Services.FileServer;
using GOVE.Models.Configurations;
using static GOVE.Infrastructure.Queries.GetNpaReportdetailsQuery;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors();
builder.Services.AddFluentValidation(x => x.RegisterValidatorsFromAssembly(Assembly.GetAssembly(typeof(GOVE.Infrastructure.Class1))));
builder.Services.AddControllers();

var configuration = builder.Configuration
                         .AddJsonFile($"appsettings.json")
                         .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json")
                         .Build();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Gove.API", Version = "v1" });
});
builder.Services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
             .AddIdentityServerAuthentication(options =>
             {
                 options.Authority = configuration["IdentityServerUrl"];
                 options.ApiName = Constants.ApiResource.UserApi;
                 options.ApiSecret = Constants.ApiResource.ApiResourceSecret;
                 options.RequireHttpsMetadata = false;
             });
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        builder => builder.WithOrigins(configuration["AllowCORSUrls"]!.Split(','))
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials());
});
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<GOVE.Admin.Services.Startup>());
builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(GOVE.Models.Constants.Startup)));
builder.Services.AddTransient<IMediator, Mediator>();
builder.Services.AddTransient<IUserRepository>(s => new UserRepository(configuration.GetConnectionString("ConnectionString")!));
builder.Services.AddTransient<IFileServerService, FileServerService>(s => new FileServerService(new FileServerConfiguration { CmsFilePath = configuration["CmsPath"], CmsUrl = configuration["CmsUrl"] }, s.GetService<ILogger<FileServerService>>()!));
builder.Services.AddTransient<IRequestHandler<GetLoginQuery.Query, Login?>, GetLoginQuery.Handler>();
builder.Services.AddTransient<IRequestHandler<GetUserMenusByUserId.Query, List<UserMenu>>, GetUserMenusByUserId.Handler>();
builder.Services.AddTransient<ICompanyRepository>(s => new ComapnyRepository(configuration.GetConnectionString("ConnectionString")!));
builder.Services.AddTransient<IRequestHandler<GetCompanyMaster.Query, CompanyMasterRequest>, GetCompanyMaster.Handler>();
builder.Services.AddTransient<IUsermanagementRepository>(s => new UserManagementRepository(configuration.GetConnectionString("ConnectionString")!));
builder.Services.AddTransient<IRequestHandler<UsertranslanderQuery.Query,List<UserTranslanderEntites>>, UsertranslanderQuery.Handler>();
builder.Services.AddTransient<IRequestHandler<UserDetailsQuery.Query, UserDetailsEntities>, UserDetailsQuery.Handler>();
builder.Services.AddTransient<IRequestHandler<UserLevelLookup.Query, List<Lookup>>, UserLevelLookup.Handler>();
builder.Services.AddTransient<IRequestHandler<GetUserDesignationlevelLookups.Query, List<Lookup>>, GetUserDesignationlevelLookups.Handler>();
builder.Services.AddTransient<IRequestHandler<UserReportinglevel.Query, List<ReportingLevel>>, UserReportinglevel.Handler>();
builder.Services.AddTransient<IRequestHandler<GetProspectLookups.Query, List<Lookup>>, GetProspectLookups.Handler>();
builder.Services.AddTransient<IRequestHandler<InsertUserDetails.Command, int>, InsertUserDetails.Handler>();
builder.Services.AddTransient<IReportsRepository>(s => new ReportsRepository(configuration.GetConnectionString("GoveConnectionString")!));
builder.Services.AddTransient<IRequestHandler<GetBranchLookupQuery.Query, List<BranchLookupEntities>>, GetBranchLookupQuery.Handler>();
builder.Services.AddTransient<IRequestHandler<GetNpaReportdetailsQuery.Query, List<NpaReportEntities>>, GetNpaReportdetailsQuery.Handler>();
builder.Services.AddTransient<IRequestHandler<GetNpasummaryReportdetailsQuery.Query, List<NpasummaryReportEntities>>, GetNpasummaryReportdetailsQuery.Handler>();
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAngularApp");
app.UseMiddleware<ExceptionMiddleware>();
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();