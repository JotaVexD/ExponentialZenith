using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;  
using System.Runtime.Serialization.Formatters.Binary;  

public class CharacterCreationScript : MonoBehaviour
{
    // Main panels
    public GameObject creationCharPanel;
    public GameObject selectingCharPanel;

    // Type Panel
    public GameObject TypePanel;

    //Text Objects
    // Class
    public Text classText;
    public Text raceText;
    public InputField charNameInput;
    public Text userTakenText;
    public Text invalidCharName;
    public Text noClassSelected;
    public Text noRaceSelected;

    // Class Panel
    // Warrior
    public GameObject warriorTypePanel;
    // Weapon-Based
    public GameObject weaponBasedTypePanel;
    // Healer
    public GameObject healerTypePanel;
    // Mage
    public GameObject mageTypePanel;

    // Race
    // Human
    public Sprite human;

    // New Player Info
    public GameObject player;
    private string name = null;
    private int classId = 0;
    private int raceId = 0;
    // BinaryFormatter playerSprite = new BinaryFormatter();

    // URL Post
    private string url = "http://192.168.0.57/mmo2d/createchar.php";

    private void OnEnable() {
        classText.enabled = false;
        raceText.enabled = false;
        userTakenText.enabled = false;
        invalidCharName.enabled = false;
        noClassSelected.enabled = false;
        noRaceSelected.enabled = false;
    }

    public void backToSelectChar(){
        creationCharPanel.SetActive(false);
        selectingCharPanel.SetActive(true);
        charNameInput.text = "";
    }

    public void changePanel(int typeValue){
        if(typeValue == 0){
            TypePanel.SetActive(false);
            warriorTypePanel.SetActive(true);
        }else if(typeValue == 1){
            TypePanel.SetActive(false);
            weaponBasedTypePanel.SetActive(true);
        }else if(typeValue == 2){
            TypePanel.SetActive(false);
            healerTypePanel.SetActive(true);
        }else if(typeValue == 3){
            TypePanel.SetActive(false);
            mageTypePanel.SetActive(true);
        }else if(typeValue == 4){
            mageTypePanel.SetActive(false);
            healerTypePanel.SetActive(false);
            weaponBasedTypePanel.SetActive(false);
            warriorTypePanel.SetActive(false);
            TypePanel.SetActive(true);
        }
    }

    public void changeClassText(string Text){
        classText.enabled = true;
        classText.GetComponent<Text>().text = Text;
    }

    public void changeRaceText(string Text){
        raceText.enabled = true;
        raceText.GetComponent<Text>().text = Text;
    }


    public void chooseClass(int ClassSelected){
        noClassSelected.enabled = false;
        classId = ClassSelected;
    }

    public void changeColor(string color){
        string[] newColor = color.Split(',');  
        player.GetComponent<SpriteRenderer>().color = new Color(float.Parse(newColor[0]),float.Parse(newColor[1]),float.Parse(newColor[2]));
    }

    public void changeFormat(string raceName){
        if(raceName == "Human"){
            player.GetComponent<SpriteRenderer>().sprite = human;
        }
    }

    public void chooseRace(int RaceSelected){
        noRaceSelected.enabled = false;
        raceId = RaceSelected;
    }

    public void submitChar(){
        if(charNameInput.text.Length <= 5){
            invalidCharName.enabled = true;
            return;
        }
        if(raceText.enabled == false)
        {
            noRaceSelected.enabled = true;
            return;
        }
        if(classText.enabled == false){
            noClassSelected.enabled = true;
            return;
        }
       
        name = charNameInput.text;

        StartCoroutine(ProcessRequest(name,classId,raceId));
        Debug.Log("ProcessRequest " + Time.time);

    }

    private IEnumerator ProcessRequest(string name,int classId,int raceId)
    {
        WWWForm form = new WWWForm();
        form.AddField("username",  UserConfig.Instance.userData.Id);
        form.AddField("name", name);
        form.AddField("class", classId);
        form.AddField("race", raceId);

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.Send();

            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                int requestCode;
                bool validRequest = int.TryParse(www.downloadHandler.text,out requestCode);

                if(validRequest == false){
                    requestCode = 6;
                    StopAllCoroutines();
                }

                // 0 == Char Name already exist
                // 1 == Char created
                // 2 == Failed creating a char

                switch(requestCode){
                    case 0:
                        userTakenText.enabled = true;
                        Debug.Log("Char Name already exist!");
                        break;
                    case 1:
                        Debug.Log("Char created!");
                        backToSelectChar();
                        break;
                    case 2:
                        Debug.Log("Failed creating a char");
                        break;
                }
            }
        }
    }
}
