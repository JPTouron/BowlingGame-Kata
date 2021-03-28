using System;
using System.Collections.Generic;
using System.Linq;

namespace Bowling.Domain
{

    internal class AGame : Game
    {
        private APlayer p1;
        private APlayer p2;


        public AGame(APlayer p1, APlayer p2)
        {
            this.p1 = p1;
            this.p2 = p2;

            InitializeFrames();

        }
        IList<Frame> p1Frames;
        IList<Frame> p2Frames;
        private void InitializeFrames()
        {
            p1Frames = new List<Frame>();
            p2Frames = new List<Frame>();


        }

        public string PlayerOne => p1.Name;
        public string PlayerTwo => p2.Name;

        public IReadOnlyList<Frame> PlayerFrames(Player player)
        {



            if (player.Name == p1.Name)
                return p1Frames.ToList();
            if (player.Name == p2.Name)
                return p2Frames.ToList();


            throw new ArgumentException("There is no player with the name supplied");
        }

        public void Roll(int pins)
        {
            throw new NotImplementedException();
        }

        public int Score()
        {
            throw new NotImplementedException();
        }
    }
}