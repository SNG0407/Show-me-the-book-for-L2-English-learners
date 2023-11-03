using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Text.RegularExpressions;
public class ClickableWord : MonoBehaviour
{
    public string word;
    //public Text meaningText; // Reference to the UI element where you display the word meaning.
    public TextMeshProUGUI meaningText; // Reference to the TextMeshPro Text element.
    private TranslationManager translator; // Reference to the Translator script
    private void Awake()
    {
        // Try to find the Translator script in the scene
        translator = FindObjectOfType<TranslationManager>();
        
    }
    private void Start()
    {
        // Attach a click event to the Button component.
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        // Display the meaning of the word.
        meaningText.text = GetMeaning(word);
        Debug.Log(meaningText.text);
        if (translator != null)
        {
            translator.Translate(meaningText.text);
        }
    }

    private string GetMeaning(string word)
    {
        // Implement your logic to retrieve the meaning of the word here.
        // You can fetch it from a database, dictionary, or any other source.
        // For this example, we return a placeholder meaning.
        //return "This is the meaning of " + word;

        //if the word has [, or . ' "] delete them. 
        //string pattern = @"\b\w+\b"; //it deletes apostrophe as well
        //string pattern = @"\b\w+(?:'\w+)?\b"; //it doesn't delete apostrophe but hyphens
        string pattern = @"\b[\w'-]+\b";
        Regex regex = new Regex(pattern);
        MatchCollection matches = regex.Matches(word);
        string largestWord = "";
        foreach (Match match in matches)
        {
            if(largestWord.Length < match.Value.Length)
            {
                word = match.Value;
            }
            //Debug.Log(word);
        }
        //word = word.Trim(new char[] { '.', ',', '!', '?','\'','"' }); // Add more punctuation marks as needed


        return word;
    }
}
