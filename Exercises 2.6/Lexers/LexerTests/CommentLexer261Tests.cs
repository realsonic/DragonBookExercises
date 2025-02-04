using FluentAssertions;

using Lexers;
using Lexers.Tokens;
using Lexers.Tokens.Keywords;

namespace LexerTests;

public class CommentLexer261Tests
{
    [Fact(DisplayName = "Строка пробельных символов не возвращает токенов")]
    public void Whitespace_string_returns_no_tokens()
    {
        // Arrange
        CommentLexer261 sut = new(" \t  \n ");

        // Act
        var tokens = sut.ScanWithMonads();

        // Assert
        tokens.Should().BeEmpty();
    }

    [Fact(DisplayName = "При двух переносах строки номер строки - 3")]
    public void Two_new_lines_gives_Line_3()
    {
        // Arrange
        CommentLexer261 sut = new(" \n \n ");

        // Act
        var tokens = sut.ScanWithMonads();
        
        // !!!!!! todo для монад у нас Line хранится в позиции, а не в лексере!

        // Assert
        tokens.Should().BeEmpty();
        sut.CurrentPosition.Line.Should().Be(3);
    }

    [Fact(DisplayName = "Числа распознаются")]
    public void Numbers_parsed()
    {
        // Arrange
        CommentLexer261 sut = new("1 23 456");

        // Act
        var tokens = sut.ScanWithMonads();

        // Assert
        tokens.Should().BeEquivalentTo(new NumberToken[]
        {
            new("1", 1),
            new("23", 23),
            new("456", 456)
        });
    }

    [Fact(DisplayName = "Булевы литералы распознаются")]
    public void Boolean_parsed()
    {
        // Arrange
        CommentLexer261 sut = new("true false");

        // Act
        Token? firstToken = sut.GetNextToken();
        Token? secondToken = sut.GetNextToken();

        // Assert
        firstToken.Should().Be(new TrueToken());
        secondToken.Should().Be(new FalseToken());
    }

    [Fact(DisplayName = "Идентификаторы распознаются")]
    public void Id_parsed()
    {
        // Arrange
        CommentLexer261 sut = new("id1 id2");

        // Act
        Token? firstToken = sut.GetNextToken();
        Token? secondToken = sut.GetNextToken();

        // Assert
        firstToken.Should().Be(new IdToken("id1"));
        secondToken.Should().Be(new IdToken("id2"));
    }

    [Fact(DisplayName = "Другие символы возвращаются как отдельные токены")]
    public void Others_parsed()
    {
        // Arrange
        CommentLexer261 sut = new(" ~ !@\n$");

        // Act
        var tokens = sut.Scan();

        // Assert
        tokens.Should().BeEquivalentTo(new Token[]
        {
            new("~"), new("!"), new("@"), new("$")
        });
    }

    [Fact(DisplayName = "Однострочные комментарии пропускаются")]
    public void Single_line_comments_skipped()
    {
        // Arrange
        CommentLexer261 sut = new("""
            // Комментарий 1
            // Комментарий 2
            """);

        // Act
        var tokens = sut.Scan();

        // Assert
        tokens.Should().BeEmpty();
    }

    [Fact(DisplayName = "Число и слэши между однострочными комментариями возвращаются")]
    public void Number_and_slash_extracted_between_singleline_comments()
    {
        // Arrange
        CommentLexer261 sut = new("""
            // Комментарий 1
            1234
            // Комментарий 2
            / /
            // Комментарий 3
            """);

        // Act
        var tokens = sut.Scan();

        // Assert
        tokens.Should().BeEquivalentTo(new Token[]
        {
            new NumberToken("1234", 1234),
            new("/"),
            new("/")
        });
    }

    [Fact(DisplayName = "Многострочные комментарии пропускаются")]
    public void Multi_line_comments_skipped()
    {
        // Arrange
        CommentLexer261 sut = new("""
            /* 
                Большой
                многострочный
                комментарий
            */
            """);

        // Act
        var tokens = sut.Scan();

        // Assert
        tokens.Should().BeEmpty();
    }
}