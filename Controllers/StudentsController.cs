using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Models;

namespace ContosoUniversity.Controllers
{
    //tinfo200:[2021-03-11-costarec-dykstra2] --  Create the StudentController and pass in the SchoolContext in the ctor.
    //The SchoolContext is passed by the dependency injection.
    public class StudentsController : Controller
    {
        private readonly SchoolContext _context;

        public StudentsController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Students
        //tinfo200:[2021-03-11-costarec-dykstra2] --  This method, using the keyword async, await, and dataType Task<> allows the web application
        //to run asynchronuously, meaning a thread relinquish the CPU to another thread while it does I/O
        public async Task<IActionResult> Index()
        {
            return View(await _context.Students.ToListAsync());
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // tinfo200:[2021-03-11-costarec-dykstra2] -- This code loads an instance of the Entity Student and include the Enrollment information.
            // It also calls the AsNoTracking method to desable syschronization between the context and the database since no change is expected during
            // the life time of the context.
            var student = await _context.Students
                .Include(s => s.Enrollments)
                    .ThenInclude(e => e.Course)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        //tinfo200:[2021-03-11-costarec-dykstra2] -- This code creates  new Student Entity and implements security check against overposting attack
        // and error handling with a try-catch block
        public async Task<IActionResult> Create([Bind("LastName,FirstMidName,EnrollmentDate")] Student student)
        {
            //tinfo200:[2021-03-11-costarec-dykstra2] -- Error handling with a try() catch(). And print message to the user in case of faillure/error
            try
            {
                //tinfo200:[2021-03-11-costarec-dykstra2] -- The code inside the if statement checks the value passed by the user to 
                //make sure no unexpected value was entered, which could be the case if we only wanted the user to modify a defined
                //number of property inside the Student object. It's also a protection again the so-called "overposting"
                if (ModelState.IsValid)
                {
                    _context.Add(student);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }

            return View(student);
        }

        // GET: Students/Edit/5  
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // tinfo200:[2021-03-11-costarec-dykstra2] -- This method allows us to only modify/update part of properties without altering other data in the Entity.
        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            // tinfo200:[2021-03-11-costarec-dykstra2] -- Use the FirstOrDefault method to get the student we want to modify using the PK id
            var studentToUpdate = await _context.Students.FirstOrDefaultAsync(s => s.ID == id);

            // tinfo200:[2021-03-11-costarec-dykstra2] -- Updates only the properties we want, therefore securing the applicaton against overposting
            if (await TryUpdateModelAsync<Student>(
                studentToUpdate,
                "",
                s => s.FirstMidName, s => s.LastName, s => s.EnrollmentDate))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            }
            return View(studentToUpdate);
        }

        // GET: Students/Delete/5
        // tinfo200:[2021-03-11-costarec-dykstra2] -- The method displays the delete menu to the user giving them the chance to delete or remove.
        // If the passage an integer (id) is passed into the method, it simply displays the delete menu and wait for the user input.
        // If both id and saveChanegesError are passed to into the method, an error message informing of the failure of the former attempt to
        // delete is shown on the screen along with the same menu letting the user decide whether they want to try again.
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students

                // tinfo200:[2021-03-11-costarec-dykstra2] -- Call the method AsNoTracking() because the context instance won't require an update for GET request
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            if (student == null)
            {
                return NotFound();
            }

            // tinfo200:[2021-03-11-costarec-dykstra2] -- Add an error message to the delete menu 
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            return View(student);
        }

        // POST: Students/Delete/5
        // tinfo200:[2021-03-11-costarec-dykstra2] -- This method gets called after the user click on "Delete" and send a POST request to remove an Student Entity.
        // More error handling code was inserted into this method to catch null pointer exception and any other external error/exception
        // 
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);

            // tinfo200:[2021-03-11-costarec-dykstra2] -- Check for null value/ Entity student return by FindAsync()
            if (student == null)
            {
                return RedirectToAction(nameof(Index));
            }

            // tinfo200:[2021-03-11-costarec-dykstra2] -- Catch all potential error using the error type "DbUpdateException", but also resend 
            // GET request to the user, hence displaying the delete menu back to the user but this time with the error message indicated a previous faillure
            // delete an Student Entity. This is accomplished by assigning "saveChangesError = true"
            try
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.ID == id);
        }
    }
}
