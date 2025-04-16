using VideoCourse.Backend.Shared.Domain.Entities;

namespace VideoCourse.Backend.Domain.Entities;

// Payment - Ödeme bilgileri
public class Payment : BaseEntity
{
    public int UserId { get; set; }
    public decimal Amount { get; set; }
    public string TransactionId { get; set; } = string.Empty;
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
    public string PaymentMethod { get; set; } = string.Empty;
    public bool IsSuccessful { get; set; }

    // Navigation property
    public User User { get; set; }
}