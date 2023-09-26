using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using SchoolManagement.Models;

namespace SchoolManagement.Controllers
{
    [Authorize]
    public class ClassezController : Controller
    {
        private readonly SchoolManagementDbContext _context;

        public ClassezController(SchoolManagementDbContext context)
        {
            _context = context;
        }

        // GET: Classez
        public async Task<IActionResult> Index()
        {
            var schoolManagementDbContext = _context.Classezs.Include(c => c.Courses).Include(c => c.Lecturer);
            return View(await schoolManagementDbContext.ToListAsync());
        }

        // GET: Classez/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Classezs == null)
            {
                return NotFound();
            }

            var classez = await _context.Classezs
                .Include(c => c.Courses)
                .Include(c => c.Lecturer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (classez == null)
            {
                return NotFound();
            }

            return View(classez);
        }

        // GET: Classez/Create
        public IActionResult Create()
        {
           CreateSelectList();
            return View();
        }

        // POST: Classez/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LecturerId,CoursesId,Time")] Classez classez)
        {
            if (ModelState.IsValid)
            {
                _context.Add(classez);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            CreateSelectList();
            return View(classez);
        }

        // GET: Classez/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Classezs == null)
            {
                return NotFound();
            }

            var classez = await _context.Classezs.FindAsync(id);
            if (classez == null)
            {
                return NotFound();
            }
            CreateSelectList();
            return View(classez);
        }

        // POST: Classez/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LecturerId,CoursesId,Time")] Classez classez)
        {
            if (id != classez.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(classez);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClassezExists(classez.Id))
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
            CreateSelectList();
            return View(classez);
        }

        // GET: Classez/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Classezs == null)
            {
                return NotFound();
            }

            var classez = await _context.Classezs
                .Include(c => c.Courses)
                .Include(c => c.Lecturer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (classez == null)
            {
                return NotFound();
            }

            return View(classez);
        }

        // POST: Classez/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Classezs == null)
            {
                return Problem("Entity set 'SchoolManagementDbContext.Classezs'  is null.");
            }
            var classez = await _context.Classezs.FindAsync(id);
            if (classez != null)
            {
                _context.Classezs.Remove(classez);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<ActionResult> ManageEnrollments(int classId)
        {
            Console.WriteLine(classId);
            var classez= await _context.Classezs
            .Include(q=>q.Courses)
            .Include(q=>q.Lecturer)
            .Include(q=>q.Enrollments)
                .ThenInclude(q=>q.Student)
            .FirstOrDefaultAsync(m=>m.Id==classId);
            Console.WriteLine(classId);
            var students= await _context.Students.ToListAsync();

            var model=new ClassEnrollmentViewModel();
            model.Classez=new ClassViewModel{
                Id=classez.Id,
                CourseName=$"{@classez.Courses.Code} - {@classez.Courses.Name}",
                LecturerName=$"{@classez.Lecturer.FirstName} {@classez.Lecturer.Lastname}",
                Time=classez.Time.ToString()
            };

            foreach (var st in students)
            {
               model.Students.Add(new StudentEnrollmentViewModel
               {
                    Id=st.Id,
                    FirstName=st.FirstName,
                    LastName=st.LastName,
                    IsEnrolled=(classez?.Enrollments?.Any(q=>q.StudentId==st.Id))  
                        .GetValueOrDefault()
               });
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnrollStudent(int classId,int studentId,bool shouldEnroll)
        {
            var enrollment=new Enrollment();
            if(shouldEnroll)
            {
                enrollment.ClassId=classId;
                enrollment.StudentId=studentId;
                await _context.AddAsync(enrollment);
            }
            else
            {
                enrollment=await _context.Enrollments.FirstOrDefaultAsync(q=>q.ClassId==classId && q.StudentId==studentId);
                if(enrollment!=null)
                {
                    _context.Remove(enrollment);
                }
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ManageEnrollments),new {id=classId});
        }
        

        private bool ClassezExists(int id)
        {
          return (_context.Classezs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        private void CreateSelectList(){
            var course=_context.Courses.Select(q=>new 
            {
                CourseName=$"{q.Code}-{q.Name}({q.Credits} Credits)",
                Id=q.Id
            });
            ViewData["CoursesId"] = new SelectList(course, "Id", "CourseName");
            var lecturer=_context.Lecturers.Select(q=>new 
            {
                FullName=$"{q.FirstName}{q.Lastname}",
                Id=q.Id
            });
            
            ViewData["LecturerId"] = new SelectList(lecturer, "Id", "FullName");
        }
    }
}
