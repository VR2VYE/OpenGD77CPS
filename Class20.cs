using System;
using System.Reflection;

internal class Class20
{
	internal delegate void Delegate2(object o);

	internal static Module module_0;

	internal static void AtV3QywwthccF(int typemdt)
	{
		Type type = Class20.module_0.ResolveType(33554432 + typemdt);
		FieldInfo[] fields = type.GetFields();
		foreach (FieldInfo fieldInfo in fields)
		{
			MethodInfo method = (MethodInfo)Class20.module_0.ResolveMethod(fieldInfo.MetadataToken + 100663296);
			fieldInfo.SetValue(null, (MulticastDelegate)Delegate.CreateDelegate(type, method));
		}
	}

	public Class20()
	{
	}

	static Class20()
	{
		Class20.module_0 = typeof(Class20).Assembly.ManifestModule;
	}
}
