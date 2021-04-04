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
        public enum FrameType
        {
            Spare,
            Strike,
            Regular
        }

        bool HasTriesLeft { get; }

        int Number { get; }

        int RemainingPins { get; }

        IPlayTry.PlayTry Try { get; }

        FrameType Type { get; }

        IReadOnlyList<KnockedPinsOnTry> GetAllKnockedDownPinsPerTry();

        KnockedPinsOnTry GetKnockedDownPinsOnTry(IPlayTry.PlayTry playTry);

        /// <summary>
        /// is called each time the player rolls a ball.  The argument is the number of pins knocked down
        /// </summary>
        void Roll(int pins);
    }

    //JP: program the tenth frame!
    internal abstract class BaseFrame : Frame
    {
        protected IList<PlayTry> tries;

        public BaseFrame(int frameNumber)
        {
            ValidateFrameNumber(frameNumber);

            Number = frameNumber;

            InitializePlayTries();

            SortTriesByNumber();

            Type = FrameType.Regular;

            RemainingPins = 10;
        }

        public abstract bool HasTriesLeft { get; }

        public int Number { get; }

        public int RemainingPins { get; private set; }

        public abstract IPlayTry.PlayTry Try { get; }

        public FrameType Type { get; private set; }

        /// <summary>
        /// this field is so the current frame can pass into the prev frame the results of each try on this frame
        /// this is in case the current frame is a spare (pass back the first try) or a strike (pass back the 2 tries)
        /// </summary>
        private Frame Previous { get; }

        public IReadOnlyList<KnockedPinsOnTry> GetAllKnockedDownPinsPerTry()
        {
            return tries.ToList();
        }

        public KnockedPinsOnTry GetKnockedDownPinsOnTry(IPlayTry.PlayTry playTry)
        {
            ValidatePlayTry(playTry);

            var result = tries.Single(x => x.TryNumber == (int)playTry);
            return result;
        }

        public void Roll(int pins)
        {
            Guard.Against.OutOfRange(pins, nameof(pins), 0, RemainingPins);

            RollInternal(pins);

            RemainingPins -= pins;

            UpdateFrameTypeIfApplicable();
        }

        protected abstract void InitializePlayTries();

        protected abstract void RollInternal(int pins);

        protected abstract void ValidateFrameNumber(int frameNumber);

        protected abstract void ValidatePlayTry(IPlayTry.PlayTry playTry);

        private void SortTriesByNumber()
        {
            tries = tries.OrderBy(x => x.TryNumber).ToList();
        }

        private void UpdateFrameTypeIfApplicable()
        {
            if (RemainingPins == 0 && HasTriesLeft == true)
                Type = FrameType.Strike;
            else if (RemainingPins == 0 && HasTriesLeft == false)
                Type = FrameType.Spare;
        }
    }
}