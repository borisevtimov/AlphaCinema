using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlphaCinema.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        
    }
}
