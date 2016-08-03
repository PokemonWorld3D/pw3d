using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Main_Menu : MonoBehaviour
{
	public Network_Manager netManager;
	public Text messageText, usernameText, passwordText, regNameText, regUsernameText, regPassText, regConfPassText, regEmailText, charNameText;
	public Text[] CharacterButtonTexts;
	public GameObject logInPanel, characterPanel, creationPanel;
	public GameObject[] CreateButtons, CharacterButtons;
	
	private string username, password, regName, regUsername, regPass, regConfPass, regEmail, charName;
	
	public void LogIn()
	{
		messageText.text = "";
		
		username = usernameText.text;
		password = passwordText.text;
		
		if (username == "" || password == "")
		{
			messageText.text = "Please complete all fields.";
		}
		else
		{
			WWWForm form = new WWWForm();
			form.AddField("username", username);
			form.AddField("password", password);
			WWW w = new WWW("http://pokemonworld3d.dx.am/login.php", form);
			StartCoroutine(LogIn(w));
		}
	}
	public void Register()
	{
		messageText.text = "";
		
		regName = regNameText.text;
		regUsername = regUsernameText.text;
		regPass = regPassText.text;
		regConfPass = regConfPassText.text;
		regEmail = regEmailText.text;
		
		if (regName == "" || regUsername == "" || regPass == "" || regConfPass == "" || regEmail == "")
		{
			messageText.text = "Please complete all fields.";
		}
		else
		{
			if(regPass == regConfPass)
			{
				WWWForm form = new WWWForm();
				form.AddField("name", regName);
				form.AddField("username", regUsername);
				form.AddField("password", regPass);
				form.AddField("email", regEmail);
				WWW w = new WWW("http://pokemonworld3d.dx.am/register.php", form);
				StartCoroutine(Register(w));
			}
			else
			{
				messageText.text = "Your passwords do not match.";
			}
		}
	}
	public void CreateCharacter()
	{
		messageText.text = "";
		
		username = netManager.username;
		charName = charNameText.text;
		
		if (charName == "")
		{
			messageText.text = "Please complete all fields.";
		}
		else
		{
			WWWForm form = new WWWForm();
			form.AddField("username", username);
			form.AddField("charactername", charName);
			WWW w = new WWW("http://pokemonworld3d.dx.am/character_creation.php", form);
			StartCoroutine(CreateCharacter(w, username));
		}
	}
	public void SelectCharacter(Text text)
	{
		charName = text.text;
		
		WWW w = new WWW("http://pokemonworld3d.dx.am/select_character.php?charactername=" + charName);
		StartCoroutine(SelectCharacter(w, charName));
	}
	
	private IEnumerator LogIn(WWW _w)
	{
		yield return _w;
		if (_w.error == null)
		{
			if (_w.text == "Log in successful!")
			{
				netManager.username = username;
				logInPanel.SetActive(false);
				characterPanel.SetActive(true);
				
				WWW ww = new WWW("http://pokemonworld3d.dx.am/character_selection.php?username=" + username);
				StartCoroutine(CharacterSelection(ww));
			}
			else
				messageText.text = _w.text;
		}
		else
		{
			messageText.text = "ERROR: " + _w.error;
		}
	}
	private IEnumerator Register(WWW _w)
	{
		yield return _w;
		if (_w.error == null)
		{
			messageText.text = _w.text;
		}
		else
		{
			messageText.text = "ERROR: " + _w.error;
		}
	}
	private IEnumerator CharacterSelection(WWW _w)
	{
		yield return _w;
		
		foreach(GameObject go in CreateButtons)
			go.SetActive(true);
		foreach(GameObject go in CharacterButtons)
			go.SetActive(false);
		
		string[] CharacterNames = _w.text.Split(',');
		
		for(int c = 0; c < CharacterNames.Length - 1; c++)
		{
			CreateButtons[c].SetActive(false);
			CharacterButtons[c].SetActive(true);
			CharacterButtonTexts[c].text = CharacterNames[c];
		}
	}
	private IEnumerator CreateCharacter(WWW _w, string _username)
	{
		yield return _w;
		messageText.text = _w.text;
		WWW ww = new WWW("http://pokemonworld3d.dx.am/character_selection.php?username=" + _username);
		creationPanel.SetActive(false);
		characterPanel.SetActive(true);
		StartCoroutine(CharacterSelection(ww));
	}
	private IEnumerator SelectCharacter(WWW w, string characterName)
	{
		yield return w;
		if(w.text == "That character doesn't exist!")
		{
			
		}
		else
		{
//			string[] results = w.text.Split(',');

			netManager.characterName = characterName;

			if(netManager.thisIsTheServer)
				netManager.InitServer();
			else
				StartCoroutine(netManager.JoinServer());
		}
	}
}
