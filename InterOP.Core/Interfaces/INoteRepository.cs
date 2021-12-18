using InterOP.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InterOP.Core.Interfaces
{
    public interface INoteRepository
    {
        Task<IEnumerable<EntNotas>> GetAllNotes();
        Task InsertProvider(EntNotas entNotas);
    }
}
