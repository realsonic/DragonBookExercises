using Lexers.Locations;
using Lexers.Tokens.Numbers;

namespace Lexers.Monads.Language.Numbers;

public record UncompletedNumberMonad(string Lexeme, Location Location) : UncompletedLexemeMonad(Lexeme, Location)
{
    public override LexemeMonad Append(char character, Position position)
    {
        if (character is '.')
            return new UncompletedDecimalNumberMonad(Lexeme + character, Location.EndAt(position));
        
        if (char.IsDigit(character))
            return new UncompletedNumberMonad(Lexeme + character, Location.EndAt(position));

        return new CompletedLexemeMonad(new IntegerNumberToken(int.Parse(Lexeme), Lexeme, Location), RootMonad.Remain(character, position));
    }

    public override LexemeMonad Finalize() => new CompletedLexemeMonad(new IntegerNumberToken(int.Parse(Lexeme), Lexeme, Location), null);
}