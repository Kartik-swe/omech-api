namespace omech.Models
{
    /// <summary>
    /// Login request model containing username and password used to obtain a JWT token.
    /// </summary>
    public class LoginRequests
    {
        /// <summary>
        /// Username of the user.
        /// </summary>
        public string Username { get; set; }  // Ensure this matches your API request

        /// <summary>
        /// Plain-text password. Transport must be secured (HTTPS).
        /// </summary>
        public string Password { get; set; }
    }
}
