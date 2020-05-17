using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DriveUI : MonoBehaviour
{
    private Sprite Offline;
    private Sprite InSurvice;

    private Image m_sourceImage;

    private void Start()
    {
        
        Offline = Resources.Load<Sprite>("offline");
        InSurvice = Resources.Load<Sprite>("inservice");
        m_sourceImage = gameObject.GetComponent<Image>();
    }

    private void Update()
    {
        if (BoatControlTrigger.ifEnterTrigger == true) // disable character move and enable boat move
        {
            m_sourceImage.sprite = InSurvice;
        }
        else
        {
            m_sourceImage.sprite = Offline;
        }
    }
}
