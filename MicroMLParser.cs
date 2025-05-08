using System.Text.RegularExpressions;

public class ASTNode
{
    public string Type { get; set; } = "";
    public string? Value { get; set; }
    public List<ASTNode> Children { get; set; } = new();
}

public static class MicroMLParser
{
    public static ASTNode Parse(string code)
    {
        var lines = code.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        var root = new ASTNode { Type = "Program" };

        foreach (var line in lines)
        {
            var trimmed = line.Trim();
            if (trimmed.StartsWith("let "))
            {
                var letNode = ParseLet(trimmed);
                root.Children.Add(letNode);
            }
        }

        return root;
    }

    private static ASTNode ParseLet(string line)
    {
        // Pattern: let name = fun param -> body
        var matchFun = Regex.Match(line, @"let\s+(\w+)\s*=\s*fun\s+(\w+)\s*->\s*(.+)");
        if (matchFun.Success)
        {
            var funcName = matchFun.Groups[1].Value;
            var param = matchFun.Groups[2].Value;
            var body = matchFun.Groups[3].Value;

            return new ASTNode
            {
                Type = "Let",
                Value = funcName,
                Children = new List<ASTNode>
                {
                    new ASTNode
                    {
                        Type = "Function",
                        Children = new List<ASTNode>
                        {
                            new ASTNode { Type = "Parameter", Value = param },
                            new ASTNode { Type = "Body", Value = body }
                        }
                    }
                }
            };
        }

        // Pattern: let result = func arg
        var matchApp = Regex.Match(line, @"let\s+(\w+)\s*=\s*(\w+)\s+(.+)");
        if (matchApp.Success)
        {
            var resultName = matchApp.Groups[1].Value;
            var functionName = matchApp.Groups[2].Value;
            var argument = matchApp.Groups[3].Value;

            return new ASTNode
            {
                Type = "Let",
                Value = resultName,
                Children = new List<ASTNode>
                {
                    new ASTNode
                    {
                        Type = "Application",
                        Children = new List<ASTNode>
                        {
                            new ASTNode { Type = "Function", Value = functionName },
                            new ASTNode { Type = "Argument", Value = argument }
                        }
                    }
                }
            };
        }

        return new ASTNode
        {
            Type = "Unknown",
            Value = line
        };
    }
}
