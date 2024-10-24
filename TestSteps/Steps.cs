using Assets.UiTest.Context;

namespace Assets.UiTest.TestSteps
{
    public class Steps
    {
        public Steps(IUiTestContext context)
        {
            UiTestStepBase.SetContext(context);
        }

        public IUiTestStepBase WaitStartLoadingStep()
        {
            return new WaitStartLoadingStep();
        }

        public IUiTestStepBase ExampleStep()
        {
            return new ExampleStep();
        }

        public IUiTestStepBase FindAndCutAllTreesStep()
        {
            return new FindAndCutAllTreesStep();
        }

        public IUiTestStepBase RemoveAxesFromInventoryStep()
        {
            return new RemoveAxesFromInventoryStep();
        }

        public IUiTestStepBase MoveAcrossBoundariesAndCheckPlayerPostitionStep()
        {
            return new MoveAcrossBoundariesAndCheckPlayerPostitionStep();
        }

        public IUiTestStepBase GetItemsAndMoveToAllBackpackCells()
        {
            return new GetItemsAndMoveToAllBackpackCellsStep();
        }

        public IUiTestStepBase CheckWoodCountInStacksStep()
        {
            return new CheckWoodCountInStacksStep();
        }

        public IUiTestStepBase CheckAxeLifecycleBasedOnTreeHitsStep()
        {
            return new CheckAxeLifecycleBasedOnTreeHitsStep();
        }

        public IUiTestStepBase BasicProgressProcessStep()
        {
            return new BasicProgressProcessStep();
        }

        public IUiTestStepBase ProgressWithSkipStep()
        {
            return new ProgressWithSkipStep();
        }

        public IUiTestStepBase FillInventoryAndStackStep()
        {
            return new FillInventoryAndStackStep();
        }
    }
}