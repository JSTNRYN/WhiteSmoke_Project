public enum MenuCategory
{
    Starters,
    Classics,
    Burgers,
    LunchSpecials,
    Sides,
    Desserts,
    StartersAndBarBites,
    Steaks,
    GreenStuff,
    Bones,
    AfterDinnerCocktails,
    Digestives,
    WhiteWine,
    RoseAndBubbles,
    RedWine,
    Cocktails,
    Beer,
    SoftDrinks,
    HotDrinks
}

public static class MenuCategoryNames
{
    public static IReadOnlyList<string> Fixed { get; } = new[]
    {
        "Starters", "Classics", "Burgers", "Lunch Specials", "Sides", "Desserts",
        "Starters & Bar Bites", "Steaks", "Green Stuff", "Bones", "After Dinner Cocktails",
        "Digestives", "White Wine", "Rosé & Bubbles", "Red Wine", "Cocktails", "Beer",
        "Soft Drinks", "Hot Drinks"
    };
}
