namespace Bowling.Domain
{
    public interface Rollable {

        /// <summary>
        /// is called each time the player rolls a ball.  The argument is the number of pins knocked down
        /// </summary>
        void Roll(int pins);
    
    }
}