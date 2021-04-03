﻿using Bowling.Domain.Frames;
using Bowling.Domain.Frames.Base;
using System;
using System.Linq;
using Xunit;

namespace Bowling.Domain.Tests
{
    public class NormalFrameShould
    {
        public class BeCreated
        {
            private Frame sut;

            [Fact]
            public void AsARegularPlayType()
            {
                sut = Utils.BuildNormalFrame();

                var expectedType = Frame.PlayType.Regular;

                Assert.Equal(expectedType, sut.Type);
            }

            [Fact]
            public void AsFirstTry()
            {
                sut = Utils.BuildNormalFrame();

                var expectedTry = IPlayTry.PlayTry.First;

                Assert.Equal(expectedTry, sut.Try);
            }

            [Theory]
            [InlineData(new object[] { 0 })]
            [InlineData(new object[] { 10 })]
            [InlineData(new object[] { int.MinValue })]
            [InlineData(new object[] { int.MaxValue })]
            public void ThrowWhenFrameNumberIsOutOfBounds(int outOfBoundsNumber)
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => new NormalFrame(outOfBoundsNumber));
            }

            [Fact]
            public void WithAllTriesNotAttemptedAndNoKnockedDownPins()
            {
                sut = Utils.BuildNormalFrame();

                var tries = sut.GetKnockedDownPinsPerTry();
                var expectedKnockedDownPins = 0;

                var @try = tries.Single(x => x.TryNumber == 1);
                Assert.Equal(expectedKnockedDownPins, @try.KnockedDownPins);

                @try = tries.Single(x => x.TryNumber == 2);
                Assert.Equal(expectedKnockedDownPins, @try.KnockedDownPins);
            }

            [Theory]
            [InlineData(new object[] { 1 })]
            [InlineData(new object[] { 2 })]
            [InlineData(new object[] { 3 })]
            [InlineData(new object[] { 4 })]
            [InlineData(new object[] { 5 })]
            [InlineData(new object[] { 6 })]
            [InlineData(new object[] { 7 })]
            [InlineData(new object[] { 8 })]
            [InlineData(new object[] { 9 })]
            public void WithAValidNumber(int withinBoundsNumber)
            {
                sut = new NormalFrame(withinBoundsNumber);

                Assert.Equal(withinBoundsNumber, sut.Number);
            }

            [Fact]
            public void WithTwoAvailableTries()
            {
                sut = Utils.BuildNormalFrame();

                var tries = sut.GetKnockedDownPinsPerTry();

                var expectedTries = 2;
                Assert.Equal(expectedTries, tries.Count);
            }
        }

        public class IncreasePlayTry
        {
            private Frame sut;

            [Fact]
            public void IncreasePlayTryAfterBallIsRolled()
            {
                sut = Utils.BuildNormalFrame();

                var pinsKnockedByRoll = Utils.GetRandomPinsToKnockDown(sut);

                sut.Roll(pinsKnockedByRoll);
                Assert.Equal(IPlayTry.PlayTry.Second, sut.Try);
            }

            [Fact]
            public void TriesContainRollDataWhenRetrieved()
            {
                sut = Utils.BuildNormalFrame();

                int pinsKnockedByRoll = sut.RollSomePinsDown();

                var expectedFirstTry = new { tryNumber = 1, knockedPins = pinsKnockedByRoll };

                pinsKnockedByRoll = sut.RollSomePinsDown();

                var expectedSecondTry = new { tryNumber = 2, knockedPins = pinsKnockedByRoll };

                var tries = sut.GetKnockedDownPinsPerTry();

                var firstTry = tries.Single(x => x.TryNumber == expectedFirstTry.tryNumber);
                var secondTry = tries.Single(x => x.TryNumber == expectedSecondTry.tryNumber);

                Assert.Equal(expectedFirstTry.tryNumber, firstTry.TryNumber);
                Assert.Equal(expectedFirstTry.knockedPins, firstTry.KnockedDownPins);
                Assert.Equal(expectedSecondTry.tryNumber, secondTry.TryNumber);
                Assert.Equal(expectedSecondTry.knockedPins, secondTry.KnockedDownPins);
            }

            [Fact]
            public void WhenBallIsRolledMoreTimesThanTwo_ThenNothingHappens()
            {
                sut = Utils.BuildNormalFrame();

                var totalRollingTries = 3;

                sut.RollSomePinsDown(totalRollingTries);

                sut.RollSomePinsDown(totalRollingTries);

                var knockedDownOnSecondRoll = sut.GetKnockedDownPinsPerTry();

                sut.RollSomePinsDown(totalRollingTries);

                var knockedDownOnThirdRoll = sut.GetKnockedDownPinsPerTry();

                Assert.Equal(knockedDownOnSecondRoll, knockedDownOnThirdRoll);
            }
        }
    }
}