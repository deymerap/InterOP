// Unit of Work
//Docs: https://code-maze.com/net-core-web-development-part4/
using InterOP.Core.Entities;
using System;
using System.Threading.Tasks;

namespace InterOP.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        //Add Respository
        ISecurityRepository SecurityRepository { get; }
        IProvidersRepository ProviderRepository { get; }
        IInvoiceRepository InvoiceAltRepository { get; }
        INoteRepository NoteAltRepository { get; }


        IRepository<EntFacturas> InvoiceRepository { get; }
        IRepository<EntNotas> NoteRepository { get; }

        void SaveChanges();
        Task SaveChangesAsync();
    }
}
