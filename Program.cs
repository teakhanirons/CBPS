using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace nidbruteforcer {
    class Program {
        public static string ValidChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890_";

        public static void Force(string prefix, string suffix, string nid, int length, int level) {
            level++;
            foreach (char c in ValidChars) {
                string test = prefix + c + suffix;
                byte[] data = UTF8Encoding.UTF8.GetBytes(test);
                using (SHA1Managed sha1 = new SHA1Managed()) {
                    data = sha1.ComputeHash(data);
                }
                byte[] swapped = new byte[4];
                swapped[0] = data[3];
                swapped[1] = data[2];
                swapped[2] = data[1];
                swapped[3] = data[0];
                string shad = BitConverter.ToString(swapped).Replace("-", "");
                Console.WriteLine(test + " -> " + shad);
                if(shad == nid) {
                    Console.WriteLine("\nFound it!");
                    Console.WriteLine(test + ": " + nid);
                    Environment.Exit(0);
                }
                if (level < length) {
                    Force(prefix + c, suffix, nid, length, level);
                }
            }
        }
        static void Main(string[] args) {
            Console.WriteLine("\nC# Bruteforcer with Prefix and Suffix (CBPS) by Team CBPS");
            Console.WriteLine("USAGE:");
            Console.WriteLine("nidbruteforcer.exe NID KNOWN_PREFIX KNOWN_SUFFIX");
            Console.WriteLine("KNOWN_PREFIX and KNOWN_SUFFIX are optional.\n");
            string prefix = "";
            string suffix = "";

            if(args.Length == 0) {
                Console.WriteLine("Need a NID.");
                Environment.Exit(0);
            }
            string nid = args[0];

            if (nid.Length % 2 != 0) {
                Console.WriteLine(nid + " is not a NID.");
                Environment.Exit(0);
            }
            Console.WriteLine("Got " + args.Length + " arguments.");
            if (args.Length > 1) {
                prefix = args[1];
                Console.WriteLine("Using " + prefix + " as prefix.");
            }
            if (args.Length > 2) {
                suffix = args[2];
                Console.WriteLine("Using " + suffix + " as suffix.");
            }

            if (nid.Contains("0x")) {
                nid = nid.Substring(2, nid.Length - 2);
            }
            nid = nid.ToUpper();

            Console.WriteLine("Using " + nid + " as NID.");
            Console.WriteLine("Starting in 5 seconds.");
            Thread.Sleep(5 * 1000);

            int length = 0;
            while (true) {
                length++;
                Console.WriteLine("Length: " +length);
                Force(prefix, suffix, nid, length, 0);
            }
        }

    }
}
