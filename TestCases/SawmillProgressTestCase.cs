using System.Collections.Generic;
using Assets.UiTest.TestSteps;

namespace Assets.UiTest.Runner;

public class SawmillProgressTestCase : UiStepsTestCase
{
    protected override IEnumerator<IUiTestStepBase> Condition()
    {
        yield return Steps.WaitStartLoadingStep();
        yield return Steps.BasicProgressProcessStep();
    }
}