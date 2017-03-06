using System;
namespace Driver.Interfaces
{
	public interface Display
	{

		byte[] makeScreenshot();

		int getScreenWidth();

		int getScreenHeight();
	}
}
