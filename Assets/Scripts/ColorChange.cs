using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChange : MonoBehaviour
{
    Material m_Material;
    LevelEvents events;

    private void Awake()
    {
        events = GameObject.Find("Level Events").GetComponent<LevelEvents>();
    }

    void Start()
    {
        m_Material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (events.getState().WallEventActive)
        {
            m_Material.color = Color.green;
        }
    }
}
