using Bastion.ConsoleV2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bastion
{
    public static class PersistanceManager
    {
        public static bool Save<T>(T data, string fileName = "save", string fileExtension = ".dat", bool useEncryption = false, bool verbose = false) where T : class
        {
            string logPrefix = $"{Branding.engineLogPrefix}[PersistanceManager.Save<T>()] ";

            if (fileExtension[0] != '.')
            {
                if (verbose)
                    Debug.Log(logPrefix + "file extension did not start with \".\"! a \".\" was added for you!");

                fileExtension = "." + fileExtension;
            }

            if (fileName == "blank" || fileName == "")
            {
                fileName = typeof(T).Name;
            }

            string path = Application.persistentDataPath + "/saves/" + fileName + fileExtension;

            string serializedData = JsonUtility.ToJson(data);

            if (useEncryption)
            {
                serializedData = Encrypt(serializedData, "0205cbcc699dce8e10a1b0d9bd9ba4f86ca7da5675af67057802dfd5c7aa932d");
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
            string logPrefix = $"{Branding.engineLogPrefix}[PersistanceManager.Load<T>()] ";

            if (fileExtension[0] != '.')
            {
                if (verbose)
                    Debug.Log(logPrefix + "file extension did not start with \".\"! a \".\" was added for you!");

                fileExtension = "." + fileExtension;
            }

            if (fileName == "blank" || fileName == "")
            {
                fileName = typeof(T).Name;
            }

            string path = Application.persistentDataPath + "/saves/" + fileName + fileExtension;

            string result = "";

            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    string line = "";

                    while ((line = sr.ReadLine()) != null)
                    {
                        result += line;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning(logPrefix + "ran into an exception that it has no way of handling!\n" + e.Message + "\n" + e.StackTrace);
            }

            if (useEncryption)
            {
                Decrypt(result, "0205cbcc699dce8e10a1b0d9bd9ba4f86ca7da5675af67057802dfd5c7aa932d");
            }

            return JsonUtility.FromJson<T>(result);
        }

        [Command("Test Encrypt func", false, true)]
        public static string Encrypt(string input, string secret)
        {
            string output = "";

            System.Random ivCreator = new System.Random();

            ulong IV = (ulong)(NextUInt64(ivCreator));
            ulong IV2 = (ulong)(NextUInt64(ivCreator));

            System.Random rand = new System.Random(FoldSeed(IV2));
            
            byte adder = (byte)(IV % 255);

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
                        adder = RotateRight(adder, sb[1] + sb[0] - adder + secret.Length - (byte)(IV % 255));
                    }
                    else
                    {
                        adder = RotateLeft(adder, sb[1] - sb[0] + adder - secret.Length + (byte)(IV % 255));
                    }

                    IV += NextUInt64(rand);
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
                        adder = RotateRight(adder, sb[1] + sb[0] - adder + secret.Length + (byte)(IV % 255));
                    }
                    else
                    {
                        adder = RotateLeft(adder, sb[1] - sb[0] + adder - secret.Length - (byte)(IV % 255));
                    }

                    IV -= NextUInt64(rand);
                }

                output += BitConverter.ToChar(cb, 0);
            }

            byte[] IVtoBytes = BitConverter.GetBytes(IV);
            byte[] IV2toBytes = BitConverter.GetBytes(IV2);

            List<char> cList = new List<Char>();

            string start = "";
            string end = "";

            byte[] bytes = new byte[2];

            for (int i = 0; i < 4; i++)
            {
                bytes[0] = IVtoBytes[i]; // 0 , 1 , 2 , 3
                bytes[1] = IVtoBytes[IVtoBytes.Length - 1 - i]; // 7 , 6 , 5 , 4

                cList.Add(BitConverter.ToChar(bytes));
            }

            for (int i = 0; i < 4; i++)
            {
                bytes[0] = IV2toBytes[i]; // 0 , 1 , 2 , 3
                bytes[1] = IV2toBytes[IV2toBytes.Length - 1 - i]; // 7 , 6 , 5 , 4

                cList.Add(BitConverter.ToChar(bytes));
            }

            for (int i = 0; i < 4; i++)
            {
                start += cList[i];
            }

            for (int i = 4; i < 8; i++)
            {
                end += cList[i];
            }

            return start + output + end;
        }

        // thanks to GPT for inversing my function in like 45 seconds with only one error. huge time save
        public static string Decrypt(string input, string secret)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            if (string.IsNullOrEmpty(secret))
                throw new ArgumentException("Secret cannot be null or empty.", nameof(secret));

            // 4 chars of IV at start, 4 chars of IV2 at end
            if (input.Length < 8)
                throw new ArgumentException("Encrypted input is too short.", nameof(input));

            string ivString = input.Substring(0, 4);
            string body = input.Substring(4, input.Length - 8);
            string iv2String = input.Substring(input.Length - 4, 4);

            ulong finalIV = CharsToULong(ivString);
            ulong IV2 = CharsToULong(iv2String);

            int charCount = body.Length;
            int opsPerChar = secret.Length * 2;

            // Rebuild the exact random stream used during encryption
            System.Random rand = new System.Random(FoldSeed(IV2));
            ulong[] deltas = new ulong[charCount * opsPerChar];

            for (int i = 0; i < deltas.Length; i++)
            {
                deltas[i] = NextUInt64(rand);
            }

            // Recover the original IV from the final IV
            ulong initialIV = finalIV;

            unchecked
            {
                for (int c = 0; c < charCount; c++)
                {
                    int baseIndex = c * opsPerChar;

                    // Undo the additions from the first secret loop
                    for (int i = 0; i < secret.Length; i++)
                        initialIV -= deltas[baseIndex + i];

                    // Undo the subtractions from the second secret loop
                    for (int i = 0; i < secret.Length; i++)
                        initialIV += deltas[baseIndex + secret.Length + i];
                }
            }

            char[] output = new char[charCount];

            ulong IV = initialIV;
            byte adder = (byte)(IV % 255);

            for (int c = 0; c < charCount; c++)
            {
                byte[] cb = BitConverter.GetBytes(body[c]);

                var firstOps = new List<(bool rotatedRight, int amount)>(secret.Length);
                var secondOps = new List<(bool rotatedLeft, int amount)>(secret.Length);

                int baseIndex = c * opsPerChar;

                // Simulate first half exactly as Encrypt does
                for (int s = 0; s < secret.Length; s++)
                {
                    byte[] sb = BitConverter.GetBytes(secret[s]);

                    bool didRotateRight = ((byte)(sb[1] + adder) % 2 == 0);
                    int rotateAmount = (int)sb[0] + adder;

                    firstOps.Add((didRotateRight, rotateAmount));

                    unchecked
                    {
                        adder += (byte)(sb[0] - sb[1]);

                        if (adder % 2 == 0)
                        {
                            adder = RotateRight(
                                adder,
                                sb[1] + sb[0] - adder + secret.Length - (byte)(IV % 255)
                            );
                        }
                        else
                        {
                            adder = RotateLeft(
                                adder,
                                sb[1] - sb[0] + adder - secret.Length + (byte)(IV % 255)
                            );
                        }

                        IV += deltas[baseIndex + s];
                    }
                }

                // Simulate second half exactly as Encrypt does
                for (int s = secret.Length - 1; s >= 0; s--)
                {
                    byte[] sb = BitConverter.GetBytes(secret[s]);

                    bool didRotateLeft = ((byte)(sb[0] + adder) % 2 == 0);
                    int rotateAmount = (int)sb[1] + adder;

                    secondOps.Add((didRotateLeft, rotateAmount));

                    unchecked
                    {
                        adder += (byte)(sb[0] - sb[1]);

                        if (adder % 2 == 0)
                        {
                            adder = RotateRight(
                                adder,
                                sb[1] + sb[0] - adder + secret.Length + (byte)(IV % 255)
                            );
                        }
                        else
                        {
                            adder = RotateLeft(
                                adder,
                                sb[1] - sb[0] + adder - secret.Length - (byte)(IV % 255)
                            );
                        }

                        int deltaIndex = baseIndex + secret.Length + (secret.Length - 1 - s);
                        IV -= deltas[deltaIndex];
                    }
                }

                // Reverse second loop on cb[1]
                for (int i = secondOps.Count - 1; i >= 0; i--)
                {
                    var op = secondOps[i];

                    if (op.rotatedLeft)
                        cb[1] = RotateRight(cb[1], op.amount);
                    else
                        cb[1] = RotateLeft(cb[1], op.amount);
                }

                // Reverse first loop on cb[0]
                for (int i = firstOps.Count - 1; i >= 0; i--)
                {
                    var op = firstOps[i];

                    if (op.rotatedRight)
                        cb[0] = RotateLeft(cb[0], op.amount);
                    else
                        cb[0] = RotateRight(cb[0], op.amount);
                }

                output[c] = BitConverter.ToChar(cb, 0);
            }

            return new string(output);
        }

        private static ulong CharsToULong(string input)
        {
            if (input == null || input.Length != 4)
                throw new ArgumentException("Input must be exactly 4 chars.", nameof(input));

            byte[] bytes = new byte[8];

            for (int i = 0; i < 4; i++)
            {
                byte[] pair = BitConverter.GetBytes(input[i]);
                bytes[i] = pair[0];
                bytes[7 - i] = pair[1];
            }

            return BitConverter.ToUInt64(bytes, 0);
        }

        private static ulong NextUInt64(System.Random rand)
        {
            byte[] bytes = new byte[8];
            rand.NextBytes(bytes);
            return BitConverter.ToUInt64(bytes, 0);
        }

        private static int FoldSeed(ulong value)
        {
            return unchecked((int)(value ^ (value >> 32)));
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
