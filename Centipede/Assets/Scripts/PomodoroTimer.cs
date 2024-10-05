using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PomodoroTimer : MonoBehaviour
{
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
    public float holdDelay = 0.25f; // Delay before continuous adjustment starts

    //Audio
    public AudioClip pomodoroEndSound; // Sound for Pomodoro end
    public AudioClip breakEndSound; // Sound for Break end

    private AudioSource audioSource; // AudioSource component reference

    //Status checkers
    private float currentTime; // Tracks the current session time (Pomodoro or break)
    private bool isSessionActive = false;
    private bool isPaused = false;
    private bool isBreakActive = false;

    void Start()
    {
        currentTime = pomodoroSessionTime; // Set current time to Pomodoro time initially
        UpdatePomodoroTimerDisplay();
        UpdateBreakTimerDisplay(); // Ensure both timers are initialized and visible
        stopButton.interactable = false; // Disable Stop button initially (greyed out)
        audioSource = GetComponent<AudioSource>();//get the AudioSource component
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
        InvokeRepeating("IncreasePomodoroTime", 0f, adjustmentSpeed); // Start continuous increase
    }

    // Start continuous decrease after delay
    void StartDecreasingPomodoro()
    {
        InvokeRepeating("DecreasePomodoroTime", 0f, adjustmentSpeed); // Start continuous decrease
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
        InvokeRepeating("IncreaseBreakTime", 0f, adjustmentSpeed); // Start continuous increase
    }

    // Start continuous decrease for break after delay
    void StartDecreasingBreak()
    {
        InvokeRepeating("DecreaseBreakTime", 0f, adjustmentSpeed); // Start continuous decrease
    }

    public void StartPomodoro()
    {
        currentTime = pomodoroSessionTime; // Reset to Pomodoro session time
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

        sessionStatusText.text = "Ready for Pomodoro";
        UpdatePomodoroTimerDisplay();
        UpdateBreakTimerDisplay();
        Debug.Log("Pomodoro session stopped!");

    }

    void EndPomodoro()
    {
        isSessionActive = false;
        currentTime = breakTime; // Switch to break time
        isBreakActive = true; // Break session starts
        isPaused = false;
        pauseButtonText.text = "Pause"; // Reset pause button text
        sessionStatusText.text = "Break Time";

        //Play Pomodoro end sound
        PlaySound(pomodoroEndSound);

        stopButton.interactable = true; // Enable the Stop button during the break session

        Debug.Log("Pomodoro session complete! Starting break.");
        isSessionActive = true; // Start the break automatically
        UpdateBreakTimerDisplay(); // Update the break timer to start counting down
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
        if (pomodoroSessionTime > 60) // Ensure time doesn't go below 1 minute
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
