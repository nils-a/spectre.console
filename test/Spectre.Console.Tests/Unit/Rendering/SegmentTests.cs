namespace Spectre.Console.Tests.Unit;

public sealed class SegmentTests
{
    [UsesVerify]
    public sealed class TheSplitMethod
    {
        [Theory]
        [InlineData("Foo Bar", 0, "", "Foo Bar")]
        [InlineData("Foo Bar", 1, "F", "oo Bar")]
        [InlineData("Foo Bar", 2, "Fo", "o Bar")]
        [InlineData("Foo Bar", 3, "Foo", " Bar")]
        [InlineData("Foo Bar", 4, "Foo ", "Bar")]
        [InlineData("Foo Bar", 5, "Foo B", "ar")]
        [InlineData("Foo Bar", 6, "Foo Ba", "r")]
        [InlineData("Foo Bar", 7, "Foo Bar", null)]
        [InlineData("Foo 测试 Bar", 0, "", "Foo 测试 Bar")]
        [InlineData("Foo 测试 Bar", 1, "F", "oo 测试 Bar")]
        [InlineData("Foo 测试 Bar", 2, "Fo", "o 测试 Bar")]
        [InlineData("Foo 测试 Bar", 3, "Foo", " 测试 Bar")]
        [InlineData("Foo 测试 Bar", 4, "Foo ", "测试 Bar")]
        [InlineData("Foo 测试 Bar", 5, "Foo 测", "试 Bar")]
        [InlineData("Foo 测试 Bar", 6, "Foo 测", "试 Bar")]
        [InlineData("Foo 测试 Bar", 7, "Foo 测试", " Bar")]
        [InlineData("Foo 测试 Bar", 8, "Foo 测试", " Bar")]
        [InlineData("Foo 测试 Bar", 9, "Foo 测试 ", "Bar")]
        [InlineData("Foo 测试 Bar", 10, "Foo 测试 B", "ar")]
        [InlineData("Foo 测试 Bar", 11, "Foo 测试 Ba", "r")]
        [InlineData("Foo 测试 Bar", 12, "Foo 测试 Bar", null)]
        public void Should_Split_Segment_Correctly(string text, int offset, string expectedFirst, string expectedSecond)
        {
            // Given
            var style = new Style(Color.Red, Color.Green, Decoration.Bold);
            var segment = new Segment(text, style);

            // When
            var (first, second) = segment.Split(offset);

            // Then
            first.Text.ShouldBe(expectedFirst);
            first.Style.ShouldBe(style);
            second?.Text?.ShouldBe(expectedSecond);
            second?.Style?.ShouldBe(style);
        }
    }

    [UsesVerify]
    public sealed class TheSplitLinesMethod
    {
        [Fact]
        public void Should_Split_Segment()
        {
            // Given, When
            var lines = Segment.SplitLines(
                new[]
                {
                        new Segment("Foo"),
                        new Segment("Bar"),
                        new Segment("\n"),
                        new Segment("Baz"),
                        new Segment("Qux"),
                        new Segment("\n"),
                        new Segment("Corgi"),
                });

            // Then
            lines.Count.ShouldBe(3);

            lines[0].Count.ShouldBe(2);
            lines[0][0].Text.ShouldBe("Foo");
            lines[0][1].Text.ShouldBe("Bar");

            lines[1].Count.ShouldBe(2);
            lines[1][0].Text.ShouldBe("Baz");
            lines[1][1].Text.ShouldBe("Qux");

            lines[2].Count.ShouldBe(1);
            lines[2][0].Text.ShouldBe("Corgi");
        }

        [Fact]
        public void Should_Split_Segment_With_Windows_LineBreak()
        {
            // Given, When
            var lines = Segment.SplitLines(
                new[]
                {
                        new Segment("Foo"),
                        new Segment("Bar"),
                        new Segment("\r\n"),
                        new Segment("Baz"),
                        new Segment("Qux"),
                        new Segment("\r\n"),
                        new Segment("Corgi"),
                });

            // Then
            lines.Count.ShouldBe(3);

            lines[0].Count.ShouldBe(2);
            lines[0][0].Text.ShouldBe("Foo");
            lines[0][1].Text.ShouldBe("Bar");

            lines[1].Count.ShouldBe(2);
            lines[1][0].Text.ShouldBe("Baz");
            lines[1][1].Text.ShouldBe("Qux");

            lines[2].Count.ShouldBe(1);
            lines[2][0].Text.ShouldBe("Corgi");
        }

        [Fact]
        public void Should_Split_Segments_With_Linebreak_In_Text()
        {
            // Given, Given
            var lines = Segment.SplitLines(
                new[]
                {
                        new Segment("Foo\n"),
                        new Segment("Bar\n"),
                        new Segment("Baz"),
                        new Segment("Qux\n"),
                        new Segment("Corgi"),
                });

            // Then
            lines.Count.ShouldBe(4);

            lines[0].Count.ShouldBe(1);
            lines[0][0].Text.ShouldBe("Foo");

            lines[1].Count.ShouldBe(1);
            lines[1][0].Text.ShouldBe("Bar");

            lines[2].Count.ShouldBe(2);
            lines[2][0].Text.ShouldBe("Baz");
            lines[2][1].Text.ShouldBe("Qux");

            lines[3].Count.ShouldBe(1);
            lines[3][0].Text.ShouldBe("Corgi");
        }
    }

    [Fact]
    public void Should_Split_with_EmptyLink_To_Non_EmptyLinks()
    {
        // Given
        const string Link = "https://some.link/somewhere";
        var segment = new Segment(Link, new Style(link: Constants.EmptyLink));

        // When
        var (first, second) = segment.Split(20);

        // Then
        first.Style.Link.ShouldBe(Link);
        second.Style.Link.ShouldBe(Link);
    }

    [Fact]
    public void Should_SplitLines_with_EmptyLink_To_Non_EmptyLinks_Long_Lines()
    {
        // Given
        const string Link = "https://some.link/somewhere";
        var segment = new Segment(Link, new Style(link: Constants.EmptyLink));

        // When
        var lines = Segment.SplitLines(new[] { segment }, 20);

        // Then
        lines.Count.ShouldBe(2);
        lines[0][0].Style.Link.ShouldBe(Link);
        lines[1][0].Style.Link.ShouldBe(Link);
    }

    [Fact]
    public void Should_SplitLines_with_EmptyLink_To_Non_EmptyLinks_Short_Lines()
    {
        // Given
        const string Link = "https://some.link/somewhere";
        var segment = new Segment(Link, new Style(link: Constants.EmptyLink));

        // When
        var lines = Segment.SplitLines(new[] { segment }, Link.Length + 1);

        // Then
        lines.Count.ShouldBe(1);
        lines[0][0].Style.Link.ShouldBe(Link);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(Overflow.Fold)]
    [InlineData(Overflow.Ellipsis)]
    [InlineData(Overflow.Crop)]
    public void Should_SplitOverflow_with_EmptyLink_To_Non_EmptyLinks(Overflow? overflow)
    {
        // Given
        const string Link = "https://some.link/somewhere";
        var segment = new Segment(Link, new Style(link: Constants.EmptyLink));

        // When
        var segments = Segment.SplitOverflow(segment, overflow, 20);

        // Then
        foreach (var s in segments)
        {
            s.Style.Link.ShouldBe(Link);
        }
    }

    [Fact]
    public void Should_Truncate_with_EmptyLink_To_Non_EmptyLinks()
    {
        // Given
        const string Link = "https://some.link/somewhere";
        var segment = new Segment(Link, new Style(link: Constants.EmptyLink));

        // When
        var segments = Segment.Truncate(new[] { segment }, 20);

        // Then
        foreach (var s in segments)
        {
            s.Style.Link.ShouldBe(Link);
        }
    }

    [Fact]
    public void Should_TruncateWithEllipsis_with_EmptyLink_To_Non_EmptyLinks()
    {
        // Given
        const string Link = "https://some.link/somewhere";
        var segment = new Segment(Link, new Style(link: Constants.EmptyLink));

        // When
        var segments = Segment.TruncateWithEllipsis(new[] { segment }, 20);

        // Then
        foreach (var s in segments)
        {
            s.Style.Link.ShouldBe(Link);
        }
    }

    [Fact]
    public void Should_Merge_with_EmptyLink_To_Two_Non_EmptyLinks()
    {
        // Given
        const string Link1 = "https://some.link/somewhere";
        const string Link2 = "https://some.other.link/somewhere/else";
        var segment1 = new Segment(Link1, new Style(link: Constants.EmptyLink));
        var segment2 = new Segment(Link2, new Style(link: Constants.EmptyLink));

        // When
        var segments = Segment.Merge(new[] { segment1, segment2 }).ToArray();

        // Then
        segments.Length.ShouldBe(2);
        segments[0].Style.Link.ShouldBe(Link1);
        segments[1].Style.Link.ShouldBe(Link2);
    }
}
