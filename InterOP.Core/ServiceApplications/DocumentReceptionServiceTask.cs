using InterOP.Core.Entities;
using InterOP.Core.Enumerations;
using InterOP.Core.Exceptions;
using InterOP.Core.InterfaceApplications;
using InterOP.Core.Interfaces;
using InterOP.Core.JsonObj;
using InterOP.Core.Responses;
using Ionic.Zip;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;


namespace InterOP.Core.ServiceApplications
{
    public class DocumentReceptionServiceTask : IDocumentReceptionServiceTask
    {
        private readonly IConfiguration IPrvObConfig;
        private readonly IUnitOfWork prvUnitOfWork;
        private readonly IS3Services IPrvObS3Services;
        private readonly IRedisCacheClient IPrvRedisCacheClient;



        //private readonly IPasswordService prvPasswordService;
        private IRedisDatabase prvRedisBD;
        private DocumentToProcess prvDoctoToProcess;
        private bool PrvBlnErrProcessingDoc = false;
        private const string vGetSearchAllProviders = "Search/Provider/all";
        private const string vGetSearchProvider = "Search/Provider";


        public DocumentReceptionServiceTask(IConfiguration pvIPrvObConfig, IUnitOfWork pvUnitOfWork, IS3Services pvS3Services, IRedisCacheClient pvRedisCacheClient)
        {
            IPrvObConfig = pvIPrvObConfig;
            prvUnitOfWork = pvUnitOfWork;
            IPrvObS3Services = pvS3Services;
            IPrvRedisCacheClient = pvRedisCacheClient;
            //prvPasswordService = pvPasswordService;      
        }


        private static readonly string[] PayloadSources = new[] {
        "http://localhost:2700/api/cars/cheap",
        "http://localhost:2700/api/cars/expensive"
    };

        public async Task<ApiResponse> ProcessDocument(RegisterDoc pvRegisterDoc, DocumentToProcess pvDoctoToProcess)
        {
            EntProveedores vObProvider = null;
            //////// Load data all providers
            await LoadRedisProviders(); 
            vObProvider = await LoadRedisGenericByKey(vGetSearchProvider, "234357689");





            //IEnumerable<Task<IEnumerable<EntProveedores>>> allTasks = PayloadSources.Select(uri => GetCarsAsync(uri));
            //IEnumerable<EntProveedores>[] allResults = await Task.WhenAll(allTasks);



            prvDoctoToProcess = pvDoctoToProcess;
            pvDoctoToProcess = null;
            IList<EntProveedores> vProvider = null;
            ResponseDocumentProcess vResponseDocument = null;
            List<TrackingId> vTrackingId = new List<TrackingId>();
            TrackingId vResValidated;
            EntFacturas vObEntFacturas = new EntFacturas();

            try
            {
                ////// Load data all providers
                //vProvider = await LoadRedisProviders();

                if (prvDoctoToProcess.zipReqPassword)
                {
                    vProvider = vProvider.Where(x => x.Nit == prvDoctoToProcess.nombre.Split("_")[0]).ToList();
                    if (vProvider == null)
                    {
                        return new ApiResponse()
                        {
                            IsSuccess = false,
                            StatusCode = (int)HttpStatusCode.NotFound,
                            Message = $"Proveedor tecnológico: '{prvDoctoToProcess.nombre.Split("_")[1]}' no encontrado."
                        };
                    }
                }
                if (pvRegisterDoc.DataZip == null || pvRegisterDoc.DataZip.Length == 0)
                {
                    return new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = (int)HttpStatusCode.PartialContent,
                        Message = "No se ha encontrado ningun archivo en la petición.",
                        Result = string.Empty
                    };
                }
                if (!CheckIfZipFile(pvRegisterDoc.DataZip))
                {
                    return new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Message = "La extensión del archivo enviado no corresponde a la requerida (.zip).",
                        Result = string.Empty
                    };
                }
                if (string.IsNullOrEmpty(pvRegisterDoc.InfoDocuments))
                {
                    return new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = (int)HttpStatusCode.PartialContent,
                        Message = "No se ha especificado la información del contenido del ZIP.",
                        Result = string.Empty
                    };
                }

                var vPathZipBase = IPrvObConfig["DocumentoProcces:FolderExtractDocument"]; //Path.Combine(Directory.GetCurrentDirectory(), "Resources\\Document");
                if (!Directory.Exists(vPathZipBase))
                    Directory.CreateDirectory(vPathZipBase);

                var vIsWriteFileSuccess = await WriteFileAsync(vPathZipBase, pvRegisterDoc.DataZip);
                if (!vIsWriteFileSuccess)
                {
                    return new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = (int)HttpStatusCode.InternalServerError,
                        Message = "Se presento un error al momento de almacenar archico ZIP.",
                        Result = string.Empty
                    };
                }

                var vIsExtractSuccess = ExtractZip(vPathZipBase, prvDoctoToProcess.nombre, vProvider[0].PwdArchivoZip);
                if (!vIsExtractSuccess.Item1)
                {
                    return new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = (int)HttpStatusCode.InternalServerError,
                        Message = "Se presento un error al momento de extraer los datos del archivo ZIP.",
                        Result = string.Empty
                    };
                }

                DirectoryInfo vDirInfo = new DirectoryInfo(vIsExtractSuccess.Item2);
                IEnumerable<FileInfo> vFileCount = vDirInfo.GetFiles("*.xml");
                if (vFileCount.Count() >= int.Parse(IPrvObConfig["DocumentoProcces:MaxNumFile"]))
                {
                    return new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = (int)HttpStatusCode.InternalServerError,
                        Message = $"El archivo comprimido contiene más de {IPrvObConfig["DocumentoProcces:MaxNumFile"]} documentos electrónicos.",
                        Result = string.Empty
                    };
                }
                if (vFileCount.Count() == 0)
                {
                    return new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = (int)HttpStatusCode.InternalServerError,
                        Message = $"El archivo comprimido {prvDoctoToProcess.nombre} no contiene documentos electrónicos.",
                        Result = string.Empty
                    };
                }

                vResponseDocument = new ResponseDocumentProcess
                {
                    timeStamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
                };
                //Proccess files 
                foreach (var vCurrentDocument in prvDoctoToProcess.documentos)
                {

                    //validate if file exist in zip
                    if (!File.Exists($"{vIsExtractSuccess.Item2}\\{vCurrentDocument.nombre}"))
                    {
                        PrvBlnErrProcessingDoc = true;
                        vTrackingId.Add(new TrackingId
                        {
                            nombreDocumento = vCurrentDocument.nombre,
                            uuid = "",
                            codigoError = (int)HttpStatusCode.NotFound,
                            mensaje = $"Documento no se encontró en el ZIP {prvDoctoToProcess.nombre}"
                        });
                        continue;
                    }
                    string vPartNameFile = vCurrentDocument.nombre.Replace(vCurrentDocument.nombre.Substring(0, 2), "").Replace(".xml", "");

                    IEnumerable<FileInfo> vFileList = vDirInfo.GetFiles("*.*");

                    ////Create the query
                    IEnumerable<FileInfo> vFileQuery = from vFiles in vFileList
                                                       where (vFiles.FullName.Contains(vPartNameFile))
                                                       //orderby file.LastWriteTime
                                                       select vFiles;

                    DateTime vDStartProcessDocument = DateTime.Now; //measure operation time

                    (vResValidated, vObEntFacturas) = await ValidateDocumentAsync($"{vIsExtractSuccess.Item2}\\{vCurrentDocument.nombre}", vCurrentDocument);
                    if (vResValidated != null)
                        vTrackingId.Add(vResValidated);

                    foreach (FileInfo vInfoFile in vFileQuery)
                    {
                        if (PrvBlnErrProcessingDoc)
                        {
                            PrvBlnErrProcessingDoc = false;
                            break;
                        }
                        string vBaseToCopyFile = $"{IPrvObConfig["DocumentoProcces:FolderAttachments"]}{vObEntFacturas.GuidDocto.ToString().ToUpper()}";
                        if (!Directory.Exists(vBaseToCopyFile))
                            Directory.CreateDirectory(vBaseToCopyFile);

                        string vFromFile = vInfoFile.FullName;
                        string vToFile = $"{vBaseToCopyFile}\\{vInfoFile.Name}";
                        File.Move(vFromFile, vToFile);
                        //Process file attachments
                        switch (vInfoFile.Extension.ToLower())
                        {
                            case TypeExtFile.XML:

                                break;
                            case TypeExtFile.PDF:

                                break;
                            case TypeExtFile.URL:

                                break;
                            case TypeExtFile.ZIP:

                                break;
                            default:
                                break;
                        }

                        bool vBlnS3UploadFile = IPrvObS3Services.UploadFileAsync(vToFile, vObEntFacturas.GuidDocto.ToString().ToUpper(), vInfoFile.Name).Result;
                        if (vBlnS3UploadFile == false)
                        {
                            //ADICIONAR A EXCEPCIONES DE PROCESAMIENTO DE ARCHIVO.
                            continue;
                        }
                    }
                    DateTime vDStopProcessDocument = DateTime.Now;
                    long vLngElapsedTime = vDStopProcessDocument.Ticks - vDStartProcessDocument.Ticks;
                    TimeSpan vElapsedTime = new TimeSpan(vLngElapsedTime);
                    if (vObEntFacturas != null)
                        vObEntFacturas.TicksRecepcion = (long)vElapsedTime.TotalMilliseconds;

                    //SOLICITAR INFORMACION A S3 DE LOS ARCHIVOS SUBIDOS 
                    string vJSonInfoFolderS3 = await IPrvObS3Services.InfoFilesDirectoryAsync(vObEntFacturas.GuidDocto.ToString().ToUpper());
                    vObEntFacturas.InfoFilesDirS3 = vJSonInfoFolderS3;
                    ////Save document
                    var vIsSuccessSaveDOcument = await prvUnitOfWork.InvoiceAltRepository.InsertDocument(vObEntFacturas, IPrvObConfig["ConfigDatabase:DataSecurity:PwdSymmetricKey"]);


                    if (vIsSuccessSaveDOcument)
                    {
                        vTrackingId.Add(new TrackingId
                        {
                            nombreDocumento = vObEntFacturas.NombreDocto,
                            uuid = vObEntFacturas.GuidDocto.ToString(),
                            codigoError = (int)HttpStatusCode.Created,
                            mensaje = "Documento encolado para procesamiento"
                        });
                    }
                    else
                    {
                        vTrackingId.Add(new TrackingId
                        {
                            nombreDocumento = vObEntFacturas.NombreDocto,
                            uuid = string.Empty,
                            codigoError = (int)HttpStatusCode.InternalServerError,
                            mensaje = "Error interno en el almacenamiento del documento electrónico"
                        });

                    }
                }


                //Response
                vResponseDocument.trackingIds = vTrackingId;
                if (!PrvBlnErrProcessingDoc)
                {
                    vResponseDocument.mensajeGlobal = "Documento electrónico radicado satisfactoriamente";
                }
                else
                {
                    vResponseDocument.mensajeGlobal = $"Documento {prvDoctoToProcess.nombre} procesado parcialmente";
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Message = "Upps!! Se ha presentado un error en el servidor, por favor contactar al administrador.",
                    Result = string.Empty
                };
                throw;
            }
            return new ApiResponse
            {
                IsSuccess = false,
                StatusCode = (int)HttpStatusCode.Created,
                Message = "",
                Result = vResponseDocument
            };
        }

        private async Task<IEnumerable<EntProveedores>> GetCarsAsync(string uri)
        {

            return await prvUnitOfWork.ProviderRepository.GetProviders(); ;
        }

        private async Task<(TrackingId, EntFacturas)> ValidateDocumentAsync(string vPahtcFile, Documento pvCurrentDocument)
        {
            XmlNamespaceManager vObjXmlManagerBase;
            XmlDocument vXmlDocAttachedDocument;
            XmlDocument vXmlSignedDocument;
            //XmlDocument vXmlDocAppResponse;
            IList<EntProveedores> vProviders = null;
            EntProveedores vProviderAdq;
            EntFacturas vObEntFacturas;

            PrvBlnErrProcessingDoc = false;
            //var vFile = File.ReadAllText(vInfoFile.FullName);
            vXmlDocAttachedDocument = new XmlDocument();
            vXmlDocAttachedDocument.Load(vPahtcFile);

            vObjXmlManagerBase = new XmlNamespaceManager(vXmlDocAttachedDocument.NameTable);
            vObjXmlManagerBase.AddNamespace("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
            vObjXmlManagerBase.AddNamespace("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
            vObjXmlManagerBase.AddNamespace("ext", "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2");

            //Get Document INV, CN o DN
            vXmlSignedDocument = new XmlDocument();
            string vXmlDocElec = vXmlDocAttachedDocument.DocumentElement.SelectSingleNode("cac:Attachment/cac:ExternalReference/cbc:Description", vObjXmlManagerBase).InnerText;
            vXmlSignedDocument.LoadXml(vXmlDocElec);

            try
            {
                vObjXmlManagerBase = null;
                vObjXmlManagerBase = new XmlNamespaceManager(vXmlSignedDocument.NameTable);
                vObjXmlManagerBase.AddNamespace("", "urn:oasis:names:specification:ubl:schema:xsd:Invoice-2");
                vObjXmlManagerBase.AddNamespace("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
                vObjXmlManagerBase.AddNamespace("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
                vObjXmlManagerBase.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
                vObjXmlManagerBase.AddNamespace("ext", "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2");
                vObjXmlManagerBase.AddNamespace("sts", "dian:gov:co:facturaelectronica:Structures-2-1");
                vObjXmlManagerBase.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");

                string vInvoiceTypeCode = string.Empty;
                switch (vXmlSignedDocument.DocumentElement.Name)
                {
                    case "Invoice":
                        vInvoiceTypeCode = vXmlSignedDocument.DocumentElement.SelectSingleNode("cbc:InvoiceTypeCode", vObjXmlManagerBase).InnerText;
                        break;
                    case "CreditNote":

                        break;
                    case "DebitNote":

                        break;
                    default:
                        break;
                }

                vObEntFacturas = new EntFacturas
                {
                    GuidDocto = Guid.NewGuid(),
                    NombreDocto = pvCurrentDocument.nombre,
                    TipoFactura = vInvoiceTypeCode,
                    Folio = vXmlSignedDocument.DocumentElement.SelectSingleNode("cbc:ID", vObjXmlManagerBase).InnerText,
                    UuidCufe = vXmlSignedDocument.DocumentElement.SelectSingleNode("cbc:UUID", vObjXmlManagerBase).InnerText,
                    Documento = vXmlDocAttachedDocument.OuterXml,
                    NotasEntrega = pvCurrentDocument.notaDeEntrega,
                    NitOfe = vXmlSignedDocument.DocumentElement.SelectSingleNode("cac:AccountingSupplierParty/cac:Party/cac:PartyTaxScheme/cbc:CompanyID", vObjXmlManagerBase).InnerText,
                    NitAdq = vXmlSignedDocument.DocumentElement.SelectSingleNode("cac:AccountingCustomerParty/cac:Party/cac:PartyTaxScheme/cbc:CompanyID", vObjXmlManagerBase).InnerText,
                    NitProveedor = vXmlSignedDocument.DocumentElement.SelectSingleNode("ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/sts:DianExtensions/sts:SoftwareProvider/sts:ProviderID", vObjXmlManagerBase).InnerText,
                    //TicksRecepcion = 0,
                    IndRepreGrafica = (short)((bool)pvCurrentDocument.representacionGraficas ? ResponseYesOrNo.YES : ResponseYesOrNo.YES),
                    IndAbjuntos = (short)((bool)pvCurrentDocument.adjuntos ? ResponseYesOrNo.YES : ResponseYesOrNo.YES),
                    ExtensAbjuntos = "",
                    TsCreacion = DateTime.Now,
                    TsModificacion = null
                };

                ////validar que el emisor de la factura sea cliente  de SIESA
                //// Load data all providers
                //vProviders = await LoadRedisProviders();
                vProviderAdq = (vProviders.Where(x => x.Nit == vObEntFacturas.NitAdq).ToList())[0];
                if (vProviderAdq == null)
                {
                    PrvBlnErrProcessingDoc = true;
                    return (
                        new TrackingId
                        {
                            nombreDocumento = vObEntFacturas.NombreDocto,
                            uuid = "",
                            codigoError = (int)HttpStatusCode.NotAcceptable,
                            mensaje = $"El cliente: {vObEntFacturas.NitOfe} destinatario de la factura electrónica no tiene convenio con el receptor."
                        },
                        null);
                }

                return (
                //new TrackingId
                //{
                //    nombreDocumento = vObEntFacturas.NombreDocto,
                //    uuid = vObEntFacturas.GuidDocto.ToString(),
                //    codigoError = (int)HttpStatusCode.Created,
                //    mensaje = $"Documento encolado para procesamiento"
                //}
                null,
                vObEntFacturas);

            }
            catch (Exception /*ex*/)
            {
                throw new BusinessException("Se presento un error al momento de obtener informacion del XML");
            }



        }

        private bool CheckIfZipFile(IFormFile dataZip)
        {
            var extension = "." + dataZip.FileName.Split('.')[1];
            return (extension == ".zip");
        }
        private async Task<bool> WriteFileAsync(string pvPathZipBase, IFormFile pvZipFile)
        {
            bool vIsSaveSuccess;
            try
            {
                var vPathFiles = Path.Combine(pvPathZipBase, pvZipFile.FileName);
                using (var vDataStream = new FileStream(vPathFiles, FileMode.Create))
                {
                    await pvZipFile.CopyToAsync(vDataStream);
                }

                vIsSaveSuccess = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return vIsSaveSuccess;
        }
        private (bool, string) ExtractZip(string pvPathZipBase, string pvZipFileName, string pvPdwZip)
        {
            bool vIsSaveSuccess;
            string vPathFolderExtract;
            string vPathZipFile;
            try
            {
                vPathFolderExtract = Path.Combine(pvPathZipBase, pvZipFileName.Split('.')[0]);
                vPathZipFile = Path.Combine(pvPathZipBase, pvZipFileName);
                if (!Directory.Exists(vPathFolderExtract))
                    Directory.CreateDirectory(vPathFolderExtract);

                //Process zip file
                using (ZipFile vZip = ZipFile.Read(vPathZipFile))
                {
                    if (!string.IsNullOrEmpty(pvPdwZip))
                        vZip.Password = pvPdwZip;

                    vZip.ExtractAll(vPathFolderExtract, ExtractExistingFileAction.OverwriteSilently);

                    //foreach (ZipEntry item in vZip)
                    //{
                    //    if(string.IsNullOrEmpty(pvPdwZip))
                    //    {
                    //        item.Extract(vPathFolderExtract, ExtractExistingFileAction.OverwriteSilently);
                    //    }
                    //    else
                    //    {   
                    //        item.ExtractWithPassword(vPathFolderExtract, ExtractExistingFileAction.OverwriteSilently, pvPdwZip);
                    //    }
                    //}
                }
                vIsSaveSuccess = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return (vIsSaveSuccess, vPathFolderExtract);
        }

        private async Task LoadRedisProviders()
        {
            IEnumerable<EntProveedores> vProvider;
            List<Tuple<string, EntProveedores>> vObTplProviders;
            prvRedisBD = IPrvRedisCacheClient.GetDbFromConfiguration();
            vProvider = await prvRedisBD.GetAsync<List<EntProveedores>>(vGetSearchAllProviders);
            if (vProvider == null)
            {
                vProvider = await prvUnitOfWork.ProviderRepository.GetProviders();
                vObTplProviders = vProvider.Select(x => new Tuple<string, EntProveedores>($"{vGetSearchProvider}/{x.Nit}", x)).ToList();

                await prvRedisBD.AddAllAsync(vObTplProviders);

                await prvRedisBD.AddAsync(vGetSearchAllProviders, vProvider);
            }
        }

        private async Task<List<T>> LoadRedisGeneric<T>(string pvGetSearchAllProvider)
        {
            List<T> vLstProvider;
            prvRedisBD = IPrvRedisCacheClient.GetDbFromConfiguration();
            string vRedisData = prvRedisBD.Database.StringGet(pvGetSearchAllProvider);
            if (vRedisData == null)
            {
                vLstProvider = (List<T>)await prvUnitOfWork.ProviderRepository.GetProviders();
                await prvRedisBD.Database.StringSetAsync(pvGetSearchAllProvider, JsonConvert.SerializeObject(vLstProvider));
                return JsonConvert.DeserializeObject<List<T>>(vRedisData);
            }
            else
            {
                return JsonConvert.DeserializeObject<List<T>>(vRedisData);
            }
        }

        private async Task<EntProveedores> LoadRedisGenericByKey(string pvGetSearchProviderBy, string pvIntNit)
        {
            prvRedisBD = IPrvRedisCacheClient.GetDbFromConfiguration();
            string vRedisData = prvRedisBD.Database.StringGet($"{pvGetSearchProviderBy}/{pvIntNit}");
            if (vRedisData == null)
            {
                EntProveedores vObProvider = await prvUnitOfWork.ProviderRepository.GetProviderBy(pvNit: pvIntNit);
                await prvRedisBD.Database.StringSetAsync($"{pvGetSearchProviderBy}/{pvIntNit}", JsonConvert.SerializeObject(vObProvider));
                return vObProvider;
            }
            else
            {
                return JsonConvert.DeserializeObject<EntProveedores>(vRedisData);
            }
        }
    }
}
