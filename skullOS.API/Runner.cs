using skullOS.Modules;
using skullOS.Modules.Interfaces;

namespace skullOS.API
{
    public class Runner
    {
        public Task StartWebApiTask(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.WebHost.UseUrls("http://*:5000;https://*:5001");

            #region Skull Modules
            builder.Services.AddScoped<ICameraModule, Camera>();
            builder.Services.AddScoped<IBuzzerModule, Buzzer>();
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

            return app.RunAsync();
        }
        public void StartWebApiAsync(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.WebHost.UseUrls("http://*:5000;https://*:5001");

            #region Skull Modules
            //builder.Services.AddScoped<ICameraModule, Camera>();
            //builder.Services.AddScoped<IBuzzerModule, Buzzer>();
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

            app.RunAsync();
        }

        public void StartWebApi(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.WebHost.UseUrls("http://*:5000;https://*:5001");

            #region Skull Modules
            builder.Services.AddScoped<ICameraModule, Camera>();
            builder.Services.AddScoped<IBuzzerModule, Buzzer>();
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
