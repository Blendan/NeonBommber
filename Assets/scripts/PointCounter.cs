﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointCounter : MonoBehaviour
{
    private int count = 0;
    public Text text;
    public string before = "P?:";

    public void madePoint()
    {
        count++;
        text.text = before + count;
    }

    public int GetPoints()
    {
        return count;
    }
}
