using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using ProdigyScout.Data;
using ProdigyScout.Models;
using ProdigyScout.ViewModels;

namespace ProdigyScout.Controllers
{
    [Authorize]
    public class ProspectsController : Controller
    {
        private readonly ProdigyScoutContext _context;

        public ProspectsController(ProdigyScoutContext context)
        {
            _context = context;
        }

        // GET: Prospects
        public async Task<IActionResult> Index(StudentViewModel studentViewModel)
        {
            studentViewModel.Students = await _context.Prospect.ToListAsync();

            return View(studentViewModel);
        }

        // GET: Prospects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            StudentViewModel studentViewModel;

            if (id == null || _context.Prospect == null)
            {
                return NotFound();
            }

            var prospect = await _context.Prospect.FirstOrDefaultAsync(m => m.Id == id);

            if (prospect == null)
            {
                return NotFound();
            }
            else
            {
                studentViewModel = new(prospect);
            }

            return View(studentViewModel);
        }

        // GET: Prospects/Create
        public IActionResult Create()
        {
            StudentViewModel studentViewModel = new();

            return View(studentViewModel);
        }

        // POST: Prospects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,email,Gender,GPA")] StudentViewModel studentViewModel)
        {
            if (ModelState.IsValid)
            {
                Prospect prospect = new()
                {
                    FirstName = studentViewModel.FirstName.Trim(),
                    LastName = studentViewModel.LastName.Trim(),
                    email = studentViewModel.email.Trim(),
                    Gender = studentViewModel.Gender.Trim(),
                    GPA = studentViewModel.GPA

                };
                _context.Add(prospect);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(studentViewModel);
        }

        // GET: Prospects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            StudentViewModel studentViewModel;

            if (id == null)
            {
                return NotFound();
            }

            var prospect = await _context.Prospect.FindAsync(id);

            if (prospect == null)
            {
                return NotFound();
            }
            else
            {
                studentViewModel = new(prospect);
            }

            return View(studentViewModel);
        }

        // POST: Prospects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,email,Gender,GPA")] StudentViewModel studentViewModel)
        {
            if (id != studentViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Prospect prospect = await _context.Prospect.FindAsync(studentViewModel.Id);

                    if (prospect == null)
                    {
                        return NotFound();
                    }

                    prospect.FirstName = studentViewModel.FirstName.Trim();
                    prospect.LastName = studentViewModel.LastName.Trim();
                    prospect.email = studentViewModel.email.Trim();
                    prospect.Gender = studentViewModel.Gender.Trim();
                    prospect.GPA = studentViewModel.GPA;

                    _context.Update(prospect);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProspectExists(studentViewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(studentViewModel);
        }

        // GET: Prospects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            StudentViewModel studentViewModel;

            if (id == null)
            {
                return NotFound();
            }

            var prospect = await _context.Prospect.FirstOrDefaultAsync(m => m.Id == id);

            if (prospect == null)
            {
                return NotFound();
            }
            else
            {
                studentViewModel = new(prospect);
            }

            return View(studentViewModel);
        }

        // POST: Prospects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prospect = await _context.Prospect.FindAsync(id);
            if (prospect != null)
            {
                _context.Prospect.Remove(prospect);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProspectExists(int id)
        {
            return _context.Prospect.Any(e => e.Id == id);
        }
    }
}
