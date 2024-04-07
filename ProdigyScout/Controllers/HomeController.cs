using Microsoft.AspNetCore.Mvc;
using ProdigyScout.Interfaces;
using ProdigyScout.Models;
using ProdigyScout.ViewModels;
using System.Diagnostics;

namespace ProdigyScout.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStudentRepository _studentRepository;

        public HomeController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }


        public async Task<IActionResult> Index()
        {
            var students = await _studentRepository.GetStudents("", "", "");
            var complexData = await _studentRepository.GetComplexData();

            var homeViewModel = new StudentViewModel
            {
                Students = students,
                ComplexData = complexData,
            };

            return View(homeViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
