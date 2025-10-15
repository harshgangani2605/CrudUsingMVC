﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyApp1.Data;
using MyApp1.Data;
using MyApp1.Models;
using MyApp1.Models;
using System.Linq;
using System.Threading.Tasks;

namespace MyNewApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ItemClientsController : Controller
    {
        private readonly MyAppContext _context;

        public ItemClientsController(MyAppContext context)
        {
            _context = context;
        }

     
        public async Task<IActionResult> Index()
        {
            var mappings = _context.ItemClients
                .Include(ic => ic.Item)
                .Include(ic => ic.Client);

            return View(await mappings.ToListAsync());
        }

    
        public IActionResult Create()
        {
            ViewData["ItemId"] = new SelectList(_context.Items, "Id", "Name");
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ItemId,ClientId")] ItemClient itemClient)
        {
            if (ModelState.IsValid)
            {

                var exists = await _context.ItemClients
                    .AnyAsync(ic => ic.ItemId == itemClient.ItemId && ic.ClientId == itemClient.ClientId);

                if (!exists)
                {
                    _context.Add(itemClient);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "This item-client mapping already exists.");
            }

            ViewData["ItemId"] = new SelectList(_context.Items, "Id", "Name", itemClient.ItemId);
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Name", itemClient.ClientId);
            return View(itemClient);
        }

        public async Task<IActionResult> Delete(int? itemId, int? clientId)
        {
            if (itemId == null || clientId == null)
            {
                return NotFound();
            }

            var mapping = await _context.ItemClients
                .Include(ic => ic.Item)
                .Include(ic => ic.Client)
                .FirstOrDefaultAsync(ic => ic.ItemId == itemId && ic.ClientId == clientId);

            if (mapping == null)
            {
                return NotFound();
            }

            return View(mapping);
        }

      
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int itemId, int clientId)
        {
            var mapping = await _context.ItemClients
                .FirstOrDefaultAsync(ic => ic.ItemId == itemId && ic.ClientId == clientId);

            if (mapping != null)
            {
                _context.ItemClients.Remove(mapping);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ItemClientExists(int itemId, int clientId)
        {
            return _context.ItemClients.Any(ic => ic.ItemId == itemId && ic.ClientId == clientId);
        }
    }
}