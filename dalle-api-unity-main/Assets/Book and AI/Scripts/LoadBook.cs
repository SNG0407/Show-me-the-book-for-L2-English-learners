using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class LoadBook : MonoBehaviour
{
    public TextMeshProUGUI displayText; // Reference to the TextMeshPro Text element.
    public Button wordButtonPrefab; // Reference to the Button prefab to create clickable buttons.
    public Transform wordButtonContainer; // The parent object for the word buttons.
    public TextMeshProUGUI meaningText; // Reference to the TextMeshPro Text element where you display the word meaning.

    private string fileName = "Alltext";
    private int lineCount = 1;
    private int[] lineSavedArr;
    public int CurrentPage = 1;
    private int LastPage = 1;
    public TextMeshProUGUI CurrentPageText; // Reference to the TextMeshPro Text element where you display the word meaning.
    public TextMeshProUGUI LastPageText; // Reference to the TextMeshPro Text element where you display the word meaning.

    private List<string> words; // List to store individual words.
    public float buttonSpacing = 5f; // Spacing between buttons.
    public float buttonWidthThreshold = 350f; // Width at which to start a new line.
    private void Awake()
    {
        //Prev_Word_Btn()
        //Prev_Word_Btn(words);
    }
    private void Start()
    {
        lineSavedArr = new int[100];
        //0 페이지는 표지로 0번째 줄
        lineSavedArr[0] = 0;
        //1 페이지는 1     ~ 1끝
        //2 페이지는 1끝+1 ~ 2끝
        //3 페이지는 2끝+1 ~ 3끝

        // Load your Excel data into the 'words' list (load from Excel or another source).
        LoadBookTxt();

        //18줄이 한 페이지

        //Prev_Word_Btn()
        //Prev_Word_Btn();

        //페이지 나누기
        MakePages();

        CurrentPage = 1;
        //첫 페이지 보여주기
        ShowCurrentPage(CurrentPage);
    }
    private void Update()
    {
          CurrentPageText.text = CurrentPage.ToString();
    }
    private void LoadBookTxt()
    {
        TextAsset textAsset = Resources.Load<TextAsset>(fileName);
        if (textAsset != null)
        {
            string textContent = textAsset.text;
            Debug.Log("Text from the file: " + textContent);

            textContent = textContent.Replace("\n", " ");

            // You can now use 'textContent' as the string containing the file's text.
            words = textContent.Split(' ').ToList();

        }
        else
        {
            Debug.LogError("Text file not found or could not be loaded.");
        }
    }
    public void Prev_Word_Btn()
    {
        float currentX = 0f; // Used to keep track of button positions.
        float currentY = 0f; // Used to keep track of button positions.
        int count = 0;
        // Create clickable word buttons. lineSavedPos
        foreach (string word in words)
        {
            
            // Create a new word button by instantiating the prefab.
            Button wordButton = Instantiate(wordButtonPrefab, wordButtonContainer);

            //// Set the button's position.
            //wordButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(currentX, 0f);

            // Display the word using TextMeshPro.
            TextMeshProUGUI buttonText = wordButton.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = word;

            // Calculate the size of the button based on the preferred width of the text.
            Vector2 buttonSize = new Vector2(buttonText.preferredWidth, 10f);

            // Check if adding this button would exceed the width threshold.

            //First line
            if (count == 0)
            {
                currentX = 0f + buttonSize.x / 2 + 15f;
                currentY -= 12f; //Next line


                count++;
            }
            else
            {
                currentX += buttonSize.x / 2;
                count++;
            }

            if (currentX + buttonSize.x > buttonWidthThreshold)
            {
                // Start a new line.
                currentX = 0f + buttonSize.x / 2 + 10;
                currentY -= 12f; //Next line
                lineCount++; //만약 18줄이 되면 다음 페이지로 넘기기 
            }

            //Debug.Log(word + ": " + buttonSize);
            //Debug.Log("currentX: " + currentX + "currentY: " + currentY);

            // Set the size of the button's RectTransform.
            wordButton.GetComponent<RectTransform>().sizeDelta = buttonSize;

            // Set the button's position.
            wordButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(currentX, currentY);

            // Update the position for the next button.
            //currentX += buttonSize.x + buttonSpacing;

            // Attach the 'ClickableWord' script to the button and provide the word and meaningText references.
            ClickableWord clickableWordScript = wordButton.gameObject.AddComponent<ClickableWord>();
            clickableWordScript.word = word;
            clickableWordScript.meaningText = meaningText;

            // Update the position for the next button.
            currentX += buttonSize.x / 2 + buttonSpacing;
        }
    }
    
    public void MakePages()
    {
        float currentX = 0f; // Used to keep track of button positions.
        float currentY = 0f; // Used to keep track of button positions.
        int count = 0; //첫 페이지의 첫 단어.words[0]임

        // Create clickable word buttons. lineSavedPos
        foreach (string word in words)
        {
            ////1~18줄 까지는 1페이지, 그 후 다시 1~18 2페이지
            if (lineCount < 19)
            {
                // Create a new word button by instantiating the prefab.
                Button wordButton = Instantiate(wordButtonPrefab, wordButtonContainer);

                //// Set the button's position.
                //wordButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(currentX, 0f);

                // Display the word using TextMeshPro.
                TextMeshProUGUI buttonText = wordButton.GetComponentInChildren<TextMeshProUGUI>();
                buttonText.text = word;

                // Calculate the size of the button based on the preferred width of the text.
                Vector2 buttonSize = new Vector2(buttonText.preferredWidth, 10f);

                // Check if adding this button would exceed the width threshold.

                //First line
                if (count == 0)
                {
                    currentX = 0f + buttonSize.x / 2 + 15f;
                    currentY -= 12f; //Next line
                    count++;
                }
                else
                {
                    currentX += buttonSize.x / 2;
                    count++;
                }

                if (currentX + buttonSize.x > buttonWidthThreshold)
                {
                    // Start a new line.
                    currentX = 0f + buttonSize.x / 2 + 10;
                    currentY -= 12f; //Next line
                    lineCount++; //만약 18줄이 되면 다음 페이지로 넘기기 
                }

                //Debug.Log(word + ": " + buttonSize);
                //Debug.Log("currentX: " + currentX + "currentY: " + currentY);

                // Set the size of the button's RectTransform.
                //wordButton.GetComponent<RectTransform>().sizeDelta = buttonSize;

                // Set the button's position.
                //wordButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(currentX, currentY);

                // Update the position for the next button.
                //currentX += buttonSize.x + buttonSpacing;

                // Attach the 'ClickableWord' script to the button and provide the word and meaningText references.
                //ClickableWord clickableWordScript = wordButton.gameObject.AddComponent<ClickableWord>();
                //clickableWordScript.word = word;
                //clickableWordScript.meaningText = meaningText;

                // Update the position for the next button.
                currentX += buttonSize.x / 2 + buttonSpacing;
            }
            else
            {   //라인이 19번째라면 다음 페이지

                //첫 페이지 정보 저장하기
                lineSavedArr[CurrentPage] = count; //몇번째 단어가 첫페이지의 끝 단어 인덱스인지.

                //이전 페이지+1 ~ 현재
                Debug.Log(CurrentPage + "페이지: " + (lineSavedArr[CurrentPage - 1] + 1) + "-" + lineSavedArr[CurrentPage]);

                //줄 카운트 초기화
                lineCount = 1;
                CurrentPage++;
                LastPage = CurrentPage;
                LastPageText.text = LastPage.ToString();
            }
        }
    }
    public void ShowCurrentPage(int CurrentPage)
    {
        float currentX = 0f; // Used to keep track of button positions.
        float currentY = 0f; // Used to keep track of button positions.
        int count = 0; //첫 페이지의 첫 단어.words[0]임

        for (int i = (lineSavedArr[CurrentPage - 1] + 1); i <= lineSavedArr[CurrentPage]; i++)
        {
            // Create a new word button by instantiating the prefab.
            Button wordButton = Instantiate(wordButtonPrefab, wordButtonContainer);

            //// Set the button's position.
            //wordButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(currentX, 0f);

            // Display the word using TextMeshPro.
            TextMeshProUGUI buttonText = wordButton.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = words[i];

            // Calculate the size of the button based on the preferred width of the text.
            Vector2 buttonSize = new Vector2(buttonText.preferredWidth, 10f);

            // Check if adding this button would exceed the width threshold.

            //First line
            if (count == 0)
            {
                currentX = 0f + buttonSize.x / 2 + 15f;
                currentY -= 12f; //Next line
                count++;
            }
            else
            {
                currentX += buttonSize.x / 2;
                count++;
            }

            if (currentX + buttonSize.x > buttonWidthThreshold)
            {
                // Start a new line.
                currentX = 0f + buttonSize.x / 2 + 10;
                currentY -= 12f; //Next line
                lineCount++; //만약 18줄이 되면 다음 페이지로 넘기기 
            }

            //Debug.Log(words[i] + ": " + buttonSize);
            //Debug.Log("currentX: " + currentX + "currentY: " + currentY);

            // Set the size of the button's RectTransform.
            wordButton.GetComponent<RectTransform>().sizeDelta = buttonSize;

            // Set the button's position.
            wordButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(currentX, currentY);

            // Update the position for the next button.
            //currentX += buttonSize.x + buttonSpacing;

            // Attach the 'ClickableWord' script to the button and provide the word and meaningText references.
            ClickableWord clickableWordScript = wordButton.gameObject.AddComponent<ClickableWord>();
            clickableWordScript.word = words[i];
            clickableWordScript.meaningText = meaningText;
            // Update the position for the next button.
            currentX += buttonSize.x / 2 + buttonSpacing;
        }
    }
    public void NextPage_Btn()
    {
        if (CurrentPage != LastPage)
        {
            CurrentPage++;
            DestroyAllChildren(wordButtonContainer);
            ShowCurrentPage(CurrentPage);
        }
        else
        {
            CurrentPage = 1;
            ShowCurrentPage(CurrentPage);
            DestroyAllChildren(wordButtonContainer);

            Debug.Log("It's the last page.");
        }
    }
    public void PrevPage_Btn()
    {
        if (CurrentPage > 1)
        {
            CurrentPage--;
            DestroyAllChildren(wordButtonContainer);

            ShowCurrentPage(CurrentPage);
        }
        else
        {
            CurrentPage = LastPage;
            DestroyAllChildren(wordButtonContainer);

            ShowCurrentPage(CurrentPage);

            Debug.Log("It's the first page.");
        }
    }
    public void DestroyAllChildren(Transform parentTransform)
    {
        // Get the Transform component of the parent GameObject.
        //Transform parentTransform = transform;

        // Loop through each child of the parent.
        foreach (Transform child in parentTransform)
        {
            // Destroy the child GameObject.
            Destroy(child.gameObject);
        }
    }

}
/*
    string longText =
        "CHAPTER ONE " +
        "THE BOY WHO LIVED Mr. and Mrs. Dursley, of number four, Privet Drive, were proud to say " +
        "that they were perfectly normal, thank you very much. They were the last " +
        "people you'd expect to be involved in anything strange or mysterious, " +
        "because they just didn't hold with such nonsense. " +
        "Mr.Dursley was the director of a firm called Grunnings, which made " +
        "drills.He was a big, beefy man with hardly any neck, although he did " +
        "have a very large mustache. Mrs.Dursley was thin and blonde and had " +
        "nearly twice the usual amount of neck, which came in very useful as she " +
        "spent so much of her time craning over garden fences, spying on the " +
        "neighbors. The Dursleys had a small son called Dudley and in their " +
        "opinion there was no finer boy anywhere. " +
        "The Dursleys had everything they wanted, but they also had a secret, and " +
        "their greatest fear was that somebody would discover it. They didn't " +
        "think they could bear it if anyone found out about the Potters.Mrs. " +
        "Potter was Mrs.Dursley's sister, but they hadn't met for several years " +
        "in fact, Mrs.Dursley pretended she didn't have a sister, because he r" +
        "sister and her good -for-nothing husband were as unDursleyish as it was " +
        "possible to be.The Dursleys shuddered to think what the neighbors would " +
        "say if the Potters arrived in the street. The Dursleys knew that the " +
        "Potters had a small son, too, but they had never even seen him. This boy " +
        "was another good reason for keeping the Potters away; they didn't want " +
        "Dudley mixing with a child like that. " +
        "When Mr. and Mrs. Dursley woke up on the dull, gray Tuesday our story " +
        "starts, there was nothing about the cloudy sky outside to suggest that " +
        "strange and mysterious things would soon be happening all over the " +
        "country. Mr.Dursley hummed as he picked out his most boring tie for ";
    */
