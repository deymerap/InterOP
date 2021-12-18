using InterOP.Core.Entities;
using InterOP.Core.Responses;
using InterOP.Core.Structs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InterOP.Core.InterfaceApplications
{
    public interface IProviderService
    {
        Task<ApiResponse> GetProviders();
        Task<ApiResponse> GetProvider(int pvId);
        Task<ApiResponse> GetProviderByEmail(string pvEmail);
        Task<ApiResponse> GetProviderBy(int pvId = 0, string pvNit = null, string pvEmail = null);
        Task<ApiResponse> InsertProvider(EntProveedores entProveedores);
        Task<ApiResponse> UpdateProvider(EntProveedores entProveedores);
        Task<ApiResponse> DeleteProvider(int pvId);
        Task<ApiResponse> ChangePasswordProvider(StrsUserChangePwd vUserChangePwd);
    }
}
