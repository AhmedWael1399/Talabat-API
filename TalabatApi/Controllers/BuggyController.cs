using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TalabatApi.Errors;
using Talabat.Repository.Data;

namespace TalabatApi.Controllers
{
    public class BuggyController : APIBaseController
    {
        private readonly TalabatDbContext _dbContext;

        public BuggyController(TalabatDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        #region Not Found

        [HttpGet("Not Found")]
        public ActionResult GetNotFoundRequest()
        {
            var Product = _dbContext.Products.Find(100);
            if (Product is null)
            {
                return NotFound(new ApiResponse(404));
            }

            return Ok(Product);
        }

        #endregion

        #region Server Error

        [HttpGet("Server Error")]
        public ActionResult GetServerErrorRequest()
        {
            var Product = _dbContext.Products.Find(100);
            var ProductToReturnDto = Product.ToString();

            return Ok(ProductToReturnDto);
        }

        #endregion

        #region Bad Request


        [HttpGet("Bad Request")]
        public ActionResult GetBadRequest()
        {
           return BadRequest(new ApiResponse(400));
        }

        #endregion

        #region Validation 


        [HttpGet("BadRequest/{id}")]
        public ActionResult GetValidationErrorRequest()
        {
            return Ok();
        }

        #endregion
    }
}
