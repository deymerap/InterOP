using InterOP.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InterOP.Core.Interfaces
{
    public interface IInvoiceRepository
    {
        Task<IEnumerable<EntFacturas>> GetAllInvoices();
        Task<bool> InsertDocument(EntFacturas entFacturas, string vPwdEncrypXml);
        
    }
}
