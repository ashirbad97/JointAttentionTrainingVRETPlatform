using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.SceneManagement;
/**
@dev: This file has to be restructured, currently it's very confusing
*/
public class InterimMenuController : MonoBehaviour
{
    [SerializeField] GameObject prevTrialNo, currentTrialNo, trialOutputDisplay, trialFeedback, remainingTrials, nextButton, quitButton, saveAndQuitButton, trialLabelArea;
    bool isEndScene;
    DateTime sessionEndTime;
    TimeSpan sessionTotalDuration;
    // Start is called before the first frame update
    void Start()
    {
        // Display Information of Trials
        prevTrialNo.GetComponent<TMP_Text>().text = (ExperimentSettings.currentTrialCount).ToString();
        currentTrialNo.GetComponent<TMP_Text>().text = (ExperimentSettings.currentTrialCount + 1).ToString();
        trialOutputDisplay.GetComponent<TMP_Text>().text = JsonUtility.ToJson(ExperimentSettings.prevTrialData);

        //Check if any remaining trials, condition to check for the last trial in the session
        if (ExperimentSettings.currentTrialCount == ExperimentSettings.trialCount) //check for last session
        {
            isEndScene = true;
            remainingTrials.GetComponent<TMP_Text>().text = "0";
            EndScene();
        }
        else
        {
            remainingTrials.GetComponent<TMP_Text>().text = (ExperimentSettings.trialCount - ExperimentSettings.currentTrialCount).ToString();
        }
    }
    // N.B: Function is invoked at the start screen
    void EndScene()
    {     // Start recording and saving of session end time and session duration
        sessionEndTime = DateTime.Now;
        ExperimentSettings.experimentSessionEndTime = sessionEndTime.ToString();
        sessionTotalDuration = sessionEndTime.Subtract(DateTime.Parse(ExperimentSettings.experimentSessionStartTime));
        ExperimentSettings.experimentSessionTotalDuration = sessionTotalDuration.ToString();
        // End recording and saving of session end time and session duration
        DataDumper.DumpSessionDataExpEnd();//Some Setting level configuration are computed at the end so will append at the end
        // Disable Next and Quit
        nextButton.SetActive(false);
        quitButton.SetActive(false);
        saveAndQuitButton.SetActive(true);
        trialLabelArea.SetActive(false);
    }
    // N.B: Function is only invoked after the user clicks it
    public void IncrementTrial()
    {
        ExperimentSettings.prevTrialData.trial_feedback = trialFeedback.GetComponent<TMP_Text>().text;
        DataDumper.DumpTrialData(ExperimentSettings.prevTrialData);
        if (!isEndScene)
        {
            //Increment the curent project counter
            ExperimentSettings.currentTrialCount += 1;
            SceneManager.LoadScene("Experiment1_Ethical_Demo");
        }
    }
    public void quitGame()
    {
        //Close the application
        Application.Quit();
    }
    public void SaveAndQuit()
    {
        IncrementTrial();
        quitGame();
    }
}
