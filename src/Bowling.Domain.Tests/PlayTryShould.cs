using System;
using Xunit;

namespace Bowling.Domain.Tests
{
    public class PlayTryShould
    {
        public class BeCreated
        {
            private PlayTry sut;

            [Theory]
            [InlineData(new object[] { 1 })]
            [InlineData(new object[] { 2 })]
            [InlineData(new object[] { 3 })]
            public void BeCreatedWithATryNumberAndNotAttemptedFlag(int tryNumber)
            {
                sut = new PlayTry(tryNumber);

                Assert.NotNull(sut);
                Assert.IsAssignableFrom<KnockedPinsOnTry>(sut);
                Assert.Equal(tryNumber, sut.TryNumber);
                Assert.False(sut.HasBeenAttempted);
            }

            [Theory]
            [InlineData(new object[] { int.MinValue })]
            [InlineData(new object[] { 0 })]
            [InlineData(new object[] { int.MaxValue })]
            [InlineData(new object[] { 4 })]
            public void ThrowWhenCreatedWithInvalidTryNumber(int tryNumber)
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => new PlayTry(tryNumber));
            }
        }

        public class SetKnockedDownPins
        {
            private PlayTry sut;

            [Fact]
            public void SetHasBeenAttemptedAsTrueAfterSettingKnockedDownPins()
            {
                sut = new PlayTry(1);

                var pins = Utils.GetRandomNumberBetween(0, 10);

                sut.SetKnockedDownPins(pins);

                Assert.True(sut.HasBeenAttempted);
            }

            [Fact]
            public void SetKnockedDownPinsAndReturnSameValue()
            {
                sut = new PlayTry(1);

                var pins = Utils.GetRandomNumberBetween(0, 10);
                sut.SetKnockedDownPins(pins);

                Assert.Equal(pins, sut.KnockedDownPins);
            }

            [Theory]
            [InlineData(new object[] { int.MinValue })]
            [InlineData(new object[] { -1 })]
            [InlineData(new object[] { int.MaxValue })]
            [InlineData(new object[] { 11 })]

            public void ThrowWhenPinsKnockDownIsCalledWithWrongNumberOfPins(int pins)
            {
                sut = new PlayTry(1);

                Assert.Throws<ArgumentOutOfRangeException>(() => sut.SetKnockedDownPins(pins));
            }

            [Fact]
            public void ThrowWhenSetKnockedDownPinsWasAlreadySet()
            {
                sut = new PlayTry(1);

                var pins = Utils.GetRandomNumberBetween(0, 10);

                sut.SetKnockedDownPins(pins);

                Assert.Throws<InvalidOperationException>(() => sut.SetKnockedDownPins(pins));
            }
        }
    }
}