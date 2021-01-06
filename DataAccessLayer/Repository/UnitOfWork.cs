using DataAccessLayer.Data;
using DataAccessLayer.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDBContext _db;
        public UnitOfWork(AppDBContext db)
        {
            _db = db;
            UserImage = new UserImageRepo(db);
            Story = new StoryRepo(db);
        }

        public IUserImageRepo UserImage { get; private set; }
        public IStoryRepo Story { get; private set; }

        public void Dispose()
        {
            _db.Dispose();
        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
