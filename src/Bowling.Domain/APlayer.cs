using Ardalis.GuardClauses;

namespace Bowling.Domain
{
    public interface Player
    {
        string Name { get; }
    }

    public class APlayer : Player
    {
        public APlayer(string name)
        {
            Guard.Against.NullOrEmpty(name, nameof(name));
            Name = name;
        }

        public string Name { get; }
    }
}