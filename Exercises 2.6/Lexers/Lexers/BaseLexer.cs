using Lexers.Tokens;
using Lexers.Tokens.Keywords;

namespace Lexers;

public class BaseLexer(IEnumerable<char> input)
{
    public IEnumerable<Token> Scan()
    {
        while (inputEnumerator.MoveNext())
        {
            if (inputEnumerator.Current is ' ' or '\t')
            {
                continue;
            }

            if (inputEnumerator.Current == '\n')
            {
                Line++;
                continue;
            }

            if (char.IsDigit(inputEnumerator.Current))
            {
                string numChars = inputEnumerator.Current.ToString();
                while (inputEnumerator.MoveNext() && char.IsDigit(inputEnumerator.Current))
                {
                    numChars += inputEnumerator.Current;
                }

                yield return new NumberToken(numChars, int.Parse(numChars));
            }

            if (char.IsLetter(inputEnumerator.Current))
            {
                string wordChars = inputEnumerator.Current.ToString();
                while (inputEnumerator.MoveNext() && char.IsLetterOrDigit(inputEnumerator.Current))
                {
                    wordChars += inputEnumerator.Current;
                }

                if (Words.TryGetValue(wordChars, out Token? token))
                {
                    yield return token;
                }

                yield return new IdToken(wordChars);
            }

            yield return new Token(inputEnumerator.Current.ToString());
        }
    }

    public int Line { get; private set; } = 1;

    private readonly IEnumerator<char> inputEnumerator = input.GetEnumerator();

    private readonly Dictionary<string, Token> Words = new()
    {
        { "true", new TrueToken() },
        { "false", new FalseToken() }
    };
}
