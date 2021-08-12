using System;
using System.Collections.Generic;
using System.Text;

namespace GFElevInterview.Interfaces
{
    /// <summary>
    /// Interface der indeholder funktioner der skal være i hver blanket af interview-delen.
    /// </summary>
    public interface IBlanket
    {
        /// <summary>
        /// Kaldes når brugeren går frem til næste side af interviewet.
        /// </summary>
        public void Frem();

        /// <summary>
        /// Kaldes når brugeren går tilbage til forrige side af interviewet.
        /// </summary>
        public void Tilbage();
    }
}