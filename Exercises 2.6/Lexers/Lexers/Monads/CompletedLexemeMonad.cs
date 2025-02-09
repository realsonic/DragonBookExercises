using Lexers.Tokens;

namespace Lexers.Monads;

public record CompletedLexemeMonad(Token Token, LexemeMonad? Remain) : LexemeMonad(Token.Lexeme, Token.Location);