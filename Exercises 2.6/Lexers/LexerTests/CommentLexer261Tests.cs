using FluentAssertions;

using Lexers;
using Lexers.Tokens;
using Lexers.Tokens.Keywords;

namespace LexerTests;

public class CommentLexer261Tests
{
    [Fact(DisplayName = "������ ���������� �������� �� ���������� �������")]
    public void Whitespace_string_returns_no_tokens()
    {
        // Arrange
        CommentLexer261 sut = new(" \t  \n ");

        // Act
        var tokens = sut.ScanWithMonads();

        // Assert
        tokens.Should().BeEmpty();
    }

    [Fact(DisplayName = "��� ���� ��������� ������ ����� ������ - 3")]
    public void Two_new_lines_gives_Line_3()
    {
        // Arrange
        CommentLexer261 sut = new(" \n \n ");

        // Act
        var tokens = sut.ScanWithMonads();
        
        // !!!!!! todo ��� ����� � ��� Line �������� � �������, � �� � �������!

        // Assert
        tokens.Should().BeEmpty();
        sut.CurrentPosition.Line.Should().Be(3);
    }

    [Fact(DisplayName = "����� ������������")]
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

    [Fact(DisplayName = "������ �������� ������������")]
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

    [Fact(DisplayName = "�������������� ������������")]
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

    [Fact(DisplayName = "������ ������� ������������ ��� ��������� ������")]
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

    [Fact(DisplayName = "������������ ����������� ������������")]
    public void Single_line_comments_skipped()
    {
        // Arrange
        CommentLexer261 sut = new("""
            // ����������� 1
            // ����������� 2
            """);

        // Act
        var tokens = sut.Scan();

        // Assert
        tokens.Should().BeEmpty();
    }

    [Fact(DisplayName = "����� � ����� ����� ������������� ������������� ������������")]
    public void Number_and_slash_extracted_between_singleline_comments()
    {
        // Arrange
        CommentLexer261 sut = new("""
            // ����������� 1
            1234
            // ����������� 2
            / /
            // ����������� 3
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

    [Fact(DisplayName = "������������� ����������� ������������")]
    public void Multi_line_comments_skipped()
    {
        // Arrange
        CommentLexer261 sut = new("""
            /* 
                �������
                �������������
                �����������
            */
            """);

        // Act
        var tokens = sut.Scan();

        // Assert
        tokens.Should().BeEmpty();
    }
}