namespace LightTown.Client.Services.Validation
{
    public class ValidationService : IValidationService
    {
        public bool ValidateLoginInput(string username, string password)
        {
            if (string.IsNullOrEmpty(username))
                return false;
            if (string.IsNullOrEmpty(password))
                return false;

            return true;
        }

        public bool ValidateProjectCreationInput(string projectName, string description)
        {
            if (string.IsNullOrEmpty(projectName))
                return false;
            if (string.IsNullOrEmpty(description))
                return false;

            return true;
        }

        public bool ValidateRegisterInput(string username, string password, string passwordConfirm, string email)
        {
            if (string.IsNullOrEmpty(username))
                return false;
            if (string.IsNullOrEmpty(password))
                return false;
            if (string.IsNullOrEmpty(passwordConfirm))
                return false;
            if (string.IsNullOrEmpty(email))
                return false;
            if (password != passwordConfirm)
                return false;

            return true;
        }

    }
}
