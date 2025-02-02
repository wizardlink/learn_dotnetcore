using Microsoft.AspNetCore.Mvc;

namespace AssigmentControllers.Controllers
{
    public class Account
    {
        public int accountNumber { get; set; }
        public string? accountHolderName { get; set; }
        public int currentBalance { get; set; }
    }

    public class HomeController : Controller
    {
        private Account accountData = new Account
        {
            accountNumber = 1001,
            accountHolderName = "Example Name",
            currentBalance = 5000,
        };

        [Route("/")]
        public IActionResult Index() => Content("Welcome to the Best Bank");

        [Route("account-details")]
        public IActionResult AccountDetails() => Json(accountData);

        [Route("account-statement")]
        public IActionResult AccountStatement() => File("document.pdf", "application/pdf");

        [Route("get-current-balance/{accountNumber:int}")]
        public IActionResult GetCurrentBalance()
        {
            int accountNumber = Convert.ToInt32(Request.RouteValues["accountNumber"]);

            if (accountNumber != 1001)
            {
                return BadRequest("Account Number should be 1001");
            }

            return Content(accountData.currentBalance.ToString());
        }

        [Route("get-current-balance")]
        public IActionResult GetCurrentBalanceRoot() => BadRequest("Account Number should be supplied");
    }
}
