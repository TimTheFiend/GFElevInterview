using GFElevInterview.Models;

namespace GFElevInterview.Data
{
    /// <summary>
    /// <see cref="ElevModel"/> objekt som holder på input-værdierne under interviewet.
    /// </summary>
    public static class CurrentElev
    {
        public static ElevModel elev = new ElevModel();

        /// <summary>
        /// Nulstiller data'en i <see cref="CurrentElev"/>
        /// </summary>
        public static void NulstilCurrentElev() {
            elev = new ElevModel();
        }
    }
}