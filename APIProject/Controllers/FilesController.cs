using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using APIProject.Models;
using System.Net.Http;
using System.Net;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Microsoft.ApplicationInsights;

namespace APIProject.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly HttpClient _apiclient;
        private readonly HttpClient _tokenclient;
        private IConfiguration _configuration;
        private readonly TelemetryClient _telemetry;

        public FilesController(IConfiguration configuration, HttpClient apiclient, HttpClient tokenclient ) 
        {
            _configuration = configuration;
            _apiclient = apiclient;
            _tokenclient = tokenclient;
        }

        [HttpGet("{fileid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<File>> GetByfileId(int fileid)
        {     
            var requesturi = ("/data/v1/files/"+ fileid.ToString());
            var request = new HttpRequestMessage(HttpMethod.Get, requesturi);
            request.Headers.Accept.Clear();
            request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            //           var stringTask = client.GetStreamAsync("/data/v1/files/101");
            try
            {
                var resource = _configuration["TargetURL"];
                System.Console.WriteLine("==>>>");
                System.Diagnostics.Trace.WriteLine("==>>> " + resource);
                System.Diagnostics.Trace.TraceInformation("==>>> " + Environment.GetEnvironmentVariable("IDENTITY_ENDPOINT"));
                System.Diagnostics.Trace.TraceInformation("==>>> " + Environment.GetEnvironmentVariable("IDENTITY_HEADER"));

                //var access_token = GetToken();
                //request.Headers.Add("Authorization", "Bearer " + access_token);
                var response = await _apiclient.SendAsync(request, System.Threading.CancellationToken.None);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    File file = JsonConvert.DeserializeObject<File>(responseContent);
                    return Ok(file);
                }
                return NotFound();

            }
            catch (Exception e){
                Console.WriteLine(e.ToString());
                throw;              
            }
        }
        public async Task<string> GetToken()
        {
            var resource = _configuration["TargetURL"];
            System.Diagnostics.Trace.TraceInformation("==>>> " + resource);
            System.Diagnostics.Trace.TraceInformation("==>>> " + Environment.GetEnvironmentVariable("IDENTITY_ENDPOINT"));
            System.Diagnostics.Trace.TraceInformation("==>>> " + Environment.GetEnvironmentVariable("IDENTITY_HEADER"));

            var request = new HttpRequestMessage(HttpMethod.Get,
                String.Format("{0}/?resource={1}&api-version=2019-08-01", Environment.GetEnvironmentVariable("IDENTITY_ENDPOINT"), resource));
            request.Headers.Add("X-IDENTITY-HEADER", Environment.GetEnvironmentVariable("IDENTITY_HEADER"));
            var response = await _tokenclient.SendAsync(request);
            var result = JsonConvert.DeserializeObject<Dictionary<string, string>>(await response.Content.ReadAsStringAsync());
            System.Diagnostics.Trace.TraceError("==>>> "+result);
            return result["access_token"];
        }



    }


}