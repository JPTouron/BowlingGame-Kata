using System;
using Xunit;

namespace Bowling.Domain.Tests
{
    public class GameShould
    {
        private AGame sut;

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

            Assert.Throws<ArgumentException>(() => sut.PlayerFrames(unknownPlayer));
        }
    }
}