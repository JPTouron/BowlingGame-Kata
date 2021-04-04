using Ardalis.GuardClauses;
using Bowling.Domain.Frames.Base;
using System.Collections.Generic;
using System.Linq;

namespace Bowling.Domain.Frames
{
    internal class TenthFrame : BaseFrame
    {
        public TenthFrame() : base(10)
        {
        }

        public override bool HasTriesLeft => !AllTriesHaveBeenAttempted();

        public override IPlayTry.PlayTry Try
        {
            get
            {
                if (AllTriesHaveBeenAttempted())
                    return IPlayTry.PlayTry.None;

                var attempt = GetNotAttemptedTry();

                return (IPlayTry.PlayTry)attempt.TryNumber;
            }
        }

        protected override void InitializePlayTries()
        {
            InitializeWithThreeTries();
        }

        protected override void RollInternal(int pins)
        {
            PlayTry attempt;

            switch (Try)
            {
                case IPlayTry.PlayTry.First:
                    attempt = GetTry(IPlayTry.PlayTry.First);
                    attempt.SetKnockedDownPins(pins);

                    break;

                case IPlayTry.PlayTry.Second:
                    attempt = GetTry(IPlayTry.PlayTry.Second);
                    attempt.SetKnockedDownPins(pins);
                    break;

                case IPlayTry.PlayTry.Third:
                    attempt = GetTry(IPlayTry.PlayTry.Third);
                    attempt.SetKnockedDownPins(pins);
                    break;
            }
        }

        protected override void UpdateRemainingPins(int pins)
        {
            if (pins == AvailablePins)
                RemainingPins = AvailablePins;
            else
                RemainingPins -= pins;
        }

        protected override void ValidateFrameNumber(int frameNumber)
        {
            //do nothing,
            //JP: POSSIBLE ISP VIOLATION, GOTTA INHERIT FROM NORMAL FRAME AND THIS METHOD IS FOR NORMAL ONLY, or leave it in base and validate up to 10 there
        }

        protected override void ValidatePlayTry(IPlayTry.PlayTry playTry)
        {
            Guard.Against.OutOfRange((int)playTry, nameof(playTry), (int)IPlayTry.PlayTry.First, (int)IPlayTry.PlayTry.Third);
        }

        private bool AllTriesHaveBeenAttempted()
        {
            return tries.All(x => x.HasBeenAttempted);
        }

        private PlayTry GetNotAttemptedTry()
        {
            return tries.First(x => x.HasBeenAttempted == false);
        }

        private PlayTry GetTry(IPlayTry.PlayTry playTry)
        {
            return tries.Single(x => x.TryNumber == (int)playTry);
        }

        private void InitializeWithThreeTries()
        {
            tries = new List<PlayTry> { new PlayTry(1), new PlayTry(2), new PlayTry(3) };
        }
    }
}