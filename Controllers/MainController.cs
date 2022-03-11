using Microsoft.AspNetCore.Mvc;

namespace AKSTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MainController : ControllerBase
    {
        private readonly ILogger<MainController> _logger;
        private static int totalRequests = 0; // must restart app to reset
        private readonly static object logLock = new object();
        static String logFilePath = "AKSServerTestLog-" + DateTime.Now.Ticks + ".log";

        public MainController(ILogger<MainController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public String Get()
        {
            // if the user sent a delay in the query string, randomize with passed in delay as max
            // wait that long in seconds before responding. This helps similate real-world conditions
            // and stops tons of requests ending at roughly the same time when we send lots at once.
            // if nothing is passed don't introduce a delay.

            int delay = 0;
            int randomDelay = 0;
            string requestID = String.Empty;

            if(Request.Query.Count > 0) {
                delay = Convert.ToInt32(Request.Query["delay"]);
                randomDelay = GetRandomDelay(delay);
                requestID = Request.Query["reqid"];
            } 
            
            System.Threading.Thread.Sleep(randomDelay * 1000); // seconds
            Console.WriteLine("{0} Request: {1} ReqID: {3} Completed in {2} seconds...", DateTime.Now.ToString("MM/dd/yy hh:mm:ss.fffffff tt"), totalRequests, randomDelay, requestID);
            AppendLog(DateTime.Now.ToString("MM/dd/yy hh:mm:ss.fffffff tt") + ": Request# " + totalRequests + " ReqID: " + requestID);
            totalRequests++;
            return "Request " + totalRequests + " completed in " + randomDelay + " seconds.";
            
        }

        Random random = new Random();
        private int GetRandomDelay(int requestDelay)
        {      
            return random.Next(0, requestDelay);
        }

        private void AppendLog(string msg)
        {
            lock (logLock)
            {
                try
                {
                    StreamWriter SW;
                    SW = System.IO.File.AppendText(logFilePath);
                    SW.WriteLine(msg);
                    SW.Close();
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error writing to log {0} \r\n {1}", logFilePath, ex.Message);
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
            }
        }
    }
}