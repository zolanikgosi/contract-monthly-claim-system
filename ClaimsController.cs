using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Contract_Monthly_Claim_System_POE.Models;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Contract_Monthly_Claim_System_POE.Controllers
{
    public class ClaimsController : Controller
    {
        private static List<Lecturer> lecturers = new List<Lecturer>
        {
            new Lecturer { LecturerID = 1, FirstName = "Kgosi", LastName = "Makuva", Email = "kgosi.makuva@example.com", HourlyRate = 50 },
            new Lecturer { LecturerID = 1, FirstName = "Zolani", LastName = "Makuva", Email = "Zolani.makuva@example.com", HourlyRate = 50 },
             new Lecturer { LecturerID = 1, FirstName = "Keaton", LastName = "Makuva", Email = "keaton.makuva@example.com", HourlyRate = 50 },
        };

        private static List<Claim> claims = new List<Claim>();

        // GET: Show claim submission form
        public IActionResult SubmitClaim()
        {
            ViewBag.Lecturers = lecturers;
            return View();
        }

        // POST: Handle claim submission, including file upload
        [HttpPost]
        public IActionResult SubmitClaim(int lecturerID, int hoursWorked, decimal hourlyRate, string notes, IFormFile supportingDoc)
        {
            var lecturer = lecturers.FirstOrDefault(l => l.LecturerID == lecturerID);
            if (lecturer == null) return NotFound();

            string filePath = null;
            if (supportingDoc != null)
            {
                var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);

                filePath = Path.Combine(uploads, supportingDoc.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    supportingDoc.CopyTo(stream);
                }
            }

            var claim = new Claim
            {
                ClaimID = claims.Count + 1,
                LecturerID = lecturerID,
                Date = DateTime.Now,
                HoursWorked = hoursWorked,
                TotalAmount = hourlyRate * hoursWorked,
                Status = "Pending",
                Notes = notes,
                SupportingDocumentPath = filePath,  // Save document path to the claim
                Lecturer = lecturer  // Populate the Lecturer property
            };

            claims.Add(claim);
            return RedirectToAction("ClaimSubmitted");
        }

        public IActionResult ClaimSubmitted()
        {
            return View();
        }

        // Action to list claims
        public IActionResult ListClaims()
        {
            return View(claims);
        }

        // Action for verification (approve/reject claims)
        [HttpPost]
        public IActionResult VerifyClaim(int claimID, string action)
        {
            var claim = claims.FirstOrDefault(c => c.ClaimID == claimID);
            if (claim == null) return NotFound();

            if (action == "Approve")
                claim.Status = "Approved";
            else if (action == "Reject")
                claim.Status = "Rejected";

            return RedirectToAction("VerifyClaims");
        }

        public IActionResult VerifyClaims()
        {
            return View(claims);  // Send all claims to view
        }
    }
}
