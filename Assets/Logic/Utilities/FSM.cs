using System;
using System.Reflection;
using System.Collections.Generic;


namespace Utilities
{
	public class FSM
	{
		// Source: Based on example from Unity Hacks
		public static Dictionary<int, Action> GetStateHandlers (object owner, Type enumType, string prefix = null, string postfix = null)
		// Returns a dictionary of delegates indexed by an enum. The delegates are created based on reflection of the owner object, the enum values and any given pre- or postfix
		{
			if (enumType == null || !enumType.IsEnum || Enum.GetUnderlyingType (enumType) != typeof (int))
			{
				throw new ArgumentException ("enumType must be an enum type with int as the underlying type");
			}

			if (owner == null)
			{
				throw new ArgumentException ("Invalid owner");
			}

			Dictionary<int, Action> handlers = new Dictionary<int, Action> ();

			MethodInfo[] methods = owner.GetType ().GetMethods (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Static);

			foreach (MethodInfo method in methods)
			// Consider all public, private and protected static and non-static methods
			{
				if (method.IsAbstract || method.ReturnType != typeof (void) || method.GetParameters ().Length > 0)
				// Ignore abstract methods or methods not matching the Action delegate type
				{
					continue;
				}

				if (prefix != null && method.Name.IndexOf (prefix) != 0)
				// If a prefix is required, but not used in this methods name, bypass it
				{
					continue;
				}

				if (postfix != null && method.Name.LastIndexOf (postfix) != method.Name.Length - postfix.Length)
				// If a postfix is required, but not used in this methods name, bypass it
				{
					continue;
				}

				// Match the method name minus any pre- and postfixes to the possible values of the passed enum //

				string handlerName = method.Name.Substring (0, postfix == null ? method.Name.Length : method.Name.Length - postfix.Length).Substring (prefix == null ? 0 : prefix.Length);
				object handlerEnum;

				try
				{
					handlerEnum = Enum.Parse (enumType, handlerName);
				}
				catch (Exception)
				{
					continue;
				}

				handlers[(int)handlerEnum] = (Action)Delegate.CreateDelegate (typeof (Action), owner, method);
					// Add the new delegate
			}

			return handlers;
		}
	}
}
