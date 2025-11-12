using System;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace HtmlParserProject
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== HTML Parser ===");
            Console.WriteLine("Zadej cestu k HTML souboru:");
            string path = Console.ReadLine();

            string obsahHtml;

            if (File.Exists(path))
            {
                obsahHtml = File.ReadAllText(path);
                Console.WriteLine("Soubor načten úspěšně.");
            }
            else
            {
                Console.WriteLine("Soubor nenalezen. Zadej HTML ručně (ukonči prázdným řádkem):");
                obsahHtml = CistTextRadky();
            }

            Console.WriteLine("\n--- Načtený HTML obsah ---");
            Console.WriteLine(obsahHtml);

            var povoleneTagy = NactiPovoleneTagy(@"..\..\..\tags.txt");
            Console.WriteLine($"Načteno {povoleneTagy.Count} povolených tagů.");

            ParseHtml(obsahHtml);
        }

        static string CistTextRadky()
        {
            string radek;
            string vysledek = "";
            while (!string.IsNullOrWhiteSpace(radek = Console.ReadLine()))
                vysledek += radek + "\n";
            return vysledek;
        }

        static HashSet<string> NactiPovoleneTagy(string path)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine($"Soubor {path} nebyl nalezen!");
                return new HashSet<string>();
            }

            var tagy = File.ReadAllLines(path);
            var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var tag in tagy)
            {
                var trimS = tag.Trim();
                if (!string.IsNullOrEmpty(trimS))
                    set.Add(trimS);
            }

            return set;
        }

        static void ParseHtml(string html)
        {
            Regex tagRegex = new Regex(@"(<[^>]+>)");
            var prvky = tagRegex.Split(html);

            Console.WriteLine("\n--- Rozdělení HTML ---");
            foreach (var prvek in prvky)
            {
                if (string.IsNullOrWhiteSpace(prvek)) continue;

                if (prvek.StartsWith("<"))
                    Console.WriteLine($"[TAG]  {prvek}");
                else
                    Console.WriteLine($"[TEXT] {prvek.Trim()}");
            }
        }
    }
}
