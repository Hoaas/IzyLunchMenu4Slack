namespace IzyAsLunchMenu;

public record GetDishesResponse(
    Body body,
    Status status
);

public record Body(
    int week,
    Day Monday,
    Day Tuesday,
    Day Wednesday,
    Day Thursday,
    Day Friday,
    Day Saturday,
    Day Sunday
);

public record Day(
    string date,
    List<Menus> menus
);

public record Menus(
    int canteen_id,
    string canteen_name,
    List<Dishes> dishes
);

public record Dishes(
    int id,
    string name,
    int position,
    List<object> food_allergen_labels
);

public record Status(
    string message,
    string code,
    string code_text,
    string response_timestamp
);

