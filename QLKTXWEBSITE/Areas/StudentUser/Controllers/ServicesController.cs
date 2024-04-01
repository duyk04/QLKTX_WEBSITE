﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLKTXWEBSITE.Models;

namespace QLKTXWEBSITE.Areas.StudentUser.Controllers
{
    [Area("StudentUser")]
    public class ServicesController : Controller
    {
        private readonly QlktxContext _context;

        public ServicesController(QlktxContext context)
        {
            _context = context;
        }

        // GET: StudentUser/Services
        public async Task<IActionResult> Index(int? studentId)
        {
            IQueryable<Service> services = _context.Services.Include(s => s.Room).Include(s => s.Student);
            if (studentId != null)
            {
                services = services.Where(o => o.StudentId == studentId);
            }

            return View(await services.ToListAsync());
        }

        // GET: StudentUser/Services/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Services == null)
            {
                return NotFound();
            }

            var service = await _context.Services
                .Include(s => s.Room)
                .Include(s => s.Student)
                .FirstOrDefaultAsync(m => m.ServiceId == id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        // GET: StudentUser/Services/Create
        public IActionResult Create()
        {
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId");
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "StudentId");
            return View();
        }

        // POST: StudentUser/Services/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ServiceId,ServiceName,Month,Price,RoomId,StudentId,Status")] Service service)
        {
            if (ModelState.IsValid)
            {
                _context.Add(service);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId", service.RoomId);
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "StudentId", service.StudentId);
            return View(service);
        }

        // GET: StudentUser/Services/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Services == null)
            {
                return NotFound();
            }

            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId", service.RoomId);
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "StudentId", service.StudentId);
            return View(service);
        }

        // POST: StudentUser/Services/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ServiceId,ServiceName,Month,Price,RoomId,StudentId,Status")] Service service)
        {
            if (id != service.ServiceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(service);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceExists(service.ServiceId))
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
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId", service.RoomId);
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "StudentId", service.StudentId);
            return View(service);
        }

        // GET: StudentUser/Services/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Services == null)
            {
                return NotFound();
            }

            var service = await _context.Services
                .Include(s => s.Room)
                .Include(s => s.Student)
                .FirstOrDefaultAsync(m => m.ServiceId == id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        // POST: StudentUser/Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Services == null)
            {
                return Problem("Entity set 'QlktxContext.Services'  is null.");
            }
            var service = await _context.Services.FindAsync(id);
            if (service != null)
            {
                _context.Services.Remove(service);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceExists(int id)
        {
          return (_context.Services?.Any(e => e.ServiceId == id)).GetValueOrDefault();
        }
    }
}
