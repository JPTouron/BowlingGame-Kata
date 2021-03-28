using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bowling.Domain
{
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
