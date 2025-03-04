namespace gitbook2pdf.net;

class Program
{
    static async Task Main(string[] args)
    {
        // License warning
        Console.WriteLine("");
        Console.WriteLine("");
        Console.WriteLine("gitbook2pdf.NET Copyright (C) 2025 alciarro93");
        Console.WriteLine("This program comes with ABSOLUTELY NO WARRANTY; for details type `show w'.");
        Console.WriteLine("This is free software, and you are welcome to redistribute it");
        Console.WriteLine("under certain conditions; type `show c' for details.");
        Console.WriteLine("");
        Console.WriteLine("");

        // URL of GitBook's table of contents
        bool url_valid = false;
        string? gitbook_url = null;
        while(!url_valid){
            Console.WriteLine("Insert URL of GitBook's table of contents: ");
            gitbook_url = Console.ReadLine();
            if (!string.IsNullOrEmpty(gitbook_url))
            {
                if (WebUtils.IsValidURL(gitbook_url))
                {
                    url_valid = true;
                }
            }
        }

        if (!string.IsNullOrEmpty(gitbook_url))
        {
            // start convertion to PDF
            await GitBookUtils.GitBookToPdf(gitbook_url);
        }
        
        // stop console
        Console.WriteLine("Press any key to close");
        Console.Read();
    }
}