namespace Lexers.Monads.Language;

public record RootMonad(Position Position) : UncompletedMonad(string.Empty, Position)
{
    public override LexemeMonad Append(char character)
    {
        if (character is ' ' or '\t' or '\r')
            return new RootMonad(Position.AddColumn());
        
        if (character is '\n')
            return new RootMonad(Position.NewLine());
        
        if (char.IsDigit(character))
            return new UncompletedNumberMonad(character.ToString(), Position.AddColumn());

        return new UnknownLexemeMonad(character.ToString(), Position.AddColumn());
    }

    public override LexemeMonad Finalize() => new RootMonad(this);
}
