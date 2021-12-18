using InterOP.Core.Responses;
using InterOP.Core.Entities;
using System.Threading.Tasks;

namespace InterOP.Core.InterfaceApplications
{
    public interface ISecurityService
    {
        public Task<ApiResponse> AuthenticateUserAsync(UserLogin pvLogin);
    }
}
