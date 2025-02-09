using Lexers.Locations;
using Lexers.Monads;
using Lexers.Monads.Language;
using Lexers.Tokens;

namespace Lexers;

public class MonadLexer(IEnumerable<char> input)
{
    public IEnumerable<Token> Scan()
    {
        UncompletedLexemeMonad uncompletedMonad = new RootMonad(Position.Initial);

        foreach ((char character, Position position) in new PositionedEnumerable(input))
        {
            LexemeMonad monad = uncompletedMonad + (character, position);

            CurrentPosition = monad.Location.End;

            while (monad is CompletedLexemeMonad completed)
            {
                yield return completed.Token;
                monad = completed.Remain ?? new RootMonad(monad.Location.End);
            }

            switch (monad)
            {
                case UncompletedLexemeMonad uncompleted:
                    uncompletedMonad = uncompleted;
                    break;
                case UnknownLexemeMonad unknown:
                    yield return new Token(unknown.Lexeme, unknown.Location);
                    uncompletedMonad = new RootMonad(unknown.Location.End);
                    break;
            }
        }

        LexemeMonad finalMonad = uncompletedMonad.Finalize();
        while (finalMonad is CompletedLexemeMonad completed)
        {
            yield return completed.Token;
            finalMonad = completed.Remain ?? new RootMonad(finalMonad.Location.End);
        }

        switch (finalMonad)
        {
            case UncompletedLexemeMonad uncompleted and not RootMonad:
                throw new InvalidOperationException($"После финализации монада не завершена: {uncompleted}");
            case UnknownLexemeMonad unknown:
                yield return new Token(unknown.Lexeme, unknown.Location);
                break;
        }
    }

    public Position CurrentPosition { get; private set; } = new(1, 0);
}
