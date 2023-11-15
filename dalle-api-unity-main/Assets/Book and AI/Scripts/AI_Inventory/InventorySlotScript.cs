using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlotScript : MonoBehaviour
{
    public TextMeshProUGUI fileNameText; // Reference to the TextMeshPro Text element.

    public Button createFrameButton;
    public RawImage fileImage;

    private string fileName;

    private InventoryScript inventoryScript;
    // Start is called before the first frame update
    public void SetItem(string fileName, Texture2D AIfileImage, InventoryScript inventoryScript)
    {
        this.fileName = fileName;
        this.inventoryScript = inventoryScript;

        //fileImage = AIfileImage;
        fileImage.texture = AIfileImage;

        fileNameText.text = fileName;
        createFrameButton.onClick.AddListener(CreateFrame);
    }
    private void CreateFrame()
    {
        // Call the function in the inventory script to create a frame using the file name
        inventoryScript.CreateFrame(fileName);
    }
}
