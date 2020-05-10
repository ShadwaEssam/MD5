using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Md5
{
    class Program
    {
        class MainClass
        {
            public static void Main(string[] args)
            {
                string input = "This is a security test";

                //List<string> mainKeyTriple = new List<string>() { "0x133457799BBCDFF1", "0x133457799BBCDFF1" };

                GetHash(input);

            }

            static int[] shiftTable = new int[]
            { 7, 12, 17, 22, 7, 12, 17, 22, 7, 12, 17, 22, 7, 12, 17, 22,
          5,  9, 14, 20,  5,  9, 14, 20,  5,  9, 14, 20,  5,  9, 14, 20,
          4, 11, 16, 23,  4, 11, 16, 23,  4, 11, 16, 23,  4, 11, 16, 23,
          6, 10, 15, 21,  6, 10, 15, 21,  6, 10, 15, 21,  6, 10, 15, 21  };

            static string[] constantsTable = new string[]
            {   "11010111011010101010010001111000", "11101000110001111011011101010110", "00100100001000000111000011011011", "11000001101111011100111011101110",
            "11110101011111000000111110101111", "01000111100001111100011000101010", "10101000001100000100011000010011", "11111101010001101001010100000001",
            "01101001100000001001100011011000", "10001011010001001111011110101111", "11111111111111110101101110110001", "10001001010111001101011110111110",
            "01101011100100000001000100100010", "11111101100110000111000110010011", "10100110011110010100001110001110", "01001001101101000000100000100001",
            "11110110000111100010010101100010", "11000000010000001011001101000000", "00100110010111100101101001010001", "11101001101101101100011110101010",
            "11010110001011110001000001011101", "00000010010001000001010001010011", "11011000101000011110011010000001", "11100111110100111111101111001000",
            "00100001111000011100110111100110", "11000011001101110000011111010110", "11110100110101010000110110000111", "01000101010110100001010011101101",
            "10101001111000111110100100000101", "11111100111011111010001111111000", "01100111011011110000001011011001", "10001101001010100100110010001010",
            "11111111111110100011100101000010", "10000111011100011111011010000001", "01101101100111010110000100100010", "11111101111001010011100000001100",
            "10100100101111101110101001000100", "01001011110111101100111110101001", "11110110101110110100101101100000", "10111110101111111011110001110000",
            "00101000100110110111111011000110", "11101010101000010010011111111010", "11010100111011110011000010000101", "00000100100010000001110100000101",
            "11011001110101001101000000111001", "11100110110110111001100111100101", "00011111101000100111110011111000", "11000100101011000101011001100101",
            "11110100001010010010001001000100", "01000011001010101111111110010111", "10101011100101000010001110100111", "11111100100100111010000000111001",
            "01100101010110110101100111000011", "10001111000011001100110010010010", "11111111111011111111010001111101", "10000101100001000101110111010001",
            "01101111101010000111111001001111", "11111110001011001110011011100000", "10100011000000010100001100010100", "01001110000010000001000110100001",
            "11110111010100110111111010000010", "10111101001110101111001000110101", "00101010110101111101001010111011", "11101011100001101101001110010001"};

            static Dictionary<string, string> initialVectorDict = new Dictionary<string, string>()
        {
            {"A", "01100111010001010010001100000001" },
            {"B", "11101111110011011010101110001001" },
            {"C", "10011000101110101101110011111110" },
            {"D", "00010000001100100101010001110110" }

        };

            public static string GetHash(string text)
            {
                //
                initialVectorDict = new Dictionary<string, string>()
        {
            {"A", "01100111010001010010001100000001" },
            {"B", "11101111110011011010101110001001" },
            {"C", "10011000101110101101110011111110" },
            {"D", "00010000001100100101010001110110" }

        };
                //

                string result = "";
                string myBinTxt = textToBinary(text);
                string processedInput = processInput(myBinTxt);
                List<string> M = new List<string>();
                Dictionary<string, string> IVDCopy = new Dictionary<string, string>();

                int g = 0;

                for (int i = 0; i < processedInput.Length; i += 512)
                {
                    IVDCopy = initialVectorDict.ToDictionary(x => x.Key, x => x.Value);
                    string currentBlock = processedInput.Substring(i, 512);
                    M = blockSeparator(currentBlock);
                    List<string> equationVariables = new List<string>() { "A", "B", "C", "D" };

                    for (int j = 0; j < 64; j++)
                    {
                        if ((j >= 0) && (j <= 15))
                        {
                            //f
                            result = F_Fn(IVDCopy[equationVariables[1]], IVDCopy[equationVariables[2]], IVDCopy[equationVariables[3]]);
                            //g represents index
                            g = j;
                        }
                        else if ((j >= 16) && (j <= 31))
                        {
                            //g
                            result = G_Fn(IVDCopy[equationVariables[1]], IVDCopy[equationVariables[2]], IVDCopy[equationVariables[3]]);
                            g = (5 * j + 1) % 16;
                        }
                        else if ((j >= 32) && (j <= 47))
                        {
                            //h
                            result = H_Fn(IVDCopy[equationVariables[1]], IVDCopy[equationVariables[2]], IVDCopy[equationVariables[3]]);
                            g = (3 * j + 5) % 16;
                        }
                        else if ((j >= 48) && (j <= 63))
                        {
                            //i
                            result = I_Fn(IVDCopy[equationVariables[1]], IVDCopy[equationVariables[2]], IVDCopy[equationVariables[3]]);
                            g = (7 * j) % 16;
                        }

                        /* steps for assigning B
                         * The fn value gets moduloadded to A
                           That gets moduloadded to M[g]
                           that gets shl-ed by the [j] value in the shift table
                           And finally that gets modulo added with B 
                         */

                        IVDCopy[equationVariables[0]] = moduloAdd(SHL(moduloAdd(moduloAdd(moduloAdd(result, IVDCopy[equationVariables[0]]), M[g]), constantsTable[j]), shiftTable[j]), IVDCopy[equationVariables[1]]);


                        string lastOperation = equationVariables[3];
                        equationVariables.RemoveAt(3);
                        equationVariables.Insert(0, lastOperation);
                    }

                    initialVectorDict["A"] = moduloAdd(initialVectorDict["A"], IVDCopy["A"]);
                    initialVectorDict["B"] = moduloAdd(initialVectorDict["B"], IVDCopy["B"]);
                    initialVectorDict["C"] = moduloAdd(initialVectorDict["C"], IVDCopy["C"]);
                    initialVectorDict["D"] = moduloAdd(initialVectorDict["D"], IVDCopy["D"]);
                }

                string str = string.Concat(initialVectorDict["A"],
                initialVectorDict["B"], initialVectorDict["C"],
                initialVectorDict["D"]);

                string MD5Digest = binToHexa(endianToBinary(str, 32));

                System.Console.WriteLine(MD5Digest);

                return MD5Digest;
            }

            //to prevent error in 32 bit string addition in case it results in 33 bits
            public static string moduloAdd(string first, string second)
            {
                string result = "";
                bool carry = false;

                for (int i = first.Length - 1; i >= 0; i--)
                {
                    string addition = "";
                    addition += (carry) ? "1" : "";

                    addition += (first[i] == '1') ? "1" : "";

                    addition += (second[i] == '1') ? "1" : "";

                    if (addition.Length == 0) { result = "0" + result; carry = false; }
                    else if (addition.Length == 1) { result = "1" + result; carry = false; }
                    else if (addition.Length == 2) { result = "0" + result; carry = true; }
                    else if (addition.Length == 3) { result = "1" + result; carry = true; }
                }

                return result.Substring(result.Length - 32, 32);
            }

            public static List<string> blockSeparator(string block)
            {

                List<string> resultList = new List<string>();
                for (int i = 0; i < block.Length; i += 32)
                {

                    string y = block.Substring(i, 32);
                    resultList.Add(y);

                }

                return resultList;

            }


            public static string processInput(string binText)
            {

                int Len = binText.Length;
                //to assure length not > 64 bits
                binText += 1;

                while ((binText.Length - 448) % 512 != 0)
                {
                    binText += 0;

                }

                binText = endianToBinary(binText, 32);
                string cnvrtdLen = Convert.ToString(Len, 2).PadLeft(64, '0');
                binText += cnvrtdLen.Substring(32, 32);
                binText += cnvrtdLen.Substring(0, 32);

                return binText;


            }

            public static string AND(string first, string second)
            {
                string result = "";
                for (int i = 0; i < first.Length; i++)
                {
                    if ((first[i] == '0') || (second[i] == '0'))

                        result += "0";

                    else
                        result += "1";
                }
                return result;
            }
            public static string OR(string first, string second)
            {
                string result = "";
                for (int i = 0; i < first.Length; i++)
                {
                    if ((first[i] == '1') || (second[i] == '1'))

                        result += "1";

                    else
                        result += "0";
                }
                return result;
            }

            public static string NOT(string first)
            {
                string result = "";
                for (int i = 0; i < first.Length; i++)
                {
                    if (first[i] == '1')

                        result += "0";

                    else
                        result += "1";
                }
                return result;
            }

            public static string SHL(string first, int N)
            {

                string toBeShifted = "";
                toBeShifted = first.Substring(0, N);
                string newfirst = first.Remove(0, N);
                newfirst += toBeShifted;
                return newfirst;

            }

            public static string XOR(string first, string second)
            {
                string result = "";
                for (int i = 0; i < first.Length; i++)
                {
                    if (first[i] == second[i])

                        result += "0";

                    else
                        result += "1";
                }
                return result;
            }

            public static string textToBinary(string plainText)
            {
                string result = "";
                for (int i = 0; i < plainText.Length; i++)
                {
                    result += Convert.ToString((int)plainText[i], 2).PadLeft(8, '0');
                }

                return result;

            }

            public static string endianToBinary(string endianBits, int chunckLength)
            {
                string result = "";

                for (int i = 0; i < endianBits.Length; i += chunckLength)
                {
                    string chunk = endianBits.Substring(i, chunckLength);

                    for (int j = chunk.Length; j > 0; j -= 8)
                    {
                        string oneByte = chunk.Substring(j - 8, 8);

                        result += oneByte;
                    }
                }

                return result;
            }

            public static string hexToBin(string hexaString)
            {
                var converter = new Dictionary<char, string>{
            { '0', "0000"},
            { '1', "0001"},
            { '2', "0010"},
            { '3', "0011"},

            { '4', "0100"},
            { '5', "0101"},
            { '6', "0110"},
            { '7', "0111"},

            { '8', "1000"},
            { '9', "1001"},
            { 'A', "1010"},
            { 'B', "1011"},

            { 'C', "1100"},
            { 'D', "1101"},
            { 'E', "1110"},
            { 'F', "1111"}};

                string result = "";
                for (int i = 0; i < hexaString.Length; i++)
                {
                    hexaString = hexaString.Replace("0x", "");
                    result += converter[hexaString[i]];
                }
                return result;

            }

            public static string binToHexa(string binString)
            {
                var converter = new Dictionary<string, char>{
            {"0000", '0'},
            {"0001", '1'},
            { "0010",'2'},
            { "0011", '3'},

            { "0100", '4'},
            { "0101",'5'},
            { "0110", '6'},
            {"0111", '7'},

            { "1000", '8'},
            { "1001", '9'},
            { "1010", 'A'},
            { "1011",'B'},

            { "1100",'C'},
            {"1101", 'D'},
            {"1110", 'E'},
            {"1111", 'F'}};

                string result = "";
                binString = binString.Replace("0x", "");

                for (int i = 0; i < binString.Length; i += 4)
                {

                    result += converter[binString.Substring(i, 4)];
                }
                return result;

            }

            public static string F_Fn(string B, string C, string D)
            {
                string LHS = AND(B, C);
                string RHS = AND(NOT(B), D);
                return OR(LHS, RHS);

            }

            public static string G_Fn(string B, string C, string D)
            {
                string LHS = AND(B, D);
                string RHS = AND(C, NOT(D));
                return OR(LHS, RHS);

            }

            public static string H_Fn(string B, string C, string D)
            {
                return XOR(XOR(B, C), D);

            }

            public static string I_Fn(string B, string C, string D)
            {
                return XOR(C, OR(B, NOT(D)));
            }
        }
    }
}