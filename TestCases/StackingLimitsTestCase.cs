using System.Collections.Generic;
using Assets.UiTest.TestSteps;

namespace Assets.UiTest.Runner;

public class StackingLimitsTestCase : UiStepsTestCase
{
    protected override IEnumerator<IUiTestStepBase> Condition()
    {
        yield return Steps.WaitStartLoadingStep();
        yield return Steps.FillInventoryAndStackStep();
    }
}