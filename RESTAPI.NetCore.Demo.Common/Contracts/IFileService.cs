using System.IO;

namespace RESTAPI.NetCore.Demo.Common.Contracts
{
    public interface IFileService
    {
        string GetImgFileUrl(string id, FileType type);

        string ReadResourceFile(string id);

        FileStream ReadImgFile(string id, FileType fileType);
    }

    public enum FileType
    {
        imageLarge = 1,
        imageThumb = 2
    }
}
