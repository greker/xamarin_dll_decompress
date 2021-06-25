using K4os.Compression.LZ4;
using System;
using System.IO;
using System.Linq;
using System.Text;

if (args.Length != 2)
{
    Console.WriteLine("help: xamarin_dll_decompress input.dll output.dll");
    return;
}

var inputFile = args[0];
var outputFile = args[1];
try
{
    var inputFileBytes = File.ReadAllBytes(inputFile);

    if (inputFileBytes.Length < 12 || !inputFileBytes[..4].SequenceEqual(Encoding.ASCII.GetBytes("XALZ")))
    {
        Console.WriteLine("The input file does not contain XALZ!");
        return;
    }

    var data = inputFileBytes[12..];
    var result = new byte[BitConverter.ToInt32(inputFileBytes[8..12])];
    LZ4Codec.Decode(data, 0, data.Length, result, 0, result.Length);
    File.WriteAllBytes(outputFile, result);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}