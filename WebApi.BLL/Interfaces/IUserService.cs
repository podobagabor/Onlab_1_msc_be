using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.BLL.Dtos;

namespace WebApi.BLL.Interfaces
{
    public interface IUserService
    {
        public Task<UserDto> GetUserAsync(int userId);
        public Task<IEnumerable<UserDto>> GetUsersAsync();
        public Task<UserDto> InsertUserAsync(UserDto user);
        public Task<UserDto> UpdateUserAsync(UserDto user);
        public Task<UserDto> DeleteUserAsync(int userId);
    }
}
