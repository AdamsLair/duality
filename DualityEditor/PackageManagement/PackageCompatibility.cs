namespace Duality.Editor.PackageManagement
{
	public enum PackageCompatibility
	{
		None,
		Unlikely,
		Likely,
		Definite
	}

	public static class ExtMethodsPackageCompatibility
	{
		public static PackageCompatibility Combine(this PackageCompatibility self, PackageCompatibility other)
		{
			return (PackageCompatibility)MathF.Min((int)self, (int)other);
		}
		public static bool Satisfies(this PackageCompatibility self, PackageCompatibility other)
		{
			return (int)self >= (int)other;
		}
	}
}
