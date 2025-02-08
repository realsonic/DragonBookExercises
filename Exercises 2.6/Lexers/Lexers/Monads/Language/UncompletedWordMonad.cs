using Lexers.Locations;
using Lexers.Tokens;
using Lexers.Tokens.Keywords;

using System.Diagnostics.CodeAnalysis;

namespace Lexers.Monads.Language;
internal record UncompletedWordMonad(string Lexeme, Location Location) : UncompletedLexemeMonad(Lexeme, Location)
{
    public override LexemeMonad Append(char character, Position position)
    {
        if (char.IsLetterOrDigit(character))
            return new UncompletedWordMonad(Lexeme + character, Location.EndAt(position));

        if (TryGetKeywordToken(Lexeme, Location, out Token? token))
            return new CompletedLexemeMonad(token, Location, RootMonad.Remain(character, position));

        return new CompletedLexemeMonad(new IdToken(Lexeme, Location), Location, RootMonad.Remain(character, position));
    }

    public override LexemeMonad Finalize()
    {
        if (TryGetKeywordToken(Lexeme, Location, out Token? token))
            return new CompletedLexemeMonad(token, Location, null);

        return new CompletedLexemeMonad(new IdToken(Lexeme, Location), Location, null);
    }

    private bool TryGetKeywordToken(string lexeme, Location location, [MaybeNullWhen(false)] out Token token)
    {
        if (keyWords.TryGetValue(lexeme, out Func<Location, Token>? tokenFunc))
        {
            token = tokenFunc(location);
            return true;
        }

        token = null;
        return false;
    }

    private readonly Dictionary<string, Func<Location, Token>> keyWords = new()
    {
        ["true"] = Location => new TrueToken(Location),
        ["false"] = Location => new FalseToken(Location)
    };
}