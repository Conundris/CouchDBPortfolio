using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CouchDBPortfolio.Models;
using MyCouch;
using MyCouch.Requests;
using MyCouch.Responses;

namespace CouchDBPortfolio.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        public async Task<DocumentHeaderResponse> Test()
        {

            using (var client = new MyCouchClient(new DbConnectionInfo("http://localhost:5984/", "test")))
            {
                return await client.Documents.PostAsync("{\"name\":\"Daniel\"}");
                
               
            }
            
            /*using(var client = new MyCouchServerClient("http://localhost:5984"))
            {
                
            }*/
            
            
        }

        public async Task<string> read()
        {
            
            using (var client = new MyCouchClient(new DbConnectionInfo("http://localhost:5984/", "test")))
            {
                var request = new Request
                
                var response = await client.Documents.GetAsync(request);
                
                return response.Content;
            }
        }

        public async Task<string> update()
        {
            using (var client = new MyCouchClient(new DbConnectionInfo("http://localhost:5984/", "test")))
            {
                var request = new PutDocumentRequest("6f0b61dc18212616462956ad7d0024a4", "1-6824e4a536fdf83a246eca26b943dfe0", "{\"name\":\"Rob\"}");

                var response = await client.Documents.PutAsync(request);
                
                return response.ToStringDebugVersion();
            }
        }
    }
}