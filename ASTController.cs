using Microsoft.AspNetCore.Mvc;

public static class ASTController
{
    public static ASTNode Parse(string code)
    {
        return MicroMLParser.Parse(code);
    }
}