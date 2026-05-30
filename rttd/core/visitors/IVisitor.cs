namespace RTTD;

public interface IVisitor<T>
{
    bool CanVisit(T target);
    bool TryVisit(T target);
}