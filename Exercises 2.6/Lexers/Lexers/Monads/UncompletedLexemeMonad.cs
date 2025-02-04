namespace Lexers.Monads;

public abstract record UncompletedLexemeMonad(string Lexeme, Position Position) : LexemeMonad(Lexeme, Position)
{
    public static LexemeMonad operator +(UncompletedLexemeMonad uncompletedLexemeMonad, char character) => uncompletedLexemeMonad.Append(character);

    public abstract LexemeMonad Append(char character);

    public abstract LexemeMonad Finalize();
}