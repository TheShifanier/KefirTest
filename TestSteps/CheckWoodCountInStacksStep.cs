using System.Collections;
using System.Collections.Generic;
using Assets.UiTest.Context.Consts;
using Assets.UiTest.Results;
using UiTest.UiTest.Checker;

namespace Assets.UiTest.TestSteps
{
    public class CheckWoodCountInStacksStep : UiTestStepBase
    {
        public override string Id => "check_wood_count_in_stacks";
        public override double TimeOut => 300;

        protected override Dictionary<string, string> GetArgs()
        {
            return new Dictionary<string, string>();
        }

        protected override IEnumerator OnRun()
        {
            var treesOnScene = Cheats.FindTree();
            var initialWoodCount = 0;
            for (int i = 0; i < treesOnScene.Count; i++)
            {
                yield return Commands.PlayerMoveCommand(treesOnScene[i].transform.position, new ResultData<PlayerMoveResult>());
                yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
                yield return CutTree();
            }

            yield return Commands.UseButtonClickCommand(Screens.Main.Button.Inventory, new ResultData<SimpleCommandResult>());
            yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());

            CheckCountInWoodStacks();
        }

        private IEnumerator CutTree()
        {
            for (int i = 0; i < 3; i++)
            {
                yield return Commands.UseButtonClickCommand(Screens.Main.Button.Use, new ResultData<SimpleCommandResult>());
                yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
            }
        }

        private void CheckCountInWoodStacks()
        {
            var cellCountChecker = new CellCountChecker(Context, Screens.Inventory.Cell.Pockets, 4, 20);
            if (!cellCountChecker.Check())
            {
                Fail("Wrong wood count at pockets cell index: 4");
            }
            cellCountChecker = new CellCountChecker(Context, Screens.Inventory.Cell.Pockets, 0, 10);
            if (!cellCountChecker.Check())
            {
                Fail("Wrong wood count at pockets cell index: 0");
            }
        }
    }
}