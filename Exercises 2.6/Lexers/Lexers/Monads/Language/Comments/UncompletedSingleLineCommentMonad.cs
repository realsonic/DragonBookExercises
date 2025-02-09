using Lexers.Locations;

namespace Lexers.Monads.Language.Comments;

public record UncompletedSingleLineCommentMonad(string Lexeme, Location Location) : UncompletedLexemeMonad(Lexeme, Location)
{
    public override LexemeMonad Append(char character, Position position) => character switch
    {
        '\n' => new RootMonad(position),
        _ => new UncompletedSingleLineCommentMonad(Lexeme + character, Location.EndAt(position))
    };

    public override LexemeMonad Finalize() => new RootMonad(Location.End);
}
