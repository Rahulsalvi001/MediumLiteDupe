using EntityModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Repository.IRepository
{
    public interface IStoryRepo : IRepository<Story>
    {
        void Update(Story story);
        List<StoryDashboard> GetStoryForDashboard();
    }
}
