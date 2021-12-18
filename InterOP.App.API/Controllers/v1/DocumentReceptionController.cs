using AutoMapper;
using InterOP.Core.InterfaceApplications;
using InterOP.Core.JsonObj;
using InterOP.Core.OptionApplications;
using InterOP.Core.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System;
using System.Net;
using System.Threading.Tasks;

namespace InterOP.App.API.Controllers.v1
{
    [Route("api/v{version:ApiVersion}/[controller]")]
    [ApiVersion("1")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Produces("application/json")]
    [ApiController]
    //[Authorize]
    public class DocumentReceptionController : ControllerBase
    {
        private readonly IDocumentReceptionService IPrvDocumentReceptionService;
        private readonly IDocumentReceptionServiceTask IPrvDocumentReceptionServiceTask;
        public DocumentReceptionController(/*IMapper pvMapper,*/ IDocumentReceptionService pvDocumentReceptionService, IDocumentReceptionServiceTask pvDocumentReceptionServiceTask)
        {
            IPrvDocumentReceptionService = pvDocumentReceptionService;
            IPrvDocumentReceptionServiceTask = pvDocumentReceptionServiceTask;
        }

        [HttpPost("Registrar")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiResponse))]
        public async Task<IActionResult> RegisterDocumentAsync([FromForm] RegisterDoc registerDoc)
        {
            var vJsonInfoDocuments = JsonConvert.DeserializeObject<DocumentToProcess>(registerDoc.InfoDocuments);
            //var response = await IPrvDocumentReceptionService.ProcessDocument(registerDoc, vJsonInfoDocuments);
            var response = await IPrvDocumentReceptionServiceTask.ProcessDocument(registerDoc, vJsonInfoDocuments);



            //ApiResponse response = new ApiResponse();
            //response.StatusCode = (int)HttpStatusCode.OK;
            //response.Result = documentToProcess;
            //var vEntProvider = prvMapper.Map<EntProveedores>(proveedoresDto);
            //var vResponse = await prvProviderService.InsertProvider(vEntProvider);

            ////var vProviderDto = prvMapper.Map<ProveedoresDto>(vResponse.Result);
            //vResponse.Result = "";
            return Ok(response);
        }
    }
}
