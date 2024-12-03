using System.ComponentModel.DataAnnotations;
namespace HTQL_DichVuYTeGiaDinh.Repository.Validation
{
    public class FileExtensionAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName);
                string[] extensions = { "jpg", "png", "jpeg" };
                bool result = extension.Any(x => extension.EndsWith(x));
                if (!result)
                {
                    return new ValidationResult("Hình ảnh phải là kiểu png hoặc jpg hoặc jpeg");
                }
            }
            return ValidationResult.Success;
        }
    }
}
