using System;
using DCL.Helpers;

internal class BridgeHandler : IDisposable
{
    private readonly IBuilderProjectsPanelBridge bridge;
    private readonly ScenesViewController scenesViewController;
    private readonly LandController landsController;
    private readonly SectionsController sectionsController;

    public BridgeHandler(IBuilderProjectsPanelBridge bridge, ScenesViewController scenesViewController, LandController landsController, SectionsController sectionsController)
    {
        this.bridge = bridge;
        this.scenesViewController = scenesViewController;
        this.landsController = landsController;
        this.sectionsController = sectionsController;

        bridge.OnLandsSet += OnLandsUpdated;
        bridge.OnProjectsSet += OnProjectsUpdated;
        
        sectionsController.OnRequestUpdateSceneData += OnRequestUpdateSceneData;
        sectionsController.OnRequestUpdateSceneContributors += OnRequestUpdateSceneContributors;
        sectionsController.OnRequestUpdateSceneAdmins += OnRequestUpdateSceneAdmins;
        sectionsController.OnRequestUpdateSceneBannedUsers += OnRequestUpdateSceneBannedUsers;
    }

    public void Dispose()
    {

    }
    
    private void OnProjectsUpdated(string payload)
    {
        if (scenesViewController != null)
        {
            var scenes = Utils.ParseJsonArray<SceneData[]>(payload);
            scenesViewController.SetScenes(scenes);
        }
    }

    private void OnLandsUpdated(string payload)
    {
        if (landsController != null)
        {
            var lands = Utils.ParseJsonArray<LandData[]>(payload);
            landsController.SetLands(lands);
        }
    }
    
    private void OnRequestUpdateSceneData(string id, SceneDataUpdatePayload dataUpdatePayload)
    {
        bridge?.SendSceneDataUpdate(id, dataUpdatePayload);
    }

    private void OnRequestUpdateSceneContributors(string id, SceneContributorsUpdatePayload payload)
    {
        bridge?.SendSceneContributorsUpdate(id, payload);
    }
    
    private void OnRequestUpdateSceneAdmins(string id, SceneAdminsUpdatePayload payload)
    {
        bridge?.SendSceneAdminsUpdate(id, payload);
    }
    
    private void OnRequestUpdateSceneBannedUsers(string id, SceneBannedUsersUpdatePayload payload)
    {
        bridge?.SendSceneBannedUsersUpdate(id, payload);
    }
}
