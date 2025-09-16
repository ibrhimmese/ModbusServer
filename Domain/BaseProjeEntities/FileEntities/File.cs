using Domain.BaseProjeEntities.CoreEntities;

namespace Domain.BaseProjeEntities.FileEntities;

public class File : Entity<Guid>
{
    public string FileName { get; set; }
    public string Path { get; set; }
    public string Storage { get; set; }

}
