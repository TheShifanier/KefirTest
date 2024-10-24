using System.Collections;
using System.Collections.Generic;
using Assets.UiTest.Context.Consts;
using Assets.UiTest.Results;

namespace Assets.UiTest.TestSteps;

public class WaitStartLoadingStep : UiTestStepBase
{
    public override string Id => "wait_start_loading";
    public override double TimeOut => 300;

    protected override Dictionary<string, string> GetArgs()
    {
        return new Dictionary<string, string>();
    }

    protected override IEnumerator OnRun()
    {
        yield return Commands.WaitDialogCommand(Screens.Start.Content.StartScreen, false, new ResultData<WaitItemResult>());
    }
}