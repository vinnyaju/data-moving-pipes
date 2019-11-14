using System;
using System.Collections.Generic;
using System.Text;

namespace DataMovingPipes.Abstractions
{
    public abstract class Transformation : CircuitBlock, ICloneable
    {
        protected readonly string transformationName;

        public Transformation(string _transformationName)
        {
            transformationName = _transformationName;
        }

        public abstract object Clone();

        protected override void ValidateConnectedBlocks()
        {
            if (this.PreviousBlock == null || this.NextBlock == null)
                throw new Exception("Both sides of a Transformation Block have to be connected to somewhere!");
        }
    }
}
