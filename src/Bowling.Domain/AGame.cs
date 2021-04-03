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
        /// is called only at the very end of the game.  It returns the total score for that game.
        /// </summary>
        int Score();
    }

    internal class AGame : Game,Rollable
    {
        private Player p1;
        private Player p2;

        private IList<Frame> p1Frames;
        private IList<Frame> p2Frames;

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
                return p1Frames.ToList();
            if (player.Name == p2.Name)
                return p2Frames.ToList();

            throw new ArgumentException("There is no player with the name supplied");
        }

        public void Roll(int pins)
        {
            var f = new NormalFrame(1);

            f.Roll(pins);

            p1Frames.Add(f);
                

        }

        public int Score()
        {
            throw new NotImplementedException();
        }

        private void InitializeFrames()
        {
            p1Frames = new List<Frame>();
            p2Frames = new List<Frame>();
        }
    }
}