using Ardalis.GuardClauses;
using System;

namespace Bowling.Domain
{
    public interface KnockedPinsOnTry
    {
        int KnockedDownPins { get; }

        int TryNumber { get; }
    }

    internal class PlayTry : KnockedPinsOnTry
    {
        public PlayTry(int tryNumber)
        {
            Guard.Against.OutOfRange(tryNumber, nameof(tryNumber), 1, 3);
            TryNumber = tryNumber;
            HasBeenAttempted = false;
        }

        public bool HasBeenAttempted { get; private set; }

        public int KnockedDownPins { get; private set; }

        public int TryNumber { get; }

        public void SetKnockedDownPins(int pins)
        {
            if (HasBeenAttempted)
                throw new InvalidOperationException($"Cannot knock more pins on try #{TryNumber}. You already knocked down {pins} pins.");

            Guard.Against.OutOfRange(pins, nameof(pins), 0, 10);

            KnockedDownPins = pins;
            HasBeenAttempted = true;
        }
    }
}