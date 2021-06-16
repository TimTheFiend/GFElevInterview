using System;
using System.Collections.Generic;
using System.Text;

namespace GFElevInterview.Interfaces
{
    public interface IBlanket
    {
        public bool Frem(out IBlanket nextBlanket);
        public bool Tilbage(out IBlanket previousBlanket);
    }
}
