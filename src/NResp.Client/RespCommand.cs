namespace NResp.Client
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class RespCommand
    {
        private static readonly string[] NoArguments = new string[0];

        public RespCommand(string name)
            : this(name, NoArguments)
        {
            this.Name = name;
        }

        public RespCommand(string name, IList<string> arguments)
        {
            this.Name = name;
            this.Arguments = new ReadOnlyCollection<string>(arguments);
        }

        public string Name { get; private set; }

        public IList<string> Arguments { get; private set; }
    }
}