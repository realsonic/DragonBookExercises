namespace Lexers.Monads;

public abstract record UncompletedMonad(string Lexeme, Position Position) : LexemeMonad(Lexeme, Position)
{
    public static LexemeMonad operator +(UncompletedMonad uncompletedMonad, char character) => uncompletedMonad.Append(character);

    public abstract LexemeMonad Append(char character);

    public abstract LexemeMonad Finalize();
}