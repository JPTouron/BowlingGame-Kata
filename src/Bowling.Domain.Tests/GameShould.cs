using System;
using System.Linq;
using Xunit;

namespace Bowling.Domain.Tests
{
    public class GameShould
    {
        private Game sut;

        [Fact]
        public void RollPinsOnceAndUpdateFrameWithRolledData()
        {
            var framePlayed = 1;
            var totalAvailablePins = 10;
            var expectedRemainingPins = 10;

            var p1 = CreatePlayerOne();
            sut = CreateAGame(p1);

            var expectedKnockedPins = sut.RollSomePinsDown();
            expectedRemainingPins = totalAvailablePins - expectedKnockedPins;

            var frames = sut.GetPlayerFrames(p1);

            Assert.NotNull(frames);
            Assert.NotNull(frames.Single(x => x.Number == framePlayed));

            var firstFrame = frames.Single(x => x.Number == framePlayed);

            Assert.Equal(Frames.Base.Frame.PlayType.Regular, firstFrame.Type);
            Assert.Equal(expectedRemainingPins, firstFrame.RemainingPins);
            Assert.Equal(expectedKnockedPins, firstFrame.GetKnockedDownPinsOnTry(IPlayTry.PlayTry.First).KnockedDownPins);
        }

        [Fact]
        public void StartWithEmptyFramesForPlayers()
        {
            var p1 = new APlayer("name1");
            var p2 = new APlayer("name2");

            var framesCount = 0;

            sut = new AGame(p1, p2);

            Assert.Equal(framesCount, sut.GetPlayerFrames(p1).Count);
            Assert.Equal(framesCount, sut.GetPlayerFrames(p2).Count);
        }

        [Fact]
        public void StartWithTwoPlayers()
        {
            var p1 = new APlayer("name1");
            var p2 = new APlayer("name2");

            sut = new AGame(p1, p2);

            Assert.NotNull(sut);
            Assert.IsAssignableFrom<AGame>(sut);
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

            Assert.Throws<ArgumentException>(() => sut.GetPlayerFrames(unknownPlayer));
        }

        private static APlayer CreatePlayerOne()
        {
            return new APlayer("name1");
        }

        private static APlayer CreatePlayerTwo()
        {
            return new APlayer("name2");
        }

        private Game CreateAGame(Player p1 = null, Player p2 = null)
        {
            return new AGame(p1 ?? CreatePlayerOne(), p2 ?? CreatePlayerTwo());
        }
    }
}