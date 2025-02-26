using FluentValidation;
using Lingo_VerticalSlice.Configurations;
using Lingo_VerticalSlice.Contracts.Services;
using Lingo_VerticalSlice.Database;
using Lingo_VerticalSlice.Features.AnonymousEmail.SendAnonymousEmail;
using Lingo_VerticalSlice.Features.CardSet.AddCardSetToExistingFolder;
using Lingo_VerticalSlice.Features.CardSet.CreateCardSetFolder;
using Lingo_VerticalSlice.Features.CardSet.DeleteCardSet;
using Lingo_VerticalSlice.Features.CardSet.GetCardSet;
using Lingo_VerticalSlice.Features.CardSetWord.AddWordToCardSet;
using Lingo_VerticalSlice.Features.CardSetWord.CheckQuizAnswer;
using Lingo_VerticalSlice.Features.CardSetWord.GetCardSetWord;
using Lingo_VerticalSlice.Features.CardSetWord.GetCardSetWordWithoutSP;
using Lingo_VerticalSlice.Features.Folder.DeleteFolder;
using Lingo_VerticalSlice.Features.Folder.GetFolderByUserId;
using Lingo_VerticalSlice.Features.User.CheckEmailExistence;
using Lingo_VerticalSlice.MiddleWare;
using Lingo_VerticalSlice.Shared;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;


var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        builder =>
        {
            builder.WithOrigins("https://lingo-bay.vercel.app"
                ,"http://localhost:5173")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

builder.Services.AddControllers();
builder.Services.AddAuthorization();    
builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddTransient<CalculateNextStepService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSqlServer<ApplicationDbContext>(builder.Configuration.GetConnectionString("DefaultConnection"));
builder.Services.AddHttpContextAccessor();


builder.Services.AddTransient<IAddCardSetToExistingFolderRepository, AddCardSetToExistingFolderRepository>();
builder.Services.AddTransient<ICreateCardSetFolderRepository, CreateCardSetFolderRepository>();
builder.Services.AddTransient<IDeleteCardSetRepository, DeleteCardSetRepository>();
builder.Services.AddTransient<IGetCardSetRepository, GetCardSetRepository>();
builder.Services.AddTransient<IAddWordToCardSetRepository, AddWordToCardSetRepository>();
builder.Services.AddTransient<IDeleteFolderRepository, DeleteFolderRepository>();
builder.Services.AddTransient<IGetFolderByUserIdRepository, GetFolderRepository>();
builder.Services.AddTransient<IGetCardSetWordRepository, GetCardSetWordRepository>();
builder.Services.AddTransient<ISendAnonymousEmailRepository,SendAnonymousEmailRepository>();
builder.Services.AddTransient<ICheckQuizAnswerRepository,CheckQuizAnswerRepository>();
builder.Services.AddTransient<ICheckEmailExistenceRepository, CheckEmailExistenceRepository>();
builder.Services.AddTransient<IGetCardSetWordWithoutSPRepository, GetCardSetWordWithoutSPRepository>();
builder.Services.AddTransient<IUserService, UserService>();

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<CustomResultFilter>();
});
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer",new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer abcdef12345\"",
        
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
builder.Services.AddOptions<BearerTokenOptions>(IdentityConstants.BearerScheme).Configure(options =>
{
    options.BearerTokenExpiration = TimeSpan.FromDays(60);
});

var assembly = typeof(Program).Assembly;
builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(assembly));
builder.Services.AddValidatorsFromAssembly(assembly);
var app = builder.Build();



app.MapIdentityApi<IdentityUser>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/logout", async (SignInManager<IdentityUser> signInManager,
        [FromBody] object empty) =>
    {
        if (empty != null)
        {
            await signInManager.SignOutAsync();
            return Results.Ok();
        }
        return Results.Unauthorized();
    })
    .WithOpenApi()
    .RequireAuthorization();

app.UseMiddleware<ErrorHandler>();
app.UseHttpsRedirection();
app.UseCors("_myAllowSpecificOrigins");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program
{
    
}