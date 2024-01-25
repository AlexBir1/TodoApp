using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoAPI.APIResponse.Implementations;
using TodoAPI.APIResponse.Interfaces;
using TodoAPI.DAL.Entities;
using TodoAPI.DAL.Repositories.Interfaces;
using TodoAPI.Services.Interfaces;
using TodoAPI.Shared.Models;

namespace TodoAPI.Services.Implementations
{
    public class AttachmentService : IAttachmentService
    {
        private readonly IAttachmentRepository _attachmentRepo;
        private readonly IMapper _mapper;
        private readonly string _rootDirectory;

        public AttachmentService(IAttachmentRepository attachmentRepo, IMapper mapper)
        {
            _attachmentRepo = attachmentRepo;
            _mapper = mapper;
            _rootDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Attachments");
        }

        public async Task<IAPIResponse<Attachment>> CreateAsync(AttachmentModel model)
        {
            Attachment attachment = new Attachment();
            attachment.GoalId = model.GoalId;

            if (!Directory.Exists(Path.Combine(_rootDirectory, model.GoalId.ToString())))
                Directory.CreateDirectory(Path.Combine(_rootDirectory, model.GoalId.ToString()));

            using (var stream = new FileStream(Path.Combine(_rootDirectory, model.GoalId.ToString(), model.File.FileName), FileMode.Create))
            {
                await model.File.CopyToAsync(stream);

                attachment.Fullpath = stream.Name;
                attachment.Filename = stream.Name.Split('\\').Last();
                attachment.Size = stream.Length;
                attachment.ContentType = model.File.ContentType;
                attachment.Extension = attachment.Filename.Split('.').Last();
            }

            var goalAttachmentsDirectory = new DirectoryInfo(Path.Combine(_rootDirectory, model.GoalId.ToString()));

            if (goalAttachmentsDirectory.GetFiles().SingleOrDefault(x => x.Name == attachment.Filename) == null)
                throw new Exception("Error while saving file.");

            var result = await _attachmentRepo.CreateAsync(attachment);
            return new APIResponse<Attachment>(true, attachment);
        }

        public async Task<IAPIResponse<Attachment>> DeleteAsync(string id)
        {
            var file = await _attachmentRepo.GetByIdAsync(id);

            var physicalFile = new DirectoryInfo(file.Fullpath.Replace(file.Filename, "")).GetFiles().SingleOrDefault(x => x.Name == file.Filename);

            if (physicalFile != null)
                physicalFile.Delete();

            if (new DirectoryInfo(file.Fullpath.Replace(file.Filename, "")).GetFiles().Length == 0)
                Directory.Delete(file.Fullpath.Replace(file.Filename, ""));

            var result = await _attachmentRepo.DeleteAsync(id);
            return new APIResponse<Attachment>(true, result);
        }

        public async Task<FileModel> DownloadAsync(string id)
        {
            var file = await _attachmentRepo.GetByIdAsync(id);

            var physicalFile = new DirectoryInfo(file.Fullpath.Replace(file.Filename, "")).GetFiles().SingleOrDefault(x => x.Name == file.Filename);

            var model = new FileModel
            {
                Filename = file.Filename,
                ContentType = file.ContentType,
            };

            using (var stream = new FileStream(file.Fullpath, FileMode.Open))
            {
                await stream.CopyToAsync(model.File);
            }

            model.File.Position = 0;

            return model;
        }

        public async Task<IAPIResponse<IEnumerable<Attachment>>> GetAllByGoalIdAsync(string goalId)
        {
            return new APIResponse<IEnumerable<Attachment>>(true, await _attachmentRepo.GetAllAsync(x => x.GoalId == Guid.Parse(goalId)));
        }
    }
}
