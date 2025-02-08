using Lexers.Locations;
using Lexers.Tokens;

namespace Lexers.Monads.Language;

public record UncompletedNumberMonad(string Lexeme, Location Location) : UncompletedLexemeMonad(Lexeme, Location)
{
    public override LexemeMonad Append(char character, Position position)
    {
        if (char.IsDigit(character))
            return new UncompletedNumberMonad(Lexeme + character, Location.EndAt(position));

        return new CompletedLexemeMonad(
            Token: new NumberToken(int.Parse(Lexeme), Lexeme, Location),
            Location: Location,
            Remain: RootMonad.Remain(character, position));
    }

    public override LexemeMonad Finalize() => new CompletedLexemeMonad(
            Token: new NumberToken(int.Parse(Lexeme), Lexeme, Location),
            Location: Location,
            Remain: null);
}