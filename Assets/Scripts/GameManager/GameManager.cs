using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;


public class GameManager : MonoBehaviour
{
  
   public int nbPlayers;
   public float audioVolume;
   public Transform ballSpawn;
   public Transform bluePlayerSpawn;
   public Transform orangePlayerSpawn;

   public GameObject ball;
   public GameObject bluePlayer;
   //public GameObject orangePlayer;

   public TMP_Text blueScoreText;
   public TMP_Text orangeScoreText;
   public TMP_Text goalScoreText;
   
   public AudioSource goalBuzzer;

   private int blueScore = 0;
   private int orangeScore = 0;
  
  
   void Awake()
   {
      
       var gms =  FindObjectsByType<GameManager>(FindObjectsSortMode.None);
      
       if(gms.Length > 1)
           DestroyImmediate(gameObject);
       else
           DontDestroyOnLoad(gameObject);
   }
   


  public void SetNumberOfPlayer(int num)
   {
       nbPlayers = num;
   }


   public void SetAudioVolume(float volume)
   {
       audioVolume = volume;
   }

    public void GoalScored(string team)
    {
        if (team == "Orange")
        {
            orangeScore++;
        }
        else if (team == "Blue")
        {
            blueScore++;
        }

        UpdateScoreUI();
        StartCoroutine(ShowGoalAnimation());
        
    }

    private void UpdateScoreUI()
    {
        blueScoreText.text = blueScore.ToString();
        orangeScoreText.text = orangeScore.ToString();
    }

    private IEnumerator ShowGoalAnimation()
    {
        // Joue le son du buzzer
        goalBuzzer.Play();

        // Active et reset le texte
        goalScoreText.gameObject.SetActive(true);
        goalScoreText.transform.localScale = Vector3.zero;

        float animationDuration = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            float scale = Mathf.Lerp(0f, 1.5f, elapsedTime / animationDuration);
            goalScoreText.transform.localScale = new Vector3(scale, scale, scale);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        yield return new WaitForSecondsRealtime(1f); // Pause 1 seconde

        goalScoreText.gameObject.SetActive(false);

        ResetPositions();
    }
    
    public void ResetPositions()
    {
        ball.transform.position = ballSpawn.position;
        ball.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        bluePlayer.transform.position = bluePlayerSpawn.position;
        bluePlayer.transform.rotation = Quaternion.identity;
        bluePlayer.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        bluePlayer.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        //orangePlayer.transform.position = orangePlayerSpawn.position;
       // orangePlayer.transform.rotation = Quaternion.identity;
        //orangePlayer.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        //orangePlayer.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }
    
    
    
}
