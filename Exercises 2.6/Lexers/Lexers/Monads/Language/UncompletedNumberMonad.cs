using Lexers.Tokens;

namespace Lexers.Monads.Language;

public record UncompletedNumberMonad(string Lexeme, Position Position) : UncompletedLexemeMonad(Lexeme, Position)
{
    public override LexemeMonad Append(char character)
    {
        if (char.IsDigit(character))
            return new UncompletedNumberMonad(Lexeme + character, Position.AddColumn());

        return new CompletedLexemeMonad(
            Token: new NumberToken(Lexeme, int.Parse(Lexeme)),
            Position: Position,
            Remain: new RootMonad(Position) + character);
    }

    public override LexemeMonad Finalize() => new CompletedLexemeMonad(
            Token: new NumberToken(Lexeme, int.Parse(Lexeme)),
            Position: Position,
            Remain: null);
}