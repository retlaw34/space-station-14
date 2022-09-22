﻿using Content.Client.UserInterface;
using Content.Client.UserInterface.Controls;
using Content.Shared.OuterRim.Generator;
using Robust.Client.AutoGenerated;
using Robust.Client.GameObjects;
using Robust.Client.UserInterface.XAML;

namespace Content.Client._00OuterRim;

[GenerateTypedNameReferences]
public sealed partial class GeneratorWindow : FancyWindow
{
    public GeneratorWindow(SolidFuelGeneratorBoundUserInterface bui, EntityUid vis)
    {
        RobustXamlLoader.Load(this);
        IoCManager.InjectDependencies(this);

        EntityView.Sprite = IoCManager.Resolve<IEntityManager>().GetComponent<SpriteComponent>(vis);
        TargetPower.ValueChanged += (_, args) =>
        {
            bui.SetTargetPower(args.Value);
        };
    }


    private SolidFuelGeneratorComponentBuiState? _lastState;

    public void Update(SolidFuelGeneratorComponentBuiState state)
    {
        // ReSharper disable once CompareOfFloatsByEqualityOperator
        if (_lastState?.TargetPower != state.TargetPower)
            TargetPower.OverrideValue((int)(state.TargetPower / 100.0f));
        Efficiency.Text = SharedGeneratorSystem.CalcFuelEfficiency(state.TargetPower, state.OptimalPower).ToString("P1");
        FuelFraction.Value = state.RemainingFuel - (int) state.RemainingFuel;
        FuelLeft.Text = ((int) MathF.Floor(state.RemainingFuel)).ToString();
        _lastState = state;
    }
}
