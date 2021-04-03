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


    internal class AGame : Game, Rollable
    {

        private Stack<Frame> AsStack(IEnumerable<Frame> frames)
        {

            return (Stack<Frame>)frames;
        }

        private Rollable AsRollable(Frame frame)
        {

            return (Rollable)frame;
        }

        private Player p1;
        private Player p2;


        private IEnumerable<Frame> p1Frames;
        private IEnumerable<Frame> p2Frames;

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
            Frame f = GetFrame();

            AsRollable(f).Roll(pins);
            AsStack(p1Frames).Push(f);

        }


        private Frame GetFrame()
        {

            var stack = AsStack(p1Frames);
            var currentFrameNumber = stack.Count;
            if (stack.TryPeek(out var f))
            {

                if (f.HasTriesLeft)
                    return stack.Pop();
                else
                    return new NormalFrame(currentFrameNumber + 1);
            }

            return new NormalFrame(1);

        }

        public int Score()
        {
            throw new NotImplementedException();
        }

        private void InitializeFrames()
        {
            p1Frames = new Stack<Frame>();
            p2Frames = new Stack<Frame>();
        }
    }
}