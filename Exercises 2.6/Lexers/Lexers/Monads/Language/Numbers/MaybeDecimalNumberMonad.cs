using Lexers.Locations;
using Lexers.Tokens;

namespace Lexers.Monads.Language.Numbers;

public record MaybeDecimalNumberMonad(string Lexeme, Location Location) : UncompletedLexemeMonad(Lexeme, Location)
{
    public override LexemeMonad Append(char character, Position position)
    {
        if (char.IsDigit(character))
            return new UncompletedDecimalNumberMonad(Lexeme + character, Location.EndAt(position));

        return new CompletedLexemeMonad(new Token(Lexeme, Location.EndAt(position)), RootMonad.Remain(character, position));
    }

    public override LexemeMonad Finalize() => new CompletedLexemeMonad(new Token(Lexeme, Location), null);
}
