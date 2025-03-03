namespace gitbook2pdf.net;

class Program
{
    static async Task Main(string[] args)
    {
        // URL of GitBook's table of contents
        bool url_valid = false;
        string? gitbook_url = null;
        while(!url_valid){
            Console.Write("Insert URL of GitBook's table of contents: ");
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