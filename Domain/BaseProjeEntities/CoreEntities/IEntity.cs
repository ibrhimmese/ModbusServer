namespace Domain.BaseProjeEntities.CoreEntities;

public interface IEntity<T>
{
    T Id { get; set; }
}



public interface IEntityTimestamps
{
    DateTime CreatedDate { get; set; }
    DateTime? UpdatedDate { get; set; }
    DateTime? DeletedDate { get; set; }
}


public class Entity<TId> : IEntity<TId>, IEntityTimestamps
{
    public TId Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public DateTime? DeletedDate { get; set; }

    public Entity()
    {
        Id = default!;
    }
    public Entity(TId id)
    {
        Id = id;
    }
}

