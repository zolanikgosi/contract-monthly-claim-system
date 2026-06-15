using Microsoft.AspNetCore.Mvc;

namespace ContractMClaimSys.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            // Render the login view
            return View();
        }

        [HttpPost]
        public IActionResult Index(string username, string email, string role, string password)
        {
            // You could add authentication logic here based on username, email, role, and password.
            // For now, we're redirecting to SubmitClaim regardless of validation.

            // Redirect to SubmitClaim
            return RedirectToAction("SubmitClaim", "Claims");
        }
    }
}