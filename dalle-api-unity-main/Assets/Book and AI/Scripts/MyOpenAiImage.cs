using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using REST_API_HANDLER;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System;
using System.IO;
using UnityEngine.UI;

public class MyOpenAiImage : MonoBehaviour
{
	public GameObject loadingpanel;
	public TMP_InputField inputText;
	public TMP_InputField LoadFileText;
	public TMP_Text resultText;
	public List<GameObject> previewObjs;

	public RawImage rawImage; // Reference to your RawImage component
	private string localImagePath = "Assets/Resources/AI_image"; // Local path to save the image

	private string IMAGE_GENERTION_API_URL = "https://api.openai.com/v1/images/generations";

	
	void Start()
	{

	}
	public void ClickedWordGenerate_Btn()
	{
		resultText.text = "";
		resultText.enabled = false;
		loadingpanel.SetActive(true);


		string description = TranslationManager.instance.wordToSearch;
		string resolution = "256x256"; // Possible Resolution 256x256, 512x512, or 1024x1024.

		GenerateImage(description, resolution, () => {
			loadingpanel.SetActive(false);
		});

	}
	public void SearchButtonClicked()
    {
		resultText.text = "";
		resultText.enabled = false;
		loadingpanel.SetActive(true);

		for (int i = 0; i < previewObjs.Count; i++)
		{
			if (previewObjs[i].GetComponent<Renderer>() != null)
				previewObjs[i].GetComponent<Renderer>().material.mainTexture = null;
			else
				Debug.Log("The preview Object has no Renderer");
		}

		string description = inputText.text;
		string resolution = "256x256"; // Possible Resolution 256x256, 512x512, or 1024x1024.

		GenerateImage(description, resolution, () => {
			loadingpanel.SetActive(false);
		});
		
	}

	public void getImageFromDisc(string fileName)
    {
		string description = inputText.text;
		// Load the saved image as a Texture2D
		Texture2D loadedTexture = Resources.Load<Texture2D>("AI_image/"+ fileName);

		if (loadedTexture != null)
		{
			// Create a new sprite from the loaded texture
			Sprite imageSprite = Sprite.Create(loadedTexture, new Rect(0, 0, loadedTexture.width, loadedTexture.height), Vector2.zero);

			// Assign the sprite to the RawImage component
			rawImage.texture = imageSprite.texture;
		}
		else
		{
			Debug.LogError("Failed to load the image from Resources.");
		}
	}
	public void GenerateImage(string description, string resolution, Action completationAction)
	{

		GenerateImageRequestModel reqModel = new GenerateImageRequestModel(description, 1 ,resolution);
		ApiCall.instance.PostRequest<GenerateImageResponseModel>(IMAGE_GENERTION_API_URL, reqModel.ToCustomHeader(), null, reqModel.ToBody(), (result =>
		{
			loadTexture(description, result.data, completationAction);
			resultText.enabled = true;

			

		}), (error =>
		{
			ErrorResponseModel entity = JsonUtility.FromJson<ErrorResponseModel>(error);
			completationAction.Invoke();
			resultText.enabled = true;
			resultText.text = entity.error.message;
		}));

	}




	async void loadTexture(string description, List<UrlClass> urls, Action completationAction)
    {
		for (int i = 0; i < urls.Count; i++)
        {
			Texture2D _texture = await GetRemoteTexture(urls[i].url);
			if (previewObjs[i].GetComponent<Renderer>() != null)
            {
				previewObjs[i].GetComponent<Renderer>().material.mainTexture = _texture;
            }
			//Utility.WriteImageOnDisk(_texture, System.DateTime.Now.Millisecond + "_createImg_" + i + "_.jpg"); inputText.text
			Utility.WriteImageOnDisk(_texture, description + ".jpg");

			// Create a new sprite from the loaded texture
			Sprite imageSprite = Sprite.Create(_texture, new Rect(0, 0, _texture.width, _texture.height), Vector2.zero);

			// Assign the sprite to the RawImage component
			rawImage.texture = imageSprite.texture;
			//StartCoroutine(WriteImageAndLoad(description, _texture, TranslationManager.instance.wordToSearch));
			string filePath = Path.Combine(Application.persistentDataPath, "Uploaded_AI_Files", description + ".jpg");
			if (File.Exists(filePath))
			{
				string fileContents = File.ReadAllText(filePath);
				// Now use the fileContents as needed, e.g., display it in a UI Text element.
				Debug.Log("File found: " + fileContents);

			}
			else
			{
				Debug.LogError("File not found: " + filePath);
				// Handle the situation where the file doesn't exist.
			}

		}

		completationAction.Invoke();
		// Call the coroutine
	}
	private IEnumerator WriteImageAndLoad(string description, Texture2D _texture, string filename)
	{
		// Write the image to disk
		Utility.WriteImageOnDisk(_texture, description + ".jpg");

		// Yield a frame to make sure the image is written to disk
		yield return null;
		Texture2D loadedTexture = Resources.Load<Texture2D>("AI_image/" + TranslationManager.instance.wordToSearch);

		if (loadedTexture != null)
		{
			// Create a new sprite from the loaded texture
			Sprite imageSprite = Sprite.Create(loadedTexture, new Rect(0, 0, loadedTexture.width, loadedTexture.height), Vector2.zero);

			// Assign the sprite to the RawImage component
			rawImage.texture = imageSprite.texture;
		}
		else
		{
			Debug.LogError("Failed to load the image from Resources.");
		}
	}



	public static async Task<Texture2D> GetRemoteTexture(string url)
	{
		using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
		{
			var asyncOp = www.SendWebRequest();

			while (asyncOp.isDone == false)
				await Task.Delay(1000 / 30);//30 hertz

			// read results:
			if (www.isNetworkError || www.isHttpError)
			{
				return null;
			}
			else
			{
				return DownloadHandlerTexture.GetContent(www);
			}
		}
	}

	private void WriteImageOnDisk(Texture2D texture, string fileName)
	{
		byte[] textureBytes = texture.EncodeToPNG();
		string path = Application.persistentDataPath + fileName;
		File.WriteAllBytes(path, textureBytes);
		Debug.Log("File Written On Disk! "  + path );
	}
}
