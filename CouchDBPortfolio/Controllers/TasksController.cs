using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CouchDBPortfolio.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;
using MyCouch;
using MyCouch.Requests;
using MyCouch.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CouchDBPortfolio.Controllers
{
    public class TasksController : Controller
    {
        // GET
        public async Task<IActionResult> Index()
        {
            using (var client = new MyCouchClient(new DbConnectionInfo("http://localhost:5984/", "test")))
            {
                var tasks = new List<TodoTask>();
                
                var request = new QueryViewRequest("_all_docs") {IncludeDocs = true};

                var response = await client.Views.QueryAsync(request);
                
                var responseList = response.Rows.ToList();
                
                //TODO: Foreach
                foreach (var responseItem in responseList)
                {
                    var model = JsonConvert.DeserializeObject<TodoTask>(responseItem.IncludedDoc);
                    model.id = responseItem.Id;
                    
                    var revJObject = (JObject)JsonConvert.DeserializeObject(responseItem.Value);
                    var revValue = revJObject["rev"].Value<string>(); 
                    
                    model.rev = revValue; //{"rev":"1-f0b8039ffb572da7d76a071be8e96fc6"}

                    
                    tasks.Add(model);
                }
                
               
                
                return View(tasks);
            }
        }

        [HttpPost]
        public async Task<DocumentHeaderResponse> Create([FromBody] TodoTask model)
        {
            Console.Write(ModelState.IsValid ? "YAY" : "NO");
            
            using (var client = new MyCouchClient(new DbConnectionInfo("http://localhost:5984/", "test")))
            {
                //client.Documents.PostAsync()
                await client.Documents.PostAsync("{\"name\":\"Daniel\"}");
                
                return null;
            }
            
            /*using(var client = new MyCouchServerClient("http://localhost:5984"))
            {
                
            }*/
        }

        public async Task<IActionResult> Details(string id, string rev)
        {
            using (var client = new MyCouchClient(new DbConnectionInfo("http://localhost:5984/", "test")))
            {
                var request = new GetDocumentRequest(id, rev);
                
                var response = await client.Documents.GetAsync(request);
                
                return View();
            }
            
            
        }
        
        public IActionResult Edit(string id, string rev)
        {
            return View();
        }
        
        public IActionResult Delete(string id)
        {
            return null;
        }
    }
}