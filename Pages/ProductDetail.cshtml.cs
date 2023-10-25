using System.Threading.Tasks;
using Gflower.Data.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gflower.Pages
{
    public class ProductDetailModel : PageModel
    {
        private readonly IProductRepository productRepository;

        public ProductDetailModel(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public Models.Product Product { get; set; }

        public async Task<IActionResult> OnGet(int productId)
        {
            // Lấy thông tin sản phẩm từ repository dựa vào productId
            Product = await productRepository.GetProduct(productId);

            // Kiểm tra xem sản phẩm có tồn tại hay không
            if (Product == null)
            {
                // Nếu sản phẩm không tồn tại, trả về trang "Not Found"
                return NotFound();
            }

            // Nếu sản phẩm tồn tại, trả về trang ProductDetail.cshtml và hiển thị thông tin sản phẩm
            return Page();
        }

        // Thêm phương thức xử lý POST để xóa sản phẩm
        public async Task<IActionResult> OnPostDelete(int productId)
        {
            // Gọi phương thức Delete từ repository để xóa sản phẩm dựa vào productId
            await productRepository.Delete(productId);

            // Đặt thông báo thành công, sẽ hiển thị ở trang ProductDetail.cshtml sau khi xóa
            TempData["SuccessMessage"] = "Xóa sản phẩm thành công.";

            // Chuyển hướng người dùng về trang ProductList.cshtml sau khi xóa thành công
            return RedirectToPage("./Product");
        }
    }
}
