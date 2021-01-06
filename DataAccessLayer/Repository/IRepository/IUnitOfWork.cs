using EntityModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Repository.IRepository
{
    public interface IUnitOfWork: IDisposable
    {
        IUserImageRepo UserImage { get; }
        IStoryRepo Story { get; }        
        void Save();
    }
}
