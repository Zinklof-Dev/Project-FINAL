using System;

/*
techreport | oneill:pcg2014

title : PCG: A Family of Simple Fast Space-Efficient Statistically Good Algorithms for Random Number Generation
author : Melissa E. O'Neill
institution : Harvey Mudd College
address : Claremont, CA
number : HMC-CS-2014-0905
year : 2014
month : Sep
xurl : https://www.cs.hmc.edu/tr/hmc-cs-2014-0905.pdf
*/

namespace Bastion
{
    /// <summary>
    /// Zrand is a random number generation class that uses the PCG RNG family of algorithms, which are code line efficient, memory efficient, speed efficient, and pretty random compared to other PRNGs. (Dont ask my source, I trusted their site :D) | 
    /// Specifically this is the PCG-XSL-RR 128/64 algorithm. which has a state of 128 bits, and can output 64 bit numbers, and is one of the fastest members in the family, albeit not as good statistically speaking with its outputs. |
    /// Uses ~16 bytes of memory when existing
    /// </summary>
    [Obsolete("CLASS IS NOT FINISHED YET, DO NOT USE!", true)]
    // Further Elaboration: not sure when i will get to this, it's a minor save over the built in random class, most of the wrappers have serious edge cases where they lose randomness or don't return the full range they should.
    // this is because i told GPT to write the wrapper parts of this class to have a frame, and never got around to actually going in and replacing them with real wrappers for each value type. if someone wants to do this on their offtime, feel free!
    // -zink
    public class Zrand64
    {
        private ulong state;
        private ulong inc;

        static ulong multiplier = 6364136223846793005u;

        // ----- class creation -----

        /// <summary>
        /// Creates a new instance of the Zrand class.
        /// </summary>
        public Zrand64()
        {
            state = (ulong)DateTime.UtcNow.Ticks;
            inc = 1442695040888963407u;
        }

        /// <summary>
        /// Creates a new instance of the Zrand class.
        /// </summary>
        /// <param name="seed">Seed to be used</param>
        public Zrand64(ulong seed)
        {
            state = seed;
            inc = 1442695040888963407u;
        }

        /// <summary>
        /// Creates a new instance of the Zrand class.
        /// </summary>
        /// <param name="seed">Seed to be used</param>
        /// <param name="increment">Custom increment to apply instead of the default hard coded one.</param>
        public Zrand64(ulong seed, ulong increment)
        {
            state = seed;
            inc = increment;
        }

        // ----- class settings functions -----

        /// <summary>
        /// Allows you to change the seed of the Zrand class (if you set the seed to the same as creation it's basically a reset)
        /// </summary>
        /// <param name="seed">New Seed</param>
        public void ReSeed(ulong seed)
        {
            state = seed;
        }

        /// <summary>
        /// Allows you to change the seed of the Zrand class (if you set the seed to the same as creation it's basically a reset)
        /// </summary>
        /// <param name="seed">New Seed</param>
        /// <param name="increment">New Increment</param>
        public void ReSeed(ulong seed, ulong increment)
        {
            state = seed;
            inc = increment;
        }

        // ----- 64 bit integer generators -----

        /// <summary>
        /// Similar to the System.Random next function, this provides a random 64 bit unsigned integer.
        /// </summary>
        /// <returns>Random value ulong</returns>
        public ulong Next()
        {
            ulong oldState = state;

            state = oldState * multiplier + inc;

            int count = (int)(oldState >> 122);
            ulong x64 = oldState ^ (oldState >> 64);

            return (x64 >> count) | (x64 << (64 - count));
        }

        /// <summary>
        /// imilar to the System.Random next function, this provides a random 64 bit signed integer.
        /// </summary>
        /// <returns>Random value long</returns>
        public long NextSigned()
        {
            ulong oldState = state;

            state = oldState * multiplier + inc;

            int count = (int)(oldState >> 122);
            ulong x64 = oldState ^ (oldState >> 64);

            ulong result = (x64 >> count) | (x64 << (64 - count));
            return unchecked((long)result); // interpret the result as a signed integer instead of an unsigned one, should work right??????????
        }

        /// <summary>
        /// Provides a random 64 bit unsigned integer ranging from your minimum (inclusive) number to your maximum (inclusive) number.
        /// </summary>
        /// <param name="min">Minimum number to return (Inclusive)</param>
        /// <param name="max">Maximum number to return (Inclusive)</param>
        /// <returns>Random ulong from min to max.</returns>
        public ulong NextRange(ulong min, ulong max)
        {
            ulong range_width = max - min + 1;
            return min + (Next() % range_width);
        }

        /// <summary>
        /// Provides a random 64 bit signed integer ranging from your minimum (inclusive) number to your maximum (inclusive) number.
        /// </summary>
        /// <param name="min">Minimum number to return (Inclusive)</param>
        /// <param name="max">Maximum number to return (Inclusive)</param>
        /// <returns>Random long from min to max.</returns>
        public long NextSignedRange(long min, long max)
        {
            long range_width = max - min + 1;
            return min + (NextSigned() % range_width);
        }

        // ----- wrappers to convert to 32 bit integers -----

        public uint Next32()
        {
            return (uint)(NextFloat() * uint.MaxValue);
        }

        // ----- wrappers to convert to double -----

        /// <summary>
        /// Provides a random 64 bit floating point number (or double as its called in c#) that ranges from 0-1 | (note the statistical odds you get 0 or 1 is probably next to zero, if at all possible with the this is coded).
        /// </summary>
        /// <returns>Random double from 0.0 - 1.0</returns>
        public double NextDouble() // 0-1
        {
            return (Double)(Next() / ulong.MaxValue);
        }

        /// <summary>
        /// Similar to NextDouble, this does the same operation to get a 0-1 double, but then does the math to change that 0-1 into a different range for you.
        /// </summary>
        /// <param name="min">Minimum number to return (Likely Exclusive)</param>
        /// <param name="max">Maximum number to return (likely Exclusive)</param>
        /// <returns>Double between min and max</returns>
        public double NextDoubleRange(double min, double max)
        {
            return min + (max - min) * (Next() / (double)ulong.MaxValue);
        }

        // ----- wrappers to convert to float -----

        /// <summary>
        /// provides a random 32 bit floating point number that ranges from 0-1 | (note the statistical odds you get 0 or 1 is probably next to zero, if at all possible with the this is coded).
        /// </summary>
        /// <returns>Random float from 0.0 - 1.0</returns>
        public float NextFloat()
        {
            return (float)(Next() / ulong.MaxValue);
        }

        /// <summary>
        /// Similar to NextFloat, this does the same operation to get a 0-1 float, but then does the math to change that 0-1 into a different range for you.
        /// </summary>
        /// <param name="min">Minimum number to return (Likely Exclusive)</param>
        /// <param name="max">Maximum number to return (likely Exclusive)</param>
        /// <returns>Float between min and max</returns>
        public float NextFloatRange(float min, float max)
        {
            return min + (max - min) * (Next() / (float)ulong.MaxValue);
        }
    }

    /* 
     * Due to constraints known as im a lazy bum, we're gonna stick with Zrand64 and just provide it some wrapper functions to get 32 bit results :D
     * 
     * 
    /// <summary>
    /// Zrand is a random number generation class that uses the PCG RNG family of algorithms, which are code line efficient, memory efficient, speed efficient, and pretty random compared to other PRNGs. (Dont ask my source, I trusted their site :D) | 
    /// Specifically this is the PCG-XSH-RR 64/32 algorithm. which has a state of 64 bits, and can output 32 bit numbers, and is one of the faster members in the family, and is statistically superior to the algorithm used in Zrand64.
    /// Uses ~16 bytes of memory when existing
    /// </summary>
    public class Zrand32
    {
        ulong state;
        ulong inc;

        static ulong multiplier = 6364136223846793005u;

        public uint Next()
        {
            ulong oldState = state;

            state = state * multiple + inc;

            int count = (int)(oldState >> 59);
            oldState ^= oldState >> 18;
            uint x = (uint)(oldState >> 27);

            return (x >> count) | (x << (32 - count));
        }
    }
    */

}