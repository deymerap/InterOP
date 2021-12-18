using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3.Util;
using InterOP.Core.Interfaces;
using InterOP.Core.OptionApplications;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace InterOP.Infrastructure.Services
{
    public class S3Service : IS3Services
    {
        private readonly AwsS3Options IPrvAwsS3Options;
        private readonly IAmazonS3 IPrvObS3Client;
        private readonly ILogger<S3Service> IPrvLogger;

        public S3Service(AwsS3Options pvAwsS3Options, IAmazonS3 pvS3Cliient, ILogger<S3Service> pvLogger)
        {
            IPrvAwsS3Options = pvAwsS3Options;
            IPrvObS3Client = pvS3Cliient;
            IPrvLogger = pvLogger;
        }

        public async Task<bool> UploadFileAsync(Stream pvFileStream, string pvFileName, string pvDirectory)
        {
            try
            {
                var pvFileTransferUtility = new TransferUtility(IPrvObS3Client);
                string vObBucketPath = !string.IsNullOrWhiteSpace(pvDirectory) ? IPrvAwsS3Options.BucketName + @"/" + pvDirectory : IPrvAwsS3Options.BucketName;

                var vObFileUploadRequest = new TransferUtilityUploadRequest()
                {
                    CannedACL = S3CannedACL.PublicRead,
                    StorageClass = S3StorageClass.ReducedRedundancy,
                    BucketName = vObBucketPath,
                    Key = pvFileName,
                    InputStream = pvFileStream
                };
                //vObFileUploadRequest.UploadProgressEvent += (sender, args) => IPrvLogger.LogInformation($"{args.FilePath} upload complete : {args.PercentDone}%");
                await pvFileTransferUtility.UploadAsync(vObFileUploadRequest);
                //IPrvLogger.LogInformation($"successfully uploaded {pvFileName} to {vObBucketPath} on {DateTime.UtcNow:O}");
                return true;
            }
            catch (AmazonS3Exception exS3)
            {
                if (exS3.ErrorCode != null && (exS3.ErrorCode.Equals("InvalidAccessKeyId") || exS3.ErrorCode.Equals("InvalidSecurity")))
                {
                    IPrvLogger.LogError("Por favor, compruebe las credenciales de AWS proporcionadas.");
                }
                else
                {
                    IPrvLogger.LogError($"Se ha producido un error con el mensaje: '{exS3.Message}' al subir el archivo: {pvFileName}");
                }
                return false;
            }   
        }

        public async Task<bool> UploadFileAsync(string pvFilePath, string pvDirectory = null)
        {
            try
            {
                var pvFileTransferUtility = new TransferUtility(IPrvObS3Client);
                string vObBucketPath = !string.IsNullOrWhiteSpace(pvDirectory) ? IPrvAwsS3Options.BucketName + @"/" + pvDirectory  : IPrvAwsS3Options.BucketName;
                // 1. Upload a file, file name is used as the object key name.
                var vObFileUploadRequest = new TransferUtilityUploadRequest()
                {
                    CannedACL = S3CannedACL.PublicRead,
                    StorageClass = S3StorageClass.ReducedRedundancy,
                    BucketName = vObBucketPath,
                    FilePath = pvFilePath
                };
                //vObFileUploadRequest.UploadProgressEvent += (sender, args) =>  IPrvLogger.LogInformation($"{args.FilePath} upload complete : {args.PercentDone}%");
                await pvFileTransferUtility.UploadAsync(vObFileUploadRequest);
                //IPrvLogger.LogInformation($"successfully uploaded {pvFilePath} to {vObBucketPath} on {DateTime.UtcNow:O}");
                return true;
            }
            catch (AmazonS3Exception exS3)
            {
                if (exS3.ErrorCode != null && (exS3.ErrorCode.Equals("InvalidAccessKeyId") || exS3.ErrorCode.Equals("InvalidSecurity")))
                {
                    IPrvLogger.LogError("Por favor, compruebe las credenciales de AWS proporcionadas.");
                }
                else
                {
                    IPrvLogger.LogError($"Se ha producido un error con el mensaje: '{exS3.Message}' al subir el archivo: {pvFilePath}");
                }
                return false;
            }
        }

        public async Task<bool> UploadFileAsync(string pvFilePath, string pvDirectory = null, string pvFileName = null)
        {
            try
            {
                var pvFileTransferUtility = new TransferUtility(IPrvObS3Client);
                string vObBucketPath = !string.IsNullOrWhiteSpace(pvDirectory) ? IPrvAwsS3Options.BucketName + @"/" + pvDirectory : IPrvAwsS3Options.BucketName;
                // 1. Upload a file, file name is used as the object key name.
                var vObFileUploadRequest = new TransferUtilityUploadRequest()
                {
                    CannedACL = S3CannedACL.PublicRead,
                    StorageClass = S3StorageClass.ReducedRedundancy,
                    BucketName = vObBucketPath,
                    FilePath = pvFilePath,
                    Key = pvFileName
                };
                //vObFileUploadRequest.UploadProgressEvent += (sender, args) =>  IPrvLogger.LogInformation($"{args.FilePath} upload complete : {args.PercentDone}%");
                await pvFileTransferUtility.UploadAsync(vObFileUploadRequest);
                //IPrvLogger.LogInformation($"successfully uploaded {pvFilePath} to {vObBucketPath} on {DateTime.UtcNow:O}");
                return true;
            }
            catch (AmazonS3Exception exS3)
            {
                if (exS3.ErrorCode != null && (exS3.ErrorCode.Equals("InvalidAccessKeyId") || exS3.ErrorCode.Equals("InvalidSecurity")))
                {
                    IPrvLogger.LogError("Por favor, compruebe las credenciales de AWS proporcionadas.");
                }
                else
                {
                    IPrvLogger.LogError($"Se ha producido un error con el mensaje: '{exS3.Message}' al subir el archivo: {pvFilePath}");
                }
                return false;
            }
            catch (Exception ex)
            {
                IPrvLogger.LogError($"Se ha producido un error con el mensaje: '{ex.Message}'");
                return false;
            }
        }

        public async Task<bool> UploadFileAsync(string pvContents, string pvContentType, string pvFileName, string pvDirectory = null)
        {
            try
            {
                string vObBucketPath = !string.IsNullOrWhiteSpace(pvDirectory) ? IPrvAwsS3Options.BucketName + @"/" + pvDirectory : IPrvAwsS3Options.BucketName;
                //1. put object 
                var putRequest = new PutObjectRequest
                {
                    BucketName = vObBucketPath,
                    Key = pvFileName,
                    ContentBody = pvContents,
                    ContentType = pvContentType,
                    CannedACL = S3CannedACL.PublicRead,
                    StorageClass = S3StorageClass.ReducedRedundancy
                };
                var response = await IPrvObS3Client.PutObjectAsync(putRequest);
                if (response.HttpStatusCode == HttpStatusCode.OK)
                {
                    //IPrvLogger.LogInformation($"Archivo {pvFileName} subido con éxito a {vObBucketPath} en {DateTime.UtcNow:O}");
                    return true;
                }

                //IPrvLogger.LogError($"El archivo {pvFileName} no pudo subir al: {vObBucketPath} en {DateTime.UtcNow:O}");
                return false;
            }
            catch (AmazonS3Exception exS3)
            {
                if (exS3.ErrorCode != null && (exS3.ErrorCode.Equals("InvalidAccessKeyId") || exS3.ErrorCode.Equals("InvalidSecurity")))
                {
                    IPrvLogger.LogError("Por favor, compruebe las credenciales de AWS proporcionadas.");
                }
                else
                {
                    IPrvLogger.LogError($"Se ha producido un error con el mensaje: '{exS3.Message}' al subir el archivo: {pvFileName}");
                }
                return false;
            }
        }

        public async Task<(Stream pvFileStream, string pvContentType)> ReadFileAsync(string pvFileName, string pvDirectory = null)
        {
            try
            {
                var pvFileTransferUtility = new TransferUtility(IPrvObS3Client);
                string vObBucketPath = !string.IsNullOrWhiteSpace(pvDirectory) ? IPrvAwsS3Options.BucketName + @"/" + pvDirectory : IPrvAwsS3Options.BucketName;
                var pvObRequest = new GetObjectRequest()
                {
                    BucketName = vObBucketPath,
                    Key = pvFileName
                };
                // 1. read files
                GetObjectResponse pvObjectResponse = await pvFileTransferUtility.S3Client.GetObjectAsync(pvObRequest);
                return (pvObjectResponse.ResponseStream, pvObjectResponse.Headers.ContentType);
            }
            catch (AmazonS3Exception exS3)
            {
                if (exS3.ErrorCode != null && (exS3.ErrorCode.Equals("InvalidAccessKeyId") || exS3.ErrorCode.Equals("InvalidSecurity")))
                {
                    IPrvLogger.LogError("Por favor, compruebe las credenciales de AWS proporcionadas.");
                }
                else
                {
                    IPrvLogger.LogError($"Se ha producido un error con el mensaje: '{exS3.Message}' al subir el archivo: {pvFileName}");
                }
                return (null, null);
            }
        }

        public async Task<List<(Stream pvFileStream, string pvFileName, string pvContentType)>> ReadDirectoryAsync(string pvDirectory)
        {
            var objectCollection = new List<(Stream, string, string)>();
            try
            {
                var pvFileTransferUtility = new TransferUtility(IPrvObS3Client);
                var pvObRequest = new ListObjectsRequest()
                {
                    BucketName = IPrvAwsS3Options.BucketName,
                    Prefix = pvDirectory
                };
                // 1. read files
                var pvObjectResponse = await pvFileTransferUtility.S3Client.ListObjectsAsync(pvObRequest);
                foreach (var entry in pvObjectResponse.S3Objects)
                {
                    var pvFileName = entry.Key.Split('/').Last();
                    if (!string.IsNullOrEmpty(pvFileName))
                    {
                        var (pvFileStream, pvContentType) = await ReadFileAsync(pvFileName, pvDirectory);
                        objectCollection.Add((pvFileStream, pvFileName, pvContentType));

                    }
                }

                return objectCollection;
            }
            catch (AmazonS3Exception exS3)
            {
                if (exS3.ErrorCode != null && (exS3.ErrorCode.Equals("InvalidAccessKeyId") || exS3.ErrorCode.Equals("InvalidSecurity")))
                {
                    IPrvLogger.LogError("Por favor, compruebe las credenciales de AWS proporcionadas.");
                }
                else
                {
                    IPrvLogger.LogError($"Se ha producido un error con el mensaje: '{exS3.Message}'");
                }
                return objectCollection;
            }
        }

        public async Task<string> InfoFilesDirectoryAsync(string pvDirectory)
        {
            var objectCollection = new List<(Stream, string, string)>();
            try
            {
                var pvFileTransferUtility = new TransferUtility(IPrvObS3Client);
                var pvObRequest = new ListObjectsRequest()
                {
                    BucketName = IPrvAwsS3Options.BucketName,
                    Prefix = pvDirectory
                };
                // 1. read files
                var pvObjectResponse = await pvFileTransferUtility.S3Client.ListObjectsAsync(pvObRequest);
                var vJSonInfoFolderS3 = JsonConvert.SerializeObject(pvObjectResponse);

                return vJSonInfoFolderS3;
            }
            catch (AmazonS3Exception exS3)
            {
                if (exS3.ErrorCode != null && (exS3.ErrorCode.Equals("InvalidAccessKeyId") || exS3.ErrorCode.Equals("InvalidSecurity")))
                {
                    IPrvLogger.LogError("Por favor, compruebe las credenciales de AWS proporcionadas.");
                }
                else
                {
                    IPrvLogger.LogError($"Se ha producido un error con el mensaje: '{exS3.Message}'");
                }
                return string.Empty;
            }
        }

        public async Task<bool> MoveFileAsync(string pvFileName, string sourceDirectory, string destDirectory)
        {
            try
            {
                var pvObCopyRequest = new CopyObjectRequest
                {
                    SourceBucket = IPrvAwsS3Options.BucketName + @"/" + sourceDirectory,
                    SourceKey = pvFileName,
                    DestinationBucket = IPrvAwsS3Options.BucketName + @"/" + destDirectory,
                    DestinationKey = pvFileName
                };
                var response = await IPrvObS3Client.CopyObjectAsync(pvObCopyRequest);
                if (response.HttpStatusCode == HttpStatusCode.OK)
                {
                    var deleteRequest = new DeleteObjectRequest
                    {
                        BucketName = IPrvAwsS3Options.BucketName + @"/" + sourceDirectory,
                        Key = pvFileName
                    };
                    await IPrvObS3Client.DeleteObjectAsync(deleteRequest);
                    return true;
                }

                return false;
            }
            catch (AmazonS3Exception exS3)
            {
                if (exS3.ErrorCode != null && (exS3.ErrorCode.Equals("InvalidAccessKeyId") || exS3.ErrorCode.Equals("InvalidSecurity")))
                {
                    IPrvLogger.LogError("Por favor, compruebe las credenciales de AWS proporcionadas.");
                }
                else
                {
                    IPrvLogger.LogError($"Se ha producido un error con el mensaje: '{exS3.Message}' al subir el archivo: {pvFileName}");
                }
                return false;
            }
        }

        public async Task<bool> RemoveFileAsync(string pvFileName, string pvDirectory = null)
        {
            try
            {
                var pvFileTransferUtility = new TransferUtility(IPrvObS3Client);
                string vObBucketPath = !string.IsNullOrWhiteSpace(pvDirectory) ? IPrvAwsS3Options.BucketName + @"/" + pvDirectory : IPrvAwsS3Options.BucketName;
                // 1. deletes files
                await pvFileTransferUtility.S3Client.DeleteObjectAsync(new DeleteObjectRequest()
                {
                    BucketName = vObBucketPath,
                    Key = pvFileName
                });
                IPrvLogger.LogInformation($"Archivo {pvFileName} eliminado con éxito  de {vObBucketPath}");
                return true;
            }
            catch (AmazonS3Exception exS3)
            {
                if (exS3.ErrorCode != null && (exS3.ErrorCode.Equals("InvalidAccessKeyId") || exS3.ErrorCode.Equals("InvalidSecurity")))
                {
                    IPrvLogger.LogError("Por favor, compruebe las credenciales de AWS proporcionadas.");
                }
                else
                {
                    IPrvLogger.LogError($"Se ha producido un error con el mensaje: '{exS3.Message}' al subir el archivo: {pvFileName}");
                }
                return false;
            }
            catch (Exception e)
            {
                IPrvLogger.LogError("Se encontro un error desconocido en el servidor. Msj'{0}' cuando se estaba escribiendo el archivo." , e.Message);
                return false;
                throw;
            }
        }

    }
}
