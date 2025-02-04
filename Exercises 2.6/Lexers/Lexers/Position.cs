namespace Lexers;

public record Position(int Line, int Column)
{
    public Position AddColumn() => new(Line, Column + 1);

    public Position NewLine() => new(Line + 1, 1);
}