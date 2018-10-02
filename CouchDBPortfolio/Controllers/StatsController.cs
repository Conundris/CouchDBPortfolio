using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CouchDBPortfolio.Models;
using Microsoft.AspNetCore.Mvc;
using MyCouch;
using MyCouch.Requests;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CouchDBPortfolio.Controllers
{
    public class StatsController : Controller
    {
        // GET
        public async Task<IActionResult> Index()
        {
            using (var client = new MyCouchClient(new DbConnectionInfo("http://localhost:5984/", "test")))
            {
                var tasks = new List<TodoTask>();
                
                var request = new QueryViewRequest("views", "allDocs") {IncludeDocs = true};

                var response = await client.Views.QueryAsync(request);
                
                var responseList = response.Rows.ToList();

                foreach (var responseItem in responseList)
                {
                    var model = JsonConvert.DeserializeObject<TodoTask>(responseItem.IncludedDoc);
                    model._id = responseItem.Id;
                    
                    var revJObject = (JObject)JsonConvert.DeserializeObject(responseItem.Value);
                    var revValue = revJObject["rev"].Value<string>(); 
                    
                    model._rev = revValue;

                    
                    tasks.Add(model);
                }
                
                return View(tasks);
            }
        }
    }
}