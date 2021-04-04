using Bowling.Domain.Frames;
using Bowling.Domain.Frames.Base;
using System;
using System.Linq;
using Xunit;

namespace Bowling.Domain.Tests
{
    public class NormalFrameShould
    {
        private NormalFrame sut;

        [Fact]
        public void HaveARegularFrameTypeWhenNotAllPinsAreTakenInTwoTries()
        {
            sut = Utils.BuildNormalFrame();

            var pinsKnockedByRoll = Utils.GetRandomPinsToKnockDown(3);
            sut.Roll(pinsKnockedByRoll);

            pinsKnockedByRoll = Utils.GetRandomPinsToKnockDown(6);
            sut.Roll(pinsKnockedByRoll);

            Assert.Equal(Frame.FrameType.Regular, sut.Type);
        }

        [Fact]
        public void HaveASpareFrameTypeWhenAllPinsAreTakenInTwoTries()
        {
            sut = Utils.BuildNormalFrame();

            var pinsKnockedByRoll = Utils.GetPinsToKnockDown(pinsToKnockDown: 3);
            sut.Roll(pinsKnockedByRoll);

            pinsKnockedByRoll = Utils.GetPinsToKnockDown(pinsToKnockDown: 7);
            sut.Roll(pinsKnockedByRoll);

            Assert.Equal(Frame.FrameType.Spare, sut.Type);
        }

        [Fact]
        public void HaveAStrikeFrameTypeWhenAllPinsAreTakenInTheFirstTry()
        {
            sut = Utils.BuildNormalFrame();

            var pinsKnockedByRoll = Utils.GetPinsToKnockDown(pinsToKnockDown: 10);
            sut.Roll(pinsKnockedByRoll);

            Assert.Equal(Frame.FrameType.Strike, sut.Type);
        }

        public class BeCreated
        {
            private NormalFrame sut;

            [Fact]
            public void AsARegularPlayType()
            {
                sut = Utils.BuildNormalFrame();

                var expectedType = Frame.FrameType.Regular;

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

                var tries = sut.GetAllKnockedDownPinsPerTry();
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

                var tries = sut.GetAllKnockedDownPinsPerTry();

                var expectedTries = 2;
                Assert.Equal(expectedTries, tries.Count);
            }
        }

        public class IncreasePlayTry
        {
            private NormalFrame sut;

            [Fact]
            public void IncreasePlayTryAfterBallIsRolled()
            {
                sut = Utils.BuildNormalFrame();

                var pinsKnockedByRoll = Utils.GetRandomPinsToKnockDown(sut.RemainingPins);

                sut.Roll(pinsKnockedByRoll);
                Assert.Equal(IPlayTry.PlayTry.Second, sut.Try);
            }

            [Fact]
            public void TriesContainRollDataWhenRetrieved()
            {
                sut = Utils.BuildNormalFrame();

                var totalRollsToUse = 2;

                var pinsKnockedDownByRoll = Utils.GetPinsToKnockDown(totalRollsToUse);
                sut.Roll(pinsKnockedDownByRoll);

                var expectedFirstTry = new { tryNumber = 1, knockedPins = pinsKnockedDownByRoll };

                pinsKnockedDownByRoll = Utils.GetPinsToKnockDown(totalRollsToUse);
                sut.Roll(pinsKnockedDownByRoll);

                var expectedSecondTry = new { tryNumber = 2, knockedPins = pinsKnockedDownByRoll };

                var tries = sut.GetAllKnockedDownPinsPerTry();

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

                var pinsKnockedDownByRoll = Utils.GetPinsToKnockDown(totalRollingTries);
                sut.Roll(pinsKnockedDownByRoll);

                pinsKnockedDownByRoll = Utils.GetPinsToKnockDown(totalRollingTries);
                sut.Roll(pinsKnockedDownByRoll);

                var knockedDownOnSecondRoll = sut.GetAllKnockedDownPinsPerTry();

                pinsKnockedDownByRoll = Utils.GetPinsToKnockDown(totalRollingTries);
                sut.Roll(pinsKnockedDownByRoll);

                var knockedDownOnThirdRoll = sut.GetAllKnockedDownPinsPerTry();

                Assert.Equal(knockedDownOnSecondRoll, knockedDownOnThirdRoll);
            }
        }
    }
}