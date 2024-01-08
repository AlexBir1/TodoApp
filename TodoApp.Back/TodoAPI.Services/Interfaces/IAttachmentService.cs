
using TodoAPI.APIResponse.Interfaces;
using TodoAPI.DAL.Entities;
using TodoAPI.Shared.Models;

namespace TodoAPI.Services.Interfaces
{
    public interface IAttachmentService
    {
        Task<IAPIResponse<Attachment>> CreateAsync(AttachmentModel model);
        Task<IAPIResponse<IEnumerable<Attachment>>> GetAllByGoalIdAsync(string goalId);
        Task<IAPIResponse<Attachment>> DeleteAsync(string id);

        Task<FileModel> DownloadAsync(string id);
    }
}
