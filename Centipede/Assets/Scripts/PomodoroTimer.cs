using UnityEngine;
using TMPro;
using UnityEngine.UI;



public class PomodoroTimer : MonoBehaviour
{
    private Scene2Handler scene2Handler;
    private DeleteButtonHandler deletebuttonHandler;
    private ButtonListHandler buttonListHandler;

    public TextMeshProUGUI pomodoroTimerText; // Link to the Pomodoro session text
    public TextMeshProUGUI breakTimerText; // Link to the break session text
    public TextMeshProUGUI pauseButtonText; // Link to the pause button text
    public TextMeshProUGUI sessionStatusText; // Link to the session status text
    public Button stopButton; // Reference to the Stop button
    public Button startButton; // Reference to the Start button
    public GameObject pomodoroAdjustButtons; // Group for Pomodoro time adjustment buttons
    public GameObject breakAdjustButtons; // Group for Break time adjustment buttons

    public float pomodoroSessionTime = 1500f; // 25 minutes in seconds
    public float breakTime = 300f; // 5 minutes in seconds
    public float adjustmentSpeed = 0.1f; // How fast the timer adjusts when holding the button
    public float holdDelay = 0.8f; // Delay before continuous adjustment starts

    //Audio
    public AudioClip pomodoroEndSound; // Sound for Pomodoro end
    public AudioClip breakEndSound; // Sound for Break end

    private AudioSource audioSource; // AudioSource component reference

    //Status checkers
    private float currentTime; // Tracks the current session time (Pomodoro or break)
    private bool isSessionActive = false;
    private bool isPaused = false;
    private bool isBreakActive = false;

    public int XP_base_rate = 20; //xp rate of 20 per min 
    public int Gold_base_rate = 1; //gold rate of 1 per min 

    public int XP_total = 0;
    public int Gold_total = 0; 

    public int XP_increment = 0;
    public int Gold_increment = 0;

    public TextMeshProUGUI Gold_count; //Link gold count text box 
    public TextMeshProUGUI Xp_count; //Link xp count text box 
    
    private const string GoldKey = "GoldTotal"; // Key for saving gold in PlayerPrefs
    private const string XPKey = "XPTotal"; // Key for saving XP in PlayerPrefs 


    void Start()
    {
        currentTime = pomodoroSessionTime; // Set current time to Pomodoro time initially
        UpdatePomodoroTimerDisplay();
        UpdateBreakTimerDisplay(); // Ensure both timers are initialized and visible
        stopButton.interactable = false; // Disable Stop button initially (greyed out)
        audioSource = GetComponent<AudioSource>();//get the AudioSource component
        scene2Handler = FindObjectOfType<Scene2Handler>();

    //
        Gold_total = PlayerPrefs.GetInt(GoldKey, Gold_total); // Default is 0 if not saved
        XP_total = PlayerPrefs.GetInt(XPKey, XP_total);       // Default is 0 if not saved

        UpdateGoldDisplay();
        UpdateXPDisplay();

    //  

        if (scene2Handler != null)
        {
            // Access extractedTime from Scene2Handler
            string currentTime = scene2Handler.extractedTime;

            if (!string.IsNullOrEmpty(currentTime))
            {
                // Do something with the extracted time
                Debug.Log("Current time from Scene2Handler: " + currentTime);
            }
            else
            {
                Debug.LogWarning("No extracted time found.");
            }
        }
        else
        {
            Debug.LogError("Scene2Handler not found in the scene.");
        }
    }

    void Update()
    {
        if (isSessionActive && !isPaused)
        {
            currentTime -= Time.deltaTime; // Decrease current time

            // Check if time has run out
            if (currentTime <= 0)
            {
                if (isBreakActive)
                {
                    EndBreak(); // End the break session
                }
                else
                {
                    EndPomodoro(); // End the Pomodoro session
                }
            }

            // Update the correct timer depending on the session type
            if (isBreakActive)
            {
                UpdateBreakTimerDisplay(); // Update break timer during break
            }
            else
            {
                UpdatePomodoroTimerDisplay(); // Update Pomodoro timer during session
            }
        }
    }

    // Handle Holding Down the Pomodoro Timer Adjustment Buttons
    public void OnIncreasePomodoroPressed()
    {
        // Delay by 0.25 seconds before starting continuous adjustment
        Invoke("StartIncreasingPomodoro", holdDelay);
    }

    public void OnDecreasePomodoroPressed()
    {
        // Delay by 0.25 seconds before starting continuous adjustment
        Invoke("StartDecreasingPomodoro", holdDelay);
    }

    public void OnPomodoroReleased()
    {
        CancelInvoke("StartIncreasingPomodoro"); // Cancel delayed increase
        CancelInvoke("StartDecreasingPomodoro"); // Cancel delayed decrease
        CancelInvoke("IncreasePomodoroTime"); // Stop continuous increase
        CancelInvoke("DecreasePomodoroTime"); // Stop continuous decrease
    }

    // Start continuous increase after delay
    void StartIncreasingPomodoro()
    {
        InvokeRepeating("IncreasePomodoroTime", holdDelay, adjustmentSpeed); // Start continuous increase
    }

    // Start continuous decrease after delay
    void StartDecreasingPomodoro()
    {
        InvokeRepeating("DecreasePomodoroTime", holdDelay, adjustmentSpeed); // Start continuous decrease
    }

    // Handle Holding Down the Break Timer Adjustment Buttons
    public void OnIncreaseBreakPressed()
    {
        // Delay by 0.25 seconds before starting continuous adjustment
        Invoke("StartIncreasingBreak", holdDelay);
    }

    public void OnDecreaseBreakPressed()
    {
        // Delay by 0.25 seconds before starting continuous adjustment
        Invoke("StartDecreasingBreak", holdDelay);
    }

    public void OnBreakReleased()
    {
        CancelInvoke("StartIncreasingBreak"); // Cancel delayed increase
        CancelInvoke("StartDecreasingBreak"); // Cancel delayed decrease
        CancelInvoke("IncreaseBreakTime"); // Stop continuous increase
        CancelInvoke("DecreaseBreakTime"); // Stop continuous decrease
    }

    // Start continuous increase for break after delay
    void StartIncreasingBreak()
    {
        InvokeRepeating("IncreaseBreakTime", holdDelay, adjustmentSpeed); // Start continuous increase
    }

    // Start continuous decrease for break after delay
    void StartDecreasingBreak()
    {
        InvokeRepeating("DecreaseBreakTime", holdDelay, adjustmentSpeed); // Start continuous decrease
    }




    public void StartPomodoro()
    {
    // Check if a task has been selected by verifying if totalMinutes has a valid value
    if (scene2Handler.totalMinutes > 0)
    {
        currentTime = scene2Handler.totalMinutes;
        Debug.Log("Task selected, using totalMinutes: " + currentTime);
    }
    else
    {
        currentTime = pomodoroSessionTime;
        Debug.Log("No task selected, using default pomodoroSessionTime: " + currentTime);
    }

    isSessionActive = true;
    isPaused = false;
    isBreakActive = false; // Not a break session
    pauseButtonText.text = "Pause";
    stopButton.interactable = true; // Enable the Stop button during the session
    sessionStatusText.text = "Pomodoro Session";

    // Hide start button and time adjustment buttons
    startButton.gameObject.SetActive(false);
    pomodoroAdjustButtons.SetActive(false);
    breakAdjustButtons.SetActive(false);

    UpdatePomodoroTimerDisplay();
    UpdateBreakTimerDisplay();
    }



    public void PausePomodoro()
    {
        if (isSessionActive)
        {
            isPaused = !isPaused; // Toggle pause state

            // Update button text based on whether the timer is paused or not
            pauseButtonText.text = isPaused ? "Resume" : "Pause";
        }
    }

    public void StopPomodoro()
    {
        isSessionActive = false;
        isPaused = false;
        currentTime = pomodoroSessionTime; // Reset the timer to Pomodoro time
        isBreakActive = false; // Stop any active break
        pauseButtonText.text = "Pause";
        stopButton.interactable = false; // Grey out the Stop button

        // Show start button and time adjustment buttons again
        startButton.gameObject.SetActive(true);
        pomodoroAdjustButtons.SetActive(true);
        breakAdjustButtons.SetActive(true);

        //reset 
        scene2Handler.totalMinutes = 0;

        sessionStatusText.text = "Ready for Pomodoro";
        UpdatePomodoroTimerDisplay();
        UpdateBreakTimerDisplay();
        Debug.Log("Pomodoro session stopped!");
        scene2Handler.displayText.text = " ";

    }

    void EndPomodoro()
    {
    if (scene2Handler.totalMinutes > 0)
    {
        // Stop everything if totalMinutes > 0
        isSessionActive = false;
        isBreakActive = false;
        isPaused = true;
        sessionStatusText.text = "Session Stopped"; // Update status text
        stopButton.interactable = true; // Disable the Stop button
        Debug.Log("Session stopped due to remaining total minutes in Scene2Handler.");
        //scene2Handler.displayText.text = "Task Completed \n Xp Gained: \n Gold Gained:";

        Gold_increment = Gold_base_rate * (int)scene2Handler.totalMinutes;
        XP_increment = XP_base_rate * (int)scene2Handler.totalMinutes;

        XP_total = XP_total + XP_base_rate * (int)scene2Handler.totalMinutes;
        Gold_total = Gold_total + Gold_base_rate * (int)scene2Handler.totalMinutes; 
        scene2Handler.displayText.text = "Task Completed \nXp Gained: +"+XP_increment.ToString()+"\nGold Gained: +"+Gold_increment.ToString(); 

        SaveGold();
        SaveXP();
        UpdateGoldDisplay();
        UpdateXPDisplay();
        
        // Retrieve the last clicked button text from PlayerPrefs
        string lastClickedButtonText = PlayerPrefs.GetString("LastClickedButtonText", null);

        if (!string.IsNullOrEmpty(lastClickedButtonText))
        {
            // Remove the task from the persistent list
            buttonListHandler.RemoveTask(lastClickedButtonText);

            // Find and destroy the button in the current scene
            foreach (Transform child in buttonListHandler.buttonListContent)
            {
                TMP_Text buttonTMPText = child.GetComponentInChildren<TMP_Text>();
                if (buttonTMPText != null && buttonTMPText.text == lastClickedButtonText)
                {
                    Destroy(child.gameObject);
                    Debug.Log($"Button '{lastClickedButtonText}' and associated task deleted.");
                    break;
                }
            }

            // Clear the stored reference in PlayerPrefs
            PlayerPrefs.DeleteKey("LastClickedButtonText");
            PlayerPrefs.Save();
        }
        else
        {
            Debug.LogWarning("No button text found to delete.");
        }



    }
    else
    {
        // Continue with the existing break logic
        isSessionActive = false;
        currentTime = breakTime; // Switch to break time
        isBreakActive = true; // Break session starts
        isPaused = false;
        pauseButtonText.text = "Pause"; // Reset pause button text
        sessionStatusText.text = "Break Time";

        // Play Pomodoro end sound
        PlaySound(pomodoroEndSound);

        stopButton.interactable = true; // Enable the Stop button during the break session

        Debug.Log("Pomodoro session complete! Starting break.");
        isSessionActive = true; // Start the break automatically
        UpdateBreakTimerDisplay(); // Update the break timer to start counting down
    }
    }

    private void UpdateGoldDisplay()
    {
        if (Gold_count != null)
        {
            Gold_count.text = Gold_total.ToString();
            
        }
        else
        {
            Debug.LogError("Gold_count text box is not assigned!");
        }
    }

    private void UpdateXPDisplay()
    {
        if (Xp_count != null)
        {
            Xp_count.text = XP_total.ToString();
            
        }
        else
        {
            Debug.LogError("Xp_count text box is not assigned!");
        }
    }

    private void SaveGold()
    {
        PlayerPrefs.SetInt(GoldKey, Gold_total); // Save gold
        PlayerPrefs.Save();
    }

    private void SaveXP()
    {
        PlayerPrefs.SetInt(XPKey, XP_total); // Save XP
        PlayerPrefs.Save();
    }














    void EndBreak()
    {
        isSessionActive = false;
        isBreakActive = false; // Break session ends
        currentTime = pomodoroSessionTime; // Reset to Pomodoro time for the next session
        stopButton.interactable = false; // Grey out the Stop button after the session

        //Play Break end Sound
        PlaySound(breakEndSound);

        // Show start button and time adjustment buttons again
        startButton.gameObject.SetActive(true);
        pomodoroAdjustButtons.SetActive(true);
        breakAdjustButtons.SetActive(true);

        sessionStatusText.text = "Ready for Pomodoro";
        Debug.Log("Break over! Ready for a new Pomodoro session.");

        UpdatePomodoroTimerDisplay(); // Reset the Pomodoro timer for the next session
        UpdateBreakTimerDisplay();
    }

    // Play the assigned sound
    void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip); // Play the sound effect once
        }
    }

    // Adjust Pomodoro time
    public void IncreasePomodoroTime()
    {
        pomodoroSessionTime += 60; // Add 1 minute
        if (!isSessionActive && !isBreakActive)
            currentTime = pomodoroSessionTime; // Update current time only if no session is active
        UpdatePomodoroTimerDisplay();
    }

    public void DecreasePomodoroTime()
    {
        if (pomodoroSessionTime > 360) // Ensure time doesn't go below 1 minute
        {
            pomodoroSessionTime -= 60; // Subtract 1 minute
            if (!isSessionActive && !isBreakActive)
                currentTime = pomodoroSessionTime; // Update current time only if no session is active
        }
        UpdatePomodoroTimerDisplay();
    }

    // Adjust Break time
    public void IncreaseBreakTime()
    {
        breakTime += 60; // Add 1 minute
        if (isBreakActive)
            currentTime = breakTime; // Update the current time if break is active
        UpdateBreakTimerDisplay();
    }

    public void DecreaseBreakTime()
    {
        if (breakTime > 60) // Ensure time doesn't go below 1 minute
        {
            breakTime -= 60; // Subtract 1 minute
            if (isBreakActive)
                currentTime = breakTime; // Update the current time if break is active
        }
        UpdateBreakTimerDisplay();
    }

    // Update Pomodoro Timer Display
    void UpdatePomodoroTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60); // Display Pomodoro countdown
        int seconds = Mathf.FloorToInt(currentTime % 60);
        pomodoroTimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // Update Break Timer Display
    void UpdateBreakTimerDisplay()
    {
        if (isBreakActive)
        {
            int breakMinutes = Mathf.FloorToInt(currentTime / 60); // Countdown for break
            int breakSeconds = Mathf.FloorToInt(currentTime % 60);
            breakTimerText.text = string.Format("{0:00}:{1:00}", breakMinutes, breakSeconds);
        }
        else
        {
            // If break is not active, show the remaining break time that can be adjusted
            int breakMinutes = Mathf.FloorToInt(breakTime / 60);
            int breakSeconds = Mathf.FloorToInt(breakTime % 60);
            breakTimerText.text = string.Format("{0:00}:{1:00}", breakMinutes, breakSeconds);
        }
    }
}