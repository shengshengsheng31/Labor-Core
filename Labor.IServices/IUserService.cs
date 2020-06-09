using Labor.Model.Models;
using Labor.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Labor.IServices
{
    public interface IUserService:IBaseService<User>
    {
        Task<User> LoginAsync(LoginViewModel model);

        Task<bool> RegisterAsync(RegisterViewModel model);
    }
}
