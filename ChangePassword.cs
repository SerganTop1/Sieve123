namespace SieveApp.Requests
{
    public class ChangePasswordRequest
    {
        public int UserId { get; set; }
        public required string OldPassword { get; set; }
        public required string NewPassword { get; set; }
    }
}

