using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class BookDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bookText;
    [SerializeField] private int charactersPerPage = 800; // Number of characters per page (adjust as needed)
    
    private List<string> pages = new List<string>();
    private int currentPage = 0;

    private void Start()
    {
        StartCoroutine(ShowLoadingThenPaginate());
    }

    private IEnumerator ShowLoadingThenPaginate()
    {
        // Show loading message
        bookText.text = "Loading EPUB...";
        Canvas.ForceUpdateCanvases();
        bookText.ForceMeshUpdate();

        // Wait for the end of the frame to ensure it's rendered
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(0.1f); // Optional buffer for stability

        // Load and paginate the text
        string fullBookText = FindObjectOfType<EpubLoader>().LoadCleanedBookText();
        yield return StartCoroutine(PaginateBook(fullBookText));
    }

    private IEnumerator PaginateBook(string fullText)
    {
        pages.Clear(); // Clear previous pages
        bookText.text = ""; // Start with empty text

        int totalChars = fullText.Length;
        int pageStart = 0;

        // Start adding characters per page
        while (pageStart < totalChars)
        {
            int pageEnd = Mathf.Min(pageStart + charactersPerPage, totalChars); // Ensure we don't go out of bounds
            string pageText = fullText.Substring(pageStart, pageEnd - pageStart); // Extract page content

            pages.Add(pageText); // Add the page to the list
            pageStart = pageEnd; // Move to the next set of characters for the next page

            yield return null; // Wait for next frame to avoid performance hit
        }

        // Once all pages are created, show the first page
        ShowPage(currentPage);
    }

    public void ShowPage(int index)
    {
        // Display text of the selected page
        if (index >= 0 && index < pages.Count)
        {
            bookText.text = pages[index];
        }
        else
        {
            Debug.LogWarning("Invalid page index.");
        }
    }

    public void NextPage()
    {
        if (currentPage < pages.Count - 1)
        {
            currentPage++;
            ShowPage(currentPage);
        }
    }

    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            ShowPage(currentPage);
        }
    }
}
