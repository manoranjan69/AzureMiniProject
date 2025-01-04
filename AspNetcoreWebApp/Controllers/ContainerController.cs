using AspNetcoreWebApp.Models;
using AspNetcoreWebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace AspNetcoreWebApp.Controllers
{
    public class ContainerController : Controller
    {
        private readonly IContainerService _containerService;
        public ContainerController(IContainerService containerService)
        {
            _containerService = containerService;
        }

        public async Task<IActionResult> Index()
        {
            var allcontainer = await _containerService.GetAllContainer();
            return View(allcontainer);
        }
        public async Task<IActionResult> Delete(string containerName)
        {
            await _containerService.DeleteContainer(containerName);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Create()
        {
            return View(new Container());
        }
        [HttpPost]
        public async Task<IActionResult> Create(Container container)
        {
            await _containerService.CreateContainer(container.Name);
            return RedirectToAction(nameof(Index));
        }
    }
}
