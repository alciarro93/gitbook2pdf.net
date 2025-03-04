using Microsoft.Extensions.DependencyInjection;
using SimpleHtmlToPdf.Interfaces;

namespace gitbook2pdf.net;

class Program
{
    static async Task Main(string[] args)
    {
        // License warning
        Console.WriteLine("gitbook2pdf.NET Copyright (C) 2025 alciarro93");
        Console.WriteLine("This program comes with ABSOLUTELY NO WARRANTY.");
        Console.WriteLine("This is free software, and you are welcome to redistribute it");
        Console.WriteLine("under certain conditions.");
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
            //Composition root
            IServiceProvider services = ConfigureServices();
            var htmlPDFServ = services.GetRequiredService<IConverter>();
            
            // start convertion to PDF
            await GitBookUtils.GitBookToPdf(gitbook_url, htmlPDFServ);
        }
        
        // stop console
        Console.WriteLine("Press any key to close");
        Console.Read();
    }

    private static IServiceProvider ConfigureServices() {
        IServiceCollection services = new ServiceCollection();
        services.AddSimpleHtmlToPdf();
        
        return services.BuildServiceProvider();
    }
}