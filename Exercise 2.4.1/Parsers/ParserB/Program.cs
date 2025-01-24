// Грамматика: S -> S ( S ) S | ϵ

using ParserB;

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

        RightRecursiveParser parser = new(source);
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