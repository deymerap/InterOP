using InterOP.Core.Entities;
using InterOP.Core.Interfaces;
using InterOP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InterOP.Core.Services
{
    public class ProviderRepository : IProvidersRepository
    {
        public InterOPDevContext prvObjContext;
        public ProviderRepository(InterOPDevContext pvContext)
        {
            prvObjContext = pvContext;
        }

        public async Task<IEnumerable<EntProveedores>> GetProviders()
        {
            return await prvObjContext.EntProveedores.AsNoTracking().ToListAsync();
        }
        public async Task<EntProveedores> GetProvider(int pvId)
        {
            ////Generate data of BD
            return await prvObjContext.EntProveedores
                                        .Where(x => x.Id == pvId)
                                        .FirstOrDefaultAsync();
        }

        public async Task<EntProveedores> GetProviderNoTracking(int pvId)
        {
            ////Generate data of BD
            return await prvObjContext.EntProveedores.AsNoTracking()
                                        .Where(x => x.Id == pvId)
                                        .FirstOrDefaultAsync();
        }

        public async Task<EntProveedores> GetProviderBy(int pvId = 0, string pvNit = null, string pvEmail = null)
        {
            ////Generate data of BD
            return await prvObjContext.EntProveedores.AsNoTracking()
                                        .Where(x => x.Id == pvId || x.Nit == pvNit || x.Email == pvEmail)
                                        .FirstOrDefaultAsync();
        }

        public async Task<EntProveedores> GetProviderByEmail(string pvEmail)
        {
            return await prvObjContext.EntProveedores
                                        .Where(x => x.Email == pvEmail)
                                        .FirstOrDefaultAsync();
        }

        public async Task<EntProveedores> GetProviderByEmailNoTracking(string pvEmail)
        {
            return await prvObjContext.EntProveedores.AsNoTracking()
                                        .Where(x => x.Email == pvEmail)
                                        .FirstOrDefaultAsync();
        }

        public async Task InsertProvider(EntProveedores entProveedores)
        {
            prvObjContext.EntProveedores.Add(entProveedores);
            await prvObjContext.SaveChangesAsync();
        }

        public async Task<bool> UpdateProvider(EntProveedores pvEntProveedores)
        {
            prvObjContext.EntProveedores.Update(pvEntProveedores);
            int vRowAffected = await prvObjContext.SaveChangesAsync();
            return vRowAffected > 0;
        }

        public async Task<bool> DeleteProvider(int pvId)
        {
            // Tracking Row
            var vCurrentProvider = await GetProvider(pvId);
            prvObjContext.EntProveedores.Remove(vCurrentProvider);
            int vRowAffected = await prvObjContext.SaveChangesAsync();
            return vRowAffected > 0;
        }

       
    }
}
