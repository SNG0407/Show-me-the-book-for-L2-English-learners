using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;


public class TranslationManager : MonoBehaviour //https://dictionaryapi.dev/
{
    public static TranslationManager instance;
    private string glosbeEndpoint = "https://api.dictionaryapi.dev/api/v2/entries/en/";

    // Add a public field for the InputField
    public TMP_InputField inputField;
    public TextMeshProUGUI ResultText;

    //public string OriginWord;
    public TextMeshProUGUI wordText;
    public TextMeshProUGUI phoneticText;
    public TextMeshProUGUI audioText;
    public TextMeshProUGUI definitionText;
    public TextMeshProUGUI exampleText;
    public Button audioButton;

    //private string AudioURL;
    private AudioSource TTS_Source;

   // public TextMeshProUGUI meaningsText;

    private string apiEndpoint = "https://api.dictionaryapi.dev/api/v2/entries/en/";

    public string wordToSearch = "people"; // Replace with the word you want to search.

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        //DictionaryData dictionaryData = new DictionaryData("Default word", "Default phonetic");
        //string json_Data = ObjectToJson(dictionaryData); //Json 형식으로
        //Debug.Log(json_Data);

        //DictionaryData dictionaryData1 = JsonToObject<DictionaryData>(json_Data); //Json 형식 받아오는
        //dictionaryData1.getword();
        //dictionaryData1.getphonetic();

        TTS_Source = GetComponent<AudioSource>();
    }
    public void TTS_Btn()
    {
        StartCoroutine(TTS_Play(audioText.text));
    }
    IEnumerator TTS_Play(string TTS_URL)
    {
        WWW www = new WWW(TTS_URL);
        yield return www;

        TTS_Source.clip = www.GetAudioClip(false, true, AudioType.MPEG);
        TTS_Source.Play();

    }
    public void Translate()
    {

        // Get the text from the InputField
        wordToSearch = inputField.text;

        // Continue with the translation logic
        StartCoroutine(GetTranslation(wordToSearch));


    }
    public void Translate(string word)
    {
        //OriginWord = word;
        // Get the text from the InputField
        wordToSearch = word;
        //wordToSearch = inputField.text;

        // Continue with the translation logic
        StartCoroutine(GetTranslation(wordToSearch));


    }
    //string ObjectToJson(object obj)
    //{
    //    //ToJson의 2번째 매개변수(prettyprint)는 생성될 Json의 형식을 이쁘게 보이게 해주기 위함
    //    return JsonUtility.ToJson(obj, true);
    //}

    //T JsonToObject<T>(string JsonData) where T : class
    //{
    //    return JsonUtility.FromJson<T>(JsonData);
    //}

    IEnumerator GetTranslation(string word)
    {
        string url = glosbeEndpoint + word;

        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError("Error: " + www.error);
                wordText.text = word;
                phoneticText.text = "No Definitions Found";
                audioText.text = "No Definitions Found";
                definitionText.text = "No Definitions Found";
                exampleText.text = "No Definitions Found";
            }
            else
            {
                // Parse and process the JSON response here
                string jsonResult = www.downloadHandler.text;
                Debug.Log("Translation response: " + jsonResult);

                DictionaryData data = NewParseDictionaryData(jsonResult);


                if (data != null)
                {
                    wordText.text = data.word;
                    phoneticText.text = data.phonetic;
                    audioText.text = data.audio;
                    definitionText.text = data.definition;
                    exampleText.text = data.example;
                }
                else
                {
                    Debug.LogError("Failed to parse JSON data.");
                }

            }
        }
    }
    //partOfSpeech
    string Parse_partOfSpeech(string jsonString, string Tag)
    {
        Tag = "partOfSpeech";
        string text = "None";
        string newTag = "\"" + Tag + "\":";
        //Debug.Log("New tag : " + newTag);

        int textStartIndex = jsonString.IndexOf(newTag);
        if (textStartIndex != -1)
        {

            //textStartIndex = textStartIndex;
            //Debug.Log(Tag + " : " + jsonString.Substring(textStartIndex, 40));
            //Debug.Log(Tag + " StartIndex: " + textStartIndex);
            int textEndIndex = jsonString.IndexOf(",", textStartIndex);
            //textEndIndex = textEndIndex+1;
            //Debug.Log(Tag + " EndIndex: " + textEndIndex);
            text = jsonString.Substring(textStartIndex, textEndIndex - textStartIndex);
        }
        Debug.Log("Tag : " + text);

        return text;
    }
    string Parse_word(string jsonString, string Tag)
    {
        Tag = "word";
        string text = "None";
        string newTag = "\"" + Tag + "\":";
        //Debug.Log("New tag : " + newTag);

        int textStartIndex = jsonString.IndexOf(newTag);
        if (textStartIndex != -1)
        {
            //textStartIndex = textStartIndex;
            //Debug.Log(Tag + " : " + jsonString.Substring(textStartIndex, 40));
            //Debug.Log(Tag + " StartIndex: " + textStartIndex);
            int textEndIndex = jsonString.IndexOf(",", textStartIndex);
            //textEndIndex = textEndIndex+1;
            //Debug.Log(Tag + " EndIndex: " + textEndIndex);
            text = jsonString.Substring(textStartIndex, textEndIndex - textStartIndex);
        }
           
        Debug.Log("Tag : " + text);

        return text;
    }
    string Parse_phonetic(string jsonString, string Tag)
    {
        Tag = "text";
        string text = "None";
        string newTag = "\"" + Tag + "\":";
        //Debug.Log("New tag : " + newTag);

        int textStartIndex = jsonString.IndexOf(newTag);
        if (textStartIndex != -1)
        {
            //textStartIndex = textStartIndex;
            //Debug.Log(Tag + " : " + jsonString.Substring(textStartIndex, 40));
            //Debug.Log(Tag + " StartIndex: " + textStartIndex);
            int textEndIndex = jsonString.IndexOf(",", textStartIndex);
            //textEndIndex = textEndIndex+1;
            //Debug.Log(Tag + " EndIndex: " + textEndIndex);
            text = jsonString.Substring(textStartIndex, textEndIndex - textStartIndex);
        }
          
        Debug.Log("Tag : " + text);

        return text;
    }
    string Parse_example(string jsonString, string Tag)
    {
        Tag = "example";
        string text = "None";
        string newTag = "\"" + Tag + "\":";
        //Debug.Log("New tag : " + newTag);

        int textStartIndex = jsonString.IndexOf(newTag);
        //textStartIndex = textStartIndex;
        //Debug.Log(Tag + " : " + jsonString.Substring(textStartIndex, 40));
        //Debug.Log(Tag + " StartIndex: " + textStartIndex);
        if(textStartIndex != -1)
        {
            int textEndIndex = jsonString.IndexOf("}", textStartIndex);
            //textEndIndex = textEndIndex+1;
            //Debug.Log(Tag + " EndIndex: " + textEndIndex);
            text = jsonString.Substring(textStartIndex, textEndIndex - textStartIndex);
        }
        Debug.Log("Tag : " + text);

        return text;
    }
    //Parse_audio
    private string Parse_audio(string jsonString, string Tag)
    {
        Tag = "audio";
        string text = "None";
        string newTag = "\"" + Tag + "\":\"h";
        //Debug.Log("New tag : " + newTag);

        int textStartIndex = jsonString.IndexOf(newTag);
        //textStartIndex = textStartIndex;
        //Debug.Log(Tag + " : " + jsonString.Substring(textStartIndex, 40));
        //Debug.Log(Tag + " StartIndex: " + textStartIndex);

        if (textStartIndex != -1)
        {
            int textEndIndex = jsonString.IndexOf(",", textStartIndex);
            //textEndIndex = textEndIndex+1;
            //Debug.Log(Tag+" EndIndex: " + textEndIndex);
            text = jsonString.Substring(textStartIndex, textEndIndex - textStartIndex);
        }
        Debug.Log("Tag : " + text);

        //substrack audio clip url //"audio":"
        newTag = "\"audio\":";
        textStartIndex = text.IndexOf(newTag) + newTag.Length;
        if (textStartIndex != -1)
        {
            int textEndIndex = text.LastIndexOf('"');
            //textEndIndex = textEndIndex+1;
            //Debug.Log(Tag+" EndIndex: " + textEndIndex);
            text = text.Substring(textStartIndex+1, textEndIndex - textStartIndex-1);
        }
        if(text.Length > 1)
        {
            Debug.Log("Tag : " + text);
        }
        else
        {
            Debug.Log("Tag : None");
            text = "None";
        }
        return text;
    }
    private string Parse_definition(string jsonString, string Tag)
    {
        Tag = "definition";
        string text = "None";
        string newTag = "\""+ Tag+"\":";
        //Debug.Log("New tag : " + newTag);

        int textStartIndex = jsonString.IndexOf(newTag);
        //textStartIndex = textStartIndex;
        //Debug.Log(Tag + " : " + jsonString.Substring(textStartIndex, 40));
        //Debug.Log(Tag + " StartIndex: " + textStartIndex);
        
        if (textStartIndex != -1)
        {
            int textEndIndex = jsonString.IndexOf("\"" + "synonyms" + "\":", textStartIndex);
            //textEndIndex = textEndIndex+1;
            //Debug.Log(Tag+" EndIndex: " + textEndIndex);
            text = jsonString.Substring(textStartIndex, textEndIndex - textStartIndex);
        }
        Debug.Log("Tag : " + text);

        return text;
    }
    private string ParseTag(string jsonString, string Tag)
    {

        string text = "None";
        if (Tag == "definition")
        {
            int textStartIndex = jsonString.IndexOf(Tag);
            textStartIndex = Tag.Length + textStartIndex + 3;
            //Debug.Log(Tag + " StartIndex: " + textStartIndex);
            int textEndIndex = jsonString.IndexOf(".", textStartIndex);
            //textEndIndex = textEndIndex;
            //Debug.Log(Tag+" EndIndex: " + textEndIndex);
            text = jsonString.Substring(textStartIndex, textEndIndex - textStartIndex);
            Debug.Log("Tag : " + text);

            text = text + ".";
            textStartIndex = text.IndexOf(Tag);
            textStartIndex = Tag.Length + textStartIndex + 3;
            //Debug.Log(Tag + " StartIndex: " + textStartIndex);
            textEndIndex = text.IndexOf(".");
            //Debug.Log(Tag+" EndIndex: " + textEndIndex);
            text = text.Substring(textStartIndex, textEndIndex - textStartIndex);
            Debug.Log("Tag2 : " + text);
        }
        else if(Tag == "example")
        {
            int textStartIndex = jsonString.IndexOf(Tag);
            textStartIndex = Tag.Length + textStartIndex + 3;
            //Debug.Log(Tag + " StartIndex: " + textStartIndex);
            int textEndIndex = jsonString.IndexOf(".", textStartIndex);
            //textEndIndex = textEndIndex - 1;
            //Debug.Log(Tag+" EndIndex: " + textEndIndex);
            text = jsonString.Substring(textStartIndex, textEndIndex - textStartIndex);
            Debug.Log("Tag : " + text);
        }
        else
        {
            int textStartIndex = jsonString.IndexOf(Tag);
            textStartIndex = Tag.Length + textStartIndex + 3;
            //Debug.Log(Tag + " StartIndex: " + textStartIndex);
            int textEndIndex = jsonString.IndexOf(",", textStartIndex);
            textEndIndex = textEndIndex - 1;
            //Debug.Log(Tag+" EndIndex: " + textEndIndex);
            text = jsonString.Substring(textStartIndex, textEndIndex - textStartIndex);
            Debug.Log("Tag : " + text);
        }
        return text;
    }

    private DictionaryData NewParseDictionaryData(string jsonString)
    {
        // Find the "phonetics" array.
        DictionaryData data = new DictionaryData(null, null);
        data.phonetic = Parse_phonetic(jsonString, "text");
        data.partOfSpeech = Parse_partOfSpeech(jsonString, "partOfSpeech");
        data.definition = Parse_definition(jsonString, "definition");
        data.example = Parse_example(jsonString, "example");
        data.word = Parse_word(jsonString, "word");
        data.audio = Parse_audio(jsonString, "audio");


        //return data.audio != null && data.phonetic != null && data.definition != null && data.example != null ? data : null;
        return data;
    }

    [System.Serializable]
    public class Phonetic
    {
        public string text;
        public string audio;
        public string sourceUrl;
        public License license;
    }

    [System.Serializable]
    public class License
    {
        public string name;
        public string url;
    }

    [System.Serializable]
    public class Definition
    {
        public string definition;
        public string example;
    }
    [System.Serializable]
    public class Meaning
    {
        public string partOfSpeech;
        public Definition[] definitions;
        public string[] synonyms;
        public string[] antonyms;
    }

    [System.Serializable]
    public class DictionaryData
    {
        public string word;
        public string phonetic;
        public string audio;
        public string partOfSpeech;
        public string definition;
        public string example;
        
        public Phonetic[] phonetics;
        public Meaning[] meanings;

        public DictionaryData(string _word, string _phonetic)
        {
            word = _word;
            phonetic = _phonetic;
            // Initialize the phonetics and meanings arrays with a specific size.
            phonetics = new Phonetic[30];
            meanings = new Meaning[30];
        }
        public string getword()
        {
            Debug.Log("word: " + word);
            return word;
        }

        public string getphonetic()
        {
            Debug.Log("phonetic: " + phonetic);
            return phonetic;
        }
    }

}
