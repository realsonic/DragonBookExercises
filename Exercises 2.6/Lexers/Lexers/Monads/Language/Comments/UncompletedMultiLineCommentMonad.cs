using Lexers.Locations;

namespace Lexers.Monads.Language.Comments;

public record UncompletedMultiLineCommentMonad(bool EndingAsteriskMet, string Lexeme, Location Location) : UncompletedLexemeMonad(Lexeme, Location)
{
    public override LexemeMonad Append(char character, Position position) => character switch
    {
        '/' when EndingAsteriskMet => new RootMonad(position),
        '*' => new UncompletedMultiLineCommentMonad(true, Lexeme + character, Location.EndAt(position)),
        _ => new UncompletedMultiLineCommentMonad(false, Lexeme + character, Location.EndAt(position))
    };

    public override LexemeMonad Finalize() => new UnknownLexemeMonad(Lexeme, Location);
}