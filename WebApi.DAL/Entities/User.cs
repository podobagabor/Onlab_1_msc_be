using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.DAL.Entities
{
    public class User : IdentityUser<int>
    {
        public string Name { get; set; } = null!;
        public string? Photo { get; set; }
    }
}
