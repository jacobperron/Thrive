﻿using System;
using Godot;

public class PatchExtinctionBox : Control
{
    [Export]
    public NodePath PatchMapDrawerPath = null!;

    [Export]
    public NodePath PatchDetailsPanelPath = null!;

    [Export]
    public NodePath AnimationPlayer = null!;

    private PatchMapDrawer patchMapDrawer = null!;
    private PatchDetailsPanel patchDetailsPanel = null!;
    private AnimationPlayer animationPlayer = null!;

    public PatchMap Map { get; set; } = null!;
    public Species PlayerSpecies { get; set; } = null!;
    public Action<Patch> GoToNewPatch { get; set; } = null!;

    public override void _Ready()
    {
        patchMapDrawer = GetNode<PatchMapDrawer>(PatchMapDrawerPath);
        patchDetailsPanel = GetNode<PatchDetailsPanel>(PatchDetailsPanelPath);
        animationPlayer = GetNode<AnimationPlayer>(AnimationPlayer);

        patchMapDrawer.Map = Map;
        patchDetailsPanel.CurrentPatch = Map.CurrentPatch;
        patchDetailsPanel.Patch = null;
        patchDetailsPanel.OnMoveToPatchClicked = NewPatchSelected;

        patchMapDrawer.OnSelectedPatchChanged = SelectedPatchChanged;

        patchMapDrawer.SetPatchEnabledStatuses(Map.Patches.Values, p => p.GetSpeciesPopulation(PlayerSpecies) > 0);
    }

    private void NewPatchSelected(Patch patch)
    {
        animationPlayer.PlayBackwards();
        TransitionManager.Instance.AddScreenFade(ScreenFade.FadeType.FadeOut, animationPlayer.CurrentAnimationLength,
            false);
        TransitionManager.Instance.StartTransitions(this, nameof(OnFadedToBlack));
        patchDetailsPanel.MouseFilter = MouseFilterEnum.Ignore;
    }

    private void OnFadedToBlack()
    {
        if (patchDetailsPanel.Patch == null)
            throw new NullReferenceException("The patch must not be null at this point");

        GoToNewPatch.Invoke(patchDetailsPanel.Patch);

        TransitionManager.Instance.AddScreenFade(ScreenFade.FadeType.FadeIn, animationPlayer.CurrentAnimationLength,
            false);
        TransitionManager.Instance.StartTransitions();
    }

    private void SelectedPatchChanged(PatchMapDrawer drawer)
    {
        patchDetailsPanel.IsPatchMoveValid = drawer.SelectedPatch != null;
        patchDetailsPanel.Patch = drawer.SelectedPatch;
    }
}