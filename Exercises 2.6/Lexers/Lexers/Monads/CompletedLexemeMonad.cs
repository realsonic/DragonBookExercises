using Lexers.Tokens;

namespace Lexers.Monads;

public record CompletedLexemeMonad(Token Token, Position Position, LexemeMonad? Remain) : LexemeMonad(Token.Lexeme, Position);