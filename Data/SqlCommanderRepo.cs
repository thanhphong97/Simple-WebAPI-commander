using System;
using System.Collections.Generic;
using System.Linq;
using Commander.Models;

namespace Commander.Data
{
    public class SqlCommanderRepo : ICommanderRepo
    {
        private readonly CommanderContext _context;

        public SqlCommanderRepo(CommanderContext context)
        {
            _context = context;
        }

        public void CreateCommand(Command cmd)
        {
            if (cmd == null)
            {
                throw new ArgumentNullException(nameof(cmd));
            }

            _context.Commands.Add(cmd);
            _context.SaveChanges();
        }

        public void DeleteCommand(Command cmd)
        {
            if (cmd == null)
            {
                throw new ArgumentNullException(nameof(cmd));
            }
            _context.Commands.Remove(cmd);
        }

        public IEnumerable<Command> GetAllCommand()
        {
            var commandsList = _context.Commands.ToList<Command>();

            return commandsList;
        }

        public Command GetCommandById(int id)
        {
            var command = _context.Commands.FirstOrDefault(cmd => cmd.Id == id);

            return command;
        }

        public void UpdateCommand(Command cmd)
        {
            if (cmd != null)
            {
                // _context.Commands.Update(cmd);
                _context.SaveChanges();
            }
        }
    }
}