using InterOP.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InterOP.Core.Interfaces
{
    public interface ISecurityRepository
    {
        public Task<IEnumerable<EntProveedores>>GetsUserById(int pvIntId);
        public Task<EntProveedores> GetUserByLogin(UserLogin pvLogin);
    }
}
