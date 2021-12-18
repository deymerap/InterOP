using AutoMapper;
using InterOP.Core.DTOs;
using InterOP.Core.Entities;
using InterOP.Core.InterfaceApplications;
using InterOP.Core.Responses;
using InterOP.Core.Structs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using System.Linq;
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
    public class ProviderController : ControllerBase
    {
        //private readonly IConfiguration prvObConfig;
        private readonly IProviderService IPrvProviderService;
        private readonly IMapper IPrvMapper;
        
        public ProviderController(IMapper pvMapper, IProviderService pvProviderService)
        {
            //prvObConfig = pvConfig;
            IPrvMapper = pvMapper;
            IPrvProviderService = pvProviderService;
        }
        // GET: api/<ProviderController>
        [HttpGet("GetAll")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<ProveedoresNoPwdDto>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiResponse))]
        public async Task<IActionResult> GetAll()
        {
            var vResponse = await IPrvProviderService.GetProviders();
            //var vProvidersDto = vResponse.Result.Result;
            var vProviders = ((IEnumerable<EntProveedores>)vResponse.Result).Select(x => new EntProveedores
            {
                Id = x.Id,
                Nit = x.Nit,
                RazonSocial = x.RazonSocial,
                Email = x.Email,
                IndCliente = x.IndCliente,
                IndEstado = x.IndEstado,
                UserUrlApiProv = x.UserUrlApiProv,
                PwdUrlApiProv = x.PwdUrlApiProv,
                Url1ApiProv = x.Url1ApiProv,
                Url2ApiProv = x.Url2ApiProv,
                Url3ApiProv = x.Url3ApiProv,
                Url4ApiProv = x.Url4ApiProv,
                UrlSftpProv = x.UrlSftpProv,
                UsuSftpProv = x.UsuSftpProv,
                PwdSftpProv = x.PwdSftpProv,
                TsCreacion = x.TsCreacion,
                TsModificacion = x.TsModificacion,
                //TsVigenciaPwd = x.TsVigenciaPwd
            });
            vResponse.Result = vProviders;
            return Ok(vResponse.Result);
        }

        [HttpGet("GetByID/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<ProveedoresNoPwdDto>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiResponse))]
        public async Task<IActionResult> GetByID(int id)
        {
            var vResponse = await IPrvProviderService.GetProvider(id);
            var vProviderDto = IPrvMapper.Map<ProveedoresNoPwdDto>(vResponse.Result);

            var vResponseProvider = new ProveedoresNoPwdDto
            {
                Id = vProviderDto.Id,
                Nit = vProviderDto.Nit,
                RazonSocial = vProviderDto.RazonSocial,
                Email = vProviderDto.Email,
                IndCliente = vProviderDto.IndCliente,
                IndEstado = vProviderDto.IndEstado,
                UserUrlApiProv = vProviderDto.UserUrlApiProv,
                PwdUrlApiProv = vProviderDto.PwdUrlApiProv,
                Url1ApiProv = vProviderDto.Url1ApiProv,
                Url2ApiProv = vProviderDto.Url2ApiProv,
                Url3ApiProv = vProviderDto.Url3ApiProv,
                Url4ApiProv = vProviderDto.Url4ApiProv,
                UrlSftpProv = vProviderDto.UrlSftpProv,
                UsuSftpProv = vProviderDto.UsuSftpProv,
                PwdSftpProv = vProviderDto.PwdSftpProv,
                //TsCreacion = vProviderDto.TsCreacion,
                //TsModificacion = vProviderDto.TsModificacion
            };
            vResponse.Result = vResponseProvider;
            return Ok(vResponse.Result);
        }

        [HttpGet("GetByEmail/{email}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var vResponse = await IPrvProviderService.GetProviderByEmail(email);
            var vProviderDto = IPrvMapper.Map<ProveedoresDto>(vResponse.Result);

            var vResponseProvider = new ProveedoresDto
            {
                Id = vProviderDto.Id,
                Nit = vProviderDto.Nit,
                RazonSocial = vProviderDto.RazonSocial,
                Email = vProviderDto.Email,
                //Pwd = vProviderDto.Pwd,
                IndCliente = vProviderDto.IndCliente,
                IndEstado = vProviderDto.IndEstado,
                UserUrlApiProv = vProviderDto.UserUrlApiProv,
                PwdUrlApiProv = vProviderDto.PwdUrlApiProv,
                Url1ApiProv = vProviderDto.Url1ApiProv,
                Url2ApiProv = vProviderDto.Url2ApiProv,
                Url3ApiProv = vProviderDto.Url3ApiProv,
                Url4ApiProv = vProviderDto.Url4ApiProv,
                UrlSftpProv = vProviderDto.UrlSftpProv,
                UsuSftpProv = vProviderDto.UsuSftpProv,
                PwdSftpProv = vProviderDto.PwdSftpProv,
                TsCreacion = vProviderDto.TsCreacion,
                TsModificacion = vProviderDto.TsModificacion,
                TsVigenciaPwd = vProviderDto.TsVigenciaPwd
            };
            vResponse.Result = vResponseProvider;
            return Ok(vResponse.Result);
        }

        [HttpPost("Create")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<ProveedoresNoPwdDto>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiResponse))]
        public async Task<IActionResult> Create(ProveedoresDto proveedoresDto)
        {
            var vEntProvider = IPrvMapper.Map<EntProveedores>(proveedoresDto);
            var vResponse = await IPrvProviderService.InsertProvider(vEntProvider);

            //var vProviderDto = prvMapper.Map<ProveedoresDto>(vResponse.Result);
            vResponse.Result = "";
            return Ok(vResponse);
        }

        [HttpPut("Update/{id}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiResponse))]
        public async Task<IActionResult> Update(int id, ProveedoresNoPwdDto proveedoresNoPwdDto)
        {
            var vEntProvider = IPrvMapper.Map<EntProveedores>(proveedoresNoPwdDto);
            vEntProvider.Id = id;

            var vResponse = await IPrvProviderService.UpdateProvider(vEntProvider);
            return Ok(vResponse);
        }

        [HttpDelete("Delete/{id}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiResponse))]
        public async Task<IActionResult> Delete(int id)
        {
            var vResponse = await IPrvProviderService.DeleteProvider(id);
            return Ok(vResponse);
        }

        [HttpPut("cambioContrasena")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiResponse))]
        public async Task<IActionResult> ChangePassword([FromHeader] string NITProveedor, string ContrasenaActual, string ContrasenaNueva)
        {
            //Compare Password
            var vUserChangePwd = new StrsUserChangePwd
            {
                NITProveedor = NITProveedor,
                ContrasenaActual = ContrasenaActual,
                ContrasenaNueva = ContrasenaNueva
            };
            var vResponseChangePwd = await IPrvProviderService.ChangePasswordProvider(vUserChangePwd);
            return Ok(vResponseChangePwd);
        }
    }
}
