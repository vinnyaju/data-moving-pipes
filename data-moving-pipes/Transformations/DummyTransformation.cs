using DataMovingPipes.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMovingPipes.Transformations
{
    public class DummyTransformation : Transformation
    {
        public DummyTransformation(string _transFormationName) : base(_transFormationName)
        {

        }

        public override object Clone()
        {
            return new DummyTransformation(this.transformationName);
        }

        internal override void Collect(byte[] dataBuffer, int dataLength)
        {
            this.NextBlock.Collect(dataBuffer, dataLength);
        }
    }
}
