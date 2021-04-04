using Ardalis.GuardClauses;
using System.Collections.Generic;
using System.Linq;
using static Bowling.Domain.Frames.Base.Frame;

//JP: please review SOLID ppls on the code and throw a coverage

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

        //int CalculateScore();

        IReadOnlyList<KnockedPinsOnTry> GetAllKnockedDownPinsPerTry();

        KnockedPinsOnTry GetKnockedDownPinsOnTry(IPlayTry.PlayTry playTry);

        /// <summary>
        /// is called each time the player rolls a ball.  The argument is the number of pins knocked down
        /// </summary>
        void Roll(int pins);
    }

    //JP: /!\ this is a sorta hack to go around the fact that i did not design the frame with a link to the next frame in line but the previous one...
    internal interface NextFrameSetter
    {
        void SetNextFrameWith(Frame frame);
    }

    //JP: program the tenth frame!
    internal abstract class BaseFrame : Frame, NextFrameSetter
    {
        protected const int AvailablePins = 10;
        protected IList<PlayTry> tries;

        /// <summary>
        /// this field is so the current frame can pass into the prev frame the results of each try on this frame
        /// this is in case the current frame is a spare (pass back the first try) or a strike (pass back the 2 tries)
        /// </summary>
        private Frame next;

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

        public int RemainingPins { get; protected set; }

        public abstract IPlayTry.PlayTry Try { get; }

        public FrameType Type { get; private set; }

        //public abstract int CalculateScore();

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

            UpdateRemainingPins(pins);

            UpdateFrameTypeIfApplicable();
        }

        protected abstract void UpdateRemainingPins(int pins);

        public void SetNextFrameWith(Frame frame)
        {
            next = frame;
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
            if (RemainingPins == 0 && tries.Single(x => x.TryNumber == (int)IPlayTry.PlayTry.First).KnockedDownPins == AvailablePins)
                Type = FrameType.Strike;
            else if (RemainingPins == 0 && HasTriesLeft == false)
                Type = FrameType.Spare;
        }
    }
}