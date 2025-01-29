namespace Lexers.Tokens;

public record NumberToken(string Lexeme, int Value) : Token(Lexeme);