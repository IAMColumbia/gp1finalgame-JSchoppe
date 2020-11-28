using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Team
{
    public byte teamID;
    public Commander[] commanders;
    public TeamStyle style;
}

[Serializable]
public struct TeamStyle
{
    public Color baseColor;
    public bool flipX;
}

public sealed class TeamsSingleton : MonoBehaviour
{
    [SerializeField] private Team[] teams = null;
    private static Team[] singletonTeams;

    private void OnValidate()
    {
        if (teams != null)
            for (byte i = 0; i < teams.Length; i++)
                teams[i].teamID = i;
    }

    private void Awake()
    {
        singletonTeams = teams;
    }

    public static Team FromID(byte ID)
    {
        return singletonTeams[ID];
    }
}
