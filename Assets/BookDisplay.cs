using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class BookDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bookText;
    [SerializeField] private int maxLinesPerPage = 15; // Set this based on your UI size

    private List<string> pages = new List<string>();
    private int currentPage = 0;

    void Start()
    {
        // Load text from EpubLoader (or use placeholder for now)
        string fullBookText = FindObjectOfType<EpubLoader>().LoadCleanedBookText();

        // Start coroutine to paginate text after layout
        StartCoroutine(PaginateBook(fullBookText));
    }

    private IEnumerator PaginateBook(string fullText)
    {
        string[] words = fullText.Split(' ');
        string currentText = "";

        bookText.text = "";
        yield return null; // Wait a frame so TMP layout updates

        for (int i = 0; i < words.Length; i++)
        {
            string testText = currentText + words[i] + " ";
            bookText.text = testText;

            yield return null; // Wait a frame for TMP to update layout

            if (bookText.textInfo.lineCount > maxLinesPerPage)
            {
                pages.Add(currentText.TrimEnd());
                currentText = words[i] + " ";
            }
            else
            {
                currentText = testText;
            }
        }

        if (!string.IsNullOrWhiteSpace(currentText))
            pages.Add(currentText.TrimEnd());

        ShowPage(currentPage);
    }

    public void ShowPage(int index)
    {
        if (index >= 0 && index < pages.Count)
        {
            bookText.text = pages[index];
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



