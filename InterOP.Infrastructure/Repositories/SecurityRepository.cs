using InterOP.Core.Entities;
using InterOP.Core.Interfaces;
using InterOP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InterOP.Infrastructure.Repositories
{
    public class SecurityRepository : ISecurityRepository
    {
        public InterOPDevContext prvObjContext;
        public SecurityRepository(InterOPDevContext pvContext)
        {
            prvObjContext = pvContext;
        }

        public Task<IEnumerable<EntProveedores>> GetsUserById(int pvIntId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<EntProveedores> GetUserByLogin(UserLogin pvLogin)
        {
            ////Generate data of BD
            return await prvObjContext.EntProveedores
                                        .Where(m => m.Nit == pvLogin.u)
                                        .FirstOrDefaultAsync();
        }
    }
}
