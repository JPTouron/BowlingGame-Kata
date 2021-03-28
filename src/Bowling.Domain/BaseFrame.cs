using System;
using System.Collections.Generic;
using static Bowling.Domain.Frame;

namespace Bowling.Domain
{
    internal abstract class BaseFrame : Frame
    {
        protected IList<PlayTry> tries;

        private IDictionary<IPlayTry.PlayTry, int> knockedDownOnTry;

        public BaseFrame(int frameNumber)
        {
            ValidateFrameNumber(frameNumber);

            Number = frameNumber;

            InitializeKnockedDownPins();

            Type = PlayType.Regular;
            Try = IPlayTry.PlayTry.First;
        }

        public int AvailablePins => 10;

        public int Number { get; }

        public IPlayTry.PlayTry Try { get; private set; }

        public PlayType Type { get; private set; }

        /// <summary>
        /// this field is so the current frame can pass into the prev frame the results of each try on this frame
        /// this is in case the current frame is a spare (pass back the first try) or a strike (pass back the 2 tries)
        /// </summary>
        private Frame Previous { get; }

        public int KnockedDownOnTry(IPlayTry.PlayTry playTry)
        {
            return 0;
        }

        public void Roll(int pins)
        {

            switch (Try)
            {
                case IPlayTry.PlayTry.First:
                    knockedDownOnTry[Try] = pins;
                    Try = IPlayTry.PlayTry.Second;
                    break;

                case IPlayTry.PlayTry.Second:
                    ThrowIfBallWasAlreadyRolledForSecondTryInFrameBelowTenth();

                    if (Number == 10)
                        Try = IPlayTry.PlayTry.Third;

                    break;

                case IPlayTry.PlayTry.Third:
                    ThrowIfBallWasAlreadyRolledForThirdTryInFrameTen();

                    break;
            }
        }

        protected abstract void ValidateFrameNumber(int frameNumber);

        private void InitializeKnockedDownPins()
        {
            var knockDownValues = new List<KeyValuePair<IPlayTry.PlayTry, int>>
            {
                new KeyValuePair<IPlayTry.PlayTry, int>(IPlayTry.PlayTry.First,   0 ),
                new KeyValuePair<IPlayTry.PlayTry, int>(IPlayTry.PlayTry.Second,   0 ),
                new KeyValuePair<IPlayTry.PlayTry, int>(IPlayTry.PlayTry.Third,   0 )
            };

            knockedDownOnTry = new Dictionary<IPlayTry.PlayTry, int>(knockDownValues);
        }

        private void ThrowIfBallWasAlreadyRolledForSecondTryInFrameBelowTenth()
        {
            if (Try == IPlayTry.PlayTry.Second && Number < 10)
                throw new InvalidOperationException($"Cannot roll ball again on frame #{Number}. You already rolled both your tries.");
        }

        private void ThrowIfBallWasAlreadyRolledForThirdTryInFrameTen()
        {
            if (Try == IPlayTry.PlayTry.Third && Number == 10)
                throw new InvalidOperationException($"Cannot roll ball again on frame #{Number}. You already rolled your three tries.");
        }
    }
}