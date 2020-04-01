using System;
using System.Collections.Generic;
using System.Text;

namespace LightTown.Web.Services.Validation
{
    public interface IValidationService
    {
        bool ValidateLoginInput(string username, string password);
        bool ValidateRegisterInput(string username, string password, string passwordConfirm, string email);
    }
}
