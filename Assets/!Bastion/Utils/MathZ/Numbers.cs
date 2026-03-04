using UnityEngine;
using System;

namespace Bastion.Utils.MathZ
{
    // Most of this is useless fluff I did when starting the Utils and obviously before splitting the utils up into Bastion where half of it isn't in the utils folder
    // I *may* remove at some point, so avoid using half of this, Math and Mahtf both contain .pow functions iirc, and you can cube/square something easily by yourself so.

    /// <summary>
    /// Doing math with El Numeros
    /// </summary>
    static public class Numbers
    {
        /// <summary>
        /// Provides the Square Value of a number.
        /// </summary>
        /// <param name="x">Value to square</param>
        /// <returns>Sqaured Value of X</returns>
        [Obsolete("Function Deprecated for perfromance reasons. It's quicker for you to do the math yourself in a script (sub milisecond save). May be removed in the future.")]
        static public double Sqr(double x) { return x * x; }
        /// <summary>
        /// Provides the Square Value of a number.
        /// </summary>
        /// <param name="x">Value to square</param>
        /// <returns>Sqaured Value of X</returns>
        [Obsolete("Function Deprecated for perfromance reasons. It's quicker for you to do the math yourself in a script (sub milisecond save). May be removed in the future.")]
        static public float Sqr(float x) { return x * x; }
        /// <summary>
        /// Provides the Square Value of a number.
        /// </summary>
        /// <param name="x">Value to square</param>
        /// <returns>Sqaured Value of X</returns>
        [Obsolete("Function Deprecated for perfromance reasons. It's quicker for you to do the math yourself in a script (sub milisecond save). May be removed in the future.")]
        static public int Sqr(int x) { return x * x; }
        /// <summary>
        /// Provides the Square Value of a number.
        /// </summary>
        /// <param name="x">Value to square</param>
        /// <returns>Sqaured Value of X</returns>
        [Obsolete("Function Deprecated for perfromance reasons. It's quicker for you to do the math yourself in a script (sub milisecond save). May be removed in the future.")]
        static public long Sqr(long x) { return x * x; }
        /// <summary>
        /// Provides the Square Value of a number.
        /// </summary>
        /// <param name="x">Value to square</param>
        /// <returns>Sqaured Value of X</returns>
        [Obsolete("Function Deprecated for perfromance reasons. It's quicker for you to do the math yourself in a script (sub milisecond save). May be removed in the future.")]
        static public short Sqr(short x) { return (short)(x * x); }
        /// <summary>
        /// Provides the Square Value of a number.
        /// </summary>
        /// <param name="x">Value to square</param>
        /// <returns>Sqaured Value of X</returns>
        [Obsolete("Function Deprecated for perfromance reasons. It's quicker for you to do the math yourself in a script (sub milisecond save). May be removed in the future.")]
        static public uint Sqr(uint x) { return x * x; }
        /// <summary>
        /// Provides the Square Value of a number.
        /// </summary>
        /// <param name="x">Value to square</param>
        /// <returns>Sqaured Value of X</returns>
        [Obsolete("Function Deprecated for perfromance reasons. It's quicker for you to do the math yourself in a script (sub milisecond save). May be removed in the future.")]
        static public ulong Sqr(ulong x) { return x * x; }
        /// <summary>
        /// Provides the Square Value of a number.
        /// </summary>
        /// <param name="x">Value to square</param>
        /// <returns>Sqaured Value of X</returns>
        [Obsolete("Function Deprecated for perfromance reasons. It's quicker for you to do the math yourself in a script (sub milisecond save). May be removed in the future.")]
        static public ushort Sqr(ushort x) { return (ushort)(x * x); }
        /// <summary>
        /// Provides the Cubed Value of a number.
        /// </summary>
        /// <param name="x">Value to cube</param>
        /// <returns>Cubed Value of X</returns>
        [Obsolete("Function Deprecated for perfromance reasons. It's quicker for you to do the math yourself in a script (sub milisecond save). May be removed in the future.")]
        static public double Cube(double x) { return x * x * x; }
        /// <summary>
        /// Provides the Cubed Value of a number.
        /// </summary>
        /// <param name="x">Value to cube</param>
        /// <returns>Cubed Value of X</returns>
        [Obsolete("Function Deprecated for perfromance reasons. It's quicker for you to do the math yourself in a script (sub milisecond save). May be removed in the future.")]
        static public float Cube(float x) { return x * x * x; }
        /// <summary>
        /// Provides the Cubed Value of a number.
        /// </summary>
        /// <param name="x">Value to cube</param>
        /// <returns>Cubed Value of X</returns>
        [Obsolete("Function Deprecated for perfromance reasons. It's quicker for you to do the math yourself in a script (sub milisecond save). May be removed in the future.")]
        static public int Cube(int x) { return x * x * x; }
        /// <summary>
        /// Provides the Cubed Value of a number.
        /// </summary>
        /// <param name="x">Value to cube</param>
        /// <returns>Cubed Value of X</returns>
        [Obsolete("Function Deprecated for perfromance reasons. It's quicker for you to do the math yourself in a script (sub milisecond save). May be removed in the future.")]
        static public long Cube(long x) { return x * x * x; }
        /// <summary>
        /// Provides the Cubed Value of a number.
        /// </summary>
        /// <param name="x">Value to cube</param>
        /// <returns>Cubed Value of X</returns>
        [Obsolete("Function Deprecated for perfromance reasons. It's quicker for you to do the math yourself in a script (sub milisecond save). May be removed in the future.")]
        static public short Cube(short x) { return (short)(x * x * x); }
        /// <summary>
        /// Provides the Cubed Value of a number.
        /// </summary>
        /// <param name="x">Value to cube</param>
        /// <returns>Cubed Value of X</returns>
        [Obsolete("Function Deprecated for perfromance reasons. It's quicker for you to do the math yourself in a script (sub milisecond save). May be removed in the future.")]
        static public uint Cube(uint x) { return x * x * x; }
        /// <summary>
        /// Provides the Cubed Value of a number.
        /// </summary>
        /// <param name="x">Value to cube</param>
        /// <returns>Cubed Value of X</returns>
        [Obsolete("Function Deprecated for perfromance reasons. It's quicker for you to do the math yourself in a script (sub milisecond save). May be removed in the future.")]
        static public ulong Cube(ulong x) { return x * x * x; }
        /// <summary>
        /// Provides the Cubed Value of a number.
        /// </summary>
        /// <param name="x">Value to cube</param>
        /// <returns>Cubed Value of X</returns>
        [Obsolete("Function Deprecated for perfromance reasons. It's quicker for you to do the math yourself in a script (sub milisecond save). May be removed in the future.")]
        static public ushort Cube(ushort x) { return (ushort)(x * x * x); }
        /// <summary>
        /// Returns X to the Y power.
        /// </summary>
        /// <param name="x">Number to raise to Y power</param>
        /// <param name="y">power to raise X to</param>
        /// <returns>X^Y / X to the Y power</returns>
        [Obsolete("Function deprecated due to being outright worse than the built in System and unity library; See MathF.Pow / Math.Pow (or just do the multiplication yourself in the instance of solid integers because thats cheaper!)")]
        static public double Power(double x, double y)
        {
            double temp = x;
            for (int i = 0; i <= y; i++)
            {
                temp = temp * x;
            }
            return temp;
        }
        /// <summary>
        /// Returns X to the Y power.
        /// </summary>
        /// <param name="x">Number to raise to Y power</param>
        /// <param name="y">power to raise X to</param>
        /// <returns>X^Y / X to the Y power</returns>
        [Obsolete("Function deprecated due to being outright worse than the built in System and unity library; See MathF.Pow / Math.Pow (or just do the multiplication yourself in the instance of solid integers because thats cheaper!)")]
        static public float Power(float x, float y)
        {
            float temp = x;
            for (int i = 0; i <= y; i++)
            {
                temp = temp * x;
            }
            return temp;
        }

        // actually useful stuff starts here -zink

        /// <summary>
        /// Authored: Dgoyette, Cole Hauser
        /// Rounds to the nearest multiple
        /// </summary>
        /// <param name="inputValue">The Number to round</param>
        /// <param name="baseNumberOfMultiple">Multiple to round to</param>
        /// <returns>InputValue rounded to the nearest occurance of the Multiple as a float.</returns>
        static public float RoundToMultiple(float inputValue, float baseNumberOfMultiple)
        {
            return Mathf.Round(inputValue / baseNumberOfMultiple) * baseNumberOfMultiple;
        }
        /// <summary>
        /// Authored: Bunny83, Dgoyette, Cole Hauser
        /// Override made by Bunny83 that allows use of tOffset when rounding.
        /// </summary>
        /// <param name="inputValue">The Number to round</param>
        /// <param name="baseNumberOfMultiple">Multiple to round to</param>
        /// <param name="tOffset">I dunno... its an offset? but how does it apply what does it do!? Cole doesn't even remember</param>
        /// <returns>InputValue rounded to the nearest occurance of the Multiple as a float. Using tOffset to offset the result.</returns>
        static public float RoundToMultiple(float inputValue, float baseNumberOfMultiple, float tOffset)
        {
            return Mathf.Round((inputValue - tOffset) / baseNumberOfMultiple) * baseNumberOfMultiple + tOffset;
        }
    }
}
