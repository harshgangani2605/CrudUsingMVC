using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyApp1.Data;
using MyApp1.Models;
using MyApp1.Data;
using MyApp1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyNewApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SerialNumbersController : Controller
    {
        private readonly MyAppContext _context;

        public SerialNumbersController(MyAppContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var myAppContext = _context.SerialNumbers.Include(s => s.Item);
            return View(await myAppContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serialNumber = await _context.SerialNumbers
                .Include(s => s.Item)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (serialNumber == null)
            {
                return NotFound();
            }

            return View(serialNumber);
        }

       
        public IActionResult Create()
        {
         
            var usedItemIds = _context.SerialNumbers
                .Select(s => s.ItemId)
                .ToList();

           
            var availableItems = _context.Items
                .Where(i => !usedItemIds.Contains(i.Id))
                .ToList();

            ViewData["ItemId"] = new SelectList(availableItems, "Id", "Name");

            return View();
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ItemId")] SerialNumber serialNumber)
        {
            if (ModelState.IsValid)
            {
                _context.Add(serialNumber);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

          
            var usedItemIds = _context.SerialNumbers
                .Select(s => s.ItemId)
                .ToList();

            var availableItems = _context.Items
                .Where(i => !usedItemIds.Contains(i.Id))
                .ToList();

            ViewData["ItemId"] = new SelectList(availableItems, "Id", "Name", serialNumber.ItemId);

            return View(serialNumber);
        }
      
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serialNumber = await _context.SerialNumbers.FindAsync(id);
            if (serialNumber == null)
            {
                return NotFound();
            }

            
            var usedItemIds = _context.SerialNumbers
                .Where(s => s.ItemId != serialNumber.ItemId)
                .Select(s => s.ItemId)
                .ToList();

           
            var availableItems = _context.Items
                .Where(i => !usedItemIds.Contains(i.Id))
                .ToList();

          
            ViewData["ItemId"] = new SelectList(availableItems, "Id", "Name", serialNumber.ItemId);

            return View(serialNumber);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ItemId")] SerialNumber serialNumber)
        {
            if (id != serialNumber.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(serialNumber);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SerialNumberExists(serialNumber.Id))
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

       
            var usedItemIds = _context.SerialNumbers
                .Where(s => s.ItemId != serialNumber.ItemId)
                .Select(s => s.ItemId)
                .ToList();

            var availableItems = _context.Items
                .Where(i => !usedItemIds.Contains(i.Id))
                .ToList();

            ViewData["ItemId"] = new SelectList(availableItems, "Id", "Name", serialNumber.ItemId);

            return View(serialNumber);
        }


       
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serialNumber = await _context.SerialNumbers
                .Include(s => s.Item)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (serialNumber == null)
            {
                return NotFound();
            }

            return View(serialNumber);
        }

     
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var serialNumber = await _context.SerialNumbers.FindAsync(id);
            if (serialNumber != null)
            {
                _context.SerialNumbers.Remove(serialNumber);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SerialNumberExists(int id)
        {
            return _context.SerialNumbers.Any(e => e.Id == id);
        }
    }
}