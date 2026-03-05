using Bastion.ConsoleV2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using UnityEngine;

namespace Bastion
{
    public static class PersistanceManager
    {
        public static bool Save<T>(T data, string fileName = "save", string fileExtension = ".dat", bool useEncryption = false, bool verbose = false) where T : class
        {
            string logPrefix = $"{Branding.engineLogPrefix}[PersistanceManager.Save ]";

            if (fileExtension[0] != '.')
            {
                if (verbose)
                    Debug.Log(logPrefix + "file extension did not start with \".\"! a \".\" was added for you!");

                fileExtension = "." + fileExtension;
            }

            if (fileName == "blank" || fileName == "")
            {
                //fileName = data.name;
            }

            string path = Application.persistentDataPath + "/saves/" + fileName + fileExtension;

            string serializedData = JsonUtility.ToJson(data);

            if (useEncryption)
            {
                //serializedData = EncryptDecrypt(serializedData, "ExampleForNow");
            }

            try
            {
                using (StreamWriter sw = new StreamWriter(path))
                {
                    sw.Write(serializedData);
                }

                return true;
            }
            catch(Exception e)
            {
                //uh oh
                Debug.LogWarning(logPrefix + "ran into an exception that it has no way of handling!\n" + e.Message + "\n" + e.StackTrace);
                return false;
            }
        }

        public static T load<T>(string fileName = "save", string fileExtension = ".dat", bool useEncryption = false, bool verbose = false) where T : class
        {
            return null;   
        }

        [Command("Test Encrypt func", false, true)]
        public static string Encrypt(string input, string secret)
        {
            string output = "";

            byte adder = 69;

            for (int c = 0; c < input.Length; c++)
            {
                byte[] cb = BitConverter.GetBytes(input[c]);

                for (int s = 0; s < secret.Length; s++)
                {
                    byte[] sb = BitConverter.GetBytes(secret[s]);

                    if ((byte)(sb[1] + adder) % 2 == 0)
                    {
                        cb[0] = RotateRight(cb[0], (int)sb[0] + adder);
                    }
                    else
                    {
                        cb[0] = RotateLeft(cb[0], (int)sb[0] + adder);
                    }

                    adder += (byte)(sb[0] - sb[1]);

                    if (adder % 2 == 0)
                    {
                        adder = RotateRight(adder, sb[1] + sb[0] - adder + secret.Length);
                    }
                    else
                    {
                        adder = RotateLeft(adder, sb[1] - sb[0] + adder - secret.Length);
                    }
                }

                for (int s = secret.Length-1; s > -1; s--)
                {
                    byte[] sb = BitConverter.GetBytes(secret[s]);

                    if ((byte)(sb[0] + adder) % 2 == 0)
                    {
                        cb[1] = RotateLeft(cb[1], (int)sb[1] + adder);
                    }
                    else
                    {
                        cb[1] = RotateRight(cb[1], (int)sb[1] + adder);
                    }

                    adder += (byte)(sb[0] - sb[1]);

                    if (adder % 2 == 0)
                    {
                        adder = RotateRight(adder, sb[1] + sb[0] - adder + secret.Length);
                    }
                    else
                    {
                        adder = RotateLeft(adder, sb[1] - sb[0] + adder - secret.Length);
                    }
                }

                output += BitConverter.ToChar(cb);
            }

            return output;
        }

        // thanks to google AI for inversing my function in like 45 seconds with only one error. huge time save
        public static string Decrypt(string input, string secret)
        {
            string output = "";
            byte adder = 69; // Must match the starting value in Encrypt

            for (int c = 0; c < input.Length; c++)
            {
                byte[] cb = BitConverter.GetBytes(input[c]);

                // --- STEP 1: Replicate Adder Progression for this character ---
                // We need to know what the adder values were at specific points 
                // in the Encrypt loops to reverse them.
                byte initialAdderForThisChar = adder;
                byte[] loop1Adders = new byte[secret.Length];

                // Simulate Loop 1
                for (int s = 0; s < secret.Length; s++)
                {
                    loop1Adders[s] = adder; // Store adder BEFORE it is updated
                    byte[] sb = BitConverter.GetBytes(secret[s]);
                    adder += (byte)(sb[0] - sb[1]);
                    if (adder % 2 == 0)
                        adder = RotateRight(adder, sb[1] + sb[0] - adder + secret.Length);
                    else
                        adder = RotateLeft(adder, sb[1] - sb[0] + adder - secret.Length);
                }

                // Simulate Loop 2
                byte[] loop2Adders = new byte[secret.Length];
                for (int s = secret.Length - 1; s > -1; s--)
                {
                    loop2Adders[s] = adder; // Store adder BEFORE it is updated
                    byte[] sb = BitConverter.GetBytes(secret[s]);
                    adder += (byte)(sb[0] - sb[1]);
                    if (adder % 2 == 0)
                        adder = RotateRight(adder, sb[1] + sb[0] - adder + secret.Length);
                    else
                        adder = RotateLeft(adder, sb[1] - sb[0] + adder - secret.Length);
                }

                // --- STEP 2: Reverse the Rotations in strict reverse order ---

                // Reverse Loop 2 (Affects cb[1])
                for (int s = 0; s < secret.Length; s++) // Reverse order of Loop 2 (0 to Length-1)
                {
                    byte[] sb = BitConverter.GetBytes(secret[s]);
                    byte adderAtThisStep = loop2Adders[s];

                    if ((byte)(sb[0] + adderAtThisStep) % 2 == 0)
                        cb[1] = RotateRight(cb[1], (int)sb[1] + adderAtThisStep); // Inverse of Left
                    else
                        cb[1] = RotateLeft(cb[1], (int)sb[1] + adderAtThisStep);  // Inverse of Right
                }

                // Reverse Loop 1 (Affects cb[0])
                for (int s = secret.Length - 1; s > -1; s--) // Reverse order of Loop 1 (Length-1 to 0)
                {
                    byte[] sb = BitConverter.GetBytes(secret[s]);
                    byte adderAtThisStep = loop1Adders[s];

                    if ((byte)(sb[1] + adderAtThisStep) % 2 == 0)
                        cb[0] = RotateLeft(cb[0], (int)sb[0] + adderAtThisStep); // Inverse of Right
                    else
                        cb[0] = RotateRight(cb[0], (int)sb[0] + adderAtThisStep); // Inverse of Left
                }

                output += BitConverter.ToChar(cb, 0);
            }

            return output;
        }

        private static byte RotateRight(byte b, int bits)
        {
            bits %= 8;

            return (byte)((b >> bits) | (b << (8 - bits)));
        }

        private static byte RotateLeft(byte b, int bits)
        {
            bits %= 8;

            return (byte)((b << bits) | (b >> (8 - bits)));
        }
    }
}