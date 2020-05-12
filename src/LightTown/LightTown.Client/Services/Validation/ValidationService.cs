namespace LightTown.Client.Services.Validation
{
    public class ValidationService : IValidationService
    {

        /// <summary>
        /// Validates if <paramref name="username"/> and <paramref name="password"/> are not <see langword="null"/> or empty.
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <para></para>
        /// <returns>Returns <see langword="true"/> when not <see langword="null"/> or empty, otherwise returns <see langword="false"/>.</returns>
        /// </summary>
        public bool ValidateLoginInput(string username, string password)
        {
            if (string.IsNullOrEmpty(username))
                return false;
            if (string.IsNullOrEmpty(password))
                return false;

            return true;
        }

        /// <summary>
        /// Validates if <paramref name="projectName"/> and <paramref name="description"/> are not <see langword="null"/> or empty.
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <para></para>
        /// <returns>Returns <see langword="true"/> when not <see langword="null"/> or empty, otherwise returns <see langword="false"/>.</returns>
        /// </summary>
        public bool ValidateProjectCreationInput(string projectName, string description)
        {
            if (string.IsNullOrEmpty(projectName))
                return false;
            if (string.IsNullOrEmpty(description))
                return false;

            return true;
        }

        /// <summary>
        /// Validates if <paramref name="username"/>, <paramref name="password"/>, <paramref name="passwordConfirm"/> and <paramref name="email"/> are not <see langword="null"/> or empty.
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <para></para>
        /// <returns>Returns <see langword="true"/> when not <see langword="null"/> or empty, otherwise returns <see langword="false"/>.</returns>
        /// </summary>
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
