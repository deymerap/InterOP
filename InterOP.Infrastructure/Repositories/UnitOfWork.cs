using InterOP.Core.Entities;
using InterOP.Core.Interfaces;
using InterOP.Core.Services;
using InterOP.Infrastructure.Data;
using System.Threading.Tasks;

namespace InterOP.Infrastructure.Repositories
{
    class UnitOfWork : IUnitOfWork
    {
        public InterOPDevContext prvObjContext;
        private ISecurityRepository securityRepository;
        private readonly IProvidersRepository providerRepository;
        private readonly IInvoiceRepository invoiceAltRepository;
        private readonly INoteRepository noteRepository;

        private readonly IRepository<EntFacturas> invoiceRepository;
        private readonly IRepository<EntNotas> notesRepository;


        public ISecurityRepository SecurityRepository
        {
            get
            {
                if (securityRepository == null)
                {
                    securityRepository = new SecurityRepository(prvObjContext);
                }
                return securityRepository;
            }
        }

        public IProvidersRepository ProviderRepository => providerRepository ?? new ProviderRepository(prvObjContext);
        public IInvoiceRepository InvoiceAltRepository => invoiceAltRepository ?? new InvoiceRepository(prvObjContext);
        public INoteRepository NoteAltRepository => noteRepository ?? new NoteRepository(prvObjContext);


        public IRepository<EntFacturas> InvoiceRepository => invoiceRepository ?? new BaseRepository<EntFacturas>(prvObjContext);
        public IRepository<EntNotas> NoteRepository => notesRepository ?? new BaseRepository<EntNotas>(prvObjContext);

        public UnitOfWork(InterOPDevContext pvContext)
        {
            prvObjContext = pvContext;
        }
        public void SaveChanges()
        {
            prvObjContext.SaveChanges();
        }
        public async Task SaveChangesAsync()
        {
            await prvObjContext.SaveChangesAsync();
        }
        public void Dispose()
        {
            if (prvObjContext != null)
            {
                prvObjContext.Dispose();
            }
        }
    }
}
