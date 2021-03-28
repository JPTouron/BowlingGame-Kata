using Ardalis.GuardClauses;

namespace Bowling.Domain
{
    internal class NormalFrame : BaseFrame
    {
        private const int maximumFrameNumber = 9;
        private const int minimumFrameNumber = 1;

        public NormalFrame(int frameNumber) : base(frameNumber)
        {
        }

        protected override void ValidateFrameNumber(int frameNumber)
        {
            Guard.Against.OutOfRange(frameNumber, nameof(frameNumber), minimumFrameNumber, maximumFrameNumber);
        }
    }
}