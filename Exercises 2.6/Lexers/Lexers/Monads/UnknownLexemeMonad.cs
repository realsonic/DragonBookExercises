namespace Lexers.Monads;

public record UnknownLexemeMonad(string Lexeme, Position Position) : LexemeMonad(Lexeme, Position);