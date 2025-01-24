// Грамматика: S -> +SS | -SS | a

using ParserA;

while (true)
    try
    {
        Console.Write("Source: ");
        string? source = Console.ReadLine();
        if (source is null)
        {
            Console.WriteLine("Exiting...");
            break;
        }

        Parser parser = new(source);
        parser.Parse();

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Parsed ok.\n");
        Console.ResetColor();
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(ex);
        Console.ResetColor();
        Console.WriteLine();
    }