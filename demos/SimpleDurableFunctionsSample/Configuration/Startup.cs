using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleDurableFunctionsSample.Data;
using SimpleDurableFunctionsSample.Services;

[assembly: FunctionsStartup(typeof(SimpleDurableFunctionsSample.Configuration.Startup))]

namespace SimpleDurableFunctionsSample.Configuration
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var configBuilder = new ConfigurationBuilder()
                .AddEnvironmentVariables();

            var intermediateConfig = configBuilder.Build();
            var workflowOptions = new WorkflowOptions();
            intermediateConfig.Bind(workflowOptions);

            builder.Services.AddTransient<IApprovalService, ApprovalService>();

            builder.Services.AddDbContext<WorkflowContext>(options =>
                options.UseSqlServer(workflowOptions.DbConnectionString));
            
            builder.Services.Configure<WorkflowOptions>(intermediateConfig);
        }
    }
}