﻿using System;
using System.Collections.Generic;
using System.Globalization;
using Godot;
using Newtonsoft.Json;
using Array = Godot.Collections.Array;

/// <summary>
///   Base HUD class for stages where the player moves a creature around
/// </summary>
/// <typeparam name="TStage">The type of the stage this HUD is for</typeparam>
[JsonObject(MemberSerialization.OptIn)]
public abstract class StageHUDBase<TStage> : Control, IStageHUD
    where TStage : Godot.Object, IStage
{
    [Export]
    public NodePath AnimationPlayerPath = null!;

    [Export]
    public NodePath PanelsTweenPath = null!;

    [Export]
    public NodePath LeftPanelsPath = null!;

    [Export]
    public NodePath MouseHoverPanelPath = null!;

    [Export]
    public NodePath HoveredCellsContainerPath = null!;

    [Export]
    public NodePath MenuPath = null!;

    [Export]
    public NodePath PauseButtonPath = null!;

    [Export]
    public NodePath ResumeButtonPath = null!;

    [Export]
    public NodePath AtpLabelPath = null!;

    [Export]
    public NodePath HpLabelPath = null!;

    [Export]
    public NodePath PopulationLabelPath = null!;

    [Export]
    public NodePath PatchLabelPath = null!;

    [Export]
    public NodePath PatchOverlayAnimatorPath = null!;

    [Export]
    public NodePath EditorButtonPath = null!;

    [Export]
    public NodePath EnvironmentPanelPath = null!;

    [Export]
    public NodePath OxygenBarPath = null!;

    [Export]
    public NodePath Co2BarPath = null!;

    [Export]
    public NodePath NitrogenBarPath = null!;

    [Export]
    public NodePath TemperaturePath = null!;

    [Export]
    public NodePath SunlightPath = null!;

    [Export]
    public NodePath PressurePath = null!;

    [Export]
    public NodePath EnvironmentPanelBarContainerPath = null!;

    [Export]
    public NodePath CompoundsPanelPath = null!;

    [Export]
    public NodePath GlucoseBarPath = null!;

    [Export]
    public NodePath AmmoniaBarPath = null!;

    [Export]
    public NodePath PhosphateBarPath = null!;

    [Export]
    public NodePath HydrogenSulfideBarPath = null!;

    [Export]
    public NodePath IronBarPath = null!;

    [Export]
    public NodePath EnvironmentPanelExpandButtonPath = null!;

    [Export]
    public NodePath EnvironmentPanelCompressButtonPath = null!;

    [Export]
    public NodePath CompoundsPanelExpandButtonPath = null!;

    [Export]
    public NodePath CompoundsPanelCompressButtonPath = null!;

    [Export]
    public NodePath CompoundsPanelBarContainerPath = null!;

    [Export]
    public NodePath AtpBarPath = null!;

    [Export]
    public NodePath HealthBarPath = null!;

    [Export]
    public NodePath AmmoniaReproductionBarPath = null!;

    [Export]
    public NodePath PhosphateReproductionBarPath = null!;

    [Export]
    public NodePath EditorButtonFlashPath = null!;

    [Export]
    public NodePath ProcessPanelPath = null!;

    [Export]
    public NodePath ProcessPanelButtonPath = null!;

    [Export]
    public NodePath HintTextPath = null!;

    [Export]
    public AudioStream MicrobePickupOrganelleSound = null!;

    [Export]
    public Texture AmmoniaBW = null!;

    [Export]
    public Texture PhosphatesBW = null!;

    [Export]
    public Texture AmmoniaInv = null!;

    [Export]
    public Texture PhosphatesInv = null!;

    [Export]
    public NodePath HotBarPath = null!;

    [Export]
    public NodePath EngulfHotkeyPath = null!;

    [Export]
    public NodePath SignallingAgentsHotkeyPath = null!;

    [Export]
    public NodePath MicrobeControlRadialPath = null!;

    [Export]
    public NodePath PausePromptPath = null!;

    [Export]
    public NodePath PauseInfoPath = null!;

    [Export]
    public NodePath HoveredCompoundsContainerPath = null!;

    [Export]
    public NodePath HoverPanelSeparatorPath = null!;

    [Export]
    public NodePath AgentsPanelPath = null!;

    [Export]
    public NodePath OxytoxyBarPath = null!;

    [Export]
    public NodePath AgentsPanelBarContainerPath = null!;

    [Export]
    public PackedScene ExtinctionBoxScene = null!;

    [Export]
    public NodePath FireToxinHotkeyPath = null!;

    protected readonly Dictionary<Species, int> hoveredSpeciesCounts = new();

    protected readonly Dictionary<Compound, HoveredCompoundControl> hoveredCompoundControls = new();

    protected Compound ammonia = null!;
    protected Compound atp = null!;
    protected Compound carbondioxide = null!;
    protected Compound glucose = null!;
    protected Compound hydrogensulfide = null!;
    protected Compound iron = null!;
    protected Compound nitrogen = null!;
    protected Compound oxygen = null!;
    protected Compound oxytoxy = null!;
    protected Compound phosphates = null!;
    protected Compound sunlight = null!;
    protected AnimationPlayer animationPlayer = null!;
    protected MarginContainer mouseHoverPanel = null!;
    protected Panel environmentPanel = null!;
    protected GridContainer? environmentPanelBarContainer;
    protected ActionButton engulfHotkey = null!;
    protected ActionButton signallingAgentsHotkey = null!;

    // Store these statefully for after player death
    private float maxHP = 1.0f;
    protected float maxATP = 1.0f;

    protected ProgressBar oxygenBar = null!;
    protected ProgressBar co2Bar = null!;
    protected ProgressBar nitrogenBar = null!;
    protected ProgressBar temperature = null!;
    protected ProgressBar sunlightLabel = null!;

    // TODO: implement changing pressure conditions
    // ReSharper disable once NotAccessedField.Local
    protected ProgressBar pressure = null!;

    protected GridContainer? compoundsPanelBarContainer;
    protected ProgressBar glucoseBar = null!;
    protected ProgressBar ammoniaBar = null!;
    protected ProgressBar phosphateBar = null!;
    protected ProgressBar hydrogenSulfideBar = null!;
    protected ProgressBar ironBar = null!;
    protected Button environmentPanelExpandButton = null!;
    protected Button environmentPanelCompressButton = null!;
    protected Button compoundsPanelExpandButton = null!;
    protected Button compoundsPanelCompressButton = null!;
    protected TextureProgress atpBar = null!;
    protected TextureProgress healthBar = null!;
    protected TextureProgress ammoniaReproductionBar = null!;
    protected TextureProgress phosphateReproductionBar = null!;
    protected Light2D editorButtonFlash = null!;
    protected PauseMenu menu = null!;
    protected TextureButton pauseButton = null!;
    protected TextureButton resumeButton = null!;
    protected Label atpLabel = null!;
    protected Label hpLabel = null!;
    protected Label populationLabel = null!;
    protected Label patchLabel = null!;
    protected TextureButton editorButton = null!;
    protected Tween panelsTween = null!;
    protected Label hintText = null!;
    protected RadialPopup packControlRadial = null!;

    protected Control winExtinctBoxHolder = null!;

    /// <summary>
    ///   Access to the stage to retrieve information for display as well as call some player initiated actions.
    /// </summary>
    protected TStage? stage;

    /// <summary>
    ///   Show mouse coordinates data in the mouse
    ///   hover box, useful during develop.
    /// </summary>
#pragma warning disable 649 // ignored until we get some GUI or something to change this
    protected bool showMouseCoordinates;
#pragma warning restore 649

    private Control pausePrompt = null!;
    private CustomRichTextLabel pauseInfo = null!;

    /// <summary>
    ///   For toggling paused with the pause button.
    /// </summary>
    private bool paused;

    private bool environmentCompressed;
    private bool compoundCompressed;
    private bool leftPanelsActive;

    protected VBoxContainer hoveredCompoundsContainer = null!;
    protected HSeparator hoveredCellsSeparator = null!;
    protected VBoxContainer hoveredCellsContainer = null!;
    protected Panel compoundsPanel = null!;
    protected HBoxContainer hotBar = null!;
    protected ActionButton fireToxinHotkey = null!;
    protected Control agentsPanel = null!;
    protected ProgressBar oxytoxyBar = null!;
    protected AnimationPlayer patchOverlayAnimator = null!;
    private CustomDialog? extinctionBox;
    protected Array compoundBars = null!;
    protected ProcessPanel processPanel = null!;
    protected TextureButton processPanelButton = null!;

    /// <summary>
    ///   Used by UpdateHoverInfo to run HOVER_PANEL_UPDATE_INTERVAL
    /// </summary>
    protected float hoverInfoTimeElapsed;

    public string HintText
    {
        get => hintText.Text;
        set => hintText.Text = value;
    }

    [JsonProperty]
    public bool EnvironmentPanelCompressed
    {
        get => environmentCompressed;
        set
        {
            if (environmentCompressed == value)
                return;

            environmentCompressed = value;
            UpdateEnvironmentPanelState();
        }
    }

    [JsonProperty]
    public bool CompoundsPanelCompressed
    {
        get => compoundCompressed;
        set
        {
            if (compoundCompressed == value)
                return;

            compoundCompressed = value;
            UpdateCompoundsPanelState();
        }
    }

    public override void _Ready()
    {
        base._Ready();

        compoundBars = GetTree().GetNodesInGroup("CompoundBar");

        winExtinctBoxHolder = GetNode<Control>("../WinExtinctBoxHolder");

        panelsTween = GetNode<Tween>(PanelsTweenPath);
        mouseHoverPanel = GetNode<MarginContainer>(MouseHoverPanelPath);
        pauseButton = GetNode<TextureButton>(PauseButtonPath);
        resumeButton = GetNode<TextureButton>(ResumeButtonPath);
        agentsPanel = GetNode<Control>(AgentsPanelPath);

        environmentPanel = GetNode<Panel>(EnvironmentPanelPath);
        environmentPanelBarContainer = GetNode<GridContainer>(EnvironmentPanelBarContainerPath);
        oxygenBar = GetNode<ProgressBar>(OxygenBarPath);
        co2Bar = GetNode<ProgressBar>(Co2BarPath);
        nitrogenBar = GetNode<ProgressBar>(NitrogenBarPath);
        temperature = GetNode<ProgressBar>(TemperaturePath);
        sunlightLabel = GetNode<ProgressBar>(SunlightPath);
        pressure = GetNode<ProgressBar>(PressurePath);

        compoundsPanel = GetNode<Panel>(CompoundsPanelPath);
        compoundsPanelBarContainer = GetNode<GridContainer>(CompoundsPanelBarContainerPath);
        glucoseBar = GetNode<ProgressBar>(GlucoseBarPath);
        ammoniaBar = GetNode<ProgressBar>(AmmoniaBarPath);
        phosphateBar = GetNode<ProgressBar>(PhosphateBarPath);
        hydrogenSulfideBar = GetNode<ProgressBar>(HydrogenSulfideBarPath);
        ironBar = GetNode<ProgressBar>(IronBarPath);

        environmentPanelExpandButton = GetNode<Button>(EnvironmentPanelExpandButtonPath);
        environmentPanelCompressButton = GetNode<Button>(EnvironmentPanelCompressButtonPath);
        compoundsPanelExpandButton = GetNode<Button>(CompoundsPanelExpandButtonPath);
        compoundsPanelCompressButton = GetNode<Button>(CompoundsPanelCompressButtonPath);

        oxytoxyBar = GetNode<ProgressBar>(OxytoxyBarPath);
        atpBar = GetNode<TextureProgress>(AtpBarPath);
        healthBar = GetNode<TextureProgress>(HealthBarPath);
        ammoniaReproductionBar = GetNode<TextureProgress>(AmmoniaReproductionBarPath);
        phosphateReproductionBar = GetNode<TextureProgress>(PhosphateReproductionBarPath);
        editorButtonFlash = GetNode<Light2D>(EditorButtonFlashPath);

        atpLabel = GetNode<Label>(AtpLabelPath);
        hpLabel = GetNode<Label>(HpLabelPath);
        menu = GetNode<PauseMenu>(MenuPath);
        animationPlayer = GetNode<AnimationPlayer>(AnimationPlayerPath);
        hoveredCompoundsContainer = GetNode<VBoxContainer>(HoveredCompoundsContainerPath);
        hoveredCellsSeparator = GetNode<HSeparator>(HoverPanelSeparatorPath);
        hoveredCellsContainer = GetNode<VBoxContainer>(HoveredCellsContainerPath);
        populationLabel = GetNode<Label>(PopulationLabelPath);
        patchLabel = GetNode<Label>(PatchLabelPath);
        patchOverlayAnimator = GetNode<AnimationPlayer>(PatchOverlayAnimatorPath);
        editorButton = GetNode<TextureButton>(EditorButtonPath);
        hintText = GetNode<Label>(HintTextPath);
        hotBar = GetNode<HBoxContainer>(HotBarPath);

        pausePrompt = GetNode<Control>(PausePromptPath);
        pauseInfo = GetNode<CustomRichTextLabel>(PauseInfoPath);

        packControlRadial = GetNode<RadialPopup>(MicrobeControlRadialPath);

        engulfHotkey = GetNode<ActionButton>(EngulfHotkeyPath);
        fireToxinHotkey = GetNode<ActionButton>(FireToxinHotkeyPath);
        signallingAgentsHotkey = GetNode<ActionButton>(SignallingAgentsHotkeyPath);

        processPanel = GetNode<ProcessPanel>(ProcessPanelPath);
        processPanelButton = GetNode<TextureButton>(ProcessPanelButtonPath);

        OnAbilitiesHotBarDisplayChanged(Settings.Instance.DisplayAbilitiesHotBar);
        Settings.Instance.DisplayAbilitiesHotBar.OnChanged += OnAbilitiesHotBarDisplayChanged;

        SetEditorButtonFlashEffect(Settings.Instance.GUILightEffectsEnabled);
        Settings.Instance.GUILightEffectsEnabled.OnChanged += SetEditorButtonFlashEffect;

        foreach (var compound in SimulationParameters.Instance.GetCloudCompounds())
        {
            var hoveredCompoundControl = new HoveredCompoundControl(compound);
            hoveredCompoundControls.Add(compound, hoveredCompoundControl);
            hoveredCompoundsContainer.AddChild(hoveredCompoundControl);
        }

        ammonia = SimulationParameters.Instance.GetCompound("ammonia");
        atp = SimulationParameters.Instance.GetCompound("atp");
        carbondioxide = SimulationParameters.Instance.GetCompound("carbondioxide");
        glucose = SimulationParameters.Instance.GetCompound("glucose");
        hydrogensulfide = SimulationParameters.Instance.GetCompound("hydrogensulfide");
        iron = SimulationParameters.Instance.GetCompound("iron");
        nitrogen = SimulationParameters.Instance.GetCompound("nitrogen");
        oxygen = SimulationParameters.Instance.GetCompound("oxygen");
        oxytoxy = SimulationParameters.Instance.GetCompound("oxytoxy");
        phosphates = SimulationParameters.Instance.GetCompound("phosphates");
        sunlight = SimulationParameters.Instance.GetCompound("sunlight");

        UpdateEnvironmentPanelState();
        UpdateCompoundsPanelState();
        UpdatePausePrompt();
    }

    public void Init(TStage containedInStage)
    {
        stage = containedInStage;
    }

    public override void _Process(float delta)
    {
        if (stage == null)
            return;

        if (stage.HasPlayer)
        {
            UpdateNeededBars();
            UpdateCompoundBars();
            UpdateReproductionProgress();
            UpdateAbilitiesHotBar();
        }

        UpdateATP(delta);
        UpdateHealth(delta);
        UpdateHoverInfo(delta);

        UpdatePopulation();
        UpdateProcessPanel();
        UpdatePanelSizing(delta);
    }

    public void PauseButtonPressed()
    {
        if (menu.Visible)
            return;

        GUICommon.Instance.PlayButtonPressSound();

        paused = !paused;
        if (paused)
        {
            pauseButton.Hide();
            resumeButton.Show();
            pausePrompt.Show();
            pauseButton.Pressed = false;

            // Pause the game
            PauseManager.Instance.AddPause(nameof(IStageHUD));
        }
        else
        {
            pauseButton.Show();
            resumeButton.Hide();
            pausePrompt.Hide();
            resumeButton.Pressed = false;

            // Unpause the game
            PauseManager.Instance.Resume(nameof(IStageHUD));
        }
    }

    private void ProcessPanelButtonPressed()
    {
        GUICommon.Instance.PlayButtonPressSound();

        if (processPanel.Visible)
        {
            processPanel.Visible = false;
        }
        else
        {
            processPanel.Show();
        }
    }

    private void OnProcessPanelClosed()
    {
        processPanelButton.Pressed = false;
    }

    private void OnAbilitiesHotBarDisplayChanged(bool displayed)
    {
        hotBar.Visible = displayed;
    }

    /// <summary>
    ///   Makes sure the game is unpaused (at least by us)
    /// </summary>
    protected void EnsureGameIsUnpausedForEditor()
    {
        if (PauseManager.Instance.Paused)
        {
            PauseButtonPressed();

            if (PauseManager.Instance.Paused)
                GD.PrintErr("Unpausing the game after editor button press didn't work");
        }
    }

    /// <summary>
    ///   Enables the editor button.
    /// </summary>
    public void ShowReproductionDialog()
    {
        if (!editorButton.Disabled || stage?.HasPlayer != true)
            return;

        GUICommon.Instance.PlayCustomSound(MicrobePickupOrganelleSound);

        editorButton.Disabled = false;
        editorButton.GetNode<TextureRect>("Highlight").Show();
        editorButton.GetNode<TextureProgress>("ReproductionBar/PhosphateReproductionBar").TintProgress =
            new Color(1, 1, 1, 1);
        editorButton.GetNode<TextureProgress>("ReproductionBar/AmmoniaReproductionBar").TintProgress =
            new Color(1, 1, 1, 1);
        editorButton.GetNode<TextureRect>("ReproductionBar/PhosphateIcon").Texture = PhosphatesBW;
        editorButton.GetNode<TextureRect>("ReproductionBar/AmmoniaIcon").Texture = AmmoniaBW;
        editorButton.GetNode<AnimationPlayer>("AnimationPlayer").Play("EditorButtonFlash");
    }

    /// <summary>
    ///   Disables the editor button.
    /// </summary>
    public void HideReproductionDialog()
    {
        if (!editorButton.Disabled)
            editorButton.Disabled = true;

        editorButton.GetNode<TextureRect>("Highlight").Hide();
        editorButton.GetNode<Control>("ReproductionBar").Show();
        editorButton.GetNode<TextureProgress>("ReproductionBar/PhosphateReproductionBar").TintProgress =
            new Color(0.69f, 0.42f, 1, 1);
        editorButton.GetNode<TextureProgress>("ReproductionBar/AmmoniaReproductionBar").TintProgress =
            new Color(1, 0.62f, 0.12f, 1);
        editorButton.GetNode<TextureRect>("ReproductionBar/PhosphateIcon").Texture = PhosphatesInv;
        editorButton.GetNode<TextureRect>("ReproductionBar/AmmoniaIcon").Texture = AmmoniaInv;
        editorButton.GetNode<AnimationPlayer>("AnimationPlayer").Stop();
    }

    protected void UpdateEnvironmentPanelState()
    {
        if (environmentPanelBarContainer == null)
            return;

        var bars = environmentPanelBarContainer.GetChildren();

        if (environmentCompressed)
        {
            environmentPanelCompressButton.Pressed = true;
            environmentPanelBarContainer.Columns = 2;
            environmentPanelBarContainer.AddConstantOverride("vseparation", 20);
            environmentPanelBarContainer.AddConstantOverride("hseparation", 17);

            foreach (ProgressBar bar in bars)
            {
                panelsTween?.InterpolateProperty(bar, "rect_min_size:x", 95, 73, 0.3f);
                panelsTween?.Start();

                bar.GetNode<Label>("Label").Hide();
                bar.GetNode<Label>("Value").Align = Label.AlignEnum.Center;
            }
        }

        if (!environmentCompressed)
        {
            environmentPanelExpandButton.Pressed = true;
            environmentPanelBarContainer.Columns = 1;
            environmentPanelBarContainer.AddConstantOverride("vseparation", 4);
            environmentPanelBarContainer.AddConstantOverride("hseparation", 0);

            foreach (ProgressBar bar in bars)
            {
                panelsTween?.InterpolateProperty(bar, "rect_min_size:x", bar.RectMinSize.x, 162, 0.3f);
                panelsTween?.Start();

                bar.GetNode<Label>("Label").Show();
                bar.GetNode<Label>("Value").Align = Label.AlignEnum.Right;
            }
        }
    }

    protected void UpdateCompoundsPanelState()
    {
        if (compoundsPanelBarContainer == null)
            return;

        var bars = compoundsPanelBarContainer.GetChildren();

        if (compoundCompressed)
        {
            compoundsPanelCompressButton.Pressed = true;
            compoundsPanelBarContainer.AddConstantOverride("vseparation", 20);
            compoundsPanelBarContainer.AddConstantOverride("hseparation", 14);

            if (bars.Count < 4)
            {
                compoundsPanelBarContainer.Columns = 2;
            }
            else
            {
                compoundsPanelBarContainer.Columns = 3;
            }

            foreach (ProgressBar bar in bars)
            {
                panelsTween?.InterpolateProperty(bar, "rect_min_size:x", 90, 64, 0.3f);
                panelsTween?.Start();

                bar.GetNode<Label>("Label").Hide();
            }
        }

        if (!compoundCompressed)
        {
            compoundsPanelExpandButton.Pressed = true;
            compoundsPanelBarContainer.Columns = 1;
            compoundsPanelBarContainer.AddConstantOverride("vseparation", 5);
            compoundsPanelBarContainer.AddConstantOverride("hseparation", 0);

            foreach (ProgressBar bar in bars)
            {
                panelsTween?.InterpolateProperty(bar, "rect_min_size:x", bar.RectMinSize.x, 220, 0.3f);
                panelsTween?.Start();

                bar.GetNode<Label>("Label").Show();
            }
        }
    }

    protected void UpdateHealth(float delta)
    {
        // https://github.com/Revolutionary-Games/Thrive/issues/1976
        if (delta <= 0)
            return;

        var hp = 0.0f;

        // Update to the player's current HP, unless the player does not exist
        if (stage!.HasPlayer)
            ReadPlayerHitpoints(out hp, out maxHP);

        healthBar.MaxValue = maxHP;
        GUICommon.SmoothlyUpdateBar(healthBar, hp, delta);
        hpLabel.Text = StringUtils.FormatNumber(Mathf.Round(hp)) + " / " + StringUtils.FormatNumber(maxHP);
    }

    protected abstract void ReadPlayerHitpoints(out float hp, out float maxHP);

    protected void SetEditorButtonFlashEffect(bool enabled)
    {
        editorButtonFlash.Visible = enabled;
    }

    protected void UpdatePopulation()
    {
        populationLabel.Text = stage!.GameWorld.PlayerSpecies.Population.FormatNumber();
    }

    /// <summary>
    ///   Received for button that opens the menu inside the Microbe Stage.
    /// </summary>
    private void OpenMicrobeStageMenuPressed()
    {
        GUICommon.Instance.PlayButtonPressSound();
        menu.Open();
    }

    private void CompoundButtonPressed()
    {
        GUICommon.Instance.PlayButtonPressSound();

        if (!leftPanelsActive)
        {
            leftPanelsActive = true;
            animationPlayer.Play("HideLeftPanels");
        }
        else
        {
            leftPanelsActive = false;
            animationPlayer.Play("ShowLeftPanels");
        }
    }

    private void OnEnvironmentPanelSizeButtonPressed(string mode)
    {
        if (mode == "compress")
        {
            EnvironmentPanelCompressed = true;
        }
        else if (mode == "expand")
        {
            EnvironmentPanelCompressed = false;
        }
    }

    private void OnCompoundsPanelSizeButtonPressed(string mode)
    {
        if (mode == "compress")
        {
            CompoundsPanelCompressed = true;
        }
        else if (mode == "expand")
        {
            CompoundsPanelCompressed = false;
        }
    }

    private void HelpButtonPressed()
    {
        GUICommon.Instance.PlayButtonPressSound();
        menu.OpenToHelp();
    }

    private void OnEditorButtonMouseEnter()
    {
        if (editorButton.Disabled)
            return;

        editorButton.GetNode<TextureRect>("Highlight").Hide();
        editorButton.GetNode<AnimationPlayer>("AnimationPlayer").Stop();
    }

    private void OnEditorButtonMouseExit()
    {
        if (editorButton.Disabled)
            return;

        editorButton.GetNode<TextureRect>("Highlight").Show();
        editorButton.GetNode<AnimationPlayer>("AnimationPlayer").Play();
    }

    public void OnEnterStageTransition(bool longerDuration)
    {
        if (stage == null)
            throw new InvalidOperationException("Stage not setup for HUD");

        // Fade out for that smooth satisfying transition
        stage.TransitionFinished = false;
        TransitionManager.Instance.AddScreenFade(ScreenFade.FadeType.FadeIn, longerDuration ? 1.0f : 0.5f);
        TransitionManager.Instance.StartTransitions(stage, nameof(MicrobeStage.OnFinishTransitioning));
    }

    public override void _Notification(int what)
    {
        if (what == NotificationTranslationChanged)
        {
            foreach (var hoveredCompoundControl in hoveredCompoundControls)
            {
                hoveredCompoundControl.Value.UpdateTranslation();
            }
        }
    }

    public void OnSuicide()
    {
        stage?.OnSuicide();
    }

    public void UpdatePatchInfo(string patchName)
    {
        patchLabel.Text = patchName;
    }

    public void PopupPatchInfo()
    {
        patchOverlayAnimator.Play("FadeInOut");
    }

    public void EditorButtonPressed()
    {
        GD.Print("Move to editor pressed");

        // TODO: find out when this can happen (this happened when a really laggy save was loaded and the editor button
        // was pressed before the stage fade in fully completed)
        if (stage?.HasPlayer != true)
        {
            GD.PrintErr("Trying to press editor button while having no player object");
            return;
        }

        // To prevent being clicked twice
        editorButton.Disabled = true;

        EnsureGameIsUnpausedForEditor();

        TransitionManager.Instance.AddScreenFade(ScreenFade.FadeType.FadeOut, 0.3f, false);
        TransitionManager.Instance.StartTransitions(stage, nameof(MicrobeStage.MoveToEditor));

        stage.MovingToEditor = true;

        // TODO: mitigation for https://github.com/Revolutionary-Games/Thrive/issues/3006 remove once solved
        // Start auto-evo if not started already to make sure it doesn't start after we are in the editor
        // scene, this is a potential mitigation for the issue linked above
        if (!Settings.Instance.RunAutoEvoDuringGamePlay)
        {
            GD.Print("Starting auto-evo while fading into the editor as mitigation for issue #3006");
            stage.GameWorld.IsAutoEvoFinished(true);
        }
    }

    public void ShowExtinctionBox()
    {
        if (extinctionBox != null)
            return;

        winExtinctBoxHolder.Show();

        extinctionBox = ExtinctionBoxScene.Instance<CustomDialog>();
        winExtinctBoxHolder.AddChild(extinctionBox);
        extinctionBox.Show();
    }

    public void UpdateEnvironmentalBars(BiomeConditions biome)
    {
        var oxygenPercentage = biome.Compounds[oxygen].Dissolved * 100;
        var co2Percentage = biome.Compounds[carbondioxide].Dissolved * 100;
        var nitrogenPercentage = biome.Compounds[nitrogen].Dissolved * 100;
        var sunlightPercentage = biome.Compounds[sunlight].Dissolved * 100;
        var averageTemperature = biome.AverageTemperature;

        var percentageFormat = TranslationServer.Translate("PERCENTAGE_VALUE");

        oxygenBar.MaxValue = 100;
        oxygenBar.Value = oxygenPercentage;
        oxygenBar.GetNode<Label>("Value").Text =
            string.Format(CultureInfo.CurrentCulture, percentageFormat, oxygenPercentage);

        co2Bar.MaxValue = 100;
        co2Bar.Value = co2Percentage;
        co2Bar.GetNode<Label>("Value").Text =
            string.Format(CultureInfo.CurrentCulture, percentageFormat, co2Percentage);

        nitrogenBar.MaxValue = 100;
        nitrogenBar.Value = nitrogenPercentage;
        nitrogenBar.GetNode<Label>("Value").Text =
            string.Format(CultureInfo.CurrentCulture, percentageFormat, nitrogenPercentage);

        sunlightLabel.GetNode<Label>("Value").Text =
            string.Format(CultureInfo.CurrentCulture, percentageFormat, sunlightPercentage);
        temperature.GetNode<Label>("Value").Text = averageTemperature + " °C";

        // TODO: pressure?
    }

    /// <summary>
    ///   Updates the GUI bars to show only needed compounds
    /// </summary>
    protected void UpdateNeededBars()
    {
        // TODO: compounds that are useful for a colony but no the player cell should be shown
        var compounds = GetPlayerUsefulCompounds();

        if (compounds?.HasAnyBeenSetUseful() != true)
            return;

        if (compounds.IsSpecificallySetUseful(oxytoxy))
        {
            agentsPanel.Show();
        }
        else
        {
            agentsPanel.Hide();
        }

        foreach (ProgressBar bar in compoundBars)
        {
            var compound = SimulationParameters.Instance.GetCompound(bar.Name);

            if (compounds.IsUseful(compound))
            {
                bar.Show();
            }
            else
            {
                bar.Hide();
            }
        }
    }

    protected abstract CompoundBag? GetPlayerUsefulCompounds();

    protected Color GetCompoundDensityCategoryColor(float amount)
    {
        return amount switch
        {
            >= Constants.COMPOUND_DENSITY_CATEGORY_AN_ABUNDANCE => new Color(0.282f, 0.788f, 0.011f),
            >= Constants.COMPOUND_DENSITY_CATEGORY_QUITE_A_BIT => new Color(0.011f, 0.768f, 0.466f),
            >= Constants.COMPOUND_DENSITY_CATEGORY_FAIR_AMOUNT => new Color(0.011f, 0.768f, 0.717f),
            >= Constants.COMPOUND_DENSITY_CATEGORY_SOME => new Color(0.011f, 0.705f, 0.768f),
            >= Constants.COMPOUND_DENSITY_CATEGORY_LITTLE => new Color(0.011f, 0.552f, 0.768f),
            >= Constants.COMPOUND_DENSITY_CATEGORY_VERY_LITTLE => new Color(0.011f, 0.290f, 0.768f),
            _ => new Color(1f, 1f, 1f),
        };
    }

    protected string? GetCompoundDensityCategory(float amount)
    {
        return amount switch
        {
            >= Constants.COMPOUND_DENSITY_CATEGORY_AN_ABUNDANCE =>
                TranslationServer.Translate("CATEGORY_AN_ABUNDANCE"),
            >= Constants.COMPOUND_DENSITY_CATEGORY_QUITE_A_BIT =>
                TranslationServer.Translate("CATEGORY_QUITE_A_BIT"),
            >= Constants.COMPOUND_DENSITY_CATEGORY_FAIR_AMOUNT =>
                TranslationServer.Translate("CATEGORY_A_FAIR_AMOUNT"),
            >= Constants.COMPOUND_DENSITY_CATEGORY_SOME =>
                TranslationServer.Translate("CATEGORY_SOME"),
            >= Constants.COMPOUND_DENSITY_CATEGORY_LITTLE =>
                TranslationServer.Translate("CATEGORY_LITTLE"),
            >= Constants.COMPOUND_DENSITY_CATEGORY_VERY_LITTLE =>
                TranslationServer.Translate("CATEGORY_VERY_LITTLE"),
            _ => null,
        };
    }

    /// <summary>
    ///   Updates the compound bars with the correct values.
    /// </summary>
    protected void UpdateCompoundBars()
    {
        var compounds = GetPlayerColonyOrPlayerStorage();

        glucoseBar.MaxValue = compounds.GetCapacityForCompound(glucose);
        glucoseBar.Value = compounds.GetCompoundAmount(glucose);
        glucoseBar.GetNode<Label>("Value").Text = glucoseBar.Value + " / " + glucoseBar.MaxValue;

        ammoniaBar.MaxValue = compounds.GetCapacityForCompound(ammonia);
        ammoniaBar.Value = compounds.GetCompoundAmount(ammonia);
        ammoniaBar.GetNode<Label>("Value").Text = ammoniaBar.Value + " / " + ammoniaBar.MaxValue;

        phosphateBar.MaxValue = compounds.GetCapacityForCompound(phosphates);
        phosphateBar.Value = compounds.GetCompoundAmount(phosphates);
        phosphateBar.GetNode<Label>("Value").Text = phosphateBar.Value + " / " + phosphateBar.MaxValue;

        hydrogenSulfideBar.MaxValue = compounds.GetCapacityForCompound(hydrogensulfide);
        hydrogenSulfideBar.Value = compounds.GetCompoundAmount(hydrogensulfide);
        hydrogenSulfideBar.GetNode<Label>("Value").Text = hydrogenSulfideBar.Value + " / " +
            hydrogenSulfideBar.MaxValue;

        ironBar.MaxValue = compounds.GetCapacityForCompound(iron);
        ironBar.Value = compounds.GetCompoundAmount(iron);
        ironBar.GetNode<Label>("Value").Text = ironBar.Value + " / " + ironBar.MaxValue;

        oxytoxyBar.MaxValue = compounds.GetCapacityForCompound(oxytoxy);
        oxytoxyBar.Value = compounds.GetCompoundAmount(oxytoxy);
        oxytoxyBar.GetNode<Label>("Value").Text = oxytoxyBar.Value + " / " + oxytoxyBar.MaxValue;
    }

    protected void UpdateReproductionProgress()
    {
        CalculatePlayerReproductionProgress(
            out Dictionary<Compound, float> gatheredCompounds,
            out Dictionary<Compound, float> totalNeededCompounds);

        float fractionOfAmmonia = 0;
        float fractionOfPhosphates = 0;

        try
        {
            fractionOfAmmonia = gatheredCompounds[ammonia] / totalNeededCompounds[ammonia];
        }
        catch (Exception e)
        {
            GD.PrintErr("can't get reproduction ammonia progress: ", e);
        }

        try
        {
            fractionOfPhosphates = gatheredCompounds[phosphates] / totalNeededCompounds[phosphates];
        }
        catch (Exception e)
        {
            GD.PrintErr("can't get reproduction phosphates progress: ", e);
        }

        ammoniaReproductionBar.Value = fractionOfAmmonia * ammoniaReproductionBar.MaxValue;
        phosphateReproductionBar.Value = fractionOfPhosphates * phosphateReproductionBar.MaxValue;

        CheckAmmoniaProgressHighlight(fractionOfAmmonia);
        CheckPhosphateProgressHighlight(fractionOfPhosphates);
    }

    protected abstract void CalculatePlayerReproductionProgress(out Dictionary<Compound, float> gatheredCompounds,
        out Dictionary<Compound, float> totalNeededCompounds);

    private void CheckAmmoniaProgressHighlight(float fractionOfAmmonia)
    {
        if (fractionOfAmmonia < 1.0f)
            return;

        ammoniaReproductionBar.TintProgress = new Color(1, 1, 1, 1);
        editorButton.GetNode<TextureRect>("ReproductionBar/AmmoniaIcon").Texture = AmmoniaBW;
    }

    private void CheckPhosphateProgressHighlight(float fractionOfPhosphates)
    {
        if (fractionOfPhosphates < 1.0f)
            return;

        phosphateReproductionBar.TintProgress = new Color(1, 1, 1, 1);
        editorButton.GetNode<TextureRect>("ReproductionBar/PhosphateIcon").Texture = PhosphatesBW;
    }

    protected void UpdateATP(float delta)
    {
        // https://github.com/Revolutionary-Games/Thrive/issues/1976
        if (delta <= 0)
            return;

        var atpAmount = 0.0f;

        // Update to the player's current ATP, unless the player does not exist
        if (stage!.HasPlayer)
        {
            var compounds = GetPlayerColonyOrPlayerStorage();

            atpAmount = compounds.GetCompoundAmount(atp);
            maxATP = compounds.GetCapacityForCompound(atp);
        }

        atpBar.MaxValue = maxATP * 10.0f;

        // If the current ATP is close to full, just pretend that it is to keep the bar from flickering
        if (maxATP - atpAmount < Math.Max(maxATP / 20.0f, 0.1f))
        {
            atpAmount = maxATP;
        }

        GUICommon.SmoothlyUpdateBar(atpBar, atpAmount * 10.0f, delta);
        atpLabel.Text = atpAmount.ToString("F1", CultureInfo.CurrentCulture) + " / "
            + maxATP.ToString("F1", CultureInfo.CurrentCulture);
    }

    protected abstract ICompoundStorage GetPlayerColonyOrPlayerStorage();

    protected void UpdateProcessPanel()
    {
        if (!processPanel.Visible)
            return;

        processPanel.ShownData = stage is { HasPlayer: true } ? GetPlayerProcessStatistics() : null;
    }

    protected abstract ProcessStatistics? GetPlayerProcessStatistics();

    protected void UpdatePanelSizing(float delta)
    {
        // https://github.com/Revolutionary-Games/Thrive/issues/1976
        if (delta <= 0)
            return;

        var environmentPanelVBoxContainer = environmentPanel.GetNode<VBoxContainer>("VBoxContainer");
        var compoundsPanelVBoxContainer = compoundsPanel.GetNode<VBoxContainer>("VBoxContainer");

        environmentPanelVBoxContainer.RectSize = new Vector2(environmentPanelVBoxContainer.RectMinSize.x, 0);
        compoundsPanelVBoxContainer.RectSize = new Vector2(compoundsPanelVBoxContainer.RectMinSize.x, 0);

        // Multiply interpolation value with delta time to make it not be affected by framerate
        var environmentPanelSize = environmentPanel.RectMinSize.LinearInterpolate(
            new Vector2(environmentPanel.RectMinSize.x, environmentPanelVBoxContainer.RectSize.y), 5 * delta);

        var compoundsPanelSize = compoundsPanel.RectMinSize.LinearInterpolate(
            new Vector2(compoundsPanel.RectMinSize.x, compoundsPanelVBoxContainer.RectSize.y), 5 * delta);

        environmentPanel.RectMinSize = environmentPanelSize;
        compoundsPanel.RectMinSize = compoundsPanelSize;
    }

    /// <summary>
    ///   Updates the mouse hover indicator / player look at box with stuff.
    /// </summary>
    protected virtual void UpdateHoverInfo(float delta)
    {
        hoverInfoTimeElapsed += delta;

        if (hoverInfoTimeElapsed < Constants.HOVER_PANEL_UPDATE_INTERVAL)
            return;

        hoverInfoTimeElapsed = 0;

        // Refresh cells list
        hoveredCellsContainer.FreeChildren();

        var container = mouseHoverPanel.GetNode("PanelContainer/MarginContainer/VBoxContainer");
        var mousePosLabel = container.GetNode<Label>("MousePos");
        var nothingHere = container.GetNode<MarginContainer>("NothingHere");

        if (showMouseCoordinates)
        {
            mousePosLabel.Text = GetMouseHoverCoordinateText() + "\n";
        }

        var hoveredCompounds = GetHoveredCompounds();

        // Show hovered compound information in GUI
        bool anyCompoundVisible = false;
        foreach (var compound in hoveredCompoundControls)
        {
            var compoundControl = compound.Value;
            hoveredCompounds.TryGetValue(compound.Key, out float amount);

            // It is not useful to show trace amounts of a compound, so those are skipped
            if (amount < Constants.COMPOUND_DENSITY_CATEGORY_VERY_LITTLE)
            {
                compoundControl.Visible = false;
                continue;
            }

            compoundControl.Category = GetCompoundDensityCategory(amount);
            compoundControl.CategoryColor = GetCompoundDensityCategoryColor(amount);
            compoundControl.Visible = true;
            anyCompoundVisible = true;
        }

        hoveredCompoundsContainer.GetParent<VBoxContainer>().Visible = anyCompoundVisible;

        // Show the species name and count of hovered cells
        hoveredSpeciesCounts.Clear();
        foreach (var (isPlayer, species) in GetHoveredSpecies())
        {
            if (isPlayer)
            {
                AddHoveredCellLabel(species.FormattedName +
                    " (" + TranslationServer.Translate("PLAYER_CELL") + ")");
                continue;
            }

            hoveredSpeciesCounts.TryGetValue(species, out int count);
            hoveredSpeciesCounts[species] = count + 1;
        }

        foreach (var hoveredSpeciesCount in hoveredSpeciesCounts)
        {
            if (hoveredSpeciesCount.Value > 1)
            {
                AddHoveredCellLabel(
                    string.Format(CultureInfo.CurrentCulture, TranslationServer.Translate("SPECIES_N_TIMES"),
                        hoveredSpeciesCount.Key.FormattedName, hoveredSpeciesCount.Value));
            }
            else
            {
                AddHoveredCellLabel(hoveredSpeciesCount.Key.FormattedName);
            }
        }

        hoveredCellsSeparator.Visible = hoveredCellsContainer.GetChildCount() > 0 &&
            anyCompoundVisible;

        hoveredCellsContainer.GetParent<VBoxContainer>().Visible = hoveredCellsContainer.GetChildCount() > 0;

        nothingHere.Visible = hoveredCellsContainer.GetChildCount() == 0 && !anyCompoundVisible;
    }

    protected abstract IEnumerable<(bool Player, Species Species)> GetHoveredSpecies();
    protected abstract IReadOnlyDictionary<Compound, float> GetHoveredCompounds();

    protected abstract string GetMouseHoverCoordinateText();

    protected void AddHoveredCellLabel(string cellInfo)
    {
        hoveredCellsContainer.AddChild(new Label
        {
            Valign = Label.VAlign.Center,
            Text = cellInfo,
        });
    }

    protected abstract void UpdateAbilitiesHotBar();

    protected void UpdateBaseAbilitiesBar(bool showEngulf, bool showToxin, bool showingSignaling, bool engulfOn)
    {
        engulfHotkey.Visible = showEngulf;
        fireToxinHotkey.Visible = showToxin;
        signallingAgentsHotkey.Visible = showingSignaling;

        engulfHotkey.Pressed = engulfOn;
        fireToxinHotkey.Pressed = Input.IsActionPressed(fireToxinHotkey.ActionName);
        signallingAgentsHotkey.Pressed = Input.IsActionPressed(signallingAgentsHotkey.ActionName);
    }

    private void UpdatePausePrompt()
    {
        pauseInfo.ExtendedBbcode = TranslationServer.Translate("PAUSE_PROMPT");
    }
}
