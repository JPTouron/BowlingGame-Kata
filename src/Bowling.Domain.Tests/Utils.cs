﻿using Bowling.Domain.Frames;
using System;

namespace Bowling.Domain.Tests
{
    public static class Utils
    {
        public static int GetRandomNumberBetween(int min, int max)
        {
            var r = new Random();
            var expectedNumber = r.Next(min, max + 1);
            return expectedNumber;
        }

        public static int GetRandomPinsToKnockDown(int? remainingPinsOverride = null)
        {
            var remaining = remainingPinsOverride ?? 0;
            return GetRandomNumberBetween(0, remaining);
        }

        public static int RollSomePinsDown(this Rollable sut, int totalRollsToUse = 1, int remainingPins = 10, int? pinsToKnockDown = null)
        {
            var pinsKnockedByRoll = pinsToKnockDown ?? GetRandomPinsToKnockDown(remainingPins / totalRollsToUse);
            sut.Roll(pinsKnockedByRoll);
            return pinsKnockedByRoll;
        }

        internal static NormalFrame BuildNormalFrame()
        {
            var frameNumber = GetRandomNumberBetween(1, 9);
            return new NormalFrame(frameNumber);
        }
    }
}