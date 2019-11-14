using System;
using System.Collections.Generic;
using System.Text;

namespace DataMovingPipes.Abstractions
{
    public abstract class CircuitBlock
    {
        public CircuitBlock NextBlock { get; protected set; }
        public CircuitBlock PreviousBlock { get; protected set; }

        internal void ConnectTo(CircuitBlock _nextBlock)
        {
            NextBlock = _nextBlock;

            if (_nextBlock.PreviousBlock == null || !_nextBlock.PreviousBlock.Equals(this))
                _nextBlock.PreviousBlock = this;

            ValidateConnectedBlocks();
        }

        protected abstract void ValidateConnectedBlocks();
        internal abstract void Collect(byte[] dataBuffer, int dataLength);
    }
}
