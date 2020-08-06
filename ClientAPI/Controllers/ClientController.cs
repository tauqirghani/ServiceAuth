using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using APIProject.Models;
using Newtonsoft.Json;
using Microsoft.Azure.Services.AppAuthentication;

namespace ClientAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly ILogger<ClientController> _logger;
        private readonly HttpClient _apiclient;
        private readonly HttpClient _tokenclient;
        private IConfiguration _configuration;

        public ClientController(ILogger<ClientController> logger, IConfiguration configuration, HttpClient apiclient, HttpClient tokenclient)
        {
            _logger = logger;
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
            var requesturi = ("/data/v1/files/" + fileid.ToString());
            var request = new HttpRequestMessage(HttpMethod.Get, requesturi);
            request.Headers.Accept.Clear();
            request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                Console.WriteLine("Inside the call anyway");
                var resource = _configuration["ApplicationIdURL"];
                _logger.LogInformation("==>GetByFileID");
                var access_token = await GetTokenFromAppAuthentication(resource);
                request.Headers.Add("Authorization", "Bearer " + access_token);
                var response = await _apiclient.SendAsync(request, System.Threading.CancellationToken.None);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    File file = JsonConvert.DeserializeObject<File>(responseContent);
                    return Ok(file);
                }
                return NotFound();

            }
            catch (Exception e)
            {
                _logger.LogInformation($"==> Exception in GetByFileiD = {e.ToString()}");
                throw;
            }
        }


        private async Task<string> GetTokenFromAppAuthentication(string resource)
        {
            _logger.LogInformation("==>Get Token from App Authentication()");
             try
            {
                //AzureServiceProvider caches the Token and retrieves again from AD neat expiry
                var azureServiceTokenProvider = new AzureServiceTokenProvider();
                _logger.LogInformation("==>No Problem neweing TokenProvider");
                string apiToken = await azureServiceTokenProvider.GetAccessTokenAsync(resource).ConfigureAwait(false);
                _logger.LogInformation("==>Token   "+apiToken.ToString() );
                return apiToken;
            }
            catch (Exception e)
            {
                _logger.LogInformation("==>Exception from AzureServiceTokenProvider");
                _logger.LogInformation(e.StackTrace);
                throw;
            }
        }


    }
}
