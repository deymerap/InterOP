using InterOP.Core.Entities;
using InterOP.Core.InterfaceApplications;
using InterOP.Core.Interfaces;
using InterOP.Core.Responses;
using InterOP.Core.Structs;
using System;
using System.Net;
using System.Threading.Tasks;

namespace InterOP.Core.ServiceApplications
{
    public class ProviderService : IProviderService
    {
        private readonly IUnitOfWork prvUnitOfWork;
        public readonly IPasswordService prvPasswordService;

        public ProviderService(IUnitOfWork pvUnitOfWork, IPasswordService pvPasswordService)
        {
            prvUnitOfWork = pvUnitOfWork;
            prvPasswordService = pvPasswordService;
        }
        public async Task<ApiResponse> GetProviders()
        {
            var vResponse = await prvUnitOfWork.ProviderRepository.GetProviders();
            return new ApiResponse
            {
                IsSuccess = true,
                StatusCode = (int)HttpStatusCode.OK,
                Message = string.Empty,
                Result = vResponse
            };
        }
        public async Task<ApiResponse> GetProvider(int pvId)
        {
            var vProvider = await prvUnitOfWork.ProviderRepository.GetProviderBy(pvId: pvId);
            if (vProvider == null)
            {
                return new ApiResponse()
                {
                    IsSuccess = false,
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Message = "Proveedor tecnológico no encontrado."
                };
            }
            return new ApiResponse()
            {
                IsSuccess = true,
                StatusCode = (int)HttpStatusCode.OK,
                Message = string.Empty,
                Result = vProvider
            };
        }
        public async Task<ApiResponse> GetProviderByEmail(string pvStrEmail)
        {
            var vProvider = await prvUnitOfWork.ProviderRepository.GetProviderBy(pvEmail: pvStrEmail);
            if (vProvider == null)
            {
                return new ApiResponse()
                {
                    IsSuccess = false,
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Message = "Proveedor tecnológico no encontrado."
                };
            }
            return new ApiResponse()
            {
                IsSuccess = true,
                StatusCode = (int)HttpStatusCode.OK,
                Message = string.Empty,
                Result = vProvider
            };
        }
        public async Task<ApiResponse> GetProviderBy(int pvId = 0, string pvNit = null, string pvEmail = null)
        {
            var vProvider = await prvUnitOfWork.ProviderRepository.GetProviderBy(pvId, pvNit, pvEmail);
            if (vProvider == null)
            {
                return new ApiResponse()
                {
                    IsSuccess = false,
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Message = "Proveedor tecnológico no encontrado."
                };
            }
            return new ApiResponse()
            {
                IsSuccess = true,
                StatusCode = (int)HttpStatusCode.OK,
                Message = string.Empty,
                Result = vProvider
            };
        }
        public async Task<ApiResponse> InsertProvider(EntProveedores entProveedores)
        {
            //Email Proveedor ya registrado
            var vProvider = await prvUnitOfWork.ProviderRepository.GetProviderByEmailNoTracking(entProveedores.Email);
            if (vProvider != null)
            {
                return new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "La dirección de correo electrónico que ha ingresado ya está registrada.",
                    Result = entProveedores
                };
            }
            entProveedores.Pwd = prvPasswordService.PasswordHash(entProveedores.Pwd);
            entProveedores.TsCreacion = DateTime.Now;
            await prvUnitOfWork.ProviderRepository.InsertProvider(entProveedores);
            return new ApiResponse
            {
                IsSuccess = true,
                StatusCode = (int)HttpStatusCode.Created,
                Message = "Proveedor tecnológico creado satisfactoriamente.",
                Result = entProveedores
            };
        }
        public async Task<ApiResponse> UpdateProvider(EntProveedores entProveedores)
        {
            var vObjEntResponse = await prvUnitOfWork.ProviderRepository.GetProviderNoTracking(entProveedores.Id);
            string vStrPwd = vObjEntResponse.Pwd;
            DateTime vStrVigenciaPwd = (DateTime)vObjEntResponse.TsVigenciaPwd;
            entProveedores.Pwd = vStrPwd;
            entProveedores.TsVigenciaPwd = vStrVigenciaPwd;
            entProveedores.TsModificacion = DateTime.Now;

            var vProvider = await prvUnitOfWork.ProviderRepository.UpdateProvider(entProveedores);
            return new ApiResponse()
            {
                IsSuccess = vProvider,
                StatusCode = (int)HttpStatusCode.OK,
                Message = vProvider ? "Proveedor tecnológico actualizado correctamente." : "El Proveedor tecnológico no fue actualizado.",
                Result = vProvider
            };
        }
        public async Task<ApiResponse> DeleteProvider(int pvId)
        {
            var vProvider = await prvUnitOfWork.ProviderRepository.DeleteProvider(pvId);
            return new ApiResponse()
            {
                IsSuccess = vProvider,
                StatusCode = (int)HttpStatusCode.OK,
                Message = vProvider ? "Proveedor tecnológico eliminado correctamente." : "El Proveedor tecnológico no fue eliminado.",
                Result = vProvider
            };
        }
        public async Task<ApiResponse> ChangePasswordProvider(StrsUserChangePwd vUserChangePwd)
        {
            var vObjProvider = await prvUnitOfWork.ProviderRepository.GetProviderBy(pvNit: vUserChangePwd.NITProveedor);
            if (vObjProvider == null)
            {
                return new ApiResponse()
                {
                    IsSuccess = false,
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Message = "Proveedor tecnológico no encontrado."
                };
            }
            bool vIsEqualsPwd = prvPasswordService.PasswordCheck(vObjProvider.Pwd, vUserChangePwd.ContrasenaActual);
            if (!vIsEqualsPwd)
            {
                return new ApiResponse()
                {
                    IsSuccess = false,
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Message = "La contraseña actual no coincide con la almacenada."
                };
            }
            vObjProvider.TsVigenciaPwd = DateTime.Now.AddMonths(12);
            vObjProvider.Pwd = prvPasswordService.PasswordHash(vUserChangePwd.ContrasenaNueva);
            var vProvider = await prvUnitOfWork.ProviderRepository.UpdateProvider(vObjProvider);
            return new ApiResponse()
            {
                IsSuccess = vProvider,
                StatusCode = (int)HttpStatusCode.OK,
                Message = vProvider ? "La contraseña fue actualizada correctamente." : "La contraseña no fue actualizada correctamente.",
                Result = vProvider
            };
        }
    }
}
