using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Models.Workplace;

namespace Api
{
    public class HelsedirMenuService : IHelsedirMenuService
    {
        private readonly IHelsedirMenuFetcher _menuFetcher;

        public HelsedirMenuService(IHelsedirMenuFetcher menuFetcher)
        {
            _menuFetcher = menuFetcher;
        }

        public async Task<Dictionary<string, List<string>>> FetchWeeklyMenu()
        {
            return ParseWeeklyTextMenu(await FetchEntireMenuAsText());
        }

        public async Task<IEnumerable<string>> FetchDailyMenu()
        {
            var lines = ParseDailyTextMenu(await FetchEntireMenuAsText());

            return lines;
        }

        public async Task<string> FetchEntireMenuAsText()
        {
            var menu = await _menuFetcher.ReadMenu();

            var menuAsText = ReadMenuResponseAsText(menu);

            return menuAsText;
        }


        private static string ReadMenuResponseAsText(WorkplaceResponse parseResponse)
        {
            var text = parseResponse.Body.Data?.First().Description;
            text = text?.Replace("¬", string.Empty);

            text = FjernHtmlTags(text);

            return text;
        }

        private static IEnumerable<string> ParseDailyTextMenu(string text)
        {
            var lines = text.Split("<br>", StringSplitOptions.RemoveEmptyEntries);

            return lines.Select(l => l.Trim());
        }

        private static string FjernHtmlTags(string orgtext)
        {
            var text = orgtext.Replace("Allergener står oppført under lunsjen i personalrestauranten.", string.Empty);

            text = text.Replace("<br>", Environment.NewLine);

            text = text.Replace("<p><strong>", Environment.NewLine);

            text = text
                .Replace("<p>", string.Empty)
                .Replace("</p>", string.Empty)
                .Replace("<strong>", string.Empty)
                .Replace("</strong>", " ");

            text = text.Replace("Varmrett:", Environment.NewLine);
            text = text.Replace("Suppe:", Environment.NewLine);
            text = text.Replace("Dessert:", Environment.NewLine);
            text = text.Replace(":", Environment.NewLine);

            // Generisk fjern alle tags
            while (text.Contains("<"))
            {
                var startBracket = text.IndexOf("<", StringComparison.Ordinal);
                var endBracket = text.IndexOf(">", startBracket, StringComparison.Ordinal);

                if (endBracket == -1) break;

                text = text.Remove(startBracket, (endBracket + 1) - startBracket);
            }

            return text;
        }

        private static Dictionary<string, List<string>> ParseWeeklyTextMenu(string text)
        {
            var dic = new Dictionary<string, List<string>>();
            var lines = text.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            string currentDay = null;
            List<string> currentDaysMenu = null;

            foreach (var line in lines)
            {
                var l = line.Trim();
                if (string.IsNullOrWhiteSpace(l)) continue;

                if (IsNewDay(l))
                {
                    if (currentDaysMenu != null)
                    {
                        dic.Add(currentDay, currentDaysMenu);
                    }
                    currentDay = l;
                    currentDaysMenu = new List<string>();
                }
                else
                {
                    if (currentDaysMenu == null) continue;
                    currentDaysMenu.Add(l);
                }
            }

            if (!string.IsNullOrWhiteSpace(currentDay))
            {
                dic.Add(currentDay, currentDaysMenu);
            }

            return dic;
        }

        private static bool IsNewDay(string line)
        {
            line = line.ToLower();

            return line.StartsWith("mandag")
                || line.StartsWith("tirsdag")
                || line.StartsWith("onsdag")
                || line.StartsWith("torsdag")
                || line.StartsWith("fredag");
        }
    }
}
