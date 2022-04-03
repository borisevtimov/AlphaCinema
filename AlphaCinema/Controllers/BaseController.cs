using AlphaCinema.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlphaCinema.Controllers
{
    [Authorize(Roles = RoleConstant.Administrator)]
    [Authorize(Roles = RoleConstant.User)]
    public class BaseController : Controller
    {
        
    }
}
