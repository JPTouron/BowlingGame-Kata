using Bowling.Domain.Frames;
using Bowling.Domain.Frames.Base;
using System;

namespace Bowling.Domain.Tests
{
    public static class Utils
    {

        public static int RollSomePinsDown(this Frame sut, int totalRollsToUse = 1)
        {
            var pinsKnockedByRoll = GetRandomPinsToKnockDown(sut, sut.RemainingPins / totalRollsToUse);
            sut.Roll(pinsKnockedByRoll);
            return pinsKnockedByRoll;
        }

        public static Frame BuildNormalFrame()
        {
            var frameNumber = GetRandomNumberBetween(1, 9);
            return new NormalFrame(frameNumber);
        }

        public static int GetRandomNumberBetween(int min, int max)
        {
            var r = new Random();
            var expectedNumber = r.Next(min, max + 1);
            return expectedNumber;
        }

        public static int GetRandomPinsToKnockDown(Frame sut, int remainingPinsOverride = 0)
        {
            var remaining = remainingPinsOverride == 0 ? sut.RemainingPins : remainingPinsOverride;
            return GetRandomNumberBetween(0, remaining);
        }
    }
}