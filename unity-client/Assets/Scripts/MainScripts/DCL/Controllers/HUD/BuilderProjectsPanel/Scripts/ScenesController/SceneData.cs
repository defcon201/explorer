﻿using System;
using UnityEngine;

internal interface ISceneData
{
    Vector2Int coords { get; }
    Vector2Int size { get; }
    string id { get; }
    string name { get; }
    string thumbnailUrl { get; }
    bool isOwner { get; }
    bool isOperator { get; }
    bool isContributor { get; }
    bool isDeployed { get; }
    int entitiesCount { get; }
    string authorThumbnail { get; }
    string authorName { get; }
}

[Serializable]
internal class SceneData : ISceneData
{
    public Vector2Int coords;
    public Vector2Int size;
    public string id;
    public string name;
    public string thumbnailUrl;
    public bool isOwner;
    public bool isOperator;
    public bool isContributor;
    public bool isDeployed;
    public int entitiesCount;
    public string authorThumbnail;
    public string authorName;

    Vector2Int ISceneData.coords => coords;
    Vector2Int ISceneData.size => size;
    string ISceneData.id => id;
    string ISceneData.name => name;
    string ISceneData.thumbnailUrl => thumbnailUrl;
    bool ISceneData.isOwner => isOwner;
    bool ISceneData.isOperator => isOperator;
    bool ISceneData.isContributor => isContributor;
    bool ISceneData.isDeployed => isDeployed;
    int ISceneData.entitiesCount => entitiesCount;
    string ISceneData.authorThumbnail => authorThumbnail;
    string ISceneData.authorName => authorName;
}