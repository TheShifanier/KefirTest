using System.Collections;
using System.Collections.Generic;
using Assets.UiTest.Context.Consts;
using Assets.UiTest.Results;
using UiTest.UiTest.Checker;

namespace Assets.UiTest.TestSteps
{
    public class ExampleStep : UiTestStepBase
    {
        public override string Id => "example";
        public override double TimeOut => 300;

        protected override Dictionary<string, string> GetArgs()
        {
            return new Dictionary<string, string>();
        }

        protected override IEnumerator OnRun()
        {
            yield return GoToFirstTree();
            CheckIsUseButtonActive();

            yield return CutTree();
            yield return CheckWoodCountAtInventory();

            yield return DragAndDropExample();
            yield return Commands.FindAndGoToSingleObjectCommand(Locations.Home.WorkbenchSawmill, new ResultData<PlayerMoveResult>());
        }

        private void CheckIsUseButtonActive()
        {
            var useButtonIsActive = new UseButtonIsActiveChecker(Context);
            if (!useButtonIsActive.Check())
            {
                Fail("Use button is not active");
            }
        }

        private IEnumerator GoToFirstTree()
        {
            var trees = Cheats.FindTree();
            yield return Commands.PlayerMoveCommand(trees[0].transform.position, new ResultData<PlayerMoveResult>());
            yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
        }

        private IEnumerator CutTree()
        {
            for (int i = 0; i < 3; i++)
            {
                yield return Commands.UseButtonClickCommand(Screens.Main.Button.Use, new ResultData<SimpleCommandResult>());
                yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
            }
        }

        private IEnumerator CheckWoodCountAtInventory()
        {
            yield return Commands.UseButtonClickCommand(Screens.Main.Button.Inventory, new ResultData<SimpleCommandResult>());
            yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());

            var cellCountChecker = new CellCountChecker(Context, Screens.Inventory.Cell.Pockets, 4, 3);
            if (!cellCountChecker.Check())
            {
                Fail("Wrong wood count at pockets 5th cell");
            }

            yield return Commands.UseButtonClickCommand(Screens.Inventory.Button.Close, new ResultData<SimpleCommandResult>());
            yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
        }

        private IEnumerator DragAndDropExample()
        {
            yield return Commands.UseButtonClickCommand(Screens.Main.Button.Inventory, new ResultData<SimpleCommandResult>());
            yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
            // First 10 cells - Pockets, Cell 10-24 - Backpack 
            yield return Commands.DragAndDropCommand(Screens.Inventory.Cell.Pockets, 3, Screens.Inventory.Cell.Backpack, 10, new ResultData<SimpleCommandResult>());
            yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
            yield return Commands.UseButtonClickCommand(Screens.Inventory.Button.Close, new ResultData<SimpleCommandResult>());
            yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
        }
    }
}