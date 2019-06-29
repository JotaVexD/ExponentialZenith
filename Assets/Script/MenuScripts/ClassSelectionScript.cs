using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;
using UnityEngine.Networking;
using Kryz.CharacterStats;
using UnityEngine.SceneManagement;

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Chars;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Chars = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Chars = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Chars;
    }
}

[Serializable]
public class CharacterData
{
    public int id;
    public int username;
    public string name;
    public string race;
    public string classe;
    public int level;
    public float experience;
    public string guild;
    public string inventory;
    public int AGI;
    public int STR;
    public int INT;
    public int VIT;
    public int LUK;
    public int DEX;
}


public class ClassSelectionScript : MonoBehaviour
{
    public GameObject[] statDis;

    public CharacterData[] characterData;
    private CharacterData activeCharacterData;
    public GameObject[] buttonsChars;

    // Panels
    private CharacterStat[] stats;
    public Text PlayerName;
    public Text Level;
    public Text Race;
    public Text ClassName;
    public GameObject creationCharPanel;
    public GameObject selectingCharPanel;

    // Player Info
    public GameObject player;
    // Race
    // Human
    public Sprite human;
    //....

    private int startValue = 0;

    void OnEnable()
    {
        int id = UserConfig.Instance.userData.Id;
        StartCoroutine(ProcessRequest(id));
        Debug.Log("ProcessRequest " + Time.time);
    }

    private IEnumerator ProcessRequest(int id)
    {
        WWWForm form = new WWWForm();
        form.AddField("id", id);
        string url = "http://192.168.0.57/mmo2d/getchars.php";

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.Send();

            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                // Print Body
                // JsonUtility.FromJson<CharacterData>();
                characterData = JsonHelper.FromJson<CharacterData>(www.downloadHandler.text);
                if(characterData.Length == 0){
                    CustomButton_onClick();
                    yield return false;
                }
                
                updateStatsPanel(startValue,characterData);

                for(int x = 0; x < buttonsChars.Length;x++){

                    if(characterData.Length > x){
                        buttonsChars[x].GetComponentInChildren<Text>().text = characterData[x].name;
                        buttonsChars[x].GetComponent<Button>().onClick.RemoveListener(CustomButton_onClick);
                    }else{
                        buttonsChars[x].GetComponentInChildren<Text>().text = "Empty";
                        buttonsChars[x].GetComponent<Button>().onClick.AddListener(CustomButton_onClick);
                    }
                }
            }
        }
    }

    public void changeCharSelected(int value){
        
        updateStatsPanel(value,characterData);
    }

    private void updateStatsPanel(int value, CharacterData[] characterData){
        if(characterData.Length <= value){
        }
        else{
            activeCharacterData = characterData[value];
        }

        PlayerName.GetComponent<Text>().text = activeCharacterData.name;
        Level.GetComponent<Text>().text = "Level "+ activeCharacterData.level.ToString();
        Race.GetComponent<Text>().text = activeCharacterData.race;
        ClassName.GetComponent<Text>().text = activeCharacterData.classe;
        
        player.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Player/Races/"+activeCharacterData.race);

        // Change Color by class 
        if(activeCharacterData.classe == "Guardian"){
            player.GetComponent<SpriteRenderer>().color = new Color(0f,0.7371f,0.5607f);
        }else if(activeCharacterData.classe == "Samurai"){
            player.GetComponent<SpriteRenderer>().color = new Color(0.8901f,0f,0f);
        }else if(activeCharacterData.classe == "Monk"){
            player.GetComponent<SpriteRenderer>().color = new Color(0.9372f,0.8274f,0f);            
        }
        
        for(int i = 0; i < statDis.Length; i++){
            if(statDis[i].name == "STR"){
                statDis[i].GetComponent<Text>().text = activeCharacterData.STR.ToString();
            }else if(statDis[i].name == "AGI"){
                statDis[i].GetComponent<Text>().text = activeCharacterData.AGI.ToString();
            }else if(statDis[i].name == "INT"){
                statDis[i].GetComponent<Text>().text = activeCharacterData.INT.ToString();
            }else if(statDis[i].name == "VIT"){
                statDis[i].GetComponent<Text>().text = activeCharacterData.VIT.ToString();
            }else if(statDis[i].name == "LUK"){
                statDis[i].GetComponent<Text>().text = activeCharacterData.LUK.ToString();
            }else if(statDis[i].name == "DEX"){
                statDis[i].GetComponent<Text>().text = activeCharacterData.DEX.ToString();
            }
            
        }
    }

    void CustomButton_onClick()
    {
        creationCharPanel.SetActive(true);
        selectingCharPanel.SetActive(false);
    }

    public void joinGame(){
        CharConfig.Instance.charData.username = activeCharacterData.username;
        CharConfig.Instance.charData.nameChar = activeCharacterData.name;
        CharConfig.Instance.charData.race = activeCharacterData.race;
        CharConfig.Instance.charData.classe = activeCharacterData.classe;
        CharConfig.Instance.charData.level = activeCharacterData.level;
        CharConfig.Instance.charData.experience = activeCharacterData.experience;
        CharConfig.Instance.charData.guild = activeCharacterData.guild;
        CharConfig.Instance.charData.inventory = activeCharacterData.inventory;
        CharConfig.Instance.charData.AGI = activeCharacterData.AGI;
        CharConfig.Instance.charData.STR = activeCharacterData.STR;
        CharConfig.Instance.charData.INT = activeCharacterData.INT;
        CharConfig.Instance.charData.VIT = activeCharacterData.VIT;
        CharConfig.Instance.charData.LUK = activeCharacterData.LUK;
        CharConfig.Instance.charData.DEX = activeCharacterData.DEX;

        SceneManager.LoadScene("Main");
    }
}
