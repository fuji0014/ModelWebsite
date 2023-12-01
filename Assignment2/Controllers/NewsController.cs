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
using Azure.Storage.Blobs;

namespace Assignment2.Controllers
{
    public class NewsController : Controller
    {
        private readonly SportsDbContext _context;
        private readonly BlobServiceClient _blobServiceClient;

        public NewsController(SportsDbContext context, BlobServiceClient blobServiceClient)
        {
            _context = context;
            _blobServiceClient = blobServiceClient;
        }

        // GET: News
        public async Task<IActionResult> Index(String id)
        {
            var newsModel = new NewsViewModel
            {
                SportClub = await _context.SportClubs.FindAsync(id),
                News = await _context.News.Where(n => n.SportClubId == id).ToListAsync()
            };

            return View(newsModel);
        }

        // GET: News/Create/A1
        public async Task<IActionResult> Create(String id)
        {
            var sportClub = await _context.SportClubs.FindAsync(id);
            if (sportClub == null)
            {
                return NotFound();
            }
            var fileInput = new FileInputViewModel
            {
                SportClubId = sportClub.Id,
                SportClubTitle = sportClub.Title,
            };

            return View(fileInput);
        }

        // POST: News/Create/A1
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(String id, [Bind("SportClubId,File")] News news)
        {
            if (!id.Equals(news.SportClubId.ToString()))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (news.File != null && news.File.Length > 0)
                    {
                        news.SportClub = await _context.SportClubs.FindAsync(id);
                        var containerName = news.SportClub.Title;
                        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName.ToLower());
                        await containerClient.CreateIfNotExistsAsync();

                        //random file name
                        news.FileName = Path.GetRandomFileName();

                        var blobClient = containerClient.GetBlobClient(news.FileName);
                        using (var stream = news.File.OpenReadStream())
                        {
                            await blobClient.UploadAsync(stream, true);
                        }

                        news.Url = blobClient.Uri.ToString();
                    }
                    else
                    {
                        // Handle the case where the news or news.File is null
                        return RedirectToAction("Create", "News", new { id });  //Error
                    }
                    _context.Add(news);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "News", new { id });
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }

            return RedirectToAction("Create", "News", new { id });
        }

        // GET: News/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.News == null)
            {
                return NotFound();
            }

            var news = await _context.News
                .Include(n => n.SportClub)
                .FirstOrDefaultAsync(m => m.Id.Equals(id));
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        // POST: News/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, [Bind("Id, SportClubId, FileName")] News news)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var sportclub = await _context.SportClubs.FindAsync(news.SportClubId);

                    if (news != null)
                    {
                        var containerName = sportclub.Title;
                        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName.ToLower());

                        if (containerClient != null)
                        {
                            var blobClient = containerClient.GetBlobClient(news.FileName);
                            await blobClient.DeleteIfExistsAsync();
                        }
                        _context.Remove(news);
                        await _context.SaveChangesAsync();
                    }

                    return RedirectToAction("Index", "News", new { sportclub.Id });
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }

            return RedirectToAction("Delete", "News", new { id });
        }
    }
}