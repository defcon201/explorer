﻿using System;

internal class SceneContextMenuHandler : IDisposable
{
    private readonly SceneCardViewContextMenu contextMenu;
    private readonly IBuilderProjectsPanelBridge bridge;
    private readonly SectionsController sectionsController;
    private readonly ScenesViewController scenesViewController;

    public SceneContextMenuHandler(SceneCardViewContextMenu contextMenu, SectionsController sectionsController,
        ScenesViewController scenesViewController, IBuilderProjectsPanelBridge bridge)
    {
        this.contextMenu = contextMenu;
        this.bridge = bridge;
        this.sectionsController = sectionsController;
        this.scenesViewController = scenesViewController;

        sectionsController.OnRequestContextMenuHide += OnRequestContextMenuHide;

        SceneCardView.OnContextMenuPressed += OnContextMenuOpen;

        contextMenu.OnSettingsPressed += OnContextMenuSettingsPressed;
        contextMenu.OnDuplicatePressed += OnContextMenuDuplicatePressed;
        contextMenu.OnDownloadPressed += OnContextMenuDownloadPressed;
        contextMenu.OnSharePressed += OnContextMenuSharePressed;
        contextMenu.OnUnpublishPressed += OnContextMenuUnpublishPressed;
        contextMenu.OnDeletePressed += OnContextMenuDeletePressed;
        contextMenu.OnQuitContributorPressed += OnContextMenuQuitContributorPressed;
    }

    public void Dispose()
    {
        sectionsController.OnRequestContextMenuHide -= OnRequestContextMenuHide;

        SceneCardView.OnContextMenuPressed -= OnContextMenuOpen;

        contextMenu.OnSettingsPressed -= OnContextMenuSettingsPressed;
        contextMenu.OnDuplicatePressed -= OnContextMenuDuplicatePressed;
        contextMenu.OnDownloadPressed -= OnContextMenuDownloadPressed;
        contextMenu.OnSharePressed -= OnContextMenuSharePressed;
        contextMenu.OnUnpublishPressed -= OnContextMenuUnpublishPressed;
        contextMenu.OnDeletePressed -= OnContextMenuDeletePressed;
        contextMenu.OnQuitContributorPressed -= OnContextMenuQuitContributorPressed;
    }

    void OnContextMenuOpen(ISceneData sceneData, SceneCardView sceneCard)
    {
        contextMenu.transform.position = sceneCard.contextMenuButton.transform.position;
        contextMenu.Show(sceneData.id, sceneData.isDeployed,
            sceneData.isOwner || sceneData.isOperator, sceneData.isContributor);
    }

    void OnRequestContextMenuHide()
    {
        contextMenu.Hide();
    }

    void OnContextMenuSettingsPressed(string id)
    {
        scenesViewController.SelectScene(id);
    }

    void OnContextMenuDuplicatePressed(string id)
    {
        bridge?.SendDuplicateProject(id);
    }

    void OnContextMenuDownloadPressed(string id)
    {
        bridge?.SendDownload(id);
    }

    void OnContextMenuSharePressed(string id)
    {
        bridge?.SendShare(id);
    }

    void OnContextMenuUnpublishPressed(string id)
    {
        bridge?.SendUnPublish(id);
    }

    void OnContextMenuDeletePressed(string id)
    {
        bridge?.SendDelete(id);
    }

    void OnContextMenuQuitContributorPressed(string id)
    {
        bridge?.SendQuitContributor(id);
    }
}
