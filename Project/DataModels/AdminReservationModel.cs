public class AdminReservationModel
{
    public long ReservationId { get; set; }
    public string Date { get; set; } = string.Empty;
    public string TimeSlot { get; set; } = string.Empty;
    public long PartySize { get; set; }
    public long UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
