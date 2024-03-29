﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class Player : MonoBehaviour
{
    public static Player Instance;

    [Header("Debug")] public bool debugKillPlayer;
    public bool debugSpawnPlayer;

    [Header("Effects")] public GameObject deathParticle;
    public GameObject spawnParticle;
    private ParticleSystem[] wispParticle;

    private static Vector3 spawnPosition;

    private ActionIntervalTimer analyticsReporterTimer;

    void Start () {
        spawnPosition = transform.position;
        wispParticle = GetComponentsInChildren<ParticleSystem>();
        Instance = this;

        analyticsReporterTimer = new ActionIntervalTimer(this, ReportPosition, 0.1f);
    }

    void ReportPosition()
    {
        AnalyticsManager.ReportLocation(transform.position);
    }

    void Update()
    {
        if (debugKillPlayer)
        {
            KillPlayer();
            debugKillPlayer = false;
        }

        if (debugSpawnPlayer)
        {
            SpawnPlayer(spawnPosition);
            debugSpawnPlayer = false;
        }
    }

    public static void SetSpawnLocation(Vector3 position)
    {
        spawnPosition = position;
    }

    public static void KillPlayer()
    {
        KillPlayer(true);
    }

    public static void KillPlayer(bool respawn)
    {
        if (Instance != null)
        {
            Instance.analyticsReporterTimer.running = false;
            Instance.StartCoroutine(Instance.Death(respawn));

            AnalyticsManager.AddDeathEntry(Instance.transform.position);
            AnalyticsManager.ReportLocation(Instance.transform.position);
            AnalyticsManager.TerminateLocation();
	        AnalyticsManager.SaveData();
            Analytics.CustomEvent("playerDeath", new Dictionary<string, object>
            {
                {"Position", (Vector2) Instance.transform.position},
                {"Level", (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name)}
            });
        }
    }

    public static void SpawnPlayer(Vector3 position)
    {
        SpawnPlayer(position, null);
    }

    public static void SpawnPlayer(Vector3 position, GameObject playerPrefab)
    {

        Instantiate(playerPrefab, position, Quaternion.identity);
    }


    IEnumerator Death(bool respawn)
    {
        GetComponent<PlayerControllerA>().enabled = false;
        GetComponent<DeathOnImpact>().enabled = false;

        foreach (ParticleSystem ps in wispParticle)
        {
            ParticleSystem.EmissionModule em = ps.emission;
            em.enabled = false;
        }

        yield return new WaitForSeconds(1);

        Destroy(gameObject);
        Instance = null;

        if (respawn)
        {
            GameObject playerPrefab = Resources.Load<GameObject>("Player");
            SpawnPlayer(spawnPosition, playerPrefab);
        }
    }
}
