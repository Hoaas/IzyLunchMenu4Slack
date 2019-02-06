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

        public async Task<Dictionary<string, List<string>>> FetchMenu()
        {
            var menu = await _menuFetcher.ReadMenu();

            var menuAsText = ReadMenuResponseAsText(menu);
            return ParseTextMenu(menuAsText);
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
            return text;
        }

        private static Dictionary<string, List<string>> ParseTextMenu(string text)
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
            dic.Add(currentDay, currentDaysMenu);

            return dic;
        }

        private static bool IsNewDay(string line)
        {
            line = line.ToLower();

            return line == "mandag"
                || line == "tirsdag"
                || line == "onsdag"
                || line == "torsdag"
                || line == "fredag";
        }
    }
}
