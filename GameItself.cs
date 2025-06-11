using System;
using static System.Console;

//Table table = new Table();
//table.PrintDeck();

//Write(table.MrSaw.ToString());

//Player test = new Player
//    (
//    "meow",
//    new CardHandful
//    {
//        unknown_card = new Card() { _value = 11 },

//        _cards = new List<Card>()
//        {
//            new Card { _value = 6},
//            new Card { _value = 7}
//        }
//    },
//    new TrumpHandful
//    {
//        _trumps = new List<Trump>()
//        {
//            new Trump { _sign = '*', _desc = "Replaces all your cards with random two"},
//            new Trump { _sign = '2', _desc = "Hits you the 2-card if there's no such on the table"}
//        }
//    }

//    );

Table test = new Table("Test dealer", "Test debtor");
test.DealCards();

test._dealer._chandful.PrintCards(false);
WriteLine("\n\n");
test._debtor._chandful.PrintCards(true);
WriteLine("\n\n");
test._debtor._thandful.PrintTrumps();

/* {
    _chandful = 
    { 
        unknown_card = new Card() { _value = 11 },

        _cards = new List<Card>()
        {
            new Card { _value = 6},
            new Card { _value = 7}
        }
    },

    _thandful =
    {
        _trumps = new List<Trump>()
        {
            new Trump { _sign = '*'},
            new Trump { _sign = '2'}
        }
    }
}; */

//test._chandful.PrintCards(false);
//WriteLine("\n\n");
//test._chandful.PrintCards(true);
//WriteLine("\n\n");
//test._thandful.PrintTrumps();

public class Table
{
    List<Card> _deck = new List<Card>()
    {
        new Card { _value = 1},
        new Card { _value = 2},
        new Card { _value = 3},
        new Card { _value = 4},
        new Card { _value = 5},
        new Card { _value = 6},
        new Card { _value = 7},
        new Card { _value = 8},
        new Card { _value = 9},
        new Card { _value = 10},
        new Card { _value = 11},
    };
    public Player _dealer { get; set; }
    public Player _debtor { get; set; }
    public List<Trump> _trumps = new List<Trump>()
    {
        new Trump { _sign = '2', _desc = "Hits you the 2-card if there's no such on the table"},
        new Trump { _sign = '3', _desc = "Hits you the 3-card if there's no such on the table"},
        new Trump { _sign = '4', _desc = "Hits you the 4-card if there's no such on the table"},
        new Trump { _sign = '5', _desc = "Hits you the 5-card if there's no such on the table"},
        new Trump { _sign = '6', _desc = "Hits you the 6-card if there's no such on the table"},
    };
    public Saw MrSaw { get; set; }

    public Table(string dealernick, string debtornick)
    {
        _dealer = new Player(dealernick);
        _debtor = new Player(debtornick);
    }

    public void ShuffleDeck()
    {
        for (int i = 0; i < _deck.Count; i++)
        {
            Card buff = _deck[0];
            _deck.RemoveAt(0);
            _deck.Insert(Random.Shared.Next(_deck.Count), buff);
        }
    }

    public void ReturnIntoDeck()
    {

        for (int i = 0; i < _dealer._chandful._cards.Count; i++)
        {
            _deck.Add(_dealer._chandful._cards[i]);
        }

        _dealer._chandful._cards.Clear();
        _dealer._thandful._trumps.Clear();

        for (int i = 0; i < _debtor._chandful._cards.Count; i++)
        {
            _deck.Add(_debtor._chandful._cards[i]);
        }

        _debtor._chandful._cards.Clear();
        _debtor._thandful._trumps.Clear();
    }

    public void DealCards()
    {
        Random rand = new Random();

        for (int i = 0; i < 2; i++)
        {
            _dealer._chandful._cards.Add(_deck[i]);
            _deck.RemoveAt(i);

            _dealer._thandful._trumps.Add(_trumps[rand.Next(_trumps.Count)]);
        }

        for (int i = 0; i < 2; i++)
        {
            _debtor._chandful._cards.Add(_deck[i]);
            _deck.RemoveAt(i);

            _debtor._thandful._trumps.Add(_trumps[rand.Next(_trumps.Count)]);
        }
    }

    public void PrintDeck()
    {
        foreach (var card in _deck)
        {
            Write("   ﹂-----﹁");
        }

        WriteLine();

        foreach (var card in _deck)
        {
            Write("   |     |");
        }

        WriteLine();

        foreach (var card in _deck)
        {
            if ((int)(card._value / 10) < 1)
            {
                Write($"   |  {card._value}  |");
            }
            else
            {
                int f = 1, s = card._value - 10;

                Write($"   | {f} {s} |");
            }
        }

        WriteLine();

        foreach (var card in _deck)
        {
            Write("   |     |");
        }

        WriteLine();

        foreach (var card in _deck)
        {
            Write("   ﹂-----﹁");
        }
    }
}

public class Player
{
    public string _nick { get; set; }
    public CardHandful _chandful { get; set; }
    public TrumpHandful _thandful { get; set; }

    public Player(string nick)
    {
        _nick = nick;

        _chandful = new CardHandful();
        _thandful = new TrumpHandful();
    }
}

public class CardHandful
{
    public List<Card> _cards { get; set; }
    public Card unknown_card { get; set; }

    public void PrintCards(bool ifwriteunk)
    {
        WriteLine();

        Write("   ﹂-----﹁");

        foreach (var card in _cards)
        {
            Write("   ﹂-----﹁");
        }

        WriteLine();

        Write("   |     |");

        foreach (var card in _cards)
        {
            Write("   |     |");
        }

        WriteLine();

        if (ifwriteunk == true)
        {
            if ((int)(unknown_card._value / 10) < 1)
            {
                Write($"   |  {unknown_card._value}  |");
            }
            else
            {
                int f = 1, s = unknown_card._value - 10;

                Write($"   | {f} {s} |");
            }
        }
        else
        {
            Write("   | ??? |");
        }

        foreach (var card in _cards)
        {
            if ((int)(card._value / 10) < 1)
            {
                Write($"   |  {card._value}  |");
            }
            else
            {
                int f = 1, s = card._value - 10;

                Write($"   | {f} {s} |");
            }
        }

        WriteLine();

        Write("   |     |");

        foreach (var card in _cards)
        {
            Write("   |     |");
        }

        WriteLine();

        Write("   ﹂-----﹁");

        foreach (var card in _cards)
        {
            Write("   ﹂-----﹁");
        }
    }
}

public class TrumpHandful
{
    public List<Trump> _trumps { get; set; }

    public void PrintTrumps()
    {
        WriteLine();

        foreach (var card in _trumps)
        {
            Write("   ﹂-----﹁");
        }

        WriteLine();

        foreach (var card in _trumps)
        {
            Write("   |     |");
        }

        WriteLine();

        foreach (var card in _trumps)
        {
            Write($"   |  {card._sign}  |");
        }

        WriteLine();

        foreach (var card in _trumps)
        {
            Write("   |     |");
        }

        WriteLine();

        foreach (var card in _trumps)
        {
            Write("   ﹂-----﹁");
        }

        WriteLine("\n\n");
        int counter = 1;

        foreach (var trump in _trumps)
        {
            WriteLine($"{counter++}. - {trump._desc}.");
        }
    }
}

public class Card
{
    public int _value { get; set; }

    public override string ToString()
    {
        if ((int)(_value / 10) < 1)
        {
            return $"  ﹂-----﹁\n  |     |\n  |  {_value}  |\n  |     |\n  ﹂-----﹁";
        }
        else
        {
            int f = 1, s = _value - 10;

            return $"  ﹂-----﹁\n  |     |\n  | {f} {s} |\n  |     |\n  ﹂-----﹁";
        }
    }
}

public class Trump
{
    public char _sign { get; set; }

    public string _desc { get; set; }

    public override string ToString()
    {
        return $"  ﹂-----﹁\n  |     |\n  |  {_sign}  |\n  |     |\n  ﹂-----﹁";
    }
}

public class Saw
{
    List<bool> bools = new List<bool>() { false, false, true, false, false };
    public int _move { get; set; }
    public string SawItself =
        "|   *****\n" +
        "|   |    \n" +
        "*********\n" +
        "    |   |\n" +
        "*****   |";

    public override string ToString()
    {
        string ret_val = " ";

        foreach (bool boolv in bools)
        {
            if (boolv == false)
            {
                ret_val += (string)"         ";
            }
            else
            {
                ret_val += SawItself;
            }
        }

        return ret_val;
    }
}