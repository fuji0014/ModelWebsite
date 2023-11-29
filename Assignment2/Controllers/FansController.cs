using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Assignment2.Data;
using Assignment2.Models;
using Assignment2.Models.ViewModels;
using Microsoft.CodeAnalysis;
using System.Diagnostics;

namespace Assignment2.Controllers
{
    public class FansController : Controller
    {
        private readonly SportsDbContext _context;

        public FansController(SportsDbContext context)
        {
            _context = context;
        }

        // GET: Fans
        public async Task<IActionResult> Index(string Id)
        {
            var viewModel = new SportClubViewModel
            {
                Fans = _context.Fans.ToList(),
                SportClubs = _context.SportClubs.ToList(),
                Subscriptions = _context.Subscriptions
                    .Include(sub => sub.Fan)
                    .Include(sub => sub.SportClub)
                    .ToList()
            };

            if (Id != null)
            {                
                ViewBag.Id = Id;
                viewModel.Subscriptions = viewModel.Fans.Where(
                    x => x.Id.ToString().Contains(Id)).Single().Subscriptions;
            }

            //return View(await _context.SportClubs.ToListAsync());

            return View(viewModel);
        }

        // GET: Fans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Fans == null)
            {
                return NotFound();
            }

            var fan = await _context.Fans
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fan == null)
            {
                return NotFound();
            }

            return View(fan);
        }

        // GET: Fans/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Fans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LastName,FirstName,BirthDate")] Fan fan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(fan);
        }

        // GET: Fans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Fans == null)
            {
                return NotFound();
            }

            var fan = await _context.Fans.FindAsync(id);
            if (fan == null)
            {
                return NotFound();
            }
            return View(fan);
        }

        // POST: Fans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LastName,FirstName,BirthDate")] Fan fan)
        {
            if (id != fan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FanExists(fan.Id))
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
            return View(fan);
        }

        // GET: Fans/EditSubscriptions/5
        public async Task<IActionResult> EditSubscriptions(int? id)
        {
           
            var fan = await _context.Fans
               .FirstOrDefaultAsync(f => f.Id == id);

            if (fan == null)
            {
                return NotFound();
            }

            var subscriptions = await _context.SportClubs
                .Select(club => new SportClubSubscriptionViewModel
                {
                    SportClubId = club.Id,
                    Title = club.Title,
                    IsMember = _context.Subscriptions.Any(s => s.FanId == id && s.SportClubId == club.Id)
                })
                .ToListAsync();

            var viewModel = new FanSubscriptionViewModel
            {
                Fan = fan,
                Subscriptions = subscriptions,
                SportClubs = await _context.SportClubs.ToListAsync()
            };

            return View(viewModel);
        }

        // POST: Fans/EditSubscriptions/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSubscriptions(int? id, [Bind("Id,LastName,FirstName,BirthDate")] Fan fan, string sportClubId)
        {

            //return RedirectToAction(nameof(Index));

            if (id != fan.Id)
            {
                return NotFound();
            }

            if (sportClubId == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fan);
                    await _context.SaveChangesAsync();

                    var isMember = await _context.Subscriptions
                        .AnyAsync(s => s.FanId == id && s.SportClubId == sportClubId);

                    // Toggle membership status by registerting/unregistering subscription
                    if (isMember)
                    {
                        // Remove subscription to indicate non-membership
                        var subscriptionToRemove = await _context.Subscriptions
                            .FirstOrDefaultAsync(s => s.FanId == id && s.SportClubId.Contains(sportClubId));
                        Debug.Write("subscriptionToRemove: " + subscriptionToRemove);

                        if (subscriptionToRemove != null)
                        {
                            _context.Subscriptions.Remove(subscriptionToRemove);
                            await _context.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        // Add subscription to indicate membership
                        _context.Subscriptions.Add(new Subscription
                        {
                            FanId = id.Value,
                            SportClubId = sportClubId
                        });

                        await _context.SaveChangesAsync();
                    }
                    return RedirectToAction("EditSubscriptions", "Fans", new {id});
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FanExists(fan.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

            }
            return View(fan);           
        }

        // GET: Fans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Fans == null)
            {
                return NotFound();
            }

            var fan = await _context.Fans
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fan == null)
            {
                return NotFound();
            }

            return View(fan);
        }

        // POST: Fans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Fans == null)
            {
                return Problem("Entity set 'SportsDbContext.Fans'  is null.");
            }
            var fan = await _context.Fans.FindAsync(id);
            if (fan != null)
            {
                _context.Fans.Remove(fan);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FanExists(int id)
        {
          return _context.Fans.Any(e => e.Id == id);
        }
    }
}
