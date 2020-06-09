using Labor.Common;
using Labor.IRepository;
using Labor.IServices;
using Labor.Model.Models;
using Labor.Model.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Labor.Services
{
    public class UserService:BaseService<User>,IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            BaseRepository = userRepository;
        }

        public async Task<User> LoginAsync(LoginViewModel model)
        {
            string pwd = Md5Helper.Md5Encrypt(model.Password);
            return await _userRepository.GetAll().FirstOrDefaultAsync(m => m.Account == model.Account && m.Password == pwd);
        }

        public async Task<bool> RegisterAsync(RegisterViewModel model)
        {
            if (await _userRepository.GetAll().AnyAsync(m => m.Account == model.Account)) return false;
            string pwd = Md5Helper.Md5Encrypt(model.Password);
            await _userRepository.CreateAsync(new User
            {
                Account = model.Account,
                Password = pwd
            });
            return true;
        }
    }
}
