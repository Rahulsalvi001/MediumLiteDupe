using EntityModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Repository.IRepository
{
    public interface IUserImageRepo : IRepository<UserImage>
    {
        void Update(UserImage image);
    }
}
