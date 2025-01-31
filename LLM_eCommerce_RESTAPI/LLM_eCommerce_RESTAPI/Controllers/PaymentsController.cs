using LLM_eCommerce_RESTAPI.AuthModels;
using LLM_eCommerce_RESTAPI.Models;
using LLM_eCommerce_RESTAPI.Services;
using LLM_eCommerce_RESTAPI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Linq;

namespace LLM_eCommerce_RESTAPI.Controllers
{
    /// <summary>
    /// A summary about PaymentsController class.
    /// </summary>
    /// <remarks>
    /// PaymentsController has the following end points:
    /// Get all Payments
    /// Get Payments with id
    /// Get Payments with method
    /// Get Payments with date
    /// Get Payments between dates
    /// Put (update) Payment with id and Payment object
    /// Post (Add) Payment using a Payments View Model 
    /// Delete Payment with id
    /// </remarks>
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly LLM_eCommerce_EFDBContext _context;
        private UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AuthenticationContext _authenticationContext;
        private readonly IdentityHelper _identityHelper;
        public PaymentsController(LLM_eCommerce_EFDBContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, AuthenticationContext authenticationContext)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _authenticationContext = authenticationContext;
            _identityHelper = new IdentityHelper(_userManager, _authenticationContext, _roleManager);
        }


        // GET: api/Payments        
        [EnableCors("AllowOrigin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPayments()
        {

            var paymentDB = await _context.Payments.ToListAsync();

            return Ok(paymentDB);
        }

        // GET: api/Payments/5
        [EnableCors("AllowOrigin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Payment>> GetPayments(int id)
        {
            List<Payment> allPayments = new List<Payment>();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var payments = await _context.Payments.FindAsync(id);

            if (payments == null)
            {
                return NotFound(new { message = "No Payment with that ID exists, please try again" });
            }
            else
            {
                var payOrderId = _context.Payments.FirstOrDefault(x => x.PaymentId == id).OrderId;
                payments.Order = _context.Orders.FirstOrDefault(o => o.OrderId == payOrderId);
            }

            return Ok(payments);
        }

        // GET: api/Products/specificPayment/method
        [EnableCors("AllowOrigin")]
        [HttpGet("specificMethod/{method}")]
        public async Task<ActionResult<List<Payment>>> GetPaymentByMethod(string method)
        {
            List<Payment> payments = new List<Payment>();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var paymentsQuery = _context.Payments.Where(x => x.PaymentMethod == method);
            if (paymentsQuery.Count() == 0)
            {
                return NotFound(new { message = "No Payment with that method exists, please try again" });
            }
            var item = paymentsQuery;
            foreach (var paymentItem in item)
            {
                int id = paymentItem.PaymentId;


                if (paymentItem == null)
                {

                    return NotFound(new { message = "No Payment with that method exists, please try again" });
                }
                else
                {
                    var payOrderId = _context.Payments.FirstOrDefault(x => x.PaymentId == id).OrderId;
                    paymentItem.Order = _context.Orders.FirstOrDefault(o => o.OrderId == payOrderId);
                }

                payments.Add(paymentItem);
            }
            return Ok(payments);
        }

        // GET: api/Payments/SpecificDate/date
        [EnableCors("AllowOrigin")]
        [HttpGet("SpecificDateASyyyy-mm-dd/{date}")]
        public async Task<ActionResult<List<Payment>>> GetPaymentByDate(DateTime date)
        {
            List<Payment> payments = new List<Payment>();
            DateTime dateOutput;
            bool valid = DateTime.TryParse(date.ToShortDateString(), CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None, out dateOutput);
            if (!valid)
            {
                return BadRequest("Error the format of the date is incorrect");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            List<Payment> temPayments = _context.Payments.ToList();
            var paymentsQuery = temPayments.Where(x => x.PaymentDate.Date == dateOutput.Date);
            if (paymentsQuery.Count() == 0)
            {
                return NotFound(new { message = "No Payment with that date exists, please try again" });
            }
            var item = paymentsQuery;
            foreach (var paymentItem in item)
            {
                int id = paymentItem.PaymentId;


                if (paymentItem == null)
                {

                    return NotFound(new { message = "No Payment with that date exists, please try again" });
                }
                else
                {
                    var payOrderId = _context.Payments.FirstOrDefault(x => x.PaymentId == id).OrderId;
                    paymentItem.Order = _context.Orders.FirstOrDefault(o => o.OrderId == payOrderId);
                }

                payments.Add(paymentItem);
            }
            return Ok(payments);
        }

        // GET: api/Payments/BetweenDates/date1/date2
        [EnableCors("AllowOrigin")]
        [HttpGet("BetweenDatesBothASyyyy-mm-dd/{{date1}}/{{date2}}")]
        public async Task<ActionResult<List<Payment>>> GetPaymentByBetweenDates(DateTime date1, DateTime date2)
        {
            List<Payment> payments = new List<Payment>();
            DateTime date1Output;
            bool valid = DateTime.TryParse(date1.ToShortDateString(), CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None, out date1Output);
            if (!valid)
            {
                return BadRequest("Error the format of the date is incorrect");
            }

            DateTime date2Output;
            bool validDate2 = DateTime.TryParse(date2.ToShortDateString(), CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None, out date2Output);
            if (!validDate2)
            {
                return BadRequest("Error the format of the date is incorrect");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            List<Payment> temPayments = _context.Payments.ToList();
            var paymentsQuery = temPayments.Where(x => x.PaymentDate.Date >= date1Output.Date && x.PaymentDate <= date2Output);
            if (paymentsQuery.Count() == 0)
            {
                return NotFound(new { message = "No Payment with that date range exists, please try again" });
            }
            var item = paymentsQuery;
            foreach (var paymentItem in item)
            {
                int id = paymentItem.PaymentId;


                if (paymentItem == null)
                {

                    return NotFound(new { message = "No Payment with that date exists, please try again" });
                }
                else
                {
                    var payOrderId = _context.Payments.FirstOrDefault(x => x.PaymentId == id).OrderId;
                    paymentItem.Order = _context.Orders.FirstOrDefault(o => o.OrderId == payOrderId);
                }

                payments.Add(paymentItem);
            }
            return Ok(payments);
        }

        // PUT: api/Payments/5
        [EnableCors("AllowOrigin")]
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutPayments(int id, PaymentsVM payment)
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            bool userSuperUserAuthorised = await _identityHelper.IsSuperUserRole(userId);
            if (!userSuperUserAuthorised)
            {
                return BadRequest(new { message = "Not authorised to update payments" });
            }


            int currentPaymentId = 0;

            try
            {
                Payment updatePayment = _context.Payments.FirstOrDefault(o => o.PaymentId == id);
                int count = 0;
                if (updatePayment == null)
                {
                    return NotFound(new { message = "No Payment with that ID exists, please try again" });
                }
                if (payment.PaymentMethod != "" || payment.PaymentMethod != null)
                {
                    if (updatePayment.PaymentMethod != payment.PaymentMethod)
                    {
                        updatePayment.PaymentMethod = payment.PaymentMethod;
                        count++;
                    }
                }

                if (payment.OrderId != 0)
                {
                    if (updatePayment.OrderId != payment.OrderId)
                    {
                        updatePayment.OrderId = payment.OrderId;
                        count++;
                    }
                }

                if (payment.Amount != 0)
                {
                    if (updatePayment.Amount != payment.Amount)
                    {
                        updatePayment.Amount = payment.Amount;
                        count++;
                    }
                }

                if (payment.Status != "" || payment.Status != null)
                {
                    if (updatePayment.Status != payment.Status)
                    {
                        updatePayment.Status = payment.Status;
                        count++;
                    }
                }

                if (count > 0)
                {
                    updatePayment.PaymentDate = DateTime.Now;
                    await _context.SaveChangesAsync();
                    currentPaymentId = id;
                }
                else
                {
                    return Ok(new { message = "no updates made" });
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentsExists(id))
                {
                    return NotFound(new { message = "Payment Id not found, no changes made, please try again" });
                }
                else
                {
                    throw;
                }
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Error, " + e.Message });
            }

            return Ok(new { message = "Payment Updated - PaymentId:" + currentPaymentId });
        }

        // POST: api/Payments
        [EnableCors("AllowOrigin")]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Payment>> PostPayments(PaymentsVM payment)
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            bool userSuperUserAuthorised = await _identityHelper.IsSuperUserRole(userId);
            bool userSellerAuthorised = await _identityHelper.IsSellerUserRole(userId);
            if (userSuperUserAuthorised)
            {

                return BadRequest(new { message = "Not authorised to add payments - Only Customers are allowed" });
            }

            if (userSellerAuthorised)
            {
                return BadRequest(new { message = "Not authorised to add payments - Only Customers are allowed" });
            }

            if (payment.OrderId == 0 || payment.Status == null || payment.Amount == 0 || payment.Status == "")
            {
                return BadRequest(new { message = "Cannot Add an empty payment, please you enter a valid payment" });
            }

            int currentPaymentId = 0;

            try
            {
                var newPayment = new Payment();
                newPayment.PaymentMethod = payment.PaymentMethod;
                if (_context.Orders.Where(c => c.OrderId == payment.OrderId).Count() == 0)
                {
                    return BadRequest(new { message = "That order does not exist please choose OrderId included in the list below", _context.Orders });
                }
                newPayment.OrderId = payment.OrderId;
                newPayment.Amount = payment.Amount;
                newPayment.Status = payment.Status;
                newPayment.PaymentDate = DateTime.Now;
                newPayment.Order = _context.Orders.FirstOrDefault(o => o.OrderId == payment.OrderId);
                _context.Payments.Add(newPayment);
                await _context.SaveChangesAsync();
                currentPaymentId = newPayment.PaymentId;
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Error in adding Payment, please try again" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Error in adding Payment, " + e.Message });
            }

            return Ok("New Payment Created - PaymentId:" + currentPaymentId);
        }

        // DELETE: api/Payments/5
        [EnableCors("AllowOrigin")]
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<Payment>> DeletePayments(int id)
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            bool userSuperUserAuthorised = await _identityHelper.IsSuperUserRole(userId);
            if (!userSuperUserAuthorised)
            {
                return BadRequest(new { message = "Not authorised to delete payments" });
            }

            var payments = await _context.Payments.FindAsync(id);
            if (payments == null)
            {
                return NotFound(new { message = "Payment ID not found, please try again" });
            }

            try
            {

                _context.Payments.Remove(payments);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Error in deleting Payment, please try again" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Error, " + e.Message });
            }
            return payments;
        }

        private bool PaymentsExists(int id)
        {
            return _context.Payments.Any(e => e.PaymentId == id);
        }
    }
}
