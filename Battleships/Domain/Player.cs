namespace Battleships.Domain
{
    public class Player : IPlayer
    {
        public Player(string name, Field field)
        {
            Name = name;
            Field = field;
        }

        public string Name { get; }
        public Field Field { get; }

        IField IPlayer.Field => Field;

        public override string ToString() => $"Player {Name}";
    }
}
