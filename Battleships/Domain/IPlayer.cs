namespace Battleships.Domain
{
    public interface IPlayer
    {
        string Name { get; }
        IField Field { get; }
    }
}
