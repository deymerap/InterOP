using InterOP.Core.JsonObj;
using InterOP.Core.Responses;
using System.Threading.Tasks;

namespace InterOP.Core.InterfaceApplications
{
    public interface IDocumentReceptionService
    {
        Task<ApiResponse> ProcessDocument(RegisterDoc registerDoc, DocumentToProcess documentToProcess);
    }

    public interface IDocumentReceptionServiceTask
    {
        Task<ApiResponse> ProcessDocument(RegisterDoc registerDoc, DocumentToProcess documentToProcess);
    }
}
