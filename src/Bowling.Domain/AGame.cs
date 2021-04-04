using Bowling.Domain.Frames;
using Bowling.Domain.Frames.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bowling.Domain
{
    public interface Game
    {
        string PlayerOne { get; }

        string PlayerTwo { get; }

        /// <summary>
        /// maybe private
        /// </summary>
        IReadOnlyList<Frame> GetPlayerFrames(Player player);

        /// <summary>
        /// is called each time the player rolls a ball.  The argument is the number of pins knocked down
        /// </summary>
        void Roll(Player player, int pins);

        /// <summary>
        /// is called only at the very end of the game.  It returns the total score for that game.
        /// </summary>
        int Score();
    }

    internal class AGame : Game
    {
        private Player p1;

        private Player p2;

        private IDictionary<Player, IEnumerable<Frame>> playerFrames;

        public AGame(Player p1, Player p2)
        {
            this.p1 = p1;
            this.p2 = p2;

            InitializeFrames();
        }

        public string PlayerOne => p1.Name;

        public string PlayerTwo => p2.Name;

        public IReadOnlyList<Frame> GetPlayerFrames(Player player)
        {
            if (player.Name == p1.Name)
                return playerFrames[player].ToList();

            if (player.Name == p2.Name)
                return playerFrames[player].ToList();

            throw new ArgumentException("No player in the game matches the player you supplied");
        }

        public void Roll(Player player, int pins)
        {
            Frame f = GetFrame(player);

            f.Roll(pins);
            AsStack(player).Push(f);
        }

        public int Score()
        {
            //JP: TEST THIS
            throw new NotImplementedException();
        }

        private Stack<Frame> AsStack(Player player)
        {
            return (Stack<Frame>)playerFrames[player];
        }

        private Frame GetFrame(Player player)
        {
            var stack = AsStack(player);
            var currentFrameNumber = stack.Count;

            if (stack.TryPeek(out var f))
            {
                if (f.HasTriesLeft)
                    return stack.Pop();
            }

            return new NormalFrame(currentFrameNumber + 1);
        }

        private void InitializeFrames()
        {
            var p1Frames = new KeyValuePair<Player, IEnumerable<Frame>>(p1, new Stack<Frame>());
            var p2Frames = new KeyValuePair<Player, IEnumerable<Frame>>(p2, new Stack<Frame>());

            playerFrames = new Dictionary<Player, IEnumerable<Frame>>();
            playerFrames.Add(p1Frames);
            playerFrames.Add(p2Frames);
        }
    }
}