using Lexers.Locations;
using Lexers.Monads;
using Lexers.Monads.Language;
using Lexers.Tokens;

namespace Lexers;

public class CommentLexer261(IEnumerable<char> input)
{
    //public CommentLexer261
    //{
    //    inputEnumerator = input.GetEnumerator();
    //    //hasCurrentChar = inputEnumerator.MoveNext();
    //}

    //public IEnumerable<Token> Scan()
    //{
    //    Token? token;
    //    while ((token = GetNextToken()) != null)
    //    {
    //        yield return token;
    //    }
    //}

    //public Token? GetNextToken()
    //{
    //    for (; ; NextChar())
    //    {
    //        if (CurrentChar is ' ' or '\t' or '\r') continue;
    //        else if (CurrentChar is '\n') /*TODO Line++*/;
    //        else if (CurrentChar is '/')
    //        {
    //            NextChar();
    //            if (CurrentChar is '/')
    //            {
    //                do
    //                {
    //                    NextChar();
    //                } while (CurrentChar is not null and not '\n');
    //            }
    //            else
    //            {
    //                return new Token("/");
    //            }
    //        }
    //        else break;
    //    }

    //    if (CurrentChar is not null && char.IsDigit(CurrentChar.Value))
    //    {
    //        string numberString = string.Empty;
    //        do
    //        {
    //            numberString += CurrentChar;
    //            NextChar();
    //        } while (CurrentChar is not null && char.IsDigit(CurrentChar.Value));

    //        return new NumberToken(numberString, int.Parse(numberString));
    //    }

    //    if (CurrentChar is not null && char.IsLetter(CurrentChar.Value))
    //    {
    //        string wordString = string.Empty;
    //        do
    //        {
    //            wordString += CurrentChar;
    //            NextChar();
    //        } while (CurrentChar is not null && char.IsLetterOrDigit(CurrentChar.Value));

    //        if (Words.TryGetValue(wordString, out Token? token))
    //            return token;

    //        IdToken idToken = new(wordString);
    //        Words.Add(wordString, idToken);
    //        return idToken;
    //    }

    //    if (CurrentChar is not null)
    //    {
    //        Token token = new(CurrentChar.Value.ToString());
    //        NextChar();
    //        return token;
    //    }

    //    return null;
    //}

    public IEnumerable<Token> ScanWithMonads()
    {
        UncompletedLexemeMonad uncompletedMonad = new RootMonad(Position.Initial);

        foreach ((char character, Position position) in new PositionedEnumerable(input))
        {
            LexemeMonad monad = uncompletedMonad + (character, position);

            CurrentPosition = monad.Location.End;

            while (monad is CompletedLexemeMonad completed)
            {
                yield return completed.Token;
                if (completed.Remain is not null)
                    monad = completed.Remain;
                else
                    break;
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
            if (completed.Remain is not null)
                finalMonad = completed.Remain;
            else
                break;
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

    //public int Line { get; private set; } = 1;
    public Position CurrentPosition { get; private set; } = new(1, 0);

    #region Private members

    //private readonly IEnumerator<char> inputEnumerator;

    //private char? CurrentChar => hasCurrentChar ? inputEnumerator.Current : null;
    //private void NextChar() => hasCurrentChar = inputEnumerator.MoveNext();

    private bool hasCurrentChar;

    //private readonly Dictionary<string, Token> Words = new()
    //{
    //    { "true", new TrueToken() },
    //    { "false", new FalseToken() }
    //};

    #endregion
}
