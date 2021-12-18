using InterOP.Core.Entities;
using InterOP.Core.Interfaces;
using InterOP.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InterOP.Infrastructure.Repositories
{
    public class NoteRepository : BaseRepository<EntFacturas>, INoteRepository
    {
        public NoteRepository(InterOPDevContext pvContext) : base(pvContext) { }

        public Task<IEnumerable<EntNotas>> GetAllNotes()
        {
            throw new NotImplementedException();
        }

        public Task InsertProvider(EntNotas entNotas)
        {
            throw new NotImplementedException();
        }
    }
}
