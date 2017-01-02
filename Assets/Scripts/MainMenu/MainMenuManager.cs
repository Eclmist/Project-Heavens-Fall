﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* IF YOU ARE NOT CALLED SAM DONT READ THIS FILE */
/* OR ELSE YOU'LL VOMIT FROM BAD CODE */

public class MainMenuManager : MonoBehaviour
{

    public CanvasGroup levelSelectorPanel;
    public Button btn_continue, btn_levelSelect;
    public Button[] btn_levels;

    private bool levelSelectOpen;

    private Animator animator;

    private float opacity;

    private string latestLevel;

    public void Awake()
    {
        string[] profiles = ProgressionManager.GetSavedProfiles();

        if (profiles.Length > 0)
        {
            ProgressionManager.LoadProfile(profiles[0]);
        }
        else
        {
            ProgressionManager.CreateNewProfile();
        }

        Assert.HardAssert((ProgressionManager.CurrentProfile != null),
            "PROFILE CREATION F***ED UP");
    }


    public void Start()
    {
        animator = GetComponent<Animator>();

        LevelSelectorActive(false);

        if (ProgressionManager.CurrentProfile.IsLevelCleared(btn_levels[0].name))
        {
            foreach (Button levelBtn in btn_levels)
            {

                if (ProgressionManager.CurrentProfile.IsLevelCleared(levelBtn.name))
                {
                    levelBtn.interactable = true;
                    latestLevel = levelBtn.name;
                }
                else
                {
                    levelBtn.interactable = false;
                }
            }

            btn_continue.interactable = true;
            btn_levelSelect.interactable = true;
        }
        else
        {
            btn_continue.interactable = false;
            btn_levelSelect.interactable = false;
        }

    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            LevelSelectorActive(false); 
    }

    public void LevelSelectorActive(bool active)
    {
        levelSelectOpen = active;
        animator.SetBool("LevelSelectOpen", active);

        iTween.ValueTo(gameObject, iTween.Hash(
            "from", opacity,
            "to", active? 1 : 0,
            "time", 0.5F,
            "onupdatetarget", gameObject,
            "onupdate", "tweenOnUpdateCallBack",
            "easetype", iTween.EaseType.easeOutQuad
            ));
    }

    void tweenOnUpdateCallBack(float newValue)
    {
        opacity = newValue;
        levelSelectorPanel.alpha = newValue;
    }

    public void ToggleLevelSelectorActive()
    {
        LevelSelectorActive(!levelSelectOpen);
    }

    public void Continue()
    {
        FindObjectOfType<LoadScene>().Load(latestLevel);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}

	