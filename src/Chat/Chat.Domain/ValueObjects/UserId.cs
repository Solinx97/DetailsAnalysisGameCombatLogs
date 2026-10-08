namespace Chat.Domain.ValueObjects;

public record UserId
{
    public UserId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static implicit operator Guid(UserId id) => id.Value;


    public static implicit operator UserId(Guid value) => new(value);
}
