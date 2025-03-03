using HtmlAgilityPack;

namespace gitbook2pdf.net.Models;

/// <summary>
/// Class of GitBook main element (table of contents)
/// </summary>
public class GitBookBase
{
    #region Properties
    
    /// <summary>
    /// Main URL of the GitBook
    /// </summary>
    public string MAIN_URL
    {
        get
        {
            return main_url;
        }
        set
        {
            main_url = value;
        }
    }
    private string main_url;

    /// <summary>
    /// List of links contained in the GitBook
    /// </summary>
    public List<string> CONTENTS_URLS
    {
        get
        {
            return contents_urls;
        }
        private set
        {
            contents_urls = value;
        }
    }
    private List<string> contents_urls;
    #endregion

    #region Contructors
    public GitBookBase(){
        main_url = "";
        contents_urls = new List<string>();
    }
    #endregion

    #region Public methods

    /// <summary>
    /// Fetch the list of links contained in the table of contents of the GitBook
    /// </summary>
    public virtual async Task<bool> Fetch(){
        bool myout = false;
        if (!string.IsNullOrEmpty(main_url)){
            using HttpClient client = new();
            var html = await client.GetStringAsync(main_url);
            if (html != null){
                if (!string.IsNullOrWhiteSpace(html))
                {
                    HtmlDocument htmlSnippet = new HtmlDocument();
                    htmlSnippet.LoadHtml(html);
                    contents_urls = new List<string>();
                    var links = htmlSnippet.DocumentNode.SelectNodes("//a[@href]");
                    if (links != null)
                    {
                        foreach (HtmlNode link in links)
                        {
                            if (link != null)
                            {
                                HtmlAttribute att = link.Attributes["href"];
                                if (att.Value.StartsWith(main_url))
                                {
                                    contents_urls.Add(att.Value);
                                }
                            }
                        }
                        myout = true;
                    }
                }
            }
        }
        return myout;
    }

    #endregion

}