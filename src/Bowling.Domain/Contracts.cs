using System;
using System.Collections.Generic;
using System.Text;

namespace Bowling.Domain
{

    public interface Game
    {

        string PlayerOne { get; }
        string PlayerTwo { get; }


        /// <summary>
        /// is called each time the player rolls a ball.  The argument is the number of pins knocked down
        /// </summary>
        void Roll(int pins);
        /// <summary>
        /// is called only at the very end of the game.  It returns the total score for that game.
        /// </summary>
        int Score();

        /// <summary>
        /// maybe private
        /// </summary>
        IReadOnlyList<Frame> PlayerFrames(Player player);

    }

    public interface Player
    {
        string Name { get; }
    }

    //info: bowling scoring - https://www.youtube.com/watch?v=oSUi1d5sAb0




    /// <summary>
    /// this should be implemented as a composite
    /// and a way to get all the frames might be by following down the composition from last node to first node
    /// </summary>
    public interface Frame
    {
        public enum PlayType
        {
            Spare,
            Strike,
            Regular
        }

        int Number { get; }
        PlayType Type { get; }

        IPlayTry.PlayTry Try { get; }

        int AvailablePins { get; }

        void Roll(int pins);



    }

    public interface IPlayTry { 
        public enum PlayTry
        {
            None=0, 
            First=1,
            Second=2,
            Third=3
        }

    
    }
}
