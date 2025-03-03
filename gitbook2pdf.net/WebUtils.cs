namespace gitbook2pdf.net;

/// <summary>
/// Class containing generic web functions
/// </summary>
public class WebUtils
{
    /// <summary>
    /// Validate HTTP URL
    /// </summary>
    /// <param name="uriName">URL to validate</param>
    /// <returns>True if the url is valid, False otherwise</returns>
    public static bool IsValidURL(string uriName){
        Uri? uriResult;
        return Uri.TryCreate(uriName, UriKind.Absolute, out uriResult) 
            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }

}