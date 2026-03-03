using System;
using System.Reflection;
using UnityEngine;
using Bastion.ConsoleV2;
using System.Collections.Generic;

namespace Bastion.ConsoleV2
{
    public static class Assembler
    {
        public static bool HasInitialized = false;
        public static Assembly[] assemblyArray = new Assembly[1];

        public static void Initialize()
        {
            if (HasInitialized)
            {
                return;
            }

            assemblyArray = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly a in assemblyArray)
            {
                foreach (Type t in a.GetTypes())
                {
                    foreach (MethodInfo m in t.GetMethods())
                    {
                        if (m.GetCustomAttributes(typeof(Command), false).Length > 0)
                        {
                            string commandName = "";

                            Command commandAttribute = m.GetCustomAttribute<Command>();

                            // check if we aren't in a dev build or the editor, and if the current command is a dev only command, return if so.
                            if (!Application.isEditor || !Debug.isDebugBuild)
                            {
                                if (commandAttribute.developmental)
                                    continue;
                            }

                            if (commandAttribute.callName == string.Empty)
                                commandName = t.Name + "." + m.Name;
                            else
                                commandName = commandAttribute.callName;

                            ParameterInfo[] parameters = m.GetParameters();

                            List<GenericCommandVariable> vars = new List<GenericCommandVariable>();

                            foreach (ParameterInfo p in parameters)
                            {
                                GenericCommandVariable var = new GenericCommandVariable(p);
                                vars.Add(var);
                            }

                            ShellCommandClass shellCommand = new ShellCommandClass(commandName, commandAttribute.helpDescription, commandAttribute.cheat, commandAttribute.developmental, vars.ToArray(), m);

                            Debug.Log(shellCommand.callName);

                            Shell.registeredCommands.Add(shellCommand);

                            Bastion.ConsoleV2.Console.CommandsRegistered++;
                        }
                    }
                }
            }

            if (Application.isEditor || Debug.isDebugBuild)
                Bastion.ConsoleV2.Console.devBuild = true;

            Bastion.ConsoleV2.Console.Log("Succsesfully built " + Bastion.ConsoleV2.Console.CommandsRegistered + " Command(s)", "Assembler");
            Bastion.ConsoleV2.Shell.PokeShell(Bastion.ConsoleV2.Console.CommandsRegistered);
            HasInitialized = true;
        }
    }
}
