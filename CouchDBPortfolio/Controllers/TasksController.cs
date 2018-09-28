using System;
using System.Threading.Tasks;
using CouchDBPortfolio.Models;
using Microsoft.AspNetCore.Mvc;
using MyCouch;
using MyCouch.Responses;

namespace CouchDBPortfolio.Controllers
{
    public class TasksController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
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
    }
}