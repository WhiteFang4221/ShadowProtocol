public interface IDamageSource
{
    public int Damage { get; }
    
    public IHealth Owner { get; }
}