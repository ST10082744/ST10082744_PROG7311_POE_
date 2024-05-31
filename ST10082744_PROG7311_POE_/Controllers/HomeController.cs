using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ST10082744_PROG7311_POE_.Areas.Identity.Data;
using ST10082744_PROG7311_POE_.Data;
using ST10082744_PROG7311_POE_.Models;
using System.Diagnostics;

namespace ST10082744_PROG7311_POE_.Controllers
{
    /// <summary>
    /// user has to be authorise to see home page , blocks not registerd users
    /// </summary>
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ST10082744_PROG7311_POE_Context _context;
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ST10082744_PROG7311_POE_User> _userManager;
//=================================================================================================================================================//
        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userManager"></param>
        /// <param name="logger"></param>
        public HomeController(ST10082744_PROG7311_POE_Context context, UserManager<ST10082744_PROG7311_POE_User> userManager, ILogger<HomeController> logger)
        {
            _context = context; 
            _logger = logger;
            _userManager = userManager;
        }
//=================================================================================================================================================//
        public IActionResult Index()
        {
            return View();
        }
//=================================================================================================================================================//
        public IActionResult AddProduct()
        {
            return View();
        }
 //=================================================================================================================================================//     
        public IActionResult Privacy()
        {
            return View();
        }
//=================================================================================================================================================//
       /// <summary>
       /// product list for products to spesific farmer with added sort function by date or category/ sort options for bot employee and farmer
       /// </summary>
       /// <param name="userId"></param>
       /// <param name="sortOrder"></param>
       /// <param name="startDate"></param>
       /// <param name="endDate"></param>
       /// <param name="category"></param>
       /// <returns></returns>
        public async Task<IActionResult> ProductList(string userId, string sortOrder, DateTime? startDate, DateTime? endDate, string category)
        {
            ViewData["ProductTypeSortParm"] = String.IsNullOrEmpty(sortOrder) ? "type_desc" : "";

            var currentUser = await _userManager.GetUserAsync(User);
            IQueryable<Product> products;
            //employee
            if (User.IsInRole("Employee"))
            {
                if (!string.IsNullOrEmpty(userId))
                {
                    products = _context.Products.Where(p => p.UserId == userId);
                }
                else
                {
                    products = _context.Products;
                }
            }
            else //farmer
            {
                products = _context.Products.Where(p => p.UserId == currentUser.Id);
            }

            if (startDate.HasValue && endDate.HasValue)
            {
                products = products.Where(p => p.ProductionDate >= startDate && p.ProductionDate <= endDate);
            }

            if (!string.IsNullOrEmpty(category))
            {
                products = products.Where(p => p.Category.Contains(category));
            }
            //acending and descending
            switch (sortOrder)
            {
                case "type_desc":
                    products = products.OrderByDescending(p => p.Category);
                    break;
                default:
                    products = products.OrderBy(p => p.Category);
                    break;
            }

            return View(await products.ToListAsync());
        }
//=================================================================================================================================================//
/// <summary>
/// metho to save product to database for the current logged in user/ with log statements to see if errors occur
/// </summary>
/// <param name="product"></param>
/// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SaveProduct(Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                  
                    _logger.LogInformation("ModelState is valid.");

                    var currentUser = await _userManager.GetUserAsync(User);
                    //assign current product to logged in user
                    product.UserId = currentUser.Id; 
                    //adding product to dbset
                    _context.Products.Add(product); 
                    await _context.SaveChangesAsync(); 

                    
                    return RedirectToAction("ProductList");
                }
                else
                {
                   
                    _logger.LogInformation("ModelState is invalid. Errors:");
                    foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                    {
                        _logger.LogInformation($"- {error.ErrorMessage}");
                    }
                }

               
                return View("AddProduct", product);
            }
            catch (Exception ex)
            {
                
                _logger.LogError(ex, "An error occurred while saving the product.");
                return RedirectToAction("Error");
            }
        }
//=================================================================================================================================================//
/// <summary>
/// edit product information
/// </summary>
/// <param name="id"></param>
/// <returns></returns>
        public IActionResult EditProduct(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
//=================================================================================================================================================//
       /// <summary>
       /// update product information
       /// </summary>
       /// <param name="product"></param>
       /// <returns></returns>
        public async Task<IActionResult> UpdateProduct(Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var currentUser = await _userManager.GetUserAsync(User);

                   //get product from database
                    var existingProduct = await _context.Products.FindAsync(product.Id);

                    if (existingProduct == null || existingProduct.UserId != currentUser.Id)
                    {
                       
                        return NotFound();
                    }

                    //update the product properties
                    existingProduct.ProductName = product.ProductName;
                    existingProduct.Category = product.Category;
                    existingProduct.ProductionDate = product.ProductionDate;

                   
                    await _context.SaveChangesAsync();

                   
                    return RedirectToAction("ProductList");
                }

               
                return View("EditProduct", product);
            }
            catch (Exception ex)
            {
               
                _logger.LogError(ex, "An error occurred while updating the product.");
                return RedirectToAction("Error");
            }
        }
//=================================================================================================================================================//

        /// <summary>
        /// method to delete product from database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult DeleteProduct(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            _context.Products.Remove(product);
            _context.SaveChanges();
            return RedirectToAction("ProductList");
        }
//=================================================================================================================================================//
        /// <summary>
        /// method to acces employe zone if emails ends with @agri.co.za
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Employee")]
        public IActionResult EmployeeZone()
        {
            var nonAgriUsers = _context.Users.Where(u => !u.Email.EndsWith("@agri.co.za")).ToList();
            return View(nonAgriUsers);
        }
//=================================================================================================================================================//
        /// <summary>
        /// method to edit famer profile
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IActionResult EditProfile(string userId)
        {
            var user = _context.Users.Find(userId);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
//=================================================================================================================================================//
        /// <summary>
        /// method to save farmer profile
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        
        [HttpPost]
        public async Task<IActionResult> EditProfile(ST10082744_PROG7311_POE_User user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _context.Users.FindAsync(user.Id);
                if (existingUser == null)
                {
                    return NotFound();
                }

                existingUser.Name = user.Name;
                existingUser.Surname = user.Surname;
                existingUser.PhoneNumber = user.PhoneNumber;

                _context.Users.Update(existingUser);
                await _context.SaveChangesAsync();

                return RedirectToAction("EmployeeZone");
            }
            return View(user);
        }
//=================================================================================================================================================//
       /// <summary>
       /// method to add user/farmer to database
       /// </summary>
       /// <param name="email"></param>
       /// <param name="password"></param>
       /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddUser(string email, string password)
        {
            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
            {
                var newUser = new ST10082744_PROG7311_POE_User { UserName = email, Email = email };
                var result = await _userManager.CreateAsync(newUser, password);

                if (result.Succeeded)
                {                  
                    return RedirectToAction("EmployeeZone");
                }
                else
                {                
                    ModelState.AddModelError(string.Empty, "Failed to create user.");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Email and password are required.");
            }

            
            var users = _context.Users.ToList();
            return View(users);
        }
//=================================================================================================================================================//
       /// <summary>
       /// method to delete farmer frm database
       /// </summary>
       /// <param name="userId"></param>
       /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("EmployeeZone");
        }
//=================================================================================================================================================//
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
//======================================================================END OF FILE====================================================================//