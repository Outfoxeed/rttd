namespace RTTD;

public interface IEntityComponent
{
    public bool Enabled { get; set; }
    
    Entity GetEntity();
}