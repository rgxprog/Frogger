using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTextLogic : MonoBehaviour
{
    //-------------------------------------------

    public TMPro.TextMeshProUGUI levelText;

    private float timeToShowLevel = 3f;
    private float currentTimeToShow = 0;
    private bool showLevel = false;

    //-------------------------------------------

    private void Update()
    {
        if (!showLevel)
            return;

        currentTimeToShow += Time.deltaTime;
        if (currentTimeToShow > timeToShowLevel)
        {
            showLevel = false;
            gameObject.SetActive(false);
        }
    }

    //-------------------------------------------

    public void ShowLevel(int level)
    {
        levelText.text = "Nivel: " + level.ToString();
        showLevel = true;
        currentTimeToShow = 0;
        gameObject.SetActive(true);
    }

    //-------------------------------------------
}
