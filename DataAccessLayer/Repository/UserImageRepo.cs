using DataAccessLayer.Data;
using DataAccessLayer.Repository.IRepository;
using EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessLayer.Repository
{
    public class UserImageRepo : Repository<UserImage>, IUserImageRepo
    {
        private readonly AppDBContext _db;
        public UserImageRepo(AppDBContext db) : base(db)
        {
            _db = db;
        }
        public void Update(UserImage image)
        {
            var objFromDB = _db.UserImage.FirstOrDefault(s => s.Id == image.Id);
            if (objFromDB != null)
            {
                
                objFromDB.Name = image.Name;
                objFromDB.ExtensionType = image.ExtensionType;
                objFromDB.Data = image.Data;
            }

        }
    }
}
