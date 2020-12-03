using System.Collections.Generic;
using UnityEngine;
using SkirmishWars.UnityEditor;

public sealed class UnitySceneParser : IDesignerParser
{

    public TileGrid GetFirstTileGrid()
    {
        return Object.FindObjectOfType<TileGridInstance>().GetInstance();
    }

    public TileActor[] GetAllPreplacedActors(TileGrid onGrid)
    {
        List<TileActor> foundActors = new List<TileActor>();
        foreach (CombatUnitInstance actor in Object.FindObjectsOfType<CombatUnitInstance>())
            foundActors.Add(actor.GetInstance(onGrid));

        return foundActors.ToArray();
    }

    public Commander[] GetAllPreplacedCommanders(TileGrid onGrid)
    {
        List<Commander> foundCommanders = new List<Commander>();
        foreach (PlayerCommanderInstance playerCommander in
            Object.FindObjectsOfType<PlayerCommanderInstance>())
            foundCommanders.Add(playerCommander.GetInstance(onGrid));
        foreach (AgentCommanderInstance agentCommander in
            Object.FindObjectsOfType<AgentCommanderInstance>())
            foundCommanders.Add(agentCommander.GetInstance(onGrid));
        return foundCommanders.ToArray();
    }

    public DamageTable GetFirstDamageTable()
    {
        return Object.FindObjectOfType<DamageTableInstance>().GetInstance();
    }
}
