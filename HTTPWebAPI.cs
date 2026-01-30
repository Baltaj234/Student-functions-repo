using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using student_functions.Models.School; 

namespace comp.function;

public class HTTPWebAPI
{

    private readonly SchoolContext _context;
public HTTPWebAPI(ILoggerFactory loggerFactory, SchoolContext context)
{
_logger = loggerFactory.CreateLogger<HTTPWebAPI>();
_context = context;
}
    private readonly ILogger<HTTPWebAPI> _logger;

    public HTTPWebAPI(ILogger<HTTPWebAPI> logger)
    {
        _logger = logger;
    }

    [Function("HTTPWebAPI")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult("Welcome to Azure Functions!");
    }

   
   
    [Function("GetStudents")]
public async Task<HttpResponseData> GetStudents(
[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "students")] HttpRequestData req)
{
_logger.LogInformation("C# HTTP GET/posts trigger function processed a request in GetStudents().");
var students = await _context.Students.ToArrayAsync<Student>();
var response = req.CreateResponse(HttpStatusCode.OK);
response.Headers.Add("Content-Type", "application/json");
await response.WriteStringAsync(JsonConvert.SerializeObject(students));
return response;
}


}