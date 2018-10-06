using System.Collections.Generic;
using System.Data.SqlTypes;
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
            var resultList = new List<StatsModel>();
            await getByIsDone(resultList);
            await getByTag(resultList);

            return View(resultList);
        }

        private async Task getByIsDone(List<StatsModel> resultList)
        {
            using (var client = new MyCouchClient(new DbConnectionInfo("http://localhost:5984/", "tasks")))
            {
                var request = new QueryViewRequest("views", "byIsDone") {Reduce = true, Group = true};

                var response = await client.Views.QueryAsync(request);
                
                var responseList = response.Rows.ToList();

                foreach (var responseItem in responseList)
                {
                    var statsObject = new StatsModel
                    {
                        Section = "byIsDone",
                        Key = responseItem.Key.ToString(), 
                        Value = responseItem.Value
                    };

                    resultList.Add(statsObject);
                }
            } 
        }
        
        private async Task getByTag(List<StatsModel> resultList)
        {
            using (var client = new MyCouchClient(new DbConnectionInfo("http://localhost:5984/", "tasks")))
            {
                var request = new QueryViewRequest("views", "byTag") {Reduce = true, Group = true};

                var response = await client.Views.QueryAsync(request);
                
                var responseList = response.Rows.ToList();

                foreach (var responseItem in responseList)
                {
                    var statsObject = new StatsModel
                    {
                        Section = "byTag",
                        Key = responseItem.Key.ToString(), 
                        Value = responseItem.Value
                    };

                    resultList.Add(statsObject);
                }
            } 
        }    
    }
}