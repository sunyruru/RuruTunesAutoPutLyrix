namespace TagGuru
{
    using Microsoft.Win32;
    using System;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Windows.Forms;

    public class Savior
    {
        public static bool IsHexadecimal(string input)
        {
            foreach (char ch in input)
            {
                if (!Uri.IsHexDigit(ch))
                {
                    return false;
                }
            }
            return true;
        }

        public static void Read(object settings)
        {
            RegistryKey userAppDataRegistry = Application.UserAppDataRegistry;
            Read(settings, userAppDataRegistry);
        }

        public static void Read(object settings, RegistryKey Key)
        {
            foreach (FieldInfo info in settings.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                object obj2;
                RegistryKey key;
                int num7;
                int[] numArray;
                int num9;
                float[] numArray2;
                int num11;
                double[] numArray3;
                int num13;
                bool[] flagArray;
                string str2;
                System.Drawing.Color color;
                if (info.FieldType.IsEnum)
                {
                    obj2 = Key.GetValue(info.Name);
                    if (obj2 != null)
                    {
                        info.SetValue(settings, Enum.Parse(info.FieldType, (string) obj2));
                    }
                    continue;
                }
                switch (info.FieldType.Name.ToLower())
                {
                    case "string":
                    {
                        obj2 = Key.GetValue(info.Name);
                        if (obj2 != null)
                        {
                            info.SetValue(settings, (string) obj2);
                        }
                        continue;
                    }
                    case "boolean":
                    {
                        obj2 = Key.GetValue(info.Name);
                        if (obj2 != null)
                        {
                            info.SetValue(settings, bool.Parse((string) obj2));
                        }
                        continue;
                    }
                    case "int32":
                    {
                        obj2 = Key.GetValue(info.Name);
                        if (obj2 != null)
                        {
                            info.SetValue(settings, (int) obj2);
                        }
                        continue;
                    }
                    case "decimal":
                    {
                        obj2 = Key.GetValue(info.Name);
                        if (obj2 != null)
                        {
                            int num6 = (int) obj2;
                            info.SetValue(settings, (decimal) num6);
                        }
                        continue;
                    }
                    case "single":
                    {
                        obj2 = Key.GetValue(info.Name);
                        if (obj2 != null)
                        {
                            info.SetValue(settings, float.Parse((string) obj2));
                        }
                        continue;
                    }
                    case "double":
                    {
                        obj2 = Key.GetValue(info.Name);
                        if (obj2 != null)
                        {
                            info.SetValue(settings, double.Parse((string) obj2));
                        }
                        continue;
                    }
                    case "point":
                    {
                        obj2 = Key.GetValue(info.Name + ".X");
                        if (obj2 != null)
                        {
                            int x = (int) obj2;
                            obj2 = Key.GetValue(info.Name + ".Y");
                            if (obj2 != null)
                            {
                                int y = (int) obj2;
                                info.SetValue(settings, new Point(x, y));
                            }
                        }
                        continue;
                    }
                    case "size":
                    {
                        obj2 = Key.GetValue(info.Name + ".Height");
                        if (obj2 != null)
                        {
                            int height = (int) obj2;
                            obj2 = Key.GetValue(info.Name + ".Width");
                            if (obj2 != null)
                            {
                                int width = (int) obj2;
                                info.SetValue(settings, new Size(width, height));
                            }
                        }
                        continue;
                    }
                    case "string[]":
                    {
                        obj2 = Key.GetValue(info.Name);
                        if (obj2 != null)
                        {
                            info.SetValue(settings, (string[]) obj2);
                        }
                        continue;
                    }
                    case "byte[]":
                    {
                        obj2 = Key.GetValue(info.Name);
                        if (obj2 != null)
                        {
                            info.SetValue(settings, (byte[]) obj2);
                        }
                        continue;
                    }
                    case "int32[]":
                        key = Key.OpenSubKey(info.Name);
                        if (key == null)
                        {
                            continue;
                        }
                        num7 = 0;
                        numArray = new int[key.ValueCount];
                        goto Label_042E;

                    case "single[]":
                        key = Key.OpenSubKey(info.Name);
                        if (key == null)
                        {
                            continue;
                        }
                        num9 = 0;
                        numArray2 = new float[key.ValueCount];
                        goto Label_048E;

                    case "double[]":
                        key = Key.OpenSubKey(info.Name);
                        if (key == null)
                        {
                            continue;
                        }
                        num11 = 0;
                        numArray3 = new double[key.ValueCount];
                        goto Label_04EE;

                    case "boolean[]":
                        key = Key.OpenSubKey(info.Name);
                        if (key == null)
                        {
                            continue;
                        }
                        num13 = 0;
                        flagArray = new bool[key.ValueCount];
                        goto Label_054F;

                    case "color":
                        obj2 = Key.GetValue(info.Name);
                        if (obj2 == null)
                        {
                            continue;
                        }
                        str2 = (string) obj2;
                        if (!IsHexadecimal(str2))
                        {
                            goto Label_05A8;
                        }
                        color = System.Drawing.Color.FromArgb(int.Parse(str2, NumberStyles.HexNumber));
                        goto Label_05B1;

                    case "timespan":
                    {
                        obj2 = Key.GetValue(info.Name);
                        if (obj2 != null)
                        {
                            info.SetValue(settings, TimeSpan.Parse((string) obj2));
                        }
                        continue;
                    }
                    case "datetime":
                    {
                        obj2 = Key.GetValue(info.Name);
                        if (obj2 != null)
                        {
                            info.SetValue(settings, DateTime.Parse((string) obj2));
                        }
                        continue;
                    }
                    case "font":
                    {
                        obj2 = Key.GetValue(info.Name + ".Name");
                        if (obj2 != null)
                        {
                            string familyName = (string) obj2;
                            obj2 = Key.GetValue(info.Name + ".Size");
                            if (obj2 != null)
                            {
                                float emSize = float.Parse((string) obj2);
                                obj2 = Key.GetValue(info.Name + ".Style");
                                if (obj2 != null)
                                {
                                    FontStyle style = (FontStyle) Enum.Parse(typeof(FontStyle), (string) obj2);
                                    info.SetValue(settings, new Font(familyName, emSize, style));
                                }
                            }
                        }
                        continue;
                    }
                    default:
                        goto Label_06B8;
                }
            Label_041E:
                numArray[num7++] = (int) obj2;
            Label_042E:
                if ((obj2 = key.GetValue(num7.ToString())) != null)
                {
                    goto Label_041E;
                }
                info.SetValue(settings, numArray);
                continue;
            Label_0479:
                numArray2[num9++] = float.Parse((string) obj2);
            Label_048E:
                if ((obj2 = key.GetValue(num9.ToString())) != null)
                {
                    goto Label_0479;
                }
                info.SetValue(settings, numArray2);
                continue;
            Label_04D9:
                numArray3[num11++] = double.Parse((string) obj2);
            Label_04EE:
                if ((obj2 = key.GetValue(num11.ToString())) != null)
                {
                    goto Label_04D9;
                }
                info.SetValue(settings, numArray3);
                continue;
            Label_0539:
                flagArray[num13] = bool.Parse((string) obj2);
                num13++;
            Label_054F:
                if ((obj2 = key.GetValue(num13.ToString())) != null)
                {
                    goto Label_0539;
                }
                info.SetValue(settings, flagArray);
                continue;
            Label_05A8:
                color = System.Drawing.Color.FromName(str2);
            Label_05B1:
                info.SetValue(settings, color);
                continue;
            Label_06B8:
                MessageBox.Show("This type has not been implemented: " + info.FieldType);
            }
        }

        public static void Read(object settings, string key)
        {
            RegistryKey key2 = Registry.CurrentUser.CreateSubKey(key);
            Read(settings, key2);
        }

        public static object ReadFromFile(string FileName)
        {
            try
            {
                IFormatter formatter = new BinaryFormatter();
                Stream serializationStream = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                object obj2 = formatter.Deserialize(serializationStream);
                serializationStream.Close();
                return obj2;
            }
            catch
            {
                MessageBox.Show("Error attempting to read the settings from a file\n\n" + FileName, "Expresso Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return null;
            }
        }

        public static void Save(object settings)
        {
            RegistryKey userAppDataRegistry = Application.UserAppDataRegistry;
            Save(settings, userAppDataRegistry);
        }

        public static void Save(object settings, RegistryKey Key)
        {
            foreach (FieldInfo info in settings.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                RegistryKey key;
                int[] numArray;
                int num2;
                bool[] flagArray;
                int num3;
                float[] numArray2;
                int num4;
                double[] numArray3;
                int num5;
                if (info.FieldType.IsEnum)
                {
                    Key.SetValue(info.Name, Enum.GetName(info.FieldType, info.GetValue(settings)));
                    continue;
                }
                switch (info.FieldType.Name.ToLower())
                {
                    case "string":
                    {
                        Key.SetValue(info.Name, (string) info.GetValue(settings));
                        continue;
                    }
                    case "boolean":
                    {
                        Key.SetValue(info.Name, (bool) info.GetValue(settings));
                        continue;
                    }
                    case "int32":
                    {
                        Key.SetValue(info.Name, (int) info.GetValue(settings));
                        continue;
                    }
                    case "decimal":
                    {
                        decimal num = (decimal) info.GetValue(settings);
                        Key.SetValue(info.Name, (int) num);
                        continue;
                    }
                    case "single":
                    {
                        Key.SetValue(info.Name, (float) info.GetValue(settings));
                        continue;
                    }
                    case "double":
                    {
                        Key.SetValue(info.Name, (double) info.GetValue(settings));
                        continue;
                    }
                    case "point":
                    {
                        Point point = (Point) info.GetValue(settings);
                        Key.SetValue(info.Name + ".X", point.X);
                        Key.SetValue(info.Name + ".Y", point.Y);
                        continue;
                    }
                    case "size":
                    {
                        Size size = (Size) info.GetValue(settings);
                        Key.SetValue(info.Name + ".Height", size.Height);
                        Key.SetValue(info.Name + ".Width", size.Width);
                        continue;
                    }
                    case "byte[]":
                    {
                        byte[] buffer = (byte[]) info.GetValue(settings);
                        if (buffer != null)
                        {
                            Key.SetValue(info.Name, buffer);
                        }
                        continue;
                    }
                    case "string[]":
                    {
                        string[] strArray = (string[]) info.GetValue(settings);
                        if (strArray != null)
                        {
                            Key.SetValue(info.Name, strArray);
                        }
                        continue;
                    }
                    case "int32[]":
                        numArray = (int[]) info.GetValue(settings);
                        if (numArray == null)
                        {
                            continue;
                        }
                        Key.DeleteSubKey(info.Name, false);
                        key = Key.CreateSubKey(info.Name);
                        num2 = 0;
                        goto Label_03E5;

                    case "boolean[]":
                        flagArray = (bool[]) info.GetValue(settings);
                        if (flagArray == null)
                        {
                            continue;
                        }
                        Key.DeleteSubKey(info.Name, false);
                        key = Key.CreateSubKey(info.Name);
                        num3 = 0;
                        goto Label_0443;

                    case "single[]":
                        numArray2 = (float[]) info.GetValue(settings);
                        if (numArray2 == null)
                        {
                            continue;
                        }
                        Key.DeleteSubKey(info.Name, false);
                        key = Key.CreateSubKey(info.Name);
                        num4 = 0;
                        goto Label_04A1;

                    case "double[]":
                        numArray3 = (double[]) info.GetValue(settings);
                        if (numArray3 == null)
                        {
                            continue;
                        }
                        Key.DeleteSubKey(info.Name, false);
                        key = Key.CreateSubKey(info.Name);
                        num5 = 0;
                        goto Label_04FF;

                    case "color":
                    {
                        System.Drawing.Color color = (System.Drawing.Color) info.GetValue(settings);
                        Key.SetValue(info.Name, color.Name);
                        continue;
                    }
                    case "timespan":
                    {
                        Key.SetValue(info.Name, ((TimeSpan) info.GetValue(settings)).ToString());
                        continue;
                    }
                    case "datetime":
                    {
                        Key.SetValue(info.Name, ((DateTime) info.GetValue(settings)).ToString());
                        continue;
                    }
                    case "font":
                    {
                        Key.SetValue(info.Name + ".Name", ((Font) info.GetValue(settings)).Name);
                        Key.SetValue(info.Name + ".Size", ((Font) info.GetValue(settings)).Size);
                        Key.SetValue(info.Name + ".Style", ((Font) info.GetValue(settings)).Style);
                        continue;
                    }
                    default:
                        goto Label_060B;
                }
            Label_03C8:
                key.SetValue(num2.ToString(), numArray[num2]);
                num2++;
            Label_03E5:
                if (num2 < numArray.Length)
                {
                    goto Label_03C8;
                }
                continue;
            Label_0426:
                key.SetValue(num3.ToString(), flagArray[num3]);
                num3++;
            Label_0443:
                if (num3 < flagArray.Length)
                {
                    goto Label_0426;
                }
                continue;
            Label_0484:
                key.SetValue(num4.ToString(), numArray2[num4]);
                num4++;
            Label_04A1:
                if (num4 < numArray2.Length)
                {
                    goto Label_0484;
                }
                continue;
            Label_04E2:
                key.SetValue(num5.ToString(), numArray3[num5]);
                num5++;
            Label_04FF:
                if (num5 < numArray3.Length)
                {
                    goto Label_04E2;
                }
                continue;
            Label_060B:
                MessageBox.Show("This Field type cannot be saved by the Savior class: " + info.FieldType);
            }
        }

        public static void Save(object settings, string key)
        {
            RegistryKey key2 = Registry.CurrentUser.CreateSubKey(key);
            Save(settings, key2);
        }

        public static bool SaveToFile(object settings, string FileName)
        {
            try
            {
                IFormatter formatter = new BinaryFormatter();
                Stream serializationStream = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.None);
                formatter.Serialize(serializationStream, settings);
                serializationStream.Close();
                return true;
            }
            catch
            {
                MessageBox.Show("Error attempting to save the settings to a file\n\n" + FileName, "Expresso Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return false;
            }
        }

        public static string ToString(object settings)
        {
            string str = "";
            foreach (FieldInfo info in settings.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                string str2 = "";
                if (info.GetValue(settings) != null)
                {
                    str2 = info.GetValue(settings).ToString();
                }
                object obj2 = str;
                str = string.Concat(new object[] { obj2, "Name: ", info.Name, " = ", str2, "\n    FieldType: ", info.FieldType, "\n\n" });
            }
            return str;
        }
    }
}

