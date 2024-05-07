using Calculator.Model;
using Calculator.Service;
using Calculator.SQLServer.DAL;
using Calculator.SQLServer.DAL.Base;
using Calculator.SQLServer.DAL.Repository;

namespace Calculator.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAutoMapper(typeof(MapperConfig).Assembly);
            builder.Services.Configure<ConnectionStringConfig>(config =>
            {
                config.ConnectionString = builder.Configuration["ConnectionStrings:Default"];
            })
            .AddDbContext<WebDbContext>()
            .AddScoped<IOperationRepository<OperationEntity>, OperationRepository>()
            .AddScoped<OperationService>();
          
            builder.Services.AddCors(config =>
            {
                config.AddDefaultPolicy(pBuilder =>
                {
                    pBuilder.AllowAnyHeader()
                    .AllowAnyOrigin()
                    .AllowAnyMethod();
                });
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors();
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
