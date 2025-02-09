using Lexers.Locations;
using Lexers.Tokens;

namespace Lexers.Monads.Language.Comparisions;

public record MaybeEqualityMonad(string Lexeme, Location Location) : UncompletedLexemeMonad(Lexeme, Location)
{
    public override LexemeMonad Append(char character, Position position) => character switch
    {
        '=' => new CompletedLexemeMonad(new ComparisionToken(Lexeme + character, Location.EndAt(position)), null),
        _ => new CompletedLexemeMonad(new Token(Lexeme, Location), RootMonad.Remain(character, position))
    };

    public override LexemeMonad Finalize() => new CompletedLexemeMonad(new ComparisionToken(Lexeme, Location), null);
}