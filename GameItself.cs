using System;
using System.ComponentModel.Design;
using System.Security.Cryptography;
using System.Text.Json;
using static System.Console;

//Game();

Menu();

static void Menu()
{
    bool showmenu = true;
    while (showmenu)
    {
        DrawWindow(8, 2, 40, 13, "Меню", true);
        SetCursorPosition(12, 5);
        WriteLine("1. Новая игра.");
        SetCursorPosition(12, 6);
        WriteLine("2. Правила.");
        SetCursorPosition(12, 7);
        WriteLine("3. Результаты.");
        SetCursorPosition(12, 8);
        WriteLine("4. Выход");

        // Обработка выбора
        SetCursorPosition(12, 11);
        Write("Введите свой выбор: ");
        SetCursorPosition(31, 12);
        WriteLine("════════");
        SetCursorPosition(32, 11);
        int choice = Convert.ToInt32(ReadLine());

        switch (choice)
        {
            case 1:
                {
                    string dealer, debtor;

                    DrawWindow(8, 2, 40, 13, "Дилер", true);
                    SetCursorPosition(12, 7);
                    WriteLine("Введите имя сегодняшнего Дилера.");
                    SetCursorPosition(14, 11);
                    WriteLine("══════════════════════════");
                    SetCursorPosition(22, 10);
                    dealer = ReadLine();

                    Clear();

                    DrawWindow(8, 2, 40, 13, "Должник", true);
                    SetCursorPosition(12, 7);
                    WriteLine("Введите имя сегодняшнего Должника.");
                    SetCursorPosition(14, 11);
                    WriteLine("══════════════════════════");
                    SetCursorPosition(22, 10);
                    debtor = ReadLine();


                    Clear();
                    Game(dealer, debtor);

                    showmenu = false;

                    break;
                }
            case 2:
                {
                    ShowRules();

                    break;
                }
            case 3:
                {
                    DownloadResults();

                    break;
                }
            case 4:
                {
                    showmenu = false;

                    break;
                }
        }
    }
}

static void Game(string dealer_name, string debtor_name)
{
    Table table = new Table(dealer_name, debtor_name);
    Table buff_table = new Table("buff", "buff");
    table._whos_turn = true;
    int rounds = 0;
    Random rand = new Random();
    string fin = "buff";

    int wx = 8, wy = 3;
    int x = wx+3, y = wy+3;
    int wid = 80, len = 34;
    DrawOrStay choice;

    table.ShuffleDeck();
    table.DealCards();

    while (table.MrSaw._dealer > 0 && table.MrSaw._debtor > 0)
    {
        rounds++;

        x = wx + 3;
        y = wy + 3;

        while (table._dealer._readyToFinish == false || table._debtor._readyToFinish == false)
        {
            x = wx + 3;
            y = wy + 3;
            if (table._whos_turn == false)
            {
                DrawWindow(wx, wy, wid, len, $"{table._dealer._nick} делает свой ход", true);

                PrintPlayersPov(table, wid, y, x);

                table.Turn += table._dealer.TakeTurn;
                table.TakingTurn(x, len - 2);

                choice = DrawOrStay.Awaitness;

                while (choice == DrawOrStay.Awaitness)
                {
                    choice = table.GetPlayerChoice(x, len -1);

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
                                DrawWindow(wx, wy, wid, len, $"{table._dealer._nick} делает свой ход", true);

                                PrintPlayersPov(table, wid, y, x);

                                PrintTrumpsWindow(x-3, y + len, len, wid, table._debtor);

                                buff_table = new Table(table._dealer.UseTrump(table, x, y+len+2));

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

                    Clear();
                    x = wx + 3;
                    y = wy + 3;

                    DrawWindow(wx, wy, wid, len, $"{table._dealer._nick} делает свой ход", true);

                    PrintPlayersPov(table, wid, y, x);
                }
            }
            else
            {
                DrawWindow(wx, wy, wid, len, $"{table._debtor._nick} делает свой ход", true);

                PrintPlayersPov(table, wid, y, x);

                table.Turn += table._debtor.TakeTurn;
                table.TakingTurn(x, len - 2);

                choice = DrawOrStay.Awaitness;

                while (choice == DrawOrStay.Awaitness)
                {
                    choice = table.GetPlayerChoice(x, len-1);

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
                                DrawWindow(wx, wy, wid, len, $"{table._debtor._nick} делает свой ход", true);

                                PrintPlayersPov(table, wid, y, x);

                                PrintTrumpsWindow(x-3, y + len, len, wid, table._debtor);

                                buff_table = new Table(table._dealer.UseTrump(table, x, y + len + 2));

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

                    Clear();
                    x = wx + 3;
                    y = wy + 3;

                    DrawWindow(wx, wy, wid, len, $"{table._debtor._nick} делает свой ход", true);

                    PrintPlayersPov(table, wid, y, x);
                }
            }

            Clear();
        }

        if (table._dealer.CurrSum(true) > table._debtor.CurrSum(true) && table._dealer.CurrSum(true) <= table._plank
            || table._dealer.CurrSum(true) > table._plank && table._debtor.CurrSum(true) > table._plank && table._dealer.CurrSum(true) < table._debtor.CurrSum(true))
        {
            table.MrSaw._dealer += table.MrSaw._move;
            table.MrSaw._debtor -= table.MrSaw._move;

            fin = $"{table._debtor._nick} проиграл.";
        }
        else if (table._dealer.CurrSum(true) < table._debtor.CurrSum(true) && table._debtor.CurrSum(true) <= table._plank
            || table._dealer.CurrSum(true) > table._plank && table._debtor.CurrSum(true) > table._plank && table._dealer.CurrSum(true) > table._debtor.CurrSum(true))
        {
            table.MrSaw._debtor += table.MrSaw._move;
            table.MrSaw._dealer -= table.MrSaw._move;

            fin = $"{table._dealer._nick} проиграл.";
        }
        else if (table._dealer.CurrSum(true) > table._plank && table._debtor.CurrSum(true) < table._plank)
        {
            table.MrSaw._debtor += table.MrSaw._move;
            table.MrSaw._dealer -= table.MrSaw._move;

            fin = $"{table._dealer._nick} проиграл.";
        }
        else if (table._dealer.CurrSum(true) < table._plank && table._debtor.CurrSum(true) > table._plank)
        {
            table.MrSaw._dealer += table.MrSaw._move;
            table.MrSaw._debtor -= table.MrSaw._move;

            fin = $"{table._debtor._nick} проиграл.";
        }

        if (table.MrSaw._dealer > 0 && table.MrSaw._debtor > 0)
        {
            DrawWindow(wx, wy, 40, 8, fin, false);
            SetCursorPosition(wx + 2, wy + 3);
            Write("Нажмите Enter для следующего раунда.");
            SetCursorPosition(wx + 19, wy + 5);
            ReadLine();

            table.ReturnIntoDeck();
            table.ShuffleDeck();
            table.DealCards();

            table._dealer._readyToFinish = false;
            table._debtor._readyToFinish = false;

            table.MrSaw._move++;
            table._plank = 21;
            table._tabled_trumps._trumps.Clear();
        }
    }

    if(table.MrSaw._debtor <= 0)
    {
        UploadResults(rounds, table._dealer._nick);
        DrawWindow(wx, wy, 40, 8, $"{table._dealer._nick} победил!", true);
        SetCursorPosition(wx + 3, wy + 3);
        Write("Нажмите Enter для завершения игры.");
        SetCursorPosition(wx + 19, wy + 5);
        ReadLine();
    }
    else if(table.MrSaw._dealer <= 0)
    {
        UploadResults(rounds, table._debtor._nick);
        DrawWindow(wx, wy, 40, 8, $"{table._debtor._nick} победил!", true);
        SetCursorPosition(wx + 3, wy + 3);
        Write("Нажмите Enter для завершения игры.");
        SetCursorPosition(wx + 19, wy + 5);
        ReadLine();
    }

    SetCursorPosition(wx + 20, wy + 10);
}

static void UploadResults(int rounds, string winner)
{
    string json_rounds = JsonSerializer.Serialize(rounds);
    File.WriteAllText("rounds.json", json_rounds);

    string json_winner = JsonSerializer.Serialize(winner);
    File.WriteAllText("winner.json", json_winner);
}

static void DownloadResults()
{
    int wx = 8, wy = 3;
    int x = wx + 3, y = wy + 3;
    int wid = 44, len = 12;

    string json_rounds = File.ReadAllText("rounds.json");
    int load_rounds = JsonSerializer.Deserialize<int>(json_rounds);

    string json_winner = File.ReadAllText("winner.json");
    string load_winner = JsonSerializer.Deserialize<string>(json_winner);

    DrawWindow(wx, wy, wid, len, $"Последние результаты", true);
    SetCursorPosition(wx + 2, wy + 2);
    Write($"- Последним победителем был игрок {load_winner}.");
    SetCursorPosition(wx + 2, wy + 4);
    Write($"- Последняя игра длилась {load_rounds} раунда(ов).");
    SetCursorPosition(wx + 4, wy + 7);
    Write("Нажмите Enter для возвращения в меню.");
    SetCursorPosition(wx + 20, wy + 9);
    ReadLine();
}

static void ShowRules()
{
    int wx = 8, wy = 3;
    int x = wx + 3, y = wy + 3;
    int wid = 80, len = 18;
    List<Trump> ruleTrumps = new List<Trump>()
    {
        new SvTPlank ( "17", "Планка очков понизится до 17."),
        new TwFPlank ( "24", "Планка очков вырастет до 24."),
        new TwSPlank ( "27", "Планка очков вырастет до 27."),
        new TwCard ( "2<", "Вы вытянете карту '2' если таковой нет на столе."),
        new ThCard ( "3<", "Вы вытянете карту '3' если таковой нет на столе."),
        new FrCard ( "4<", "Вы вытянете карту '4' если таковой нет на столе."),
        new FvCard ( "5<", "Вы вытянете карту '5' если таковой нет на столе."),
        new SxCard ( "6<", "Вы вытянете карту '6' если таковой нет на столе."),
        new SvCard ( "7<", "Вы вытянете карту '7' если таковой нет на столе."),
        new PrfCard( "*<", "Вы вытянете наиболее удачную для Вас карту."),
        new IncBet ( "˄", "Текущая ставка увеличится на 1."),
        new DiscBet ( "˅", "Текущая ставка уменьшится на 1."),
        new Destroy ( "X", "Обнуляет действие последнего козыря на столе."),
        new Disservice ( ">?", "Оппонент вытянет одну случайную карту."),
        new Switch ( "><", "Произойдёт обмен последними вытянутыми картами.")
    };

    DrawWindow(wx, wy, wid, len, $"Правила", true);

    SetCursorPosition(x + 2, y);
    Write("- Ваша цель: набрать сумму карт равную 21 или приближенное к нему.");
    y += 2;
    SetCursorPosition(x + 2, y);
    Write("- Карты не могут повторяться.");
    y += 2;
    SetCursorPosition(x + 2, y);
    Write("- Ставка увеличивается на 1 после каждого раунда.");
    y += 2;
    SetCursorPosition(x + 2, y);
    Write("- Раунд продолжается до тех пор, пока оба игрока не выберут (S)tay.");
    y += 2;
    SetCursorPosition(x + 2, y);
    Write("- Если игрок 1 выбрал (S)tay а игрок 1 использовал какую-либо");
    y += 1;
    SetCursorPosition(x + 3, y);
    Write("козырную карту, то ход переходит к игроку 1 вместо подведения итогов.");
    y += 2;
    SetCursorPosition(x + 2, y);
    Write("- Игра заканчивается, тогда когда очки одного из игроков равны 0.");

    x += 6;
    wx += 6;
    foreach (var card in ruleTrumps)
    {
        DrawWindow(wx, wy + y + 2, wid - 12, 7, $"Козырь '{card._sign}'", false);

        y+=6;
        SetCursorPosition(x, y);
        Write("╔═════╗");

        SetCursorPosition(x, y + 1);
        Write("║     ║");

        SetCursorPosition(x, y + 2);

        if ((int)(card._sign.Length) < 2)
        {
            Write($"║  {card._sign}  ║");
        }
        else
        {
            char f = card._sign[0], s = card._sign[1];

            Write($"║ {f} {s} ║");
        }

        SetCursorPosition(x, y + 3);
        Write("║     ║");

        SetCursorPosition(x, y + 4);
        Write("╚═════╝");

        SetCursorPosition(x + 10, y + 2);
        Write(card._desc);
        y += 2;
    }

    x += 12;
    wx += 12;

    DrawWindow(wx, wy + y + 2, 44, 7, $"Конец", false);
    SetCursorPosition(wx + 4, wy + y + 4);
    Write("Нажмите Enter для возвращения в меню.");
    SetCursorPosition(wx + wx / 2 +8, wy + y + 6);
    ReadLine();
}

static void DrawWindow(int x, int y, int width, int height, string title, bool clearornot)
{
    if (clearornot)
    {
        Clear();
    }

    // Верхняя граница
    SetCursorPosition(x, y);
    Write("╔" + new string('═', width - 2) + "╗");

    // Заголовок
    SetCursorPosition(x + (width - title.Length) / 2, y);
    Write(title);

    // Боковые границы
    for (int i = 1; i < height - 1; i++)
    {
        SetCursorPosition(x, y + i);
        Write("║");
        SetCursorPosition(x + width - 1, y + i);
        Write("║");
    }

    // Нижняя граница
    SetCursorPosition(x, y + height - 1);
    Write("╚" + new string('═', width - 2) + "╝");
}

static void PrintPlayersPov(Table table, int wid, int y, int x)
{
    if (table._whos_turn == true)
    {
        table._dealer._chandful.PrintCards(false, x, y);
        y += 6;
        SetCursorPosition(x, y);
        WriteLine($"Счёт: ??? + {table._dealer.CurrSum(false)}/{table._plank}");
        y += 2;
        SetCursorPosition(x-2, y);
        WriteLine(new string('═', wid-2));
        y+=2;
        SetCursorPosition(x + 2, y);
        Write($"{table.MrSaw._dealer}");
        y += 2;
        SetCursorPosition(x + 2, y);
        Write($"Ставка: {table.MrSaw._move}");
        y+=2;
        SetCursorPosition(x + 2, y);
        Write($"{table.MrSaw._debtor}");
        table._tabled_trumps.PrintTrumps(false, x + 9, y - 4);
        y += 2;
        SetCursorPosition(x - 2, y);
        WriteLine(new string('═', wid-2));
        y++;
        SetCursorPosition(x, y);
        if (table._debtor.CurrSum(true) <= table._plank)
        {
            WriteLine($"Счёт: {table._debtor.CurrSum(true)}/{table._plank}");
        }
        else
        {
            WriteLine($"Счёт: {table._debtor.CurrSum(true)}/{table._plank} Кажется, у кого-то проблемы..");
        }
        y += 3;
        table._debtor._chandful.PrintCards(true, x, y);
    }
    else
    {
        table._debtor._chandful.PrintCards(false, x, y);
        y += 6;
        SetCursorPosition(x, y);
        WriteLine($"Счёт: ??? + {table._debtor.CurrSum(false)}/{table._plank}");
        y += 2;
        SetCursorPosition(x - 2, y);
        WriteLine(new string('═', wid - 2));
        y += 2;
        SetCursorPosition(x + 2, y);
        Write($"{table.MrSaw._debtor}");
        y+=2;
        SetCursorPosition(x + 2, y);
        Write($"Ставка: {table.MrSaw._move}");
        y+=2;
        SetCursorPosition(x + 2, y);
        Write($"{table.MrSaw._dealer}");
        table._tabled_trumps.PrintTrumps(false, x + 9, y - 4);
        y += 2;
        SetCursorPosition(x - 2, y);
        WriteLine(new string('═', wid - 2));
        y++;
        SetCursorPosition(x, y);
        if (table._dealer.CurrSum(true) <= table._plank)
        {
            WriteLine($"Счёт: {table._dealer.CurrSum(true)}/{table._plank}");
        }
        else
        {
            WriteLine($"Счёт: {table._dealer.CurrSum(true)}/{table._plank} Кажется, у кого-то проблемы..");
        }
        y += 3;
        table._dealer._chandful.PrintCards(true, x, y);
    }
}

static void PrintTrumpsWindow(int x, int y, int len, int wid, Player player)
{
    DrawWindow(x, y, wid, len, new string(player._nick + "'s trump cards"), false);

    player._thandful.PrintTrumps(true, x+2, y+3);
}

public enum TypesOfReturns { PlankChangers = 1, CardGivers, BetChangers, Destroyer, Disservicer, Switcher }
public enum DrawOrStay { Draw, Stay, PrintTrumps, Awaitness }

public delegate void TakeTurn(int x, int y);

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
        new SvTPlank ( "17", "Планка очков понизится до 17."),
        new TwFPlank ( "24", "Планка очков вырастет до 24."),
        new TwSPlank ( "27", "Планка очков вырастет до 27."),
        new TwCard ( "2<", "Вы вытянете карту '2' если таковой нет на столе."),
        new ThCard ( "3<", "Вы вытянете карту '3' если таковой нет на столе."),
        new FrCard ( "4<", "Вы вытянете карту '4' если таковой нет на столе."),
        new FvCard ( "5<", "Вы вытянете карту '5' если таковой нет на столе."),
        new SxCard ( "6<", "Вы вытянете карту '6' если таковой нет на столе."),
        new SvCard ( "7<", "Вы вытянете карту '7' если таковой нет на столе."),
        new PrfCard( "*<", "Вы вытянете наиболее удачную для Вас карту."),
        new IncBet ( "˄", "Текущая ставка увеличится на 1."),
        new DiscBet ( "˅", "Текущая ставка уменьшится на 1."),
        new Destroy ( "X", "Обнуляет действие последнего козыря на столе."),
        new Disservice ( ">?", "Оппонент вытянет одну случайную карту."),
        new Switch ( "><", "Произойдёт обмен последними вытянутыми картами.")
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

    public void TakingTurn(int x, int y)
    {
        Turn(x, y);
    }

    public DrawOrStay GetPlayerChoice(int x, int y)
    {
        while (true)
        {
            SetCursorPosition(x, y);

            Write("(В)ытянуть карту, (О)статься или (П)осмотреть козыри?");

            SetCursorPosition(x, y + 1);
            string input = ReadLine().ToUpper();

            switch (input)
            {
                case "В": return DrawOrStay.Draw;

                case "О": return DrawOrStay.Stay;

                case "П": return DrawOrStay.PrintTrumps;
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

    public void PrintDeck(int x = 6, int y = 3)
    {

        foreach (var card in _deck)
        {
            x += 8;

            SetCursorPosition(x, y);
            Write("╔═════╗");

            SetCursorPosition(x, y + 1);
            Write("║     ║");

            SetCursorPosition(x, y + 2);

            if ((int)(card._value / 10) < 1)
            {
                Write($"║  {card._value}  ║");
            }
            else
            {
                int f = 1, s = card._value - 10;

                Write($"║ {f} {s} ║");
            }

            SetCursorPosition(x, y + 3);
            Write("║     ║");

            SetCursorPosition(x, y + 4);
            Write("╚═════╝");
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
    }

    public void Stay()
    {
        _readyToFinish = true;
    }

    public void TakeTurn(int x, int y)
    {
        SetCursorPosition(x, y );
        WriteLine($"- {_nick} делает свой выбор. Пожелаем ему удачи!");
    }

    public Table UseTrump(Table buffT, int x, int y)
    {
        WriteLine("Введите номер козыря который желаете использовать (0 - выход): ");
        SetCursorPosition(x + 42, y+7);
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

                if (buffT._whos_turn == false)
                {
                    buffT._tabled_trumps._trumps.Add(buffT._dealer._thandful._trumps[trump]);
                }
                else
                {
                    buffT._tabled_trumps._trumps.Add(buffT._debtor._thandful._trumps[trump]);
                }
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

                if (buffT._whos_turn == false)
                {
                    buffT._tabled_trumps._trumps.Add(buffT._dealer._thandful._trumps[trump]);
                }
                else
                {
                    buffT._tabled_trumps._trumps.Add(buffT._debtor._thandful._trumps[trump]);
                }
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

    public void PrintCards(bool ifwriteunk, int x = 6, int y = 3)
    {
        SetCursorPosition(x, y);
        Write("╔═════╗");

        SetCursorPosition(x, y + 1);
        Write("║     ║");

        SetCursorPosition(x, y + 2);

        if (ifwriteunk)
        {
            if ((int)(_unknown_card._value / 10) < 1)
            {
                Write($"║  {_unknown_card._value}  ║");
            }
            else
            {
                int f = 1, s = _unknown_card._value - 10;

                Write($"║ {f} {s} ║");
            }
        }
        else
        {
            Write($"║ ??? ║");
        }

            SetCursorPosition(x, y + 3);
        Write("║     ║");

        SetCursorPosition(x, y + 4);
        Write("╚═════╝");

        x += 8;

        foreach (var card in _cards)
        {
            SetCursorPosition(x, y);
            Write("╔═════╗");

            SetCursorPosition(x, y + 1);
            Write("║     ║");

            SetCursorPosition(x, y + 2);

            if ((int)(card._value / 10) < 1)
            {
                Write($"║  {card._value}  ║");
            }
            else
            {
                int f = 1, s = card._value - 10;

                Write($"║ {f} {s} ║");
            }

            SetCursorPosition(x, y + 3);
            Write("║     ║");

            SetCursorPosition(x, y + 4);
            Write("╚═════╝");

            x += 8;
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

    public int PrintTrumps(bool need_desc, int x = 6, int y = 3)
    {
        int buffx = x;

        foreach (var card in _trumps)
        {
            SetCursorPosition(x, y);
            Write("╔═════╗");

            SetCursorPosition(x, y + 1);
            Write("║     ║");

            SetCursorPosition(x, y + 2);

            if ((int)(card._sign.Length) < 2)
            {
                Write($"║  {card._sign}  ║");
            }
            else
            {
                char f = card._sign[0], s = card._sign[1];

                Write($"║ {f} {s} ║");
            }

            SetCursorPosition(x, y + 3);
            Write("║     ║");

            SetCursorPosition(x, y + 4);
            Write("╚═════╝");

            x += 8;
        }

        if (need_desc)
        {
            int count = 1;
            y += 6;
            x = buffx;

            SetCursorPosition(x, y);

            foreach (var card in _trumps)
            {
                y += count;
                WriteLine($"{count++}. - {card._desc}");
                SetCursorPosition(x, y);
            }
        }

        return y;
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
}

public abstract class Trump
{
    public string _sign { get; set; }

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

    public Trump(string sign, string desc)
    {
        _sign = sign;
        _desc = desc;
    }
}

public class Saw
{
    public int _dealer {  get; set; }
    public int _debtor { get; set; }
    public int _move { get; set; }

    public Saw() { }
    public Saw(Saw saw)
    {
        _dealer = saw._dealer;
        _debtor = saw._debtor;
        _move = saw._move;
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
    public SvTPlank(string sign, string desc) : base(sign, desc) { }
    public TypesOfReturns _tabled_type = TypesOfReturns.PlankChangers;

    public override ReturningTrump UseTrump(Table buffT)
    {
        return new ReturningTrump() { _is_succesed = true, _type = TypesOfReturns.PlankChangers, _plank = 17 };
    }
}

public class TwFPlank : Trump
{
    public TwFPlank(string sign, string desc) : base(sign, desc) { }
    public TypesOfReturns _tabled_type = TypesOfReturns.PlankChangers;

    public override ReturningTrump UseTrump(Table buffT)
    {
        return new ReturningTrump() { _is_succesed = true, _type = TypesOfReturns.PlankChangers, _plank = 24 };
    }
}

public class TwSPlank : Trump
{
    public TwSPlank(string sign, string desc) : base(sign, desc) { }
    public TypesOfReturns _tabled_type = TypesOfReturns.PlankChangers;

    public override ReturningTrump UseTrump(Table buffT)
    {
        return new ReturningTrump() { _is_succesed = true, _type = TypesOfReturns.PlankChangers, _plank = 27 };
    }
}


public class SvCard : Trump
{
    public SvCard(string sign, string desc) : base(sign, desc) { }

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
    public SxCard(string sign, string desc) : base(sign, desc) { }

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
    public FvCard(string sign, string desc) : base(sign, desc) { }

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
    public FrCard(string sign, string desc) : base(sign, desc) { }

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
    public ThCard(string sign, string desc) : base(sign, desc) { }

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
    public TwCard(string sign, string desc) : base(sign, desc) { }

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
    public PrfCard(string sign, string desc) : base(sign, desc) { }

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
    public IncBet(string sign, string desc) : base(sign, desc) { }
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
    public DiscBet(string sign, string desc) : base(sign, desc) { }
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
    public Destroy(string sign, string desc) : base(sign, desc) { }

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
    public Disservice(string sign, string desc) : base(sign, desc) { }

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
    public Switch(string sign, string desc) : base(sign, desc) { }

    public override ReturningTrump UseTrump(Table buffT)
    {
        if (buffT._dealer._chandful._cards.Count > 0 && buffT._debtor._chandful._cards.Count > 0)
        {
            return new ReturningTrump() { _is_succesed = true, _type = TypesOfReturns.Switcher, _dealer_sw_ind = buffT._dealer._chandful._cards.Count-1, _debtor_sw_ind = buffT._debtor._chandful._cards.Count - 1 };
        }
        return new ReturningTrump() { _is_succesed = false };
    }

}