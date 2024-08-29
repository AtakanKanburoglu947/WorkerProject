using AutoMapper;
using System.Text.Json;
using WorkerService.Context;
using WorkerService.Models;
using WorkerService.Services;

namespace WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceProvider _serviceProvider;
       
        public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }
         
                using (var scope = _serviceProvider.CreateScope())
                {
                    var apiService = scope.ServiceProvider.GetRequiredService<APIService>();
                    var dbService = scope.ServiceProvider.GetRequiredService<dbService>();
                    var mvcService = scope.ServiceProvider.GetRequiredService<mvcService>();
                    var content = await apiService.GetData(stoppingToken);
                    var existingUsers = dbService.GetUsers();
                    var mvcExistingUsers = mvcService.GetUsers();
                    using (JsonDocument document = JsonDocument.Parse(content))
                {
                    JsonElement root = document.RootElement;
                    JsonElement dataArray = root.GetProperty("data");
                    foreach (JsonElement userElement in dataArray.EnumerateArray())
                    {
                        var response = apiService.ParseJson(userElement);
                        var user = response;
                        var existingUser = existingUsers.FirstOrDefault(u => u.UserId == user.UserId);
                        var mvcExistingUser = mvcExistingUsers.FirstOrDefault(u=> u.UserId == user.UserId);

                            if (existingUser != null)
                            {
                               await dbService.UpdateUser(existingUser,user,stoppingToken);

                            }
                            else
                            {
                                await dbService.AddUser(user, stoppingToken);


                            }
                            if (mvcExistingUser != null)
                            {
                                var dbUser = dbService.FirstOrDefault(user);
                               await mvcService.UpdateUser(mvcExistingUser, dbUser,stoppingToken);
                            }
                            else
                            {
                                var dbUser = dbService.FirstOrDefault(user);
                                await mvcService.AddUser(dbUser, stoppingToken);
                            }




                            await dbService.SaveChangesAsync(stoppingToken);
                            await mvcService.SaveChangesAsync(stoppingToken);
                    }
                }
                }

                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}
