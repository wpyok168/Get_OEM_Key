# 获取系统OEM密钥

~~~
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace 密钥解密
{
    internal class Program
    {
        static void Main(string[] args)
        {
			//HKEY_LOCAL_MACHINE\SYSTEM\Setup\Source OS (Updated on 7/11/2022 08:44:42)
			//HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion
			RegistryKey regkey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(@"SYSTEM\Setup\Source OS (Updated on 7/11/2022 08:44:42)");
			byte[] DigitalProductId = regkey.GetValue("DigitalProductId") as byte[];
			byte[] DigitalProductId4 = regkey.GetValue("DigitalProductId4") as byte[];
			string numkey = DecodeProductKey(DigitalProductId);
			string unknownkey = DecodeProductKey(DigitalProductId4);

			RegistryKey regkey1 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
			byte[] DigitalProductId1 = regkey1.GetValue("DigitalProductId") as byte[];
			byte[] DigitalProductId41 = regkey.GetValue("DigitalProductId4") as byte[];
			string installkey1 = DecodeProductKey(DigitalProductId1);
			string unknownkey1 = DecodeProductKey(DigitalProductId41);
		}
		public static string DecodeProductKey(byte[] digitalProductId)
		{
			char[] array = new char[29];
			char[] array2 = new char[]
			{
				'B',
				'C',
				'D',
				'F',
				'G',
				'H',
				'J',
				'K',
				'M',
				'P',
				'Q',
				'R',
				'T',
				'V',
				'W',
				'X',
				'Y',
				'2',
				'3',
				'4',
				'6',
				'7',
				'8',
				'9'
			};
			string result;
			try
			{
				int num = digitalProductId[66] >> 3 & 1;
				digitalProductId[66] = (byte)((int)(digitalProductId[66] & 247) | (num & 2) << 2);
				ArrayList arrayList = new ArrayList();
				for (int i = 52; i <= 67; i++)
				{
					arrayList.Add(digitalProductId[i]);
				}
				for (int j = 28; j >= 0; j--)
				{
					if ((j + 1) % 6 == 0)
					{
						array[j] = '-';
					}
					else
					{
						int num2 = 0;
						for (int k = 14; k >= 0; k--)
						{
							int num3 = num2 << 8 | (int)((byte)arrayList[k]);
							arrayList[k] = (byte)(num3 / 24);
							num2 = num3 % 24;
							array[j] = array2[num2];
						}
					}
				}
				if (num != 0)
				{
					int num4 = 0;
					for (int l = 0; l < array.Length; l++)
					{
						if (array[0] == array2[l])
						{
							num4 = l;
							break;
						}
					}
					string text = new string(array);
					text = text.Replace("-", string.Empty).Remove(0, 1);
					text = text.Substring(0, num4) + "N" + text.Remove(0, num4);
					text = string.Concat(new string[]
					{
						text.Substring(0, 5),
						"-",
						text.Substring(5, 5),
						"-",
						text.Substring(10, 5),
						"-",
						text.Substring(15, 5),
						"-",
						text.Substring(20, 5)
					});
					array = text.ToCharArray();
				}
				result = new string(array);
			}
			catch (Exception ex)
			{
				string[] array3 = new string[6];
				array3[0] = "Error: DecodeProductKey";
				array3[1] = Environment.NewLine;
				array3[2] = ex.Message;
				array3[3] = Environment.NewLine;
				array3[4] = "Decodedchars:";
				int num5 = 5;
				char[] array4 = array;
				array3[num5] = ((array4 != null) ? array4.ToString() : null);
				//MessageBox.Show(string.Concat(array3), "Windows Key information:" + ex.Source, MessageBoxButton.OK, MessageBoxImage.Asterisk);
				result = "Unable to decode product key";
			}
			return result;
		}
	}
}

~~~
