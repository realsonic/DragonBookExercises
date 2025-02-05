using Lexers.Tokens;
using Lexers.Tokens.Keywords;

namespace Lexers;

public class BaseLexer
{
    public BaseLexer(IEnumerable<char> input)
    {
        inputEnumerator = input.GetEnumerator();
        hasCurrentChar = inputEnumerator.MoveNext();
    }

    public IEnumerable<Token> Scan()
    {
        Token? token;
        while ((token = GetNextToken()) != null)
        {
            yield return token;
        }
    }

    public Token? GetNextToken()
    {
        for (; ; NextChar())
        {
            if (CurrentChar is ' ' or '\t') continue;
            else if (CurrentChar is '\n') Line++;
            else break;
        }

        if (CurrentChar is not null && char.IsDigit(CurrentChar.Value))
        {
            string numberString = string.Empty;
            do
            {
                numberString += CurrentChar;
                NextChar();
            } while (CurrentChar is not null && char.IsDigit(CurrentChar.Value));

            return new NumberToken(numberString, null/*TODO*/, int.Parse(numberString));
        }

        if (CurrentChar is not null && char.IsLetter(CurrentChar.Value))
        {
            string wordString = string.Empty;
            do
            {
                wordString += CurrentChar;
                NextChar();
            } while (CurrentChar is not null && char.IsLetterOrDigit(CurrentChar.Value));

            if (Words.TryGetValue(wordString, out Token? token))
                return token;

            IdToken idToken = new(wordString, null/*TODO*/);
            Words.Add(wordString, idToken);
            return idToken;
        }

        if (CurrentChar is not null)
        {
            Token token = new(CurrentChar.Value.ToString(), null/*TODO*/);
            NextChar();
            return token;
        }

        return null;
    }

    public int Line { get; private set; } = 1;

    #region Private members

    private readonly IEnumerator<char> inputEnumerator;

    private char? CurrentChar => hasCurrentChar ? inputEnumerator.Current : null;
    private void NextChar() => hasCurrentChar = inputEnumerator.MoveNext();

    private bool hasCurrentChar;

    private readonly Dictionary<string, Token> Words = new()
    {
        { "true", new TrueToken(null) },
        { "false", new FalseToken(null) }
    };

    #endregion
}
