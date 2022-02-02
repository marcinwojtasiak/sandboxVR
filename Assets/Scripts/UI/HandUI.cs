using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandUI : MonoBehaviour
{
    [SerializeField] private GameObject HandMenu;
    [SerializeField] private GameObject UICanvas;

    [SerializeField] private RectTransform statIndicators;
    private RectTransform indicatorHP;
    private RectTransform indicatorHunger;
    private RectTransform indicatorThirst;
    private Image indicatorHPImage;
    private Image indicatorHungerImage;
    private Image indicatorThirstImage;

    [SerializeField] private float fov = 70; //should be a bit bigger than cameras FoV
    [SerializeField] private float transitionTime = 0.3f;

    private bool isOpen = false;
    private bool canOpen = false;
    private float transition = 0;

    private const float baseScale = 0.7f;
    private const float enlargedScale = 1f;

    void Start()
    {
        indicatorHP = (RectTransform)statIndicators.Find("Health");
        indicatorHunger = (RectTransform)statIndicators.Find("Hunger");
        indicatorThirst = (RectTransform)statIndicators.Find("Thirst");
        indicatorHPImage = indicatorHP.GetComponent<Image>();
        indicatorHungerImage = indicatorHunger.GetComponent<Image>();
        indicatorThirstImage = indicatorThirst.GetComponent<Image>();
    }


    void Update()
    {
        UpdateIndicators();

        float handCamAngle = (Vector3.Angle(Camera.main.transform.position - transform.position, -transform.right));

        float inFovAngle;
        inFovAngle = (Vector3.Angle(transform.position - Camera.main.transform.position, Camera.main.transform.forward));

        if (handCamAngle <= 40.0f && inFovAngle <= fov / 2)
        {
            Magnify();
        }
        else if (!isOpen)
        {
            Reduce();
        }
    }

    private void UpdateIndicators()
    {
        if (!indicatorHP || !indicatorHunger || !indicatorThirst || !PlayerStats.current)
            return;
            
        indicatorHPImage.fillAmount = (PlayerStats.current.hp / PlayerStats.current.maxHP) / 2;
        indicatorHungerImage.fillAmount = (PlayerStats.current.hunger / PlayerStats.maxHungerAndThirstVal) / 4;
        indicatorThirstImage.fillAmount = (PlayerStats.current.thirst / PlayerStats.maxHungerAndThirstVal) / 4;
    }

    private void Magnify()
    {
        transition = Mathf.Clamp(transition += Time.deltaTime * (1 / transitionTime), 0, 1);
        float currentScale = Mathf.Lerp(baseScale, enlargedScale, transition);

        statIndicators.localScale = new Vector3(currentScale, currentScale, 1);

        canOpen = true;
    }

    private void Reduce()
    {
        transition = Mathf.Clamp(transition -= Time.deltaTime * (1 / transitionTime), 0, 1);
        float currentScale = Mathf.Lerp(baseScale, enlargedScale, transition);

        statIndicators.localScale = new Vector3(currentScale, currentScale, 1);

        canOpen = false;
    }

    public void ChangeMenuState()
    {
        if (!isOpen) // then open
        {
            if (canOpen)
            {
                HandMenu.SetActive(true); // add some kind of transition
                isOpen = true;
            }         
        }
        else // then close
        {
            HandMenu.SetActive(false);
            isOpen = false;
        }
    }
}
