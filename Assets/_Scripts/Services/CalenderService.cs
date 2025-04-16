using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalenderService : ScriptableObjectService<CalenderSO>, ISave, ILoad
{
    [Header("Configurations")]
    [SerializeField] private int addMinutes = 15;
    [SerializeField] private float intervall = 3f;

    private DateTime currentDateTime;
    private float cooldown;

    // events
    public static event TimeDelegate OnTimeChanged;
    public delegate void TimeDelegate(DateTime datetime);

    /// <summary>
    /// It's possible to do cool checks like this currentDateTime.DayOfWeek == DayOfWeek.Friday for special interactions
    /// </summary>
    private void Update()
    {
        cooldown -= Time.deltaTime;
        if (cooldown <= 0)
        {
            currentDateTime = currentDateTime.AddMinutes(addMinutes);
            cooldown = intervall;

            OnTimeChanged?.Invoke(currentDateTime);
        }
    }

    public override void CreateScriptableObjects()
    {
        CalenderData rawData = DataService.CreateSingleDataFromFilesAndMods<CalenderData>("Calender");

        // Correctly parse the UNIX timestamp as seconds
        long unixSeconds = long.Parse(rawData.StartDateUnix);
        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(unixSeconds);
        DateTime dateTime = dateTimeOffset.DateTime;

        CalenderSO calenderSO = ScriptableObject.CreateInstance<CalenderSO>();
        calenderSO.Id = rawData.Id;
        calenderSO.StartDate = dateTime;

        scriptableObjects.Add(calenderSO);
    }

    public void InstantiateDateTime()
    {
        CalenderSO calenderSO = GetScriptableObject(0);
        currentDateTime = calenderSO.StartDate;
    }

    /// <summary>
    /// Convert UNIX to dateTime
    /// </summary>
    public void Load(Save save)
    {
        long unixTime = long.Parse(save.Calender.DateTimeUnix);
        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(unixTime);
        currentDateTime = dateTimeOffset.LocalDateTime;

        OnTimeChanged?.Invoke(currentDateTime);
    }

    /// <summary>
    /// Convert currentDateTime to UNIX UTC 
    /// </summary>
    public void Save(Save save)
    {
        long unixTime = ((DateTimeOffset)currentDateTime.ToUniversalTime()).ToUnixTimeSeconds();
        save.Calender.DateTimeUnix = unixTime.ToString();
    }
}