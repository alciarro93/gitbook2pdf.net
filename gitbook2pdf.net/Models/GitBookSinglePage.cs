using HtmlAgilityPack;

namespace gitbook2pdf.net.Models;

/// <summary>
/// Class of GitBook single page
/// </summary>
public class GitBookSinglePage : GitBookBase
{
    #region Properties
    
    /// <summary>
    /// Page's title
    /// </summary>
    public string Title
    {
        get
        {
            return title;
        }
        set
        {
            title = value;
        }
    }
    private string title;

    /// <summary>
    /// Page's content
    /// </summary>
    public string Content
    {
        get
        {
            return content;
        }
        set
        {
            content = value;
        }
    }
    private string content;

    #endregion

    #region Contructors
    public GitBookSinglePage(){
        title = "";
        content = "";
    }
    #endregion

    #region Public methods

    /// <summary>
    /// Fetch the html content of a page of the GitBook
    /// </summary>
    /// <returns></returns>
    public override async Task<bool> Fetch()
    {
        bool myout = false;
        if (!string.IsNullOrEmpty(MAIN_URL)){
            using HttpClient client = new();
            var html = await client.GetStringAsync(MAIN_URL);
            if (html != null){
                if (!string.IsNullOrWhiteSpace(html))
                {
                    HtmlDocument htmlSnippet = new HtmlDocument();
                    htmlSnippet.LoadHtml(html);

                    var titoloNodo = htmlSnippet.DocumentNode.SelectSingleNode("//title");
                    if (titoloNodo != null)
                    {
                        if (titoloNodo.InnerText != null)
                        {
                            if (!string.IsNullOrWhiteSpace(titoloNodo.InnerText))
                            {
                                title = titoloNodo.InnerText;
                            }
                            else{
                                title = "GitBook Document";
                            }
                        }
                    }

                    //SelectContentStructure(htmlSnippet, "//div[@class='book-body']");
                    SelectContentStructure(htmlSnippet, "//main/header");
                    SelectContentStructure(htmlSnippet, "//main/div");
                    // Add more checks based on your GitBook structure

                    myout = true;
                }
            }
        }
        return myout;
    }

    /// <summary>
    /// Get all the matching selectors contained in the html snippet
    /// </summary>
    /// <param name="htmlSnippet">HTML snippet</param>
    /// <param name="myfilter">Selector to find</param>
    private void SelectContentStructure(HtmlDocument htmlSnippet, string myfilter){
        var nodes = htmlSnippet.DocumentNode.SelectNodes(myfilter);
        if (nodes != null)
        {
            HtmlNode mynode = nodes.First();
            if (mynode != null)
            {
                content += mynode.InnerHtml;
            }
        }
    }

    /// <summary>
    /// Return if the fetch of html content is valid
    /// </summary>
    /// <returns>True if it's valid, False otherwhise</returns>
    public bool IsFetchValid(){
        return !string.IsNullOrEmpty(content);
    }

    #endregion

}