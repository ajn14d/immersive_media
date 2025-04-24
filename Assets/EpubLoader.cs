using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using VersOne.Epub;

public class EpubLoader : MonoBehaviour
{
    public string epubFileName = "example.epub";  // Replace with your file name

    void Start()
    {
        Debug.Log("EpubLoader Start method called!");

        string text = LoadCleanedBookText();
        if (text != null)
        {
            Debug.Log("Book loaded successfully!");
        }
        else
        {
            Debug.LogError("Failed to load the book.");
        }
    }

    public string LoadCleanedBookText()
    {
        string fullPath = Path.Combine(Application.streamingAssetsPath, epubFileName);

        if (!File.Exists(fullPath))
        {
            Debug.LogError("EPUB file not found at: " + fullPath);
            return null;
        }

        // Read the EPUB file
        EpubBook book = EpubReader.ReadBook(fullPath);
        string combinedText = "";

        // Get the HTML files from the EPUB content
        var htmlFiles = book.Content.Html.Local; // ✅ use .Local to access list of HTML files
        Debug.Log($"Total HTML files found: {htmlFiles.Count}");

        // Loop through each HTML file and process the content
        foreach (EpubLocalTextContentFile htmlFile in htmlFiles)
        {
            // Debug log the raw HTML content (useful for inspecting it)
            Debug.Log("RAW HTML:\n" + htmlFile.Content);

            // Optionally disable stripping to see unfiltered content
            combinedText += htmlFile.Content + "\n\n"; // Append raw HTML content to combinedText

            // Uncomment the following section to enable HTML stripping
            // string plainText = StripHtml(htmlFile.Content);
            // if (!string.IsNullOrWhiteSpace(plainText) && plainText.Length > 50)
            // {
            //     combinedText += plainText.Trim() + "\n\n";  // Add cleaned text
            // }
        }

        // Debug log the length of the unfiltered content
        Debug.Log("Unfiltered length: " + combinedText.Length);

        // Optionally strip HTML and process text if necessary
        string cleanText = StripHtml(combinedText);

        // Log the final clean text length
        Debug.Log("Clean text length: " + cleanText.Length);

        return cleanText;  // Return the cleaned text
    }

    // Function to strip HTML tags from content
    private string StripHtml(string html)
    {
        // Regex pattern to remove HTML tags
        return Regex.Replace(html, "<.*?>", string.Empty)
                    .Replace("&nbsp;", " ")
                    .Replace("&amp;", "&")
                    .Replace("&quot;", "\"")
                    .Replace("&lt;", "<")
                    .Replace("&gt;", ">");
    }
}

