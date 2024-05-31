using ST10082744_PROG7311_POE_.Areas.Identity.Data;

namespace ST10082744_PROG7311_POE_.Models
{
    public class Product
    {
        /// <summary>
        /// stores product id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// stores product name
        /// </summary>
        public string? ProductName { get; set; }
        /// <summary>
        /// stores the category of the product
        /// </summary>
        public string? Category { get; set; }
        /// <summary>
        /// stores the time of production
        /// </summary>
        public DateTime ProductionDate { get; set; }
//=================================================================================================================================================//
        /// <summary>
        /// stores user id/ used as foreigh key to get current logged in user 
        /// </summary>
        public string? UserId { get; set; }
        public ST10082744_PROG7311_POE_User? User { get; set; }
//=================================================================================================================================================//
    }
}
