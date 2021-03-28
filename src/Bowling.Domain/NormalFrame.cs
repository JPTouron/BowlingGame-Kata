using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bowling.Domain
{
    internal class NormalFrame : BaseFrame
    {
        private const int maximumFrameNumber = 9;
        private const int minimumFrameNumber = 1;

        public override IPlayTry.PlayTry Try
        {
            get
            {


                if (tries.Any(x => x.HasBeenAttempted == false) == false)
                    return IPlayTry.PlayTry.None;


                var attempt = tries.First(x => x.HasBeenAttempted == false);

                return (IPlayTry.PlayTry)attempt.TryNumber;


            }
        }

        public NormalFrame(int frameNumber) : base(frameNumber)
        {
        }

        public override void Roll(int pins)
        {
            PlayTry attempt;

            switch (Try)
            {
                case IPlayTry.PlayTry.First:
                    knockedDownOnTry[Try] = pins;
                    attempt = tries.Single(x => x.TryNumber == (int)IPlayTry.PlayTry.First);
                    attempt.SetKnockedDownPins(pins);
                    break;

                case IPlayTry.PlayTry.Second:
                    attempt = tries.Single(x => x.TryNumber == (int)IPlayTry.PlayTry.Second);
                    attempt.SetKnockedDownPins(pins);
                    break;



            }
        }

        protected override void InitializePlayTries()
        {
            InitializeWithTwoTries();

        }

        private void InitializeWithTwoTries()
        {
            tries = new List<PlayTry> { new PlayTry(1), new PlayTry(2) };
        }

        protected override void ValidateFrameNumber(int frameNumber)
        {
            Guard.Against.OutOfRange(frameNumber, nameof(frameNumber), minimumFrameNumber, maximumFrameNumber);
        }





    }
}