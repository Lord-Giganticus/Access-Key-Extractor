using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace Access_Key_Extractor.Library
{
    public class RomFile
    {
        protected FileInfo InFile { get; set; }

        private RomFile() { }

        public RomFile(FileInfo file)
        {
            if (!file.Exists) throw new FileNotFoundException();
            InFile = file.Extension switch
            {
                ".elf" or ".bin" => file,
                _ => throw new Exception("File extension is not supported. It must be elf or bin."),
            };
        }

        public RomFile(string file) : this(new FileInfo(file)) { }

        public string[] GetKeys() => GetKeys(File.ReadAllBytes(InFile.FullName));

        public static string[] GetKeys(byte[] Buffer)
        {
            using var ms = new MemoryStream();
            ms.Write(Buffer, 0, Buffer.Length);
            ms.Seek(0, SeekOrigin.Begin);
            using var sr = new StreamReader(ms, Encoding.Latin1);
            var contents = sr.ReadToEnd();
            var reg = new Regex(@"([a-f0-9]\0){8}");
            var matches = reg.Matches(contents);
            if (matches.Count <= 0)
                return Array.Empty<string>();
            var possiblekeys = matches.Select(Decode).Distinct().ToArray();
            for (int i = 0; i < possiblekeys.Length; i++)
                possiblekeys[i] = possiblekeys[i][1..].Replace("\0", string.Empty);
            return possiblekeys;
        }

        protected static string Decode(Match m)
        {
            using var ms = new MemoryStream();
            using var sw = new StreamWriter(ms, Encoding.Unicode);
            sw.Write(m.Value);
            sw.Flush();
            return Encoding.Unicode.GetString(ms.ToArray());
        }
    }
}
