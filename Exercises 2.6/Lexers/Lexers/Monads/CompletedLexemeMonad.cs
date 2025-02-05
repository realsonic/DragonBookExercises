using Lexers.Locations;
using Lexers.Tokens;

namespace Lexers.Monads;

public record CompletedLexemeMonad(Token Token, Location Location, LexemeMonad? Remain) : LexemeMonad(Token.Lexeme, Location);