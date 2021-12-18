using System;
using System.Net;
using System.Threading.Tasks;
using InterOP.Core.Entities;
using InterOP.Core.Responses;

using InterOP.Core.Interfaces;
using InterOP.Core.InterfaceApplications;

namespace InterOP.Core.ServiceApplications
{
    public class SecurityService : ISecurityService
    {
        private readonly IUnitOfWork prvUnitOfWork;
        public SecurityService(IUnitOfWork pvUnitOfWork)
        {
            prvUnitOfWork = pvUnitOfWork;
        }

        public async Task<ApiResponse> AuthenticateUserAsync(UserLogin pvLogin)
        {
            ////Generate data of BD
            var vObUser = await prvUnitOfWork.SecurityRepository.GetUserByLogin(pvLogin);
            if (vObUser == null)
            {
                return new ApiResponse
                {
                    IsSuccess = false,
                    Message = string.Empty,
                    Result = new { StatusCode = HttpStatusCode.Unauthorized, message = "Usuario y/o contraseña inválida", currentDate = DateTime.Now },
                };
            }

            return new ApiResponse
            {
                IsSuccess = true,
                Message = "Ok",
                Result = vObUser
            };

        }
    }
}
