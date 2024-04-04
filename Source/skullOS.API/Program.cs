using skullOS.Core;
using skullOS.Modules;
using skullOS.Modules.Interfaces;

namespace skullOS.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.WebHost.UseUrls("http://*:5000;");

            #region Skull Configuration
            FileManager.CreateSkullDirectory(false);
            #endregion

            #region Skull Modules
            builder.Services.AddSingleton<ICameraModule, Camera>();
            builder.Services.AddSingleton<IBuzzerModule, Buzzer>();
            #endregion


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            if (!app.Environment.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }


            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}