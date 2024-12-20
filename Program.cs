using Confluent.Kafka;
using Gvz.Laboratory.UserService;
using Gvz.Laboratory.UserService.Abstractions;
using Gvz.Laboratory.UserService.Infrastructure;
using Gvz.Laboratory.UserService.Kafka;
using Gvz.Laboratory.UserService.Repositories;
using Gvz.Laboratory.UserService.Services;
using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));

var jwtOptions = builder.Configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();

// Add JWT bearer authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions?.SecretKey ?? "secretkeysecretkeysecretkeysecretkeysecretkeysecretkeysecretkey"))
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies["test-cookies"];
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddDbContext<GvzLaboratoryUserServiceDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAuthorization();

builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IPartyService, PartyService>();
builder.Services.AddScoped<IPartyRepository, PartyRepository>();

builder.Services.AddSingleton(() =>
{
    var config = new TypeAdapterConfig();
    new RegisterMapper().Register(config);
    return config;
});
builder.Services.AddScoped<IUserMapper, UserMapper>();

var config = new ProducerConfig
{
    BootstrapServers = "kafka:29092"
};
builder.Services.AddSingleton<IProducer<Null, string>>(new ProducerBuilder<Null, string>(config).Build());
builder.Services.AddScoped<IUserKafkaProducer, UserKafkaProducer>();

var consumerConfig = new ConsumerConfig
{
    BootstrapServers = "kafka:29092",
    GroupId = "user-group-id",
    AutoOffsetReset = AutoOffsetReset.Earliest
};
builder.Services.AddSingleton(consumerConfig);

builder.Services.AddSingleton<AddPartyKafkaConsumer>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<AddPartyKafkaConsumer>());

builder.Services.AddSingleton<UpdatePartyKafkaConsumer>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<UpdatePartyKafkaConsumer>());

builder.Services.AddSingleton<DeletePartyKafkaConsumer>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<DeletePartyKafkaConsumer>());

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors(x =>
{
    x.WithHeaders().AllowAnyHeader();
    x.WithOrigins("http://localhost:3000");
    x.WithMethods().AllowAnyMethod();
});

app.Run();