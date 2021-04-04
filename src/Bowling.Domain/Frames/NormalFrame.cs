﻿using Ardalis.GuardClauses;
using Bowling.Domain.Frames.Base;
using System.Collections.Generic;
using System.Linq;

namespace Bowling.Domain.Frames
{
    internal class NormalFrame : BaseFrame
    {
        private const int maximumFrameNumber = 9;
        private const int minimumFrameNumber = 1;

        public NormalFrame(int frameNumber) : base(frameNumber)
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

        //public override int CalculateScore()
        //{
        //    switch (Type)
        //    {
        //        case Frame.FrameType.Spare:
        //            return CalculateSpareScore();
        //        case Frame.FrameType.Strike:
        //            return CalculateStrikeScore();
        //        case Frame.FrameType.Regular:
        //            return CalculateRegularScore();
        //        default:
        //            return 0;
        //    }

        //}

        //private int CalculateSpareScore()
        //{
        //    throw new System.NotImplementedException();
        //}

        //private int CalculateStrikeScore()
        //{
        //    throw new System.NotImplementedException();
        //}

        //private int CalculateRegularScore()
        //{
        //        var score = tries.Sum(x => x.KnockedDownPins);

        //    return score;
        //}

        protected override void InitializePlayTries()
        {
            InitializeWithTwoTries();
        }

        protected override void RollInternal(int pins)
        {
            PlayTry attempt;

            switch (Try)
            {
                case IPlayTry.PlayTry.First:
                    attempt = GetTry(IPlayTry.PlayTry.First);
                    attempt.SetKnockedDownPins(pins);
                    IfFirstTryIsAStrikeThenCountOutSecondTry(pins);

                    break;

                case IPlayTry.PlayTry.Second:
                    attempt = GetTry(IPlayTry.PlayTry.Second);
                    attempt.SetKnockedDownPins(pins);
                    break;
            }
        }

        protected override void UpdateRemainingPins(int pins)
        {

                RemainingPins -= pins;
        }

        protected override void ValidateFrameNumber(int frameNumber)
        {
            Guard.Against.OutOfRange(frameNumber, nameof(frameNumber), minimumFrameNumber, maximumFrameNumber);
        }

        protected override void ValidatePlayTry(IPlayTry.PlayTry playTry)
        {
            Guard.Against.OutOfRange((int)playTry, nameof(playTry), (int)IPlayTry.PlayTry.First, (int)IPlayTry.PlayTry.Second);
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

        private void IfFirstTryIsAStrikeThenCountOutSecondTry(int pins)
        {
            if (pins == AvailablePins)
            {
                var attempt = GetTry(IPlayTry.PlayTry.Second);
                attempt.SetKnockedDownPins(0);
            }
        }

        private void InitializeWithTwoTries()
        {
            tries = new List<PlayTry> { new PlayTry(1), new PlayTry(2) };
        }
    }
}