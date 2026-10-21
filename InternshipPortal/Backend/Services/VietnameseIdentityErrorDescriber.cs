using Microsoft.AspNetCore.Identity;

namespace InternshipPortal.Services;

public class VietnameseIdentityErrorDescriber : IdentityErrorDescriber
{
    private static IdentityError Error(string code, string description) => new() { Code = code, Description = description };
    public override IdentityError DefaultError() => Error(nameof(DefaultError), "Không thể xử lý tài khoản. Vui lòng thử lại.");
    public override IdentityError DuplicateEmail(string email) => Error(nameof(DuplicateEmail), "Email này đã được sử dụng.");
    public override IdentityError DuplicateUserName(string userName) => Error(nameof(DuplicateUserName), "Email này đã được sử dụng.");
    public override IdentityError InvalidEmail(string? email) => Error(nameof(InvalidEmail), "Email chưa đúng định dạng.");
    public override IdentityError InvalidUserName(string? userName) => Error(nameof(InvalidUserName), "Email chứa ký tự chưa hợp lệ.");
    public override IdentityError PasswordTooShort(int length) => Error(nameof(PasswordTooShort), $"Mật khẩu cần ít nhất {length} ký tự.");
    public override IdentityError PasswordRequiresDigit() => Error(nameof(PasswordRequiresDigit), "Mật khẩu cần ít nhất một chữ số.");
    public override IdentityError PasswordRequiresLower() => Error(nameof(PasswordRequiresLower), "Mật khẩu cần ít nhất một chữ thường.");
    public override IdentityError PasswordRequiresUpper() => Error(nameof(PasswordRequiresUpper), "Mật khẩu cần ít nhất một chữ hoa.");
    public override IdentityError PasswordRequiresNonAlphanumeric() => Error(nameof(PasswordRequiresNonAlphanumeric), "Mật khẩu cần ít nhất một ký tự đặc biệt.");
    public override IdentityError PasswordRequiresUniqueChars(int uniqueChars) => Error(nameof(PasswordRequiresUniqueChars), $"Mật khẩu cần ít nhất {uniqueChars} ký tự khác nhau.");
    public override IdentityError ConcurrencyFailure() => Error(nameof(ConcurrencyFailure), "Thông tin đã thay đổi. Vui lòng tải lại trang và thử lại.");
}
