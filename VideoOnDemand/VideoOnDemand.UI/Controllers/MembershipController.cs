using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VideoOnDemand.Data.Data.Entities;
using VideoOnDemand.Data.Repositories;

namespace VideoOnDemand.UI.Controllers
{
    public class MembershipController : Controller
    {
        private string _userId;
        private IMapper _mapper;
        private IReadRepository _readRepository;

        public MembershipController(IHttpContextAccessor httpContextAcessor, 
            UserManager<User> userManager, 
            IMapper mapper,
            IReadRepository readRepository)
        {
            var user = httpContextAcessor.HttpContext.User;
            _userId = userManager.GetUserId(user);
            _mapper = mapper;
            _readRepository = readRepository;
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult Course(int id)
        {
            return View();
        }

        public IActionResult Video(int id)
        {
            return View();
        }
    }
}