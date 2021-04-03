using System;
using Xunit;

namespace Bowling.Domain.Tests
{
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