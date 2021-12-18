using InterOP.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InterOP.Core.Interfaces
{
    public interface IProvidersRepository
    {
        Task<IEnumerable<EntProveedores>> GetProviders();
        Task<EntProveedores> GetProvider(int pvId);
        Task<EntProveedores> GetProviderNoTracking(int pvId);
        Task<EntProveedores> GetProviderByEmail(string pvEmail);
        Task<EntProveedores> GetProviderBy(int pvId = 0, string pvNit = null, string pvEmail = null);
        Task<EntProveedores> GetProviderByEmailNoTracking(string pvEmail);
        Task InsertProvider(EntProveedores entProveedores);
        Task<bool> UpdateProvider(EntProveedores entProveedores);
        Task<bool> DeleteProvider(int pvId);
    }
}
