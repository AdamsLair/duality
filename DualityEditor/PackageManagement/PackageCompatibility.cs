namespace Duality.Editor.PackageManagement
{
	public enum PackageCompatibility
	{
		Unknown,

		None,
		Unlikely,
		Likely,
		Definite
	}

	public static class ExtMethodsPackageCompatibility
	{
		public static PackageCompatibility Combine(this PackageCompatibility self, PackageCompatibility other)
		{
			if (self == PackageCompatibility.Unknown || other == PackageCompatibility.Unknown)
				return PackageCompatibility.Unknown;
			else
				return (PackageCompatibility)MathF.Min((int)self, (int)other);
		}
		public static bool IsAtLeast(this PackageCompatibility self, PackageCompatibility other)
		{
			return (int)self >= (int)other;
		}
	}
}
