using System.ComponentModel.DataAnnotations;

namespace ams.Entities.Common;

public abstract class BaseEntity
{
    protected BaseEntity()
    {
        Id = Ulid.NewUlid();
    }

    [Key] [StringLength(80)] public Ulid Id { get; set; }


    public override bool Equals(object? obj)
    {
        var other = obj as BaseEntity;
        if (ReferenceEquals(other, null))
            return false;
        if (ReferenceEquals(this, other))
            return true;
        if (GetType() != other.GetType())
            return false;
        if (Id == default || other.Id == default)
            return false;

        return Id == other.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}