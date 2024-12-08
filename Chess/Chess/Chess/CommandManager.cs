using System.Collections.Generic;

namespace Chess
{
    public class CommandManager
    {
        private readonly Stack<ICommand> _executedCommands = new Stack<ICommand>();
        private readonly Stack<ICommand> _undoneCommands = new Stack<ICommand>();

        public void ExecuteCommand(ICommand command)
        {
            command.Execute();
            _executedCommands.Push(command);
            _undoneCommands.Clear();
        }

        public void Undo()
        {
            if (_executedCommands.Count > 0)
            {
                var command = _executedCommands.Pop();
                command.Undo();
                _undoneCommands.Push(command);
            }
        }

        public void Redo()
        {
            if (_undoneCommands.Count > 0)
            {
                var command = _undoneCommands.Pop();
                command.Execute();
                _executedCommands.Push(command);
            }
        }

        public void Replay()
        {
            foreach (var command in _executedCommands)
            {
                command.Execute();
                System.Threading.Thread.Sleep(500); // Pause for 500ms between moves
            }
        }
    }
}
