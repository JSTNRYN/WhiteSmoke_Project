public class ReservationNoticeModel
{
    public long Id { get; set; }
    public long ReservationId { get; set; }
    public long MenuItemId { get; set; }
    public string Message { get; set; } = string.Empty;
    public string CreatedAt { get; set; } = string.Empty;
}
