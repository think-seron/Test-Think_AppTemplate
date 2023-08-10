using System;
using System.Runtime.InteropServices;
namespace Think_App.iOS
{
	public class DeviceModel
	{
		public const string HardwareProperty = "hw.machine";

		[DllImport("libc", CallingConvention = CallingConvention.Cdecl)]
		static internal extern int sysctlbyname([MarshalAs(UnmanagedType.LPStr)] string property,
			IntPtr output, IntPtr oldLen, IntPtr newp, uint newlen);

		public static string GetModelName()
		{
			var deviceVersion = string.Empty;

			var pLength = IntPtr.Zero;
			var pString = IntPtr.Zero;
			try
			{
				pLength = Marshal.AllocHGlobal(sizeof(int));
				sysctlbyname(DeviceModel.HardwareProperty, IntPtr.Zero, pLength, IntPtr.Zero, 0);
				var length = Marshal.ReadInt32(pLength);
				if (length <= 0)
				{
					return string.Empty;
				}

				pString = Marshal.AllocHGlobal(length);
				sysctlbyname(DeviceModel.HardwareProperty, pString, pLength, IntPtr.Zero, 0);

				deviceVersion = Marshal.PtrToStringAnsi(pString);
			}
			finally
			{
				if (pLength != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(pLength);
				}
				if (pString != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(pString);
				}
			}

			return deviceVersion;
		}

	}
}
