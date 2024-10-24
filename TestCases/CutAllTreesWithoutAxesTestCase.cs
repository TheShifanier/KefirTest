using System.Collections.Generic;
using Assets.UiTest.TestSteps;

namespace Assets.UiTest.Runner;

// In the TestTask's specification it doesn't mentioned is it possible to cut wood using hands without axe
public class CutAllTreesWithoutAxesTestCase : UiStepsTestCase
{
    protected override IEnumerator<IUiTestStepBase> Condition()
    {
        yield return Steps.WaitStartLoadingStep();
        yield return Steps.RemoveAxesFromInventoryStep();
        /* 
         * In this scenario it is possible to cut the trees with the same speed as with axe
         * Decided to leave it as is, however looks like bug and developer may decide to fix it 
         * In different ways according to specification for example it might be impossible to cut tree using empty hands 
         * or cut speed might be decreased
        */
        yield return Steps.FindAndCutAllTreesStep();
    }
}