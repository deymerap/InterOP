using InterOP.Core.Entities;
using InterOP.Core.Interfaces;
using InterOP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterOP.Infrastructure.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly InterOPDevContext pvObContext;
        protected readonly DbSet<T> PubEntities;

        public BaseRepository(InterOPDevContext pvInterOPDevContext)
        {
            pvObContext = pvInterOPDevContext;
            PubEntities = pvObContext.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return PubEntities.AsEnumerable();
        }

        public async Task<T> GetById(int id)
        {
            return await PubEntities.FindAsync(id);
        }

        public async Task Insert(T entity)
        {
            await PubEntities.AddAsync(entity);
        }

        public void Update(T entity)
        {
            PubEntities.Update(entity);
        }

        public async Task Delete(int id)
        {
            T entity = await GetById(id);
            PubEntities.Remove(entity);
        }
    }
}
