using Lexers.Locations;
using Lexers.Tokens;

namespace Lexers.Monads.Language.Comments;
public record MaybeCommentMonad(string Lexeme, Location Location) : UncompletedLexemeMonad(Lexeme, Location)
{
    public override LexemeMonad Append(char character, Position position) => character switch
    {
        '/' => new UncompletedSingleLineCommentMonad(Lexeme + character, Location.EndAt(position)),
        '*' => new UncompletedMultiLineCommentMonad(false, Lexeme + character, Location.EndAt(position)),
        _ => new CompletedLexemeMonad(new Token(Lexeme, Location), RootMonad.Remain(character, position))
    };

    public override LexemeMonad Finalize() => new CompletedLexemeMonad(new Token(Lexeme, Location), null);
}
