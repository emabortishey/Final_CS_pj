using System;
using System.ComponentModel.Design;
using static System.Console;

Game();

static void Game()
{
    Table table = new Table("Dealer", "Debtor");
    Table buff_table = new Table("buff", "buff");
    table._whos_turn = true;
    int rounds = 1;
    Random rand = new Random();

    DrawOrStay choice;

    table.ShuffleDeck();
    table.DealCards();

    while (table.MrSaw._dealer != 0 || table.MrSaw._debtor != 0)
    {
        WriteLine($"Round №{rounds++}.");

        while (table._dealer._readyToFinish == false || table._debtor._readyToFinish == false)
        {
            if (table._whos_turn == false)
            {
                table._debtor._chandful.PrintCards(false);
                WriteLine($"\nSumm: ??? + {table._debtor.CurrSum(false)}/{table._plank}");
                WriteLine("\n__________________________________________________________\n");
                Write($"Plank: {table._plank}\n{table.MrSaw._debtor}\n\t{table.MrSaw._move}\n{table.MrSaw._debtor}");
                table._tabled_trumps.PrintTrumps();
                WriteLine("\n__________________________________________________________\n");
                table._dealer._chandful.PrintCards(true);
                if (table._dealer.CurrSum(true) <= table._plank)
                {
                    WriteLine($"\nSumm: {table._dealer.CurrSum(true)}/{table._plank}");
                }
                else
                {
                    WriteLine($"\nSumm: {table._dealer.CurrSum(true)}/{table._plank} Someone's got overload! Who might it be..");
                }

                table.Turn += table._dealer.TakeTurn;
                table.TakingTurn();

                choice = DrawOrStay.Awaitness;

                while (choice == DrawOrStay.Awaitness)
                {
                    choice = table.GetPlayerChoice();

                    switch (choice)
                    {
                        case DrawOrStay.Draw:
                            {
                                if (table._deck.Count > 0)
                                {
                                    table._dealer.Draw(table._deck[table._deck.Count - 1]);
                                    table._deck.RemoveAt(table._deck.Count - 1);
                                    table._dealer.DrawTrump(table._trumps[rand.Next(table._trumps.Count)]);
                                }

                                break;
                            }
                        case DrawOrStay.Stay:
                            {
                                table._dealer.Stay();
                                break;
                            }
                        case DrawOrStay.PrintTrumps:
                            {
                                table._dealer._thandful.PrintTrumps();

                                buff_table = new Table(table._dealer.UseTrump(table));

                                if (table == buff_table)
                                {
                                    table._dealer._readyToFinish = true;
                                }
                                else
                                {
                                    table = new Table(buff_table);
                                    choice = DrawOrStay.Awaitness;
                                }

                                break;
                            }
                    }

                    table.Turn -= table._dealer.TakeTurn;
                    table._whos_turn = true;
                }
            }
            else
            {
                table._dealer._chandful.PrintCards(false);
                WriteLine($"\nSumm: ??? + {table._dealer.CurrSum(false)}/{table._plank}");
                WriteLine("\n__________________________________________________________\n");
                Write($"Plank: {table._plank}\n{table.MrSaw._dealer}\n\t{table.MrSaw._move}\n{table.MrSaw._debtor}");
                table._tabled_trumps.PrintTrumps();
                WriteLine("\n__________________________________________________________\n");
                table._debtor._chandful.PrintCards(true);
                if (table._debtor.CurrSum(true) <= table._plank)
                {
                    WriteLine($"\nSumm: {table._debtor.CurrSum(true)}/{table._plank}");
                }
                else
                {
                    WriteLine($"\nSumm: {table._debtor.CurrSum(true)}/{table._plank} Someone's got overload! Who might it be..");
                }

                table.Turn += table._debtor.TakeTurn;
                table.TakingTurn();

                choice = DrawOrStay.Awaitness;

                while (choice == DrawOrStay.Awaitness)
                {
                    choice = table.GetPlayerChoice();

                    switch (choice)
                    {
                        case DrawOrStay.Draw:
                            {
                                if (table._deck.Count > 0)
                                {
                                    table._debtor.Draw(table._deck[table._deck.Count - 1]);
                                    table._deck.RemoveAt(table._deck.Count - 1);
                                    table._debtor.DrawTrump(table._trumps[rand.Next(table._trumps.Count)]);
                                }

                                break;
                            }
                        case DrawOrStay.Stay:
                            {
                                table._debtor.Stay();
                                break;
                            }
                        case DrawOrStay.PrintTrumps:
                            {
                                table._debtor._thandful.PrintTrumps();

                                buff_table = new Table(table._dealer.UseTrump(table));

                                if (table == buff_table)
                                {
                                    table._debtor._readyToFinish = true;
                                }
                                else
                                {
                                    table = new Table(buff_table);
                                    choice = DrawOrStay.Awaitness;
                                }

                                break;
                            }
                    }

                    table.Turn -= table._debtor.TakeTurn;
                    table._whos_turn = false;
                }
            }
        }

        if (table._dealer.CurrSum(true) > table._debtor.CurrSum(true) && table._dealer.CurrSum(true) <= table._plank
            || table._dealer.CurrSum(true) > table._plank && table._debtor.CurrSum(true) > table._plank && table._dealer.CurrSum(true) < table._debtor.CurrSum(true))
        {
            table.MrSaw._dealer += table.MrSaw._move;
            table.MrSaw._debtor -= table.MrSaw._move;

            table.MrSaw._move++;

            WriteLine($"\n{table._debtor._nick} is loser.");
        }
        else if (table._dealer.CurrSum(true) < table._debtor.CurrSum(true) && table._debtor.CurrSum(true) <= table._plank
            || table._dealer.CurrSum(true) > table._plank && table._debtor.CurrSum(true) > table._plank && table._dealer.CurrSum(true) > table._debtor.CurrSum(true))
        {
            table.MrSaw._debtor += table.MrSaw._move;
            table.MrSaw._dealer -= table.MrSaw._move;

            table.MrSaw._move++;

            WriteLine($"\n{table._dealer._nick} is loser.");
        }
        else if (table._dealer.CurrSum(true) > table._plank && table._debtor.CurrSum(true) < table._plank)
        {
            table.MrSaw._debtor += table.MrSaw._move;
            table.MrSaw._dealer -= table.MrSaw._move;

            table.MrSaw._move++;

            WriteLine($"\n{table._dealer._nick} is loser.");
        }
        else if (table._dealer.CurrSum(true) < table._plank && table._debtor.CurrSum(true) > table._plank)
        {
            table.MrSaw._dealer += table.MrSaw._move;
            table.MrSaw._debtor -= table.MrSaw._move;

            table.MrSaw._move++;

            WriteLine($"\n{table._debtor._nick} is loser.");
        }

        table.ReturnIntoDeck();
        table.ShuffleDeck();
        table.DealCards();

        table._dealer._readyToFinish = false;
        table._debtor._readyToFinish = false;

        table._plank = 21;
        table._tabled_trumps._trumps.Clear();
    }
}

public enum TypesOfReturns { PlankChangers = 1, CardGivers, BetChangers, Destroyer, Disservicer, Switcher }
public enum DrawOrStay { Draw, Stay, PrintTrumps, Awaitness }

public delegate void TakeTurn();

public class Table
{
    public event TakeTurn Turn;
    public int _plank;

    public List<Card> _deck { get; set; }

    public Player _dealer { get; set; }
    public Player _debtor { get; set; }
    public bool _whos_turn { get; set; }

    public List<Trump> _trumps = new List<Trump>()
    {
        new SvTPlank ( '*', "Plank is gonna be scanged to the 17."),
        new TwFPlank ( '!', "Plank is gonna be scanged to the 24."),
        new TwSPlank ( '@', "Plank is gonna be scanged to the 27."),
        new TwCard ( '2', "Hits you the 2-card if there's no such on the table."),
        new ThCard ( '3', "Hits you the 3-card if there's no such on the table."),
        new FrCard ( '4', "Hits you the 4-card if there's no such on the table."),
        new FvCard ( '5', "Hits you the 5-card if there's no such on the table."),
        new SxCard ( '6', "Hits you the 6-card if there's no such on the table."),
        new SvCard ( '7', "Hits you the 7-card if there's no such on the table."),
        new PrfCard( '+', "Hits you perfect possible card from the deck"),
        new IncBet ( '>', "Increases bet (Mr. Saw's move) for one."),
        new DiscBet ( '<', "Discreases bet (Mr. Saw's move) for one."),
        new Destroy ('$', "Destroys the last trump card opponent placed on the table."),
        new Disservice ( '-', "Opponent draws one card."),
        new Switch ('"', "Switches the last drawn by you and your opponent cards.")
    };

    public TrumpHandful _tabled_trumps { get; set; }
    public Saw MrSaw { get; set; }

    public Table(string dealernick, string debtornick)
    {
        _deck = new List<Card>  
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

        _plank = 21;
        _tabled_trumps = new TrumpHandful();
        MrSaw = new Saw() { _move = 1, _debtor = 2, _dealer = 2 };
        _dealer = new Player(dealernick);
        _debtor = new Player(debtornick);
    }
    public Table(Table table)
    {
        _tabled_trumps = new TrumpHandful(table._tabled_trumps._trumps);

        _plank = table._plank;

        _deck = new List<Card>(table._deck);

        MrSaw = new Saw(table.MrSaw);

        _dealer = new Player(table._dealer);
        _debtor = new Player(table._debtor);
    }

    public void TakingTurn()
    {
        WriteLine("\n\nAttention!");

        Turn();
    }

    public DrawOrStay GetPlayerChoice()
    {
        while (true)
        {
            Console.Write("\n(D)raw, (S)tay or (P)rint trumps?");

            string input = ReadLine().ToUpper();

            switch (input)
            {
                case "D": return DrawOrStay.Draw;

                case "S": return DrawOrStay.Stay;

                case "P": return DrawOrStay.PrintTrumps;
            }
        }
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

        for (int i = 0; i < _debtor._chandful._cards.Count; i++)
        {
            _deck.Add(_debtor._chandful._cards[i]);
        }

        _debtor._chandful._cards.Clear();
    }

    public void DealCards()
    {
        Random rand = new Random();

        _dealer._chandful._unknown_card = _deck[0];
        _dealer._chandful._cards.Add(_deck[1]);
        _deck.RemoveAt(0);
        _deck.RemoveAt(0);

        _dealer._thandful._trumps.Add(_trumps[rand.Next(_trumps.Count)]);
        _dealer._thandful._trumps.Add(_trumps[rand.Next(_trumps.Count)]);

        _debtor._chandful._unknown_card = _deck[0];
        _debtor._chandful._cards.Add(_deck[1]);
        _deck.RemoveAt(0);
        _deck.RemoveAt(0);

        _debtor._thandful._trumps.Add(_trumps[rand.Next(_trumps.Count)]);
        _debtor._thandful._trumps.Add(_trumps[rand.Next(_trumps.Count)]);
    }

    public bool Contains(int value)
    {
        bool ret;

        foreach(var card in _deck)
        {
            if(card._value == value) return true;
        }

        return false;
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
    public bool _readyToFinish {  get; set; }

    public Player(string nick)
    {
        _nick = nick;
        _readyToFinish = false;
        _chandful = new CardHandful();
        _thandful = new TrumpHandful();
    }
    public Player(Player player)
    {
        _nick = player._nick;
        _readyToFinish = player._readyToFinish;
        _chandful = new CardHandful(player._chandful);
        _thandful = new TrumpHandful(player._thandful);
    }

    public int CurrSum(bool showfully)
    {
        int sum = 0;

        foreach(var card in _chandful._cards)
        {
            sum += card._value;
        }

        if (showfully == true)
        {
            sum += _chandful._unknown_card._value;
        }

        return sum;
    }

    public void DrawTrump(Trump trump)
    {
        _thandful._trumps.Add(trump);
    }

    public void Draw(Card drawncard)
    {
        _chandful._cards.Add(drawncard);

        _readyToFinish = false;

        WriteLine($"{_nick} drew card with value {drawncard._value}");
    }

    public void Stay()
    {
        _readyToFinish = true;

        WriteLine($"{_nick} decided to stay.");
    }

    public void TakeTurn()
    {
        WriteLine($"{_nick}'s choosing their next move. Better do it carefully.");
    }

    public Table UseTrump(Table buffT)
    {
        WriteLine("Put the number of trump card that you want to use (0 in case you don't want to): ");

        int trump = Convert.ToInt32(ReadLine());

        if(trump == 0)
        {
            if (buffT._whos_turn == false)
            {
                buffT._dealer._readyToFinish = true;
            }
            else
            {
                buffT._debtor._readyToFinish = true;
            }

            return buffT;
        }

        if (buffT._whos_turn == false)
        {
            buffT._dealer._readyToFinish = false;
        }
        else
        {
            buffT._debtor._readyToFinish = false;
        }

        trump -= 1;

        ReturningTrump ret;

        if (buffT._whos_turn == false)
        {
            ret = buffT._dealer._thandful._trumps[trump].UseTrump(buffT);
        }
        else
        {
            ret = buffT._debtor._thandful._trumps[trump].UseTrump(buffT);
        }

        if (ret._is_succesed == true)
        {
            if (ret._type == TypesOfReturns.PlankChangers)
            {
                buffT._plank = ret._plank;

                buffT._tabled_trumps._trumps.Add(_thandful._trumps[trump]);
            }
            else if(ret._type == TypesOfReturns.CardGivers)
            {
                if(buffT._whos_turn == false)
                {
                    buffT._dealer.Draw(ret._ret_card);
                }
                else
                {
                    buffT._debtor.Draw(ret._ret_card);
                }

                buffT._deck.Remove(ret._ret_card);
            }
            else if(ret._type == TypesOfReturns.BetChangers)
            {
                buffT.MrSaw._move += ret._bet;

                buffT._tabled_trumps._trumps.Add(_thandful._trumps[trump]);
            }
            else if(ret._type == TypesOfReturns.Destroyer)
            {
                if (buffT._tabled_trumps._trumps[buffT._tabled_trumps._trumps.Count - 1]._tabled_type == TypesOfReturns.PlankChangers)
                {
                    buffT._plank = 21;
                }
                else if(buffT._tabled_trumps._trumps[buffT._tabled_trumps._trumps.Count - 1]._tabled_type == TypesOfReturns.BetChangers)
                {
                    if(buffT._tabled_trumps._trumps[buffT._tabled_trumps._trumps.Count - 1]._inc_or_dis == '-')
                    {
                        buffT.MrSaw._move--;
                    }
                    else if (buffT._tabled_trumps._trumps[buffT._tabled_trumps._trumps.Count - 1]._inc_or_dis == '+')
                    {
                        buffT.MrSaw._move++;
                    }
                }

                buffT._tabled_trumps._trumps.RemoveAt(buffT._tabled_trumps._trumps.Count - 1);
            }
            else if(ret._type == TypesOfReturns.Disservicer)
            {
                if (buffT._whos_turn == false)
                {
                    buffT._debtor.Draw(ret._disserv_ret_card);
                }
                else
                {
                    buffT._dealer.Draw(ret._disserv_ret_card);
                }
            }
            else if(ret._type == TypesOfReturns.Switcher)
            {
                Card buff1 = buffT._dealer._chandful._cards[ret._dealer_sw_ind], buff2 = buffT._debtor._chandful._cards[ret._debtor_sw_ind];

                buffT._dealer._chandful._cards[ret._dealer_sw_ind] = buff2;
                buffT._debtor._chandful._cards[ret._debtor_sw_ind] = buff1;
            }
        }

        if (buffT._whos_turn == false)
        {
            buffT._dealer._thandful._trumps.RemoveAt(trump);
        }
        else
        {
            buffT._debtor._thandful._trumps.RemoveAt(trump);
        }

        return buffT;
    }
}

public class CardHandful
{
    public List<Card> _cards { get; set; }
    public Card _unknown_card { get; set; }

    public CardHandful()
    {
        _cards = new List<Card>();
        _unknown_card = new Card();
    }
    public CardHandful(CardHandful cards)
    {
        _cards = new List<Card>();
        _unknown_card = new Card();

        foreach (var card in cards._cards)
        {
            _cards.Add(card);
        }

        _unknown_card = cards._unknown_card;
    }

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
            if ((int)(_unknown_card._value / 10) < 1)
            {
                Write($"   |  {_unknown_card._value}  |");
            }
            else
            {
                int f = 1, s = _unknown_card._value - 10;

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

    public TrumpHandful()
    {
        _trumps = new List<Trump>();
    }

    public TrumpHandful(TrumpHandful trumps)
    {
        _trumps = new List<Trump>();

        foreach (var card in trumps._trumps)
        {
            _trumps.Add(card);
        }
    }

    public TrumpHandful(List<Trump> trumps)
    {
        _trumps = new List<Trump>();

        foreach (var trump in trumps)
        {
            _trumps.Add(trump);
        }
    }

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

    public Card() { }
    public Card(int value)
    {
        _value = value;
    }
    public Card(Card card)
    {
        _value = card._value;
    }

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

public abstract class Trump
{
    public char _sign { get; set; }

    public string _desc { get; set; }

    public TypesOfReturns _tabled_type;

    public char _inc_or_dis;

    public Trump() { }
    public Trump(Trump trump)
    {
        _sign = trump._sign;
        _desc = trump._desc;
        _tabled_type = trump._tabled_type;
        _inc_or_dis = trump._inc_or_dis;
    }

    public abstract ReturningTrump UseTrump(Table buffT);

    public Trump(char sign, string desc)
    {
        _sign = sign;
        _desc = desc;
    }

    public override string ToString()
    {
        return $"  ﹂-----﹁\n  |     |\n  |  {_sign}  |\n  |     |\n  ﹂-----﹁";
    }
}

public class Saw
{
    public int _dealer {  get; set; }
    public int _debtor { get; set; }
    public int _move { get; set; }

    public string SawItself =
        "|   *****\n" +
        "|   |    \n" +
        "*********\n" +
        "    |   |\n" +
        "*****   |";

    public Saw() { }
    public Saw(Saw saw)
    {
        _dealer = saw._dealer;
        _debtor = saw._debtor;
        _move = saw._move;
    }

    public override string ToString()
    {
        string ret_val = " ";

        for (int i = 0; i< _dealer; i++)
        {
            ret_val += (string)"         ";
        }

        ret_val += SawItself + '\n';

        for (int i = 0; i < _debtor; i++)
        {
            ret_val += (string)"         ";
        }

        return ret_val;
    }
}

public class ReturningTrump
{
    public bool _is_succesed { get; set; }
    public TypesOfReturns _type { get; set; }

    public int _plank { get; set; }
    public int _bet { get; set; }
    public int _destroy { get; set; }
    public Card _ret_card { get; set; }
    public Card _disserv_ret_card { get; set; }
    public int _dealer_sw_ind {  get; set; }
    public int _debtor_sw_ind { get; set; }
}



public class SvTPlank : Trump
{
    public SvTPlank(char sign, string desc) : base(sign, desc) { }
    public TypesOfReturns _tabled_type = TypesOfReturns.PlankChangers;

    public override ReturningTrump UseTrump(Table buffT)
    {
        return new ReturningTrump() { _is_succesed = true, _type = TypesOfReturns.PlankChangers, _plank = 17 };
    }
}

public class TwFPlank : Trump
{
    public TwFPlank(char sign, string desc) : base(sign, desc) { }
    public TypesOfReturns _tabled_type = TypesOfReturns.PlankChangers;

    public override ReturningTrump UseTrump(Table buffT)
    {
        return new ReturningTrump() { _is_succesed = true, _type = TypesOfReturns.PlankChangers, _plank = 24 };
    }
}

public class TwSPlank : Trump
{
    public TwSPlank(char sign, string desc) : base(sign, desc) { }
    public TypesOfReturns _tabled_type = TypesOfReturns.PlankChangers;

    public override ReturningTrump UseTrump(Table buffT)
    {
        return new ReturningTrump() { _is_succesed = true, _type = TypesOfReturns.PlankChangers, _plank = 27 };
    }
}


public class SvCard : Trump
{
    public SvCard(char sign, string desc) : base(sign, desc) { }

    public override ReturningTrump UseTrump(Table buffT)
    {
        if (buffT.Contains(7))
        {
            return new ReturningTrump() { _is_succesed = true, _type = TypesOfReturns.CardGivers, _ret_card = new Card(7) };
        }
        return new ReturningTrump() { _is_succesed = false };
    }
}

public class SxCard : Trump
{
    public SxCard(char sign, string desc) : base(sign, desc) { }

    public override ReturningTrump UseTrump(Table buffT)
    {
        if (buffT.Contains(6))
        {
            return new ReturningTrump() { _is_succesed = true, _type = TypesOfReturns.CardGivers, _ret_card = new Card(6) };
        }
        return new ReturningTrump() { _is_succesed = false };
    }
}

public class FvCard : Trump
{
    public FvCard(char sign, string desc) : base(sign, desc) { }

    public override ReturningTrump UseTrump(Table buffT)
    {
        if (buffT.Contains(5))
        {
            return new ReturningTrump() { _is_succesed = true, _type = TypesOfReturns.CardGivers, _ret_card = new Card(5) };
        }
        return new ReturningTrump() { _is_succesed = false };
    }
}

public class FrCard : Trump
{
    public FrCard(char sign, string desc) : base(sign, desc) { }

    public override ReturningTrump UseTrump(Table buffT)
    {
        if (buffT.Contains(4))
        {
            return new ReturningTrump() { _is_succesed = true, _type = TypesOfReturns.CardGivers, _ret_card = new Card(4) };
        }
        return new ReturningTrump() { _is_succesed = false };
    }
}

public class ThCard : Trump
{
    public ThCard(char sign, string desc) : base(sign, desc) { }

    public override ReturningTrump UseTrump(Table buffT)
    {
        if (buffT.Contains(3))
        {
            return new ReturningTrump() { _is_succesed = true, _type = TypesOfReturns.CardGivers, _ret_card = new Card(3) };
        }
        return new ReturningTrump() { _is_succesed = false };
    }
}

public class TwCard : Trump
{
    public TwCard(char sign, string desc) : base(sign, desc) { }

    public override ReturningTrump UseTrump(Table buffT)
    {
        if (buffT.Contains(2))
        {
            return new ReturningTrump() { _is_succesed = true, _type = TypesOfReturns.CardGivers, _ret_card = new Card(2) };
        }
        return new ReturningTrump() { _is_succesed = false };
    }
}

public class PrfCard : Trump
{
    public PrfCard(char sign, string desc) : base(sign, desc) { }

    public override ReturningTrump UseTrump(Table buffT)
    {
        int summ = 0;

        if (buffT._whos_turn == false)
        {
            foreach (Card card in buffT._dealer._chandful._cards)
            {
                summ += card._value;
            }
        }
        else
        {
            foreach (Card card in buffT._debtor._chandful._cards)
            {
                summ += card._value;
            }
        }

        int _roof = 21 - summ;

        for (int i = _roof; i > 0; i--)
        {
            if (buffT._deck.Contains(new Card() { _value = i }) == true)
            {
                return new ReturningTrump() { _is_succesed = true, _type = TypesOfReturns.CardGivers, _ret_card = new Card() { _value = i } };
            }
        }

        return new ReturningTrump() { _is_succesed = false };
    }
}

public class IncBet : Trump
{
    public IncBet(char sign, string desc) : base(sign, desc) { }
    public TypesOfReturns _tabled_type = TypesOfReturns.BetChangers;
    public char _inc_or_dis = '+';

    public override ReturningTrump UseTrump(Table buffT)
    {
        if (buffT.MrSaw._move > 0)
        {
            return new ReturningTrump() { _is_succesed = true, _type = TypesOfReturns.BetChangers, _bet = 1 };
        }
        return new ReturningTrump() { _is_succesed = false };
    }
}

public class DiscBet : Trump
{
    public DiscBet(char sign, string desc) : base(sign, desc) { }
    public TypesOfReturns _tabled_type = TypesOfReturns.BetChangers;
    public char _inc_or_dis = '-';

    public override ReturningTrump UseTrump(Table buffT)
    {
        if (buffT.MrSaw._move > 0)
        {
            return new ReturningTrump() { _is_succesed = true, _type = TypesOfReturns.BetChangers, _bet = -1 };
        }
        return new ReturningTrump() { _is_succesed = false };
    }
}

public class Destroy : Trump
{
    public Destroy(char sign, string desc) : base(sign, desc) { }

    public override ReturningTrump UseTrump(Table buffT)
    {
        if (buffT._tabled_trumps._trumps.Count > 0)
        {
            return new ReturningTrump() { _is_succesed = true, _type = TypesOfReturns.Destroyer, _destroy = buffT._tabled_trumps._trumps.Count - 1 };
        }
        return new ReturningTrump() { _is_succesed = false };
    }
}

public class Disservice : Trump
{
    public Disservice(char sign, string desc) : base(sign, desc) { }

    public override ReturningTrump UseTrump(Table buffT)
    {
        if (buffT._deck.Count > 0)
        {
            return new ReturningTrump() { _is_succesed = true, _type = TypesOfReturns.Disservicer, _disserv_ret_card = buffT._deck[0] };
        }
        return new ReturningTrump() { _is_succesed = false };
    }
}

public class Switch : Trump
{
    public Switch(char sign, string desc) : base(sign, desc) { }

    public override ReturningTrump UseTrump(Table buffT)
    {
        if (buffT._dealer._chandful._cards.Count > 0 && buffT._debtor._chandful._cards.Count > 0)
        {
            return new ReturningTrump() { _is_succesed = true, _type = TypesOfReturns.Switcher, _dealer_sw_ind = buffT._dealer._chandful._cards.Count-1, _debtor_sw_ind = buffT._debtor._chandful._cards.Count - 1 };
        }
        return new ReturningTrump() { _is_succesed = false };
    }

}