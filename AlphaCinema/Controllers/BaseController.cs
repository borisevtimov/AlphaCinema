using AlphaCinema.Core.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlphaCinema.Controllers
{
    [Authorize(Roles = $"{RoleConstant.Administrator},{RoleConstant.User}")]
    public class BaseController : Controller
    {
        
    }
}
