using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace InterOP.Core.Interfaces
{
    public interface IS3Services
    {
        Task<bool> UploadFileAsync(Stream pvFileStream, string pvFileName, string pvDirectory);
        Task<bool> UploadFileAsync(string pvFilePath, string pvDirectory, string pvFileName);
        Task<bool> UploadFileAsync(string pvFilePath, string pvDirectory);
        Task<bool> UploadFileAsync(string pvContents, string pvContentType, string pvFileName, string pvDirectory);
        Task<(Stream pvFileStream, string pvContentType)> ReadFileAsync(string pvFileName, string pvDirectory);
        Task<List<(Stream pvFileStream, string pvFileName, string pvContentType)>> ReadDirectoryAsync(string pvDirectory);
        Task<string> InfoFilesDirectoryAsync(string pvDirectory);
        Task<bool> MoveFileAsync(string pvFileName, string pvSourcepvDirectory, string pvDestDirectory);
        Task<bool> RemoveFileAsync(string pvFileName, string pvDirectory);
    }
}
