using HTQL_DichVuYTeGiaDinh.Repository;

namespace HTQL_DichVuYTeGiaDinh.Middlewares
{
    public class UserInfoMiddleware
    {
        private readonly RequestDelegate _next;

        public UserInfoMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, DataContext dataContext)
        {
            try
            {
                if (!context.Session.IsAvailable)
                {
                    context.Items["Avatar"] = "default-avatar.png";
                    context.Items["UserName"] = "Khách";
                }
                else
                {
                    var maTk = context.Session.GetInt32("MaTk"); // Lấy mã tài khoản từ Session

                    if (maTk.HasValue)
                    {
                        // Lấy thông tin tài khoản từ CSDL
                        var taiKhoan = dataContext.TaiKhoans.FirstOrDefault(t => t.MaTk == maTk.Value);

                        if (taiKhoan != null)
                        {
                            // Thêm thông tin tài khoản vào HttpContext.Items để layout sử dụng
                            context.Items["Avatar"] = !string.IsNullOrEmpty(taiKhoan.HinhAnh) ? taiKhoan.HinhAnh : "default-avatar.png";
                            context.Items["UserName"] = taiKhoan.HoTen;
                        }
                        else
                        {
                            // Nếu không tìm thấy tài khoản trong CSDL
                            context.Items["Avatar"] = "default-avatar.png";
                            context.Items["UserName"] = "Khách";
                            context.Session.Clear(); // Xóa session không hợp lệ
                        }
                    }
                    else
                    {
                        // Nếu không có session
                        context.Items["Avatar"] = "default-avatar.png";
                        context.Items["UserName"] = "Khách";
                    }
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và gán giá trị mặc định
                context.Items["Avatar"] = "default-avatar.png";
                context.Items["UserName"] = "Khách";
                Console.WriteLine($"Error in UserInfoMiddleware: {ex.Message}");
            }

            // Chuyển tiếp sang middleware tiếp theo
            await _next(context);
        }
    }

}
