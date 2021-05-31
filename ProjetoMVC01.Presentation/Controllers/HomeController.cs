using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoMVC01.Presentation.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        //método para abrir a página /Home/Index
        public IActionResult Index() //page load!
        {
            return View();
        }
    }
}
