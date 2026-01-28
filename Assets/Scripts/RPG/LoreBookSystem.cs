using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages discoverable lore books and documents that expand the game's narrative.
/// Players can collect and read books to learn about the world's history and lore.
/// </summary>
public class LoreBookSystem : MonoBehaviour
{
    public static LoreBookSystem Instance { get; private set; }
    
    private Dictionary<string, LoreBook> _allBooks = new Dictionary<string, LoreBook>();
    private HashSet<string> _discoveredBooks = new HashSet<string>();
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializeLoreBooks();
    }

    private void InitializeLoreBooks()
    {
        // The Shire Books
        AddBook(new LoreBook(
            "shire_001",
            "The Red Book of Westmarch",
            LoreCategory.History,
            "In a hole in the ground there lived a hobbit. Not a nasty, dirty, wet hole, filled with the ends of worms and an oozy smell, nor yet a dry, bare, sandy hole with nothing in it to sit down on or to eat: it was a hobbit-hole, and that means comfort.\n\nThis ancient tome chronicles the peaceful life of hobbits in the Shire, their love of simple pleasures, and their unexpected role in the great events that shaped Middle-earth.",
            "The Shire"
        ));

        AddBook(new LoreBook(
            "shire_002",
            "On Pipeweed and Its Uses",
            LoreCategory.Culture,
            "The finest pipeweed grows in the Southfarthing, particularly the varieties Longbottom Leaf, Old Toby, and Southern Star. The art of pipe-smoking is considered a refined hobby among hobbits, often accompanied by good food and even better company.\n\nMany consider the simple pleasures of the Shire to be the true treasures of Middle-earth.",
            "The Shire"
        ));

        // Rohan Books
        AddBook(new LoreBook(
            "rohan_001",
            "The Riders of the Mark",
            LoreCategory.History,
            "The Rohirrim are a proud people, horse-lords who have defended their lands for generations. Their bond with their steeds is legendary, and their cavalry is feared throughout Middle-earth.\n\nWhen darkness threatens, the Riders of Rohan answer the call. Their horns echo through the plains, bringing hope to allies and terror to enemies.",
            "Plains of Rohan"
        ));

        AddBook(new LoreBook(
            "rohan_002",
            "Songs of the Horse-Lords",
            LoreCategory.Culture,
            "Arise, arise, Riders of Théoden!\nFell deeds awake: fire and slaughter!\nSpear shall be shaken, shield be splintered,\nA sword-day, a red day, ere the sun rises!\n\nThese words have rallied the Rohirrim in their darkest hours, reminding them of their duty and their courage.",
            "Plains of Rohan"
        ));

        // Mordor Books
        AddBook(new LoreBook(
            "mordor_001",
            "The Shadow in the East",
            LoreCategory.History,
            "Long ago, a great evil was cast down but not destroyed. From the ruins of Barad-dûr, the Dark Lord built his power anew. Mordor became a land of ash and shadow, where no living thing could flourish.\n\nYet even in the darkest places, hope endures. For it is said that the smallest person can change the course of the future.",
            "Lands of Mordor"
        ));

        AddBook(new LoreBook(
            "mordor_002",
            "On Rings of Power",
            LoreCategory.Magic,
            "Three Rings for the Elven-kings under the sky,\nSeven for the Dwarf-lords in their halls of stone,\nNine for Mortal Men doomed to die,\nOne for the Dark Lord on his dark throne.\n\nThe Rings of Power were crafted with great skill, but the One Ring held dominion over all. To destroy it would mean the end of the Dark Lord's power forever.",
            "Lands of Mordor"
        ));

        // Dungeon Books
        AddBook(new LoreBook(
            "dungeon_001",
            "Tales of the Deep Places",
            LoreCategory.History,
            "Deep beneath the mountains lie ancient halls and forgotten treasures. But beware - these places are not empty. Creatures of darkness make their homes in the deep, far from the light of sun and star.\n\nMany brave souls have ventured into these depths seeking glory and riches. Few have returned.",
            "Unknown"
        ));

        AddBook(new LoreBook(
            "dungeon_002",
            "A Survivor's Guide to Dungeon Delving",
            LoreCategory.Survival,
            "Rule 1: Never venture alone.\nRule 2: Always bring extra rope and torches.\nRule 3: If you hear chanting, run.\nRule 4: Mark your path - getting lost is a death sentence.\nRule 5: Trust your instincts. If something feels wrong, it probably is.\n\nFollow these rules and you might survive long enough to enjoy your treasure.",
            "Unknown"
        ));

        // Character Backstories
        AddBook(new LoreBook(
            "character_001",
            "Gandalf: The Grey Pilgrim",
            LoreCategory.Characters,
            "The wizard known as Gandalf the Grey has walked Middle-earth for thousands of years, guiding and advising the free peoples in their struggles against darkness.\n\nThough he appears as an old man with a staff, his true power and purpose are known to few. Some say he is more than he seems - a guardian sent from the West.",
            "Unknown"
        ));

        AddBook(new LoreBook(
            "character_002",
            "Legolas of the Woodland Realm",
            LoreCategory.Characters,
            "Prince Legolas, son of King Thranduil, is one of the finest archers in all of Middle-earth. His keen eyes can spot a sparrow flying in the dark, and his arrows never miss their mark.\n\nElves are immortal and ageless, and Legolas has lived for thousands of years, yet he retains the energy and curiosity of youth.",
            "Unknown"
        ));

        // Magic and Lore
        AddBook(new LoreBook(
            "magic_001",
            "The Nature of Magic",
            LoreCategory.Magic,
            "Magic in Middle-earth is ancient and subtle, woven into the very fabric of the world. It is not flashy or common, but rather a deep power that only the wisest can wield.\n\nElves possess natural magic, tied to nature and song. Wizards draw on ancient powers beyond mortal understanding. And then there are the Rings of Power - artifacts of such potency that they can corrupt even the noblest of hearts.",
            "Unknown"
        ));

        AddBook(new LoreBook(
            "magic_002",
            "Mithril: The Silver Steel",
            LoreCategory.Magic,
            "Mithril is a precious metal, light as a feather yet harder than steel. It was mined in the depths of Moria by the Dwarves, who crafted wondrous items from it.\n\nA shirt of mithril mail can turn aside any blade and stop any arrow. It is worth more than the entire Shire, and its value is beyond measure.",
            "Unknown"
        ));

        // World History
        AddBook(new LoreBook(
            "history_001",
            "The First Age",
            LoreCategory.History,
            "In the Elder Days, when the world was young, great wars were fought between the powers of light and darkness. Morgoth, the first Dark Lord, sought to dominate all of Middle-earth.\n\nHeroes of legend rose to fight him - Beren and Lúthien, Túrin Turambar, and many others. Their deeds echo through the ages.",
            "Unknown"
        ));

        AddBook(new LoreBook(
            "history_002",
            "The Fall of Númenor",
            LoreCategory.History,
            "Long ago, the island kingdom of Númenor stood as the greatest realm of Men. But pride and the whispers of Sauron led them to defy the Valar themselves.\n\nIn punishment, the island was drowned beneath the waves. Only a faithful remnant escaped to found the kingdoms of Gondor and Arnor in Middle-earth.",
            "Unknown"
        ));

        // Bestiary
        AddBook(new LoreBook(
            "bestiary_001",
            "On Orcs and Goblins",
            LoreCategory.Bestiary,
            "Orcs are twisted creatures born of cruelty and malice. They fear the sun and prefer the darkness. Though individually weak, they are dangerous in numbers.\n\nGoblins are their smaller cousins, equally vicious but more cunning. They dwell in caves and mountains, emerging at night to raid and pillage.",
            "Unknown"
        ));

        AddBook(new LoreBook(
            "bestiary_002",
            "Dragons of Middle-earth",
            LoreCategory.Bestiary,
            "Dragons are ancient and terrible, creatures of fire and greed. They hoard treasure and guard it jealously, burning any who dare approach.\n\nThe great dragons of old - Glaurung, Ancalagon, Smaug - are gone, but lesser drakes and hatchlings still pose a mortal threat to the unwary.",
            "Unknown"
        ));

        // Prophecies
        AddBook(new LoreBook(
            "prophecy_001",
            "The Prophecy of the North",
            LoreCategory.Prophecy,
            "When darkness spreads and hope seems lost,\nWhen evil rises at terrible cost,\nA light will shine from unlikely places,\nIn the smallest of hands, courage traces.\n\nThe ring must go into the fire's embrace,\nTo save all realms from the Shadow's face.",
            "Unknown"
        ));

        AddBook(new LoreBook(
            "misc_001",
            "A Hobbit's Guide to Second Breakfast",
            LoreCategory.Culture,
            "The hobbits of the Shire recognize seven meals throughout the day: Breakfast, Second Breakfast, Elevenses, Luncheon, Afternoon Tea, Dinner, and Supper.\n\nAny respectable hobbit knows that Second Breakfast is not optional, and should consist of eggs, bacon, sausages, mushrooms, tomatoes, and plenty of toast.",
            "The Shire"
        ));
    }

    private void AddBook(LoreBook book)
    {
        _allBooks[book.bookId] = book;
    }

    public bool DiscoverBook(string bookId)
    {
        if (_allBooks.ContainsKey(bookId) && !_discoveredBooks.Contains(bookId))
        {
            _discoveredBooks.Add(bookId);
            var book = _allBooks[bookId];
            
            Debug.Log($"📖 LORE DISCOVERED: {book.title}");
            Debug.Log($"Category: {book.category} | Location: {book.location}");
            
            if (AchievementSystem.Instance != null)
            {
                AchievementSystem.Instance.OnLoreDiscovered(bookId);
            }
            
            return true;
        }
        return false;
    }

    public LoreBook GetBook(string bookId)
    {
        return _allBooks.ContainsKey(bookId) ? _allBooks[bookId] : null;
    }

    public List<LoreBook> GetDiscoveredBooks()
    {
        var discovered = new List<LoreBook>();
        foreach (var bookId in _discoveredBooks)
        {
            if (_allBooks.ContainsKey(bookId))
            {
                discovered.Add(_allBooks[bookId]);
            }
        }
        return discovered;
    }

    public List<LoreBook> GetBooksByCategory(LoreCategory category)
    {
        var filtered = new List<LoreBook>();
        foreach (var book in _allBooks.Values)
        {
            if (book.category == category && _discoveredBooks.Contains(book.bookId))
            {
                filtered.Add(book);
            }
        }
        return filtered;
    }

    public int GetTotalBooks()
    {
        return _allBooks.Count;
    }

    public int GetDiscoveredCount()
    {
        return _discoveredBooks.Count;
    }

    public float GetCompletionPercentage()
    {
        if (_allBooks.Count == 0) return 0f;
        return (_discoveredBooks.Count / (float)_allBooks.Count) * 100f;
    }

    public bool HasDiscovered(string bookId)
    {
        return _discoveredBooks.Contains(bookId);
    }

    public void ReadBook(string bookId)
    {
        var book = GetBook(bookId);
        if (book != null && _discoveredBooks.Contains(bookId))
        {
            Debug.Log($"\n=== {book.title} ===");
            Debug.Log($"[{book.category}] - Found in: {book.location}\n");
            Debug.Log(book.content);
            Debug.Log($"\n{'='.ToString().PadLeft(book.title.Length + 8, '=')}");
        }
        else
        {
            Debug.Log("This book has not been discovered yet.");
        }
    }
    
    private void OnDestroy()
    {
        // Clear collections to prevent memory leaks
        _allBooks.Clear();
        _discoveredBooks.Clear();
        
        if (Instance == this)
        {
            Instance = null;
        }
    }
}

[System.Serializable]
public class LoreBook
{
    public string bookId;
    public string title;
    public LoreCategory category;
    public string content;
    public string location; // Where the book can be found
    public bool isRead;

    public LoreBook(string id, string bookTitle, LoreCategory cat, string text, string foundLocation)
    {
        bookId = id;
        title = bookTitle;
        category = cat;
        content = text;
        location = foundLocation;
        isRead = false;
    }
}

public enum LoreCategory
{
    History,
    Culture,
    Magic,
    Characters,
    Bestiary,
    Prophecy,
    Survival
}
