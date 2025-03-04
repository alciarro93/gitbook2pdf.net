using gitbook2pdf.net.Models;
using SimpleHtmlToPdf;
using SimpleHtmlToPdf.Interfaces;
using SimpleHtmlToPdf.Settings;
using SimpleHtmlToPdf.Settings.Enums;
using SimpleHtmlToPdf.UnmanagedHandler;

namespace gitbook2pdf.net;

/// <summary>
/// Class of usefull functions for GitBook convertion
/// </summary>
public class GitBookUtils
{
    /// <summary>
    /// Fetch all GitBook's pages and converts them in pdf
    /// </summary>
    /// <param name="gitbook_url">URL gitbook</param>
    /// <param name="htmlPDFServ">Service of package SimpleHtmlToPdf</param>
    /// <returns>Esito operazione</returns>
    public static async Task<bool> GitBookToPdf(string gitbook_url, IConverter htmlPDFServ)
    {
        bool myout = false;
        // Fetch all page URLs
        List<string> page_urls = await FetchAllPageURLs(gitbook_url);

        if (page_urls != null)
        {
            if (page_urls.Count > 0)
            {
                Console.WriteLine("Found " + page_urls.Count + " pages to convert.");
                string full_content = "";
                
                // fetch content of each URL
                int i = 0;
                foreach (var page_url in page_urls)
                {
                    var fetch = await FetchGitbookContent(page_url);
                    if (fetch != null)
                    {
                        if (fetch.IsFetchValid())
                        {
                            full_content += $"{fetch.Content}";
                            Console.WriteLine($"Fetched and added content from page {i+1}/{page_urls.Count}: {page_url}");
                        }
                        else{
                            Console.WriteLine($"Warning: Content for page {page_url} could not be fetched or was empty.");
                        }
                    }
                    else{
                        Console.WriteLine($"Warning: Content for page {page_url} could not be fetched or was empty.");
                    }
                    i++;
                }

                // Read GitBook CSS file
                using StreamReader reader = new("assets/css/style.css");
                string gitbook_css = reader.ReadToEnd();
                
                // Generate full HTML document
                string html_content = $"""
                    <!DOCTYPE html>
                    <html lang="en">
                    <head>
                        <meta charset="UTF-8">
                        <title>{gitbook_url} - GitBook PDF</title>
                        <style>{gitbook_css}</style>
                    </head>
                    <body>
                        {full_content}
                    </body>
                    </html>
                    """;

                // Convert to PDF
                ConvertToPdf(html_content, gitbook_url, htmlPDFServ);
            }
            else{
                Console.WriteLine("No pages found to convert.");
            }
        }
        else{
            Console.WriteLine("No pages found to convert.");
        }
        return myout;
    }

    /// <summary>
    /// Get all URLs to fetch
    /// </summary>
    /// <param name="gitbook_url">URL gitbook</param>
    /// <returns>Lista URLs</returns>
    private static async Task<List<string>> FetchAllPageURLs(string gitbook_url)
    {
        var gb = new GitBookBase();
        gb.MAIN_URL = gitbook_url;
        await gb.Fetch();
        return gb.CONTENTS_URLS;
    }

    /// <summary>
    /// Get HTML GitBook page
    /// </summary>
    /// <param name="page_url">URL GitBook link</param>
    /// <returns>Fetch object</returns>
    private static async Task<GitBookSinglePage> FetchGitbookContent(string page_url)
    {
        var gb = new GitBookSinglePage();
        gb.MAIN_URL = page_url;
        await gb.Fetch();
        return gb;
    }

    /// <summary>
    /// HTML to PDF
    /// </summary>
    /// <param name="html_content">HTML to convert to PDF</param>
    /// <param name="gitbook_url">URL of GitBook</param>
    /// <param name="htmlPDFServ">Service of package SimpleHtmlToPdf</param>
    /// <returns>Esito operazione</returns>
    private static void ConvertToPdf(string html_content, string gitbook_url, IConverter htmlPDFServ)
    {
        var uri = new Uri(gitbook_url);
        var ouputpdf = gitbook_url
            .Replace("http://", "")
            .Replace("https://", "")
            .Replace("%", "")
            .Replace("?", "")
            .Replace("=", "")
            .Replace("/", "_")
            + ".pdf";
        string outputFolder = "output/";

        if(!Directory.Exists(Environment.CurrentDirectory + @"\" + outputFolder))
            Directory.CreateDirectory(outputFolder);


        // HTML to PDF convertion
        var doc = new HtmlToPdfDocument()
        {
            GlobalSettings = {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings(50, 80, 50, 80),
                DPI = 300,
            },
            Objects = {
                new ObjectSettings()
                {
                    HtmlContent = html_content,
                    WebSettings = { DefaultEncoding = "utf-8" },
                },
            }
        };

        var mypdfFile = htmlPDFServ.Convert(doc);
        if(mypdfFile != null){
            try
            {
                File.WriteAllBytes(outputFolder + ouputpdf, mypdfFile);
                Console.WriteLine("PDF is available here: " + outputFolder + ouputpdf);
            }
            catch (System.Exception e)
            {
                Console.WriteLine("Error:" + e.Message);
                //throw;
            }
            
        }
        else{
            Console.WriteLine("Error converting HTML to PDF");
        }

        
        
    }
    
}
