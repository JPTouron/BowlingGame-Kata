using System;
using System.Collections.Generic;
using System.Linq;
using static Bowling.Domain.Frame;

namespace Bowling.Domain
{
    internal abstract class BaseFrame : Frame
    {
        protected IList<PlayTry> tries;

        protected IDictionary<IPlayTry.PlayTry, int> knockedDownOnTry;

        public BaseFrame(int frameNumber)
        {
            ValidateFrameNumber(frameNumber);

            Number = frameNumber;

            InitializeKnockedDownPins();
            InitializePlayTries();

            Type = PlayType.Regular;
        }

        protected abstract void InitializePlayTries();

        public int AvailablePins => 10;

        public int Number { get; }

        public abstract IPlayTry.PlayTry Try { get; }
        

        public PlayType Type { get; private set; }

        /// <summary>
        /// this field is so the current frame can pass into the prev frame the results of each try on this frame
        /// this is in case the current frame is a spare (pass back the first try) or a strike (pass back the 2 tries)
        /// </summary>
        private Frame Previous { get; }

        public abstract void Roll(int pins);


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

    }
}