using RESTAPI.NetCore.Demo.Common.Models;
using RESTAPI.NetCore.Demo.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RESTAPI.NetCore.Demo.Web.Domain.Contracts
{
    public interface IUserService
    {
        Task<List<UserViewModel>> Get(string name = "", int pageSize = 1, int pageNum = 1);
        Task<UserViewModel> GetById(Guid id);
        Task<List<UserViewModel>> BulkGenerate(int count);
        Task<UserViewModel> Update(User user);
        Task Delete(Guid id);
    }

}
