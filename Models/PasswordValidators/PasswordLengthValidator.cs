using System.Threading.Tasks;
using Caffe.Models.MongoModels;
using Microsoft.AspNetCore.Identity;

namespace Caffe.Models.PasswordValidators
{
    public class PasswordLengthValidator : IPasswordValidator<MongoUser>

    {
        public async Task<IdentityResult> ValidateAsync(UserManager<MongoUser> manager, MongoUser user, string password)
        {
            if (password.Length < 8)
                return IdentityResult.Failed(new IdentityError()
                {
                    Code = "PasswordTooShort",
                    Description = "Пароль должен быть не менее 8 символов"
                });
            return IdentityResult.Success;
        }
    }
}