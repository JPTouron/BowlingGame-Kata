using Bowling.Domain.Frames;
using System;
using System.Linq;
using Xunit;

namespace Bowling.Domain.Tests
{
    public class GameShould
    {
        private AGame sut;

        [Fact]
        public void RollPinsOnceAndUpdateFrameWithRolledData()
        {
            var p1 = CreatePlayerOne();
            sut = CreateAGame(p1);

            var expectedframePlayed = 1;
            var totalAvailablePins = 10;
            var expectedKnockedPins = Utils.GetRandomPinsToKnockDown();
            var expectedRemainingPins = totalAvailablePins - expectedKnockedPins;

            sut.RollSomePinsDown(pinsToKnockDown: expectedKnockedPins);

            var frames = sut.GetPlayerFrames(p1);

            Assert.NotNull(frames);
            Assert.NotNull(frames.Single(x => x.Number == expectedframePlayed));

            var firstFrame = frames.Single(x => x.Number == expectedframePlayed);

            Assert.Equal(expectedRemainingPins, firstFrame.RemainingPins);
            Assert.Equal(expectedKnockedPins, firstFrame.GetKnockedDownPinsOnTry(IPlayTry.PlayTry.First).KnockedDownPins);
        }

        [Fact]
        public void RollPinsTwiceAndUpdateFrameWithRolledData()
        {
            var p1 = CreatePlayerOne();
            sut = CreateAGame(p1);

            var expectedframePlayed = 1;
            var totalAvailablePins = 10;

            var pinsKnockedDownOnFirstTry = Utils.GetRandomPinsToKnockDown(7);
            var pinsKnockedDownOnSecondTry = Utils.GetRandomPinsToKnockDown(3);
            var expectedRemainingPins = totalAvailablePins - pinsKnockedDownOnFirstTry - pinsKnockedDownOnSecondTry;

            sut.RollSomePinsDown(pinsToKnockDown: pinsKnockedDownOnFirstTry);
            sut.RollSomePinsDown(pinsToKnockDown: pinsKnockedDownOnSecondTry);

            var frames = sut.GetPlayerFrames(p1);

            Assert.NotNull(frames);
            Assert.NotNull(frames.Single(x => x.Number == expectedframePlayed));

            var firstFrame = frames.Single(x => x.Number == expectedframePlayed);

            Assert.Equal(expectedRemainingPins, firstFrame.RemainingPins);
            Assert.Equal(pinsKnockedDownOnFirstTry, firstFrame.GetKnockedDownPinsOnTry(IPlayTry.PlayTry.First).KnockedDownPins);
            Assert.Equal(pinsKnockedDownOnSecondTry, firstFrame.GetKnockedDownPinsOnTry(IPlayTry.PlayTry.Second).KnockedDownPins);
        }

        [Fact]
        public void ChangeFrameWhenRollingAfterCompletingTwoRollsOnANormalFrame()
        {
            var p1 = CreatePlayerOne();
            sut = CreateAGame(p1);

            var expectedCurrentFrameNumber = 2;

            var totalRollsToUse = 2;

            sut.RollSomePinsDown(totalRollsToUse);
            sut.RollSomePinsDown(totalRollsToUse);
            
            sut.RollSomePinsDown();

            var frames = sut.GetPlayerFrames(p1);

            Assert.Equal(expectedCurrentFrameNumber, frames.Count);


        }

        [Fact]
        public void HavePreviousToCurrentFrameWithTwoTriesAttemptedWhenCurrentFrameIsNormalFrame()
        {
            var p1 = CreatePlayerOne();
            sut = CreateAGame(p1);


            var totalFramesToPlay = 9;
            var framesPlayed = 0;
            for (int currentFrameNumber = 1; currentFrameNumber <= totalFramesToPlay; currentFrameNumber++)
            {

                var totalRollsToUse = 2;

                sut.RollSomePinsDown(totalRollsToUse);
                sut.RollSomePinsDown(totalRollsToUse);
                framesPlayed++;

                var frames = sut.GetPlayerFrames(p1);

                Assert.IsAssignableFrom<NormalFrame>(frames.Single(x => x.Number == currentFrameNumber));

                if (currentFrameNumber > 1)
                {
                    var expectedTotalTries = 2;
                    var previousFrame = frames.Single(x => x.Number == currentFrameNumber - 1);
                    var tries = previousFrame.GetAllKnockedDownPinsPerTry();

                    Assert.Equal(expectedTotalTries, tries.Count);
                    Assert.True(tries.All(x=>x.HasBeenAttempted));
                }
                Assert.Equal(framesPlayed, frames.Count);
            }

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

        private AGame CreateAGame(Player p1 = null, Player p2 = null)
        {
            return new AGame(p1 ?? CreatePlayerOne(), p2 ?? CreatePlayerTwo());
        }
    }
}