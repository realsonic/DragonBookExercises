// Грамматика: S ➡️ 0 S 1 | 0 1

using ParserC;

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

        ParserWithRewrittenGrammar parser = new(source);
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