using Lexers.Locations;
using Lexers.Tokens.Numbers;

using System.Globalization;

namespace Lexers.Monads.Language.Numbers;

public record UncompletedDecimalNumberMonad(string Lexeme, Location Location) : UncompletedLexemeMonad(Lexeme, Location)
{
    public override LexemeMonad Append(char character, Position position)
    {
        if (char.IsDigit(character))
            return new UncompletedDecimalNumberMonad(Lexeme + character, Location.EndAt(position));

        return new CompletedLexemeMonad(new DecimalNumberToken(decimal.Parse(Lexeme, CultureInfo.InvariantCulture), Lexeme, Location), RootMonad.Remain(character, position));
    }

    public override LexemeMonad Finalize() => new CompletedLexemeMonad(new DecimalNumberToken(decimal.Parse(Lexeme, CultureInfo.InvariantCulture), Lexeme, Location), null);
}