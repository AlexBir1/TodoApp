using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoAPI.APIResponse.Implementations;
using TodoAPI.APIResponse.Interfaces;
using TodoAPI.DAL.Entities;
using TodoAPI.DAL.UOW.Interfaces;
using TodoAPI.Services.Interfaces;
using TodoAPI.Shared.Models;

namespace TodoAPI.Services.Implementations
{
    public class AttachmentService : IAttachmentService
    {
        private readonly IDBRepository _dbRepo;
        private readonly IMapper _mapper;
        private readonly string _rootDirectory;

        public AttachmentService(IDBRepository dbRepo, IMapper mapper)
        {
            _dbRepo = dbRepo;
            _mapper = mapper;
            _rootDirectory = Directory.GetCurrentDirectory();
        }

        public async Task<IAPIResponse<Attachment>> CreateAsync(AttachmentModel model)
        {
            Attachment attachment = new Attachment();

            using (var stream = new FileStream(Path.Combine(_rootDirectory, model.GoalId.ToString(), model.File.Name), FileMode.Create))
            {
                await model.File.CopyToAsync(stream);

                attachment.Fullpath = stream.Name;
                attachment.Filename = stream.Name.Split('\\').Last();
                attachment.Size = stream.Length;
                attachment.ContentType = model.File.ContentType;
                attachment.Extension = attachment.Filename.Split('.').Last();
            }

            if (new DirectoryInfo(Path.Combine(_rootDirectory, model.GoalId.ToString())).GetFiles().SingleOrDefault(x => x.Name == attachment.Filename) == null)
                throw new Exception("Error while saving file.");

            var result = await _dbRepo.AttachmentRepo.CreateAsync(attachment);
            await _dbRepo.CommitAsync();
            return new APIResponse<Attachment>(true, attachment);
        }

        public async Task<IAPIResponse<Attachment>> DeleteAsync(string id)
        {
            var file = await _dbRepo.AttachmentRepo.GetByIdAsync(id);

            var physicalFile = new DirectoryInfo(file.Fullpath).GetFiles().SingleOrDefault(x => x.Name == file.Filename);

            if (physicalFile != null)
                physicalFile.Delete();

            var result = await _dbRepo.AttachmentRepo.DeleteAsync(id);
            await _dbRepo.CommitAsync();
            return new APIResponse<Attachment>(true, result);
        }

        public async Task<FileModel> DownloadAsync(string id)
        {
            var file = await _dbRepo.AttachmentRepo.GetByIdAsync(id);

            var physicalFile = new DirectoryInfo(file.Fullpath).GetFiles().SingleOrDefault(x => x.Name == file.Filename);

            var model = new FileModel
            {
                Filename = file.Filename,
                ContentType = file.ContentType,
            };

            using (var stream = new FileStream(file.Fullpath, FileMode.Open))
            {
                await stream.CopyToAsync(model.File);
            }

            return model;
        }

        public async Task<IAPIResponse<IEnumerable<Attachment>>> GetAllByGoalIdAsync(string goalId)
        {
            return new APIResponse<IEnumerable<Attachment>>(true, await _dbRepo.AttachmentRepo.GetAllAsync(x => x.GoalId == Guid.Parse(goalId)));
        }
    }
}
