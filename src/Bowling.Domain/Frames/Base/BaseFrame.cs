using Ardalis.GuardClauses;
using System.Collections.Generic;
using System.Linq;
using static Bowling.Domain.Frames.Base.Frame;

namespace Bowling.Domain.Frames.Base
{
    /// <summary>
    /// this should be implemented as a composite
    /// and a way to get all the frames might be by following down the composition from last node to first node
    /// </summary>
    public interface Frame
    {
        public enum PlayType
        {
            Spare,
            Strike,
            Regular
        }

        int RemainingPins { get; }

        int Number { get; }

        IPlayTry.PlayTry Try { get; }

        PlayType Type { get; }

        IReadOnlyList<KnockedPinsOnTry> GetKnockedDownPinsPerTry();

        void Roll(int pins);
    }

    internal abstract class BaseFrame : Frame
    {
        protected IList<PlayTry> tries;

        public BaseFrame(int frameNumber)
        {
            ValidateFrameNumber(frameNumber);

            Number = frameNumber;

            InitializePlayTries();

            SortTriesByNumber();

            Type = PlayType.Regular;

            RemainingPins = 10;
        }

        public int RemainingPins { get; private set; }

        public int Number { get; }

        public abstract IPlayTry.PlayTry Try { get; }

        public PlayType Type { get; private set; }

        /// <summary>
        /// this field is so the current frame can pass into the prev frame the results of each try on this frame
        /// this is in case the current frame is a spare (pass back the first try) or a strike (pass back the 2 tries)
        /// </summary>
        private Frame Previous { get; }

        public IReadOnlyList<KnockedPinsOnTry> GetKnockedDownPinsPerTry()
        {
            return tries.ToList();
        }

        public  void Roll(int pins) {

            Guard.Against.OutOfRange(pins, nameof(pins), 0, RemainingPins);

            RollInternal(pins);

            RemainingPins -= pins;
        }

        protected abstract void RollInternal(int pins);
        protected abstract void InitializePlayTries();

        protected abstract void ValidateFrameNumber(int frameNumber);

        private void SortTriesByNumber()
        {
            tries = tries.OrderBy(x => x.TryNumber).ToList();
        }
    }
}