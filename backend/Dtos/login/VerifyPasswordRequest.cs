namespace backend.Dtos
{
    public class VerifyPasswordRequest
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
