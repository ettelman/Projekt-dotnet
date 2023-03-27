using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Projekt_dotnet.Data;
using Projekt_dotnet.Models;

namespace Projekt_dotnet.Controllers
{
    public class CdsController : Controller
    {


        private readonly ApplicationDbContext _context;

        public CdsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Cds
        public async Task<IActionResult> Index()
        {
            return _context.Cd != null ?
                        View(await _context.Cd.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Cd'  is null.");
        }

        // GET: Cds/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Cd == null)
            {
                return NotFound();
            }

            var cd = await _context.Cd
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cd == null)
            {
                return NotFound();
            }

            return View(cd);
        }

        // GET: Cds/Create
        public IActionResult Create()
        {
            return View();
        }
        // GET: Cds/List
        public async Task<IActionResult> List(string searchString)
        {
            if (_context.Cd == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Cd' is null.");
            }
            // fetching from db
            var cds = from m in _context.Cd
                      select m;

            // checking search-string
            if (!String.IsNullOrEmpty(searchString))
            {
                cds = cds.Where(s => s.Name!.Contains(searchString));
                ViewData["Search"] = searchString;
            }

            return View(await cds.ToListAsync());
        }

        // POST: Cds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Category,Artist,Title")] Cd cd, IFormFile ImageFile)
        {
            
            
            if (ModelState.IsValid)
            {
                // adding image with IformFile
                // getting the path to wwwroot/images
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", ImageFile.FileName);
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                    stream.Close();
                }
                // adding the filename to the database
                cd.ImageName = ImageFile.FileName;
                _context.Add(cd);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cd);
        }

        // GET: Cds/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Cd == null)
            {
                return NotFound();
            }

            var cd = await _context.Cd.FindAsync(id);
            if (cd == null)
            {
                return NotFound();
            }
            return View(cd);
        }

        // POST: Cds/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Category,Artist,Title")] Cd cd, IFormFile ImageFile, string ImageName)
        {
            if (id != cd.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // imagename is a hidden field in the edit view. It contains the name of the image that is currently in the database.
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", ImageName);
                    if (System.IO.File.Exists(imagePath)) System.IO.File.Delete(imagePath);
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", ImageFile.FileName);
                    using (FileStream stream = new FileStream(path, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                        stream.Close();
                    }

                    cd.ImageName = ImageFile.FileName;

                    _context.Update(cd);
                    await _context.SaveChangesAsync();
                   

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CdExists(cd.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(List));
           } 
            return View(cd);
        }

        // GET: Cds/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Cd == null)
            {
                return NotFound();
            }

            var cd = await _context.Cd
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cd == null)
            {
                return NotFound();
            }
            return View(cd);
        }

        // POST: Cds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Cd == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Cd'  is null.");
            }
            var cd = await _context.Cd.FindAsync(id);
            if (cd != null)
            {
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", cd.ImageName); 
                System.IO.File.Delete(imagePath);
                _context.Cd.Remove(cd);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(List));
        }

        private bool CdExists(int id)
        {
          return (_context.Cd?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
