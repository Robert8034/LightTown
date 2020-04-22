namespace LightTown.Web.Services.Validation
{
    public interface IValidationService
    {
        bool ValidateLoginInput(string username, string password);
        bool ValidateRegisterInput(string username, string password, string passwordConfirm, string email);
        bool ValidateProjectCreationInput(string projectName, string description);
    }
}
