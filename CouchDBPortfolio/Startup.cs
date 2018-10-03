using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyCouch;
using MyCouch.Requests;

namespace CouchDBPortfolio
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            prepareDB();
        }

        private async Task prepareDB()
        {
            using(var client = new MyCouchServerClient("http://localhost:5984"))
            {
                var test = await client.Databases.GetAsync(new GetDatabaseRequest("tasks"));

                if (!test.IsSuccess && test.Reason == "not_found")
                {
                    await client.Databases.PutAsync(new PutDatabaseRequest("tasks"));
                }
            }

            using (var client = new MyCouchClient("http://localhost:5984", "tasks"))
            {
                var viewResponse = await client.Documents.GetAsync(new GetDocumentRequest("_design/views"));

                if (!viewResponse.IsSuccess && viewResponse.Error == "not_found")
                {
                    var designDocument = new PostDocumentRequest("{\n  \"_id\": \"_design/views\",\n  \"_rev\": \"13-e7f20fd89680fba468de1b2d2bfaffdc\",\n  \"views\": {\n    \"allDocs\": {\n      \"map\": \"function (doc) {\\n  emit(doc._id, doc._rev);\\n}\"\n    },\n    \"TasksByName\": {\n      \"map\": \"function(doc) {\\r\\nif (\'name\' in doc) { \\r\\n    emit(doc.name, doc._id);\\r\\n  }\\r\\n} \\r\\n\"\n    },\n    \"byTag\": {\n      \"reduce\": \"_count\",\n      \"map\": \"function (doc) {\\n  emit(doc.tag, 1);\\n}\"\n    },\n    \"byIsDone\": {\n      \"map\": \"function (doc) {\\n  emit(doc.isDone, 1);\\n}\",\n      \"reduce\": \"_count\"\n    }\n  },\n  \"language\": \"javascript\"\n}");
                    
                    await client.Documents.PostAsync(designDocument);  
                }
            }
        }
    }
}