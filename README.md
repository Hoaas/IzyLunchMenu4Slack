# izy.as menu to Slack
Slack integration for the lunch menu for the building at [Vitaminveien 4, Oslo, Norway](https://www.google.com/maps/place/Vitaminveien+4,+0485+Oslo), but should in theory work for any building using the same system.

Menu fetched from the API behind the [Workplace Oo app](https://play.google.com/store/apps/details?id=no.fourservice.workplace). (https://workplace.izy.as/)

- Fetches menu from endpoints used by app
- Sends menu to ChatGPT to get a better query for the next step
- Searches Bing Images for an image of the menu item - picks a random one from the top 10 results

![Example image from Slack, showing menu with two items with images to the right of the text, in norwegian. Text: Meny for mandag 5. februar, Pasta Bolognese - Svart Oliven Salat, Søtpotetsuppe](https://i.imgur.com/svXeEVF.png)

## Issues
You need to use a user account to get access, but this causes the user to be logged out other places ... so that's a bit annoying.

[![Build Status](https://dev.azure.com/hoaas/GitHub%20Releases/_apis/build/status%2FHoaas.IzyLunchMenu4Slack?branchName=main)](https://dev.azure.com/hoaas/GitHub%20Releases/_build/latest?definitionId=11&branchName=main)

🍝
