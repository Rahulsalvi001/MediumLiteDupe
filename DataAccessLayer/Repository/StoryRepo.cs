using DataAccessLayer.Data;
using DataAccessLayer.Repository.IRepository;
using EntityModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace DataAccessLayer.Repository
{
    public class StoryRepo : Repository<Story>, IStoryRepo
    {
        private readonly AppDBContext _db;
        public StoryRepo(AppDBContext db) : base(db)
        {
            _db = db;
        }

        public List<StoryDashboard> GetStoryForDashboard()
        {
            var List = (from story in _db.Story
                         join usr in _db.AppUsers                         
                         on story.UserProfileId equals usr.UserProfileId
                        join img in _db.UserImage 
                        on usr.ImageId equals img.Id into usrImgInfo
                        from usrImg in usrImgInfo.DefaultIfEmpty()
                        where story.Status == 2
                        select new StoryDashboard
                         {
                             Id = story.Id,
                             UserName = usr.Name,
                             UserImgData = usrImg.Data,
                             UserImgType = usrImg.ExtensionType,
                             Title = story.Title,
                             CreatedOn = story.CreatedOn,
                             ModifiedOn = story.ModifiedOn

                         }).OrderByDescending(x => x.ModifiedOn).ToList();
            return List;
        }

        public void Update(Story story)
        {
            var objFromDB = _db.Story.Include("StoryData").FirstOrDefault(s => s.Id == story.Id);
            if (objFromDB != null)
            {
                objFromDB.Title = story.Title;
                objFromDB.Description = story.Description;
                objFromDB.StoryData.Data = story.StoryData.Data;
                objFromDB.Status = story.Status;
                objFromDB.ModifiedOn = DateTime.Now;
            }

        }
    }
}
