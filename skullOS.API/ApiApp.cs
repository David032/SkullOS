using System.Reflection;

namespace skullOS.API
{
    public class ApiApp
    {
        public ApiApp(string[] args, List<string> controllers)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.WebHost.UseUrls("http://*:5000;https://*:5001");


            Assembly availableControllers = Assembly.Load("skullOS.Controllers");
            IEnumerable<TypeInfo> definedTypes = availableControllers.DefinedTypes;
            var definedControllers = definedTypes.Select(x => x).Where(x => x.Name.Contains("Controller"));
            foreach (var item in controllers)
            {
                if (definedControllers.Any(x => x.Name == item))
                {

                }
            }

            WebApplication app = builder.Build();

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
