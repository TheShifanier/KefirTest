using System.Collections.Generic;
using Assets.UiTest.TestSteps;

namespace Assets.UiTest.Runner
{
    public class TestCase0 : UiStepsTestCase
    {
        protected override IEnumerator<IUiTestStepBase> Condition()
        {
            yield return Steps.WaitStartLoadingStep();
        }
    }
}