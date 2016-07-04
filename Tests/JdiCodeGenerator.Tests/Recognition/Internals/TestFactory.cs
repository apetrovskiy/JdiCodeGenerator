namespace JdiCodeGenerator.Tests.Recognition.Internals
{
    using System.IO;

    public class TestFactory
    {
        // public static string[] ExcludeList = new[] { "html", "head", "body", "#comment", "#text", "div", "meta", "p", "h1", "h2", "h3", "h4", "h5", "h6", "small", "font", "script", "i", "br", "hr", "strong", "style", "title", "li", "ul", "img", "span", "noscript" };
        public static string[] ExcludeList = { "html", "head", "body", "#comment", "#text", "meta", "h1", "h2", "h3", "h4", "h5", "h6", "small", "font", "script", "i", "br", "hr", "strong", "style", "title", "img", "noscript" };

        public static string GetBootstrap3Page(string path)
        {
            return GetHtml(Bootstrap3FirstPart, path, Bootstrap3LastPart);
        }

        public static string GetPlainHtml5Page(string path)
        {
            return GetHtml(PlainHtml5FirstPart, path, PlainHtml5LastPart);
        }

        static string GetHtml(string firstPart, string path, string lastPart)
        {
            var fullHtml = string.Empty;
#if DEBUG
            path = @"Debug\" + path;
#else
            path = @"Release\" + path;
#endif
            using (var reader = new StreamReader(path))
            {
                fullHtml = firstPart + reader.ReadToEnd() + lastPart;
                reader.Close();
            }
            return fullHtml;
        }

        const string Bootstrap3FirstPart = @"
<!DOCTYPE html>
<html lang=""en"">
  <head>
    <meta charset=""utf-8"">
    <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1"">
    <!-- The above 3 meta tags *must* come first in the head; any other head content must come *after* these tags -->
    <title>Bootstrap 101 Template</title>

    <!-- Bootstrap -->
    <link href=""css/bootstrap.min.css"" rel=""stylesheet"">

    <!-- HTML5 shim and Respond.js for IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
      <script src=""https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js""></script>
      <script src=""https://oss.maxcdn.com/respond/1.4.2/respond.min.js""></script>
    <![endif]-->
  </head>
  <body>
";
        const string Bootstrap3LastPart = @"
    <!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
    <script src=""https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js""></script>
    <!-- Include all compiled plugins (below), or include individual files as needed -->
    <script src=""js/bootstrap.min.js""></script>
  </body>
</html>
";

        const string PlainHtml5FirstPart = @"
<!DOCTYPE html>
<html lang=""en"">
  <head>
    <meta charset=""utf-8"">
    <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1"">
    <!-- The above 3 meta tags *must* come first in the head; any other head content must come *after* these tags -->
    <title>HTML5 Template</title>

  </head>
  <body>
";
        const string PlainHtml5LastPart = @"
  </body>
</html>
";
    }
}