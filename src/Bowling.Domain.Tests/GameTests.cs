using System;
using Xunit;

namespace Bowling.Domain.Tests
{
    public class NormalFrameShould
    {
        public class BeCreated
        {
            Frame sut;

            [Fact]
            public void AsARegularPlayType()
            {
                int expectedNumber = Utils.GetRandomNumberBetween(1, 9);
                sut = new NormalFrame(expectedNumber);

                var expectedType = Frame.PlayType.Regular;

                Assert.Equal(expectedType, sut.Type);
            }

            [Fact]
            public void AsFirstTry()
            {
                int frameNumber = Utils.GetRandomNumberBetween(1, 9);
                sut = new NormalFrame(frameNumber);

                var expectedTry = IPlayTry.PlayTry.First;

                Assert.Equal(expectedTry, sut.Try);
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
            public void WithTotalPinsAsTenAndNoKnockedDownPins()
            {
                int frameNumber = Utils.GetRandomNumberBetween(1, 9);
                sut = new NormalFrame(frameNumber);

                var expectedTotal = 10;
                var expectedKnockedDownPins = 0;

                Assert.Equal(expectedTotal, sut.AvailablePins);

                Assert.Equal(expectedKnockedDownPins, sut.KnockedDownOnTry(IPlayTry.PlayTry.First));
                Assert.Equal(expectedKnockedDownPins, sut.KnockedDownOnTry(IPlayTry.PlayTry.Second));
                Assert.Equal(expectedKnockedDownPins, sut.KnockedDownOnTry(IPlayTry.PlayTry.Third));
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
        }

        public class IncreasePlayTry
        {
            Frame sut;

            //[Fact]
            //public void IncreasePlayTryCardinalWhenBallIsRolled_AndFrameIsNotTenth()
            //{
            //    var frameNumber = Utils.GetRandomNumberBetween(1, 9);
            //    sut = new NormalFrame(frameNumber);

            //    var pinsKnockedByRoll = Utils.GetRandomNumberBetween(0, 10);

            //    Assert.Equal(IPlayTry.PlayTry.First, sut.Try);

            //    sut.Roll(pinsKnockedByRoll);
            //    Assert.Equal(IPlayTry.PlayTry.Second, sut.Try);
            //}

            //[Fact]
            //public void IncreasePlayTryCardinalWhenBallIsRolled_AndFrameIsTenth()
            //{
            //    var frameNumber = 10;
            //    sut = new NormalFrame(frameNumber);

            //    var pinsKnockedByRoll = Utils.GetRandomNumberBetween(0, 10);

            //    Assert.Equal(IPlayTry.PlayTry.First, sut.Try);

            //    sut.Roll(pinsKnockedByRoll);
            //    Assert.Equal(IPlayTry.PlayTry.Second, sut.Try);

            //    sut.Roll(pinsKnockedByRoll);
            //    Assert.Equal(IPlayTry.PlayTry.Third, sut.Try);
            //}

            [Fact]
            public void ThrowWhenBallIsRolledTwiceForSecondTry_AndFrameIsNotTenth()
            {
                var frameNumber = Utils.GetRandomNumberBetween(1, 9);
                sut = new NormalFrame(frameNumber);

                var pinsKnockedByRoll = Utils.GetRandomNumberBetween(0, 10);

                Assert.Equal(IPlayTry.PlayTry.First, sut.Try);

                sut.Roll(pinsKnockedByRoll);
                Assert.Equal(IPlayTry.PlayTry.Second, sut.Try);

                Assert.Throws<InvalidOperationException>(() => sut.Roll(pinsKnockedByRoll));
            }

            //[Fact]
            //public void ThrowWhenBallIsRolledTwiceForThirdTry_AndFrameIsTenth()
            //{
            //    var frameNumber = 10;
            //    sut = new NormalFrame(frameNumber);

            //    var pinsKnockedByRoll = Utils.GetRandomNumberBetween(0, 10);

            //    Assert.Equal(IPlayTry.PlayTry.First, sut.Try);

            //    sut.Roll(pinsKnockedByRoll);
            //    Assert.Equal(IPlayTry.PlayTry.Second, sut.Try);

            //    sut.Roll(pinsKnockedByRoll);
            //    Assert.Equal(IPlayTry.PlayTry.Third, sut.Try);

            //    Assert.Throws<InvalidOperationException>(() => sut.Roll(pinsKnockedByRoll));
            //}

        }


    }

    public class GameShould
    {
        private Game sut;

        [Fact]
        public void StartWithEmptyFramesForPlayers()
        {
            var p1 = new APlayer("name1");
            var p2 = new APlayer("name2");

            var framesCount = 0;

            sut = new AGame(p1, p2);

            Assert.Equal(framesCount, sut.PlayerFrames(p1).Count);
            Assert.Equal(framesCount, sut.PlayerFrames(p2).Count);
        }

        [Fact]
        public void StartWithTwoPlayers()
        {
            var p1 = new APlayer("name1");
            var p2 = new APlayer("name2");

            sut = new AGame(p1, p2);

            Assert.NotNull(sut);
            Assert.IsAssignableFrom<Game>(sut);
            Assert.Equal(p1.Name, sut.PlayerOne);
            Assert.Equal(p2.Name, sut.PlayerTwo);
        }

        [Fact]
        public void ThrowWhenRequestingFramesForUnknownPlayer()
        {
            var p1 = new APlayer("name1");
            var p2 = new APlayer("name2");
            var unknownPlayer = new APlayer("unknownPlayer");

            sut = new AGame(p1, p2);

            Assert.Throws<ArgumentException>(() => sut.PlayerFrames(unknownPlayer));
        }
    }

    public class PlayerShould
    {
        private Player sut;

        [Fact]
        public void BeCreatedWithAName()
        {
            var name = "PlayerName";
            sut = new APlayer(name);

            Assert.NotNull(sut);
            Assert.IsAssignableFrom<Player>(sut);
            Assert.Equal(name, sut.Name);
        }

        [Fact]
        public void NotBeCreatedWithANullOrEmptyNameName()
        {
            string name = null;
            Assert.Throws<ArgumentNullException>(() => new APlayer(name));

            name = "";
            Assert.Throws<ArgumentException>(() => new APlayer(name));
        }
    }
}