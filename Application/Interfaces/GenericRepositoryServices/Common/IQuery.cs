namespace Application.GenericRepositoryFiles.Common;

public interface IQuery<T>
{
    IQueryable<T> Query();
}
