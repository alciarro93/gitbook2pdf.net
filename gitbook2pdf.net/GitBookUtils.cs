using gitbook2pdf.net.Models;
using HiQPdf;

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
    /// <returns>Esito operazione</returns>
    public static async Task<bool> GitBookToPdf(string gitbook_url)
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
                using StreamReader reader = new("assets/css/html5_ua.css");
                string gitbook_css = reader.ReadToEnd();
                using StreamReader reader1 = new("assets/css/gitbook.css");
                gitbook_css += reader1.ReadToEnd();
                
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
                ConvertToPdf(html_content, gitbook_url);
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
    /// <returns>Esito operazione</returns>
    private static void ConvertToPdf(string html_content, string gitbook_url)
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

        /* -------------------- */
        // ONLY FOR DEBUG REASONS
        //using (StreamWriter outputFile = new StreamWriter(outputFolder + "/test.txt"))
        //{
        //    outputFile.WriteLine(html_content);
        //}
        /* -------------------- */

        // HTML to PDF convertion
        HtmlToPdf converter = new HtmlToPdf();
        converter.Document.PageOrientation = PdfPageOrientation.Portrait;
        converter.Document.PageSize = PdfPageSize.A4;
        converter.Document.Margins = new PdfMargins(50, 50, 30, 30);
        var pdfDoc = converter.ConvertHtmlToPdfDocument(html_content, uri.Host);
        pdfDoc.WriteToFile(outputFolder + ouputpdf);

        Console.WriteLine("PDF is available here: " + outputFolder + ouputpdf);
    }
    
}
