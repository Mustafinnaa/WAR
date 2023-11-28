using System;

public class Program
{
    public static Random Random = new Random();

    // Создаем игровое поле 10x10
    public static int[,] boardPlayer = new int[10, 10];
    public static int[,] boardBot = new int[10, 10];
    public static int lastShotX = 1;
    public static int lastShotY = 1;

    // создаем игровое поле, где x - это наши кораблики
    public static void PrintBoard(int[,] board)
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                if (board[j, i] == 1)
                {
                    Console.Write(" -O- ");
                }
                else
                {
                    Console.Write(" ~~~ ");
                }
            }

            Console.WriteLine();
        }
        Console.WriteLine();
    }


    public static void PrintBoardBot()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                if (boardBot[j, i] == 2)
                {
                    Console.Write(" X ");
                }
                else if (boardBot[j, i] == 3)
                {
                    Console.Write(" O ");
                }
                else
                {
                    Console.Write(" ~ ");
                }
            }

            Console.WriteLine();
        }
        Console.WriteLine();
    }


    public static bool[,] hasShot = new bool[10, 10];

    public static void Main()
    {
        // отображаем где расположены наши кораблики
        PlaceShips();
        PrintBoard(boardPlayer);

        // Основной игровой цикл
        while (true)
        {
        meow:
            Console.Write("Введите координаты для выстрела (x, y):\n");
            int x = Convert.ToInt32(Console.ReadLine());
            int y = Convert.ToInt32(Console.ReadLine());

            if (x >= 10 || y >= 10)
            {
                Console.WriteLine("Координаты выходят за пределы игрового поля, попробуйте еще раз (введите координаты от 0 до 10)");
                goto meow;
            }

            if (hasShot[x, y])
            {
                Console.WriteLine("Сюда уже стреляли, го другую координату");
                goto meow;
            }

            hasShot[x, y] = true;

            // Проверяем, попал ли пользователь по кораблю

            if (boardPlayer[x, y] == 1)
            {
                Console.WriteLine("Вы попали по кораблику!!");

                goto meow;
            }

            // Ход компьютера
            ComputerTurn();

        }
    }

    // Размещаем корабли на игровом поле
    // Проверяем, можно ли разместить корабль на заданных координатах
    public static bool CanAddShip(int x, int y, int length, int orientation)
    {
        if (IsShipClose(x, y, length, orientation))
        {
            return false;
        }

        if (orientation == 0)
        {
            for (int i = x; i < x + length; i++)
            {
                if (i >= 10 || boardPlayer[i, y] == 1)
                {
                    return false;
                }
            }
        }
        else
        {
            for (int i = y; i < y + length; i++)
            {
                if (i >= 10 || boardPlayer[x, i] == 1)
                {
                    return false;
                }
            }
        }

        return true;
    }


    // Размещаем корабль заданной длины на игровом поле
    public static void PlaceShip(int length = 10)
    {
        while (true)
        {
            int x = Random.Next(10);
            int y = Random.Next(10);
            int orientation = Random.Next(2);
            if (CanAddShip(x, y, length, orientation))
            {
                PlaceShipOnBoard(x, y, length, orientation);
                break;
            }
        }
    }



    // Размещаем корабль на игровом поле
    public static void PlaceShipOnBoard(int x, int y, int length, int orientation)
    {
        if (orientation == 0)
        {
            for (int i = x; i < x + length; i++)
            {
                boardPlayer[i, y] = 1;
            }
        }
        else
        {
            for (int i = y; i < y + length; i++)
            {
                boardPlayer[x, i] = 1;
            }
        }
    }
    public static bool IsShipClose(int x, int y, int length, int orientation)
    {
        if (orientation == 0 || length == 10)
        {
            for (int i = Math.Max(0, x - 1); i <= Math.Min(9, x + length); i++)
            {
                for (int j = Math.Max(0, y - 1); j <= Math.Min(9, y + 1); j++)
                {
                    if (boardPlayer[i, j] == 1)
                    {
                        return true;
                    }
                }
            }
        }
        else
        {
            for (int i = Math.Max(0, x - 1); i <= Math.Min(9, x + 1); i++)
            {
                for (int j = Math.Max(0, y - 1); j <= Math.Min(9, y + length); j++)
                {
                    if (boardPlayer[i, j] == 1)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public static void PlaceShips()
    {
        // Размещаем четырехпалубный корабль
        for (int i = 0; i < 1; i++)
        {
            PlaceShip(4);
        }

        // Размещаем трехпалубные корабли
        for (int i = 0; i < 2; i++)
        {
            PlaceShip(3);
        }

        // Размещаем двухпалубные корабли
        for (int i = 0; i < 3; i++)
        {
            PlaceShip(2);
        }

        // Размещаем однопалубные корабли
        for (int i = 0; i < 4; i++)
        {
            PlaceShip(1);
        }
    }
    public static bool IsLastShotHit()
    {
        if (boardPlayer[lastShotX, lastShotY] == 1)
        {
            return true;
        }
        return false;
    }

    // Ход компьютера
    public static void ComputerTurn()
    {
        int x = Random.Next(10);
        int y = Random.Next(10);

        Console.WriteLine($"Компьютер выстрелил в координаты ({x}, {y})");

        // Проверяем, попал ли компьютер по кораблю

        if (boardPlayer[x, y] == 1)
        {
            Console.WriteLine("Компьютер попал по вашему кораблю!");
            boardBot[x, y] = 2;
            lastShotX = x;
            lastShotY = y;

            // Проверяем, был ли последний выстрел по кораблю
            if (IsLastShotHit())
            {
                for (int i = x - 1; i <= x + 1; i++)
                {
                    for (int j = y - 0; j <= y + 1; j++)
                    {
                        if (i >= 0 && i < 10 && j >= 0 && j < 10 && boardBot[i, j] == 0)
                        {
                            Console.WriteLine("Компьютер стреляет в клетку ({0}, {1})", i, j);
                            boardBot[i, j] = 3;
                            PrintBoardBot();
                            return;
                        }
                    }
                }

            }


        }
        else
        {
            Console.WriteLine("Компьютер промахнулся!");
            boardBot[x, y] = 3;
            lastShotX = x;
            lastShotY = y;
        }

        PrintBoardBot();
    }


} 
