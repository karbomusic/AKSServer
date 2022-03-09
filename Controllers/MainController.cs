using Microsoft.AspNetCore.Mvc;

namespace AKSTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MainController : ControllerBase
    {
        private readonly ILogger<MainController> _logger;
        private static int totalRequests = 0; // must restart app to reset

        public MainController(ILogger<MainController> logger)
        {
            _logger = logger;
        }



        [HttpGet]
        public String Get()
        {
            // if the user sent a delay in the query string, wait that long in seconds before responding.
            // if nothing is passed don't wait.
            int delay = 0;
            if(Request.Query.Count > 0) {
                delay = Convert.ToInt32(Request.Query["delay"]);
            } 
            System.Threading.Thread.Sleep(delay*1000); // seconds
            Console.WriteLine("{0} Request: {1} Completed in {2} seconds...", DateTime.Now.ToString("MM/dd/yy hh:mm:ss.fffffff tt"), totalRequests, delay);
            totalRequests++;
            return "Request " + totalRequests + " completed in " + delay + " seconds.";
            
        }
    }
}