using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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

        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([Bind(include:"name, description, isDone, tag")] TodoTask model)
        {
            Console.Write(ModelState.IsValid ? "YAY" : "NO");
            
            using (var client = new MyCouchClient(new DbConnectionInfo("http://localhost:5984/", "test")))
            {
                var content = JsonConvert.SerializeObject(model);
                
                var request = new PostDocumentRequest(content);
                
                var response = await client.Documents.PostAsync(request);

                return RedirectToAction("Index", "Tasks");
            }
        }

        public async Task<IActionResult> Details(string id, string rev)
        {
            using (var client = new MyCouchClient(new DbConnectionInfo("http://localhost:5984/", "test")))
            {
                var task = new TodoTask();
                
                var request = new GetDocumentRequest(id, rev);
                
                var response = await client.Documents.GetAsync(request);

                task = JsonConvert.DeserializeObject<TodoTask>(response.Content);
                task._id = response.Id;
                task._rev = response.Rev; 
                
                return View(task);
            }
            
            
        }
        
        [HttpPost]
        public async Task<IActionResult> Edit([Bind(include:"_id, _rev, name, description, isDone, tag")] TodoTask model)
        {
            
            using (var client = new MyCouchClient(new DbConnectionInfo("http://localhost:5984/", "test")))
            {
                var content = JsonConvert.SerializeObject(model);
                
                var request = new PutDocumentRequest(model._id, model._rev, content);

                var response = await client.Documents.PutAsync(request);
    
                return RedirectToAction("Index", "Tasks");
            }
        }
        
        public async Task<IActionResult> Delete(string id, string rev)
        {
            using (var client = new MyCouchClient(new DbConnectionInfo("http://localhost:5984/", "test")))
            {
                var request = new DeleteDocumentRequest(id, rev);

                var response = await client.Documents.DeleteAsync(request);
                
                return RedirectToAction("Index", "Tasks");
            }
        }
    }
}