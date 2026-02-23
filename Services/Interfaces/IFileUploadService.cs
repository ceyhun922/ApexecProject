namespace ApexWebAPI.Services.Interfaces
{
    public interface IFileUploadService
    {
        Task<string> UploadAsync(IFormFile file, string subfolder);
        Task<string> UploadAutoDetectAsync(IFormFile file);
        void DeleteFile(string? relativePath);
        bool IsImageFile(string extension);
        bool IsVideoFile(string extension);
    }
}
