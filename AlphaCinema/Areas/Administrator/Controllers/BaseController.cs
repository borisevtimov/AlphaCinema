using AlphaCinema.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlphaCinema.Areas.Administrator.Controllers
{
    [Authorize(Roles = RoleConstant.Administrator)]
    [Area(RoleConstant.Administrator)]
    public class BaseController : Controller
    {
    
    }
}
