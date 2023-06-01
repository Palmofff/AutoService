using AutoService.Data;
using AutoService.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoService.Controllers
{
    public class MechanicsController : Controller
    {
        private readonly IAutoServiceRepository _repository;


        public MechanicsController(IAutoServiceRepository repository)
        {
            _repository = repository;
        }

        // GET: Mechanics
        public async Task<IActionResult> Index()
        {
            return View(await _repository.GetAllMechanicsAsync());
        }

        // GET: Mechanics/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Mechanics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,PhoneNumber")] Mechanic mechanic)
        {
            if (ModelState.IsValid)
            {
                await _repository.AddNewMechanic(mechanic);
                return RedirectToAction(nameof(Index));
            }
            return View(mechanic);
        }

        // GET: Mechanics/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mechanic = await _repository.GetMechanicAsync(id);
            if (mechanic == null)
            {
                return NotFound();
            }
            return View(mechanic);
        }

        // POST: Mechanics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,PhoneNumber")] Mechanic mechanic)
        {
            if (id != mechanic.Id)
            {
                return NotFound();
            }

            switch (ModelState.IsValid)
            {
                case true:
                    await _repository.EditMechanicAsync(mechanic);
                    return RedirectToAction(nameof(Index));
                default:
                    return View(mechanic);
            }
        }
    }
}
