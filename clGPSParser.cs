using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static TeltonikaGPS.clGPSParser;

namespace TeltonikaGPS
{
    public class clGPSParser
    {
        string IMEI = "350317174918627";
        public static ListBox ListBox;
        public static List<Byte> CRCbar = new List<Byte>();

        public static clGPS getDatensaetzeByNetworkStream(NetworkStream stream)
        {
            return getDatensaetzeByNetworkStream(stream, true, true, true);
        }

        public static clGPS getDatensaetzeByNetworkStream(NetworkStream stream, bool crcCheck, bool ackSenden, bool insertIntoDB)
        {
            try
            {
                clGPS clgps = new clGPS();
                clgps.Lby.Clear();

                // Die IMEI länge anfordern
                int IMEILength = getTwoByteByStream(stream, false, clgps);

                // Die IMEI auslesen
                byte[] bar = ReadBytes(stream, IMEILength, false, clgps, false);

                // Empfangene Byte[] in String
                string receivedIMEI = Encoding.Default.GetString(bar);

                clgps.IMEI = Convert.ToInt64(receivedIMEI);

                // Zum senden String in Byte[] 
                byte[] sbar = HexStringToBytes("01");

                // Server immer Ja antworten
                stream.Write(sbar, 0, sbar.Length);

                int ZeroBytes = getFourByteByStream(stream, false, clgps);

                int DataLength = getFourByteByStream(stream, false, clgps);

                int CodecID = getOneByteByStream(stream, true, clgps);

                int NumberOfData1 = getOneByteByStream(stream, true, clgps);
                AddToProtokoll("IMEI: " + clgps.IMEI.ToString());

                for (int n = 0; n < NumberOfData1; n++)
                {
                    clGPSDatensatz ds = new clGPSDatensatz();

                    // AVL Data - Timestamp, Priority, GPS Element, IO Element

                    // Timestamp
                    long TimeStamp = getEightByteByStream(stream, true, clgps);
                    DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);

                    ds.TimeStamp = origin.AddMilliseconds(Convert.ToDouble(TimeStamp));
                    AddToProtokoll("Timestamp: " + ds.TimeStamp.ToString());

                    // Priority
                    int Priority = getOneByteByStream(stream, true, clgps);

                    // GPS Element - Longitude, Latitude, Altitude,	Angle, Satellites, Speed
                    byte[] GPSElement = ReadBytes(stream, 15, true, clgps, false);

                    AddToProtokoll(GPSElement.Length.ToString() + "Bytes - GPS-Elemente werden gelesen");

                    // Longitude
                    string longt = string.Empty;
                    GPSElement.Take(4).ToList().ForEach(delegate (byte b) { longt += String.Format("{0:X2}", b); });
                    double longtitude = ((double)Convert.ToInt32(longt, 16)) / 10000000;
                    ds.Longitude = longtitude;
                    AddToProtokoll("Longitude: " + longtitude);

                    // Latitude
                    string lat = string.Empty;
                    GPSElement.Skip(4).Take(4).ToList().ForEach(delegate (byte b) { lat += String.Format("{0:X2}", b); });
                    double latitude = ((double)Convert.ToInt32(lat, 16)) / 10000000;
                    ds.Latitude = latitude;
                    AddToProtokoll("Latitude: " + latitude);

                    // Altitude
                    string alt = string.Empty;
                    GPSElement.Skip(8).Take(2).ToList().ForEach(delegate (byte b) { alt += String.Format("{0:X2}", b); });
                    int altitude = Convert.ToInt32(alt, 16);
                    ds.Altitude = altitude;
                    AddToProtokoll("Altitude: " + altitude);

                    // Angle
                    string ang = string.Empty;
                    GPSElement.Skip(10).Take(2).ToList().ForEach(delegate (byte b) { ang += String.Format("{0:X2}", b); });
                    int angle = Convert.ToInt32(ang, 16);
                    ds.Angle = angle;
                    AddToProtokoll("Angle: " + angle);

                    // Satellites
                    int satellites = Convert.ToInt32(GPSElement.Skip(12).Take(1).ToList()[0]);
                    ds.Satellites = satellites;
                    AddToProtokoll("Satellites: " + satellites);

                    // Speed
                    string sp = string.Empty;
                    GPSElement.Skip(13).Take(2).ToList().ForEach(delegate (byte b) { sp += String.Format("{0:X2}", b); });
                    int speed = Convert.ToInt32(sp, 16);
                    ds.SpeedKMH = speed;
                    AddToProtokoll("SpeedKMH: " + speed);


                    // Lese IO Element ID
                    long IOElementID = getOneByteByStream(stream, true, clgps);
                    int IOElementRecord = getOneByteByStream(stream, true, clgps);

                    if (IOElementRecord != 0)
                    {
                        // Wieviele IO Data Elemente gibt es mit 1 Byte?
                        var anzahl1Byte = getOneByteByStream(stream, true, clgps);

                        if (anzahl1Byte > 0)
                        {
                            for (var i = 0; i < anzahl1Byte; i++)
                            {
                                var clio = new clIODaten
                                {
                                    ID = getOneByteByStream(stream, true, clgps).ToString(),
                                    Value = getOneByteByStream(stream, true, clgps).ToString()
                                };
                                ds.IODaten.Add(clio);
                            }
                        }

                        // Wieviele IO Data Elemente gibt es mit 2 Byte?
                        var anzahl2Byte = getOneByteByStream(stream, true, clgps);

                        if (anzahl2Byte > 0)
                        {
                            for (var i = 0; i < anzahl2Byte; i++)
                            {
                                var clio = new clIODaten
                                {
                                    ID = getOneByteByStream(stream, true, clgps).ToString(),
                                    Value = getTwoByteByStream(stream, true, clgps).ToString()
                                };
                                ds.IODaten.Add(clio);
                            }
                        }

                        // Wieviele IO Data Elemente gibt es mit 4 Byte?
                        var anzahl4Byte = getOneByteByStream(stream, true, clgps);

                        if (anzahl4Byte > 0)
                        {
                            for (var i = 0; i < anzahl4Byte; i++)
                            {
                                var clio = new clIODaten
                                {
                                    ID = getOneByteByStream(stream, true, clgps).ToString(),
                                    Value = getFourByteByStream(stream, true, clgps).ToString()
                                };
                                ds.IODaten.Add(clio);
                            }
                        }

                        // Wieviele IO Data Elemente gibt es mit 8 Byte?
                        var anzahl8Byte = getOneByteByStream(stream, true, clgps);

                        if (anzahl8Byte > 0)
                        {
                            for (var i = 0; i < anzahl8Byte; i++)
                            {
                                var clio = new clIODaten
                                {
                                    ID = getOneByteByStream(stream, true, clgps).ToString(),
                                    Value = getEightByteByStream(stream, true, clgps).ToString()
                                };
                                ds.IODaten.Add(clio);
                            }
                        }
                    }

                    // Check IODaten for Events
                    foreach (var gp in ds.IODaten)
                    {
                        // Value = 0 – Ignition Off, 1 – Ignition On
                        if (gp.ID == "239")
                            ds.Zuendung = StringToBool(gp.Value);

                        if (gp.ID == "78")
                        {
                            ds.IButtonLong = gp.Value;
                            ds.IButtonHex = GetHexFromIButton(gp.Value);
                        }
                    }

                    clgps.Datensaetze.Add(ds);
                }

                // Number Of Data 2	- CHECK
                int NumberOfData2 = getOneByteByStream(stream, true, clgps);
                AddToProtokoll(NumberOfData2.ToString());

                //CRC for check of data correction and request again data from device if it not correct
                int calculatedCRC = GetCRC16(clgps.Lby.ToArray());

                int CRC = getFourByteByStream(stream, false, clgps);

                clgps.Lby.Clear();
                if (calculatedCRC == CRC)
                {
                    // Zum senden String in Byte[] 
                    sbar = new byte[] { 0x00, 0x00, 0x00, Convert.ToByte(NumberOfData1) };

                    // Server mit "Ja" antworten
                    stream.Write(sbar, 0, sbar.Length);
                    return clgps;
                }
                else
                {
                    // Zum senden String in Byte[] 
                    sbar = HexStringToBytes("00");

                    // Server mit "Nein" antworten
                    stream.Write(sbar, 0, sbar.Length);
                    return null;
                }
            }
            catch (Exception err)
            {
                AddToProtokoll(err.Message);
                return null;
            }
        }

        private static byte[] ReadBytes(NetworkStream ns, int Anzahl, bool ByteArrayHinzuefuegen, clGPS clgps, bool switchen = true)
        {
            // Es müssen diese Anzahl Bytes gelesen werden.
            var Result = new byte[Anzahl];
            var EsMussNochGelesenWerden = Anzahl;

            var BereitsGelesen = 0;
            var ResultPos = 0;

            while (true)
            {
                // max. 10 Sekunden Wartezeit
                if (isReaderReady(ns, 10000))
                {
                    // Die reinkommenden Daten können sowohl größer als auch kleiner der Anzahl sein.
                    var myReadBuffer = EsMussNochGelesenWerden < 1024 ? new byte[EsMussNochGelesenWerden] : new byte[1024];
                    var numberOfBytesRead = ns.Read(myReadBuffer, 0, myReadBuffer.Length);

                    // Copies the source Array to the target Array, starting at index 6.
                    if (numberOfBytesRead > 0)
                        Buffer.BlockCopy(myReadBuffer, 0, Result, ResultPos, numberOfBytesRead);

                    EsMussNochGelesenWerden -= numberOfBytesRead;
                    ResultPos += numberOfBytesRead;
                    BereitsGelesen += numberOfBytesRead;

                    if (BereitsGelesen >= Anzahl)
                        break;
                }
                else
                {
                    // Nach 10 Sekunden keine Antwort
                    return null;
                }
            }

            if (ByteArrayHinzuefuegen && clgps != null)
                clgps.Lby.AddRange(Result);

            if (switchen)
            {
                var nbar = new byte[Anzahl];
                var pos = Anzahl - 1;

                for (var i = 0; i < Anzahl; i++)
                {
                    nbar[pos] = Result[i];
                    pos--;
                }

                return nbar;
            }

            return Result;
        }

        public static Int16 getOneByteByStream(NetworkStream stream, bool ByteArrayHinzuefuegen, clGPS clgps)
        {
            //if (clgps != null)
            {
                byte[] bar = ReadBytes(stream, 1, ByteArrayHinzuefuegen, clgps, false);

                if (bar != null)
                    return bar[0];
            }

            return -1;
        }
        public static Int16 getTwoByteByStream(NetworkStream stream, bool ByteArrayHinzuefuegen, clGPS clgps)
        {
            //if (clgps != null)
            {
                byte[] bar = ReadBytes(stream, 2, ByteArrayHinzuefuegen, clgps);

                if (bar != null)
                    return BitConverter.ToInt16(bar, 0);
            }

            return -1;
        }

        public static Int32 getFourByteByStream(NetworkStream stream, bool ByteArrayHinzuefuegen, clGPS clgps)
        {
            //if (clgps != null)
            {
                byte[] bar = ReadBytes(stream, 4, ByteArrayHinzuefuegen, clgps);

                if (bar != null)
                    return BitConverter.ToInt32(bar, 0);
            }

            return -1;
        }

        public static long getEightByteByStream(NetworkStream stream, bool ByteArrayHinzuefuegen, clGPS clgps)
        {
            //if (clgps != null)
            {
                byte[] bar = ReadBytes(stream, 8, ByteArrayHinzuefuegen, clgps);

                if (bar != null)
                    return BitConverter.ToInt64(bar, 0);
            }

            return -1;
        }

        public static string ByteArrayToHexString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public static int HexStringToInt(string str)
        {
            string prefixedHex = str;
            int intValue = Convert.ToInt32(prefixedHex, 16);

            return intValue;
        }

        public static byte[] HexStringToBytes(string str)
        {
            try
            {
                if (str == null)
                {
                    return null;
                }
                else if (str.Length < 2)
                {
                    return null;
                }
                else
                {
                    int len = str.Length / 2;
                    byte[] buffer = new byte[len];

                    for (int i = 0; i < len; i++)
                    {
                        buffer[i] = Convert.ToByte(str.Substring(i * 2, 2), 16);
                    }
                    return buffer;
                }
            }
            catch { return null; }
        }

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
        public static string GetHexFromIButton(string ibutton)
        {
            long codeAsLong = Convert.ToInt64(ibutton);

            byte[] bar = BitConverter.GetBytes(codeAsLong);

            string hex = BitConverter.ToString(bar);
            hex = hex.Replace("-", "");

            return hex;
        }
        private static int GetCRC16(byte[] buffer)
        {
            return GetCRC16(buffer, buffer.Length, 0xA001);
        }
        private static int GetCRC16(byte[] buffer, int bufLen, int polynom)
        {
            polynom &= 0xFFFF;
            int crc = 0;
            for (int i = 0; i < bufLen; i++)
            {
                int data = buffer[i] & 0xFF;
                crc ^= data;
                for (int j = 0; j < 8; j++)
                {
                    if ((crc & 0x0001) != 0)
                    {
                        crc = (crc >> 1) ^ polynom;
                    }
                    else
                    {
                        crc = crc >> 1;
                    }
                }
            }
            return crc & 0xFFFF;
        }
        private static bool isReaderReady(NetworkStream ns, double timeout)
        {
            while (true)
            {
                var now = DateTime.Now;

                try
                {
                    while (!ns.DataAvailable && timeout > 0)
                    {
                        Thread.Sleep(100);
                        timeout -= 100;
                    }

                    return ns.DataAvailable;
                }
                catch (IOException)
                {
                    return false;
                }
                catch (Exception)
                {
                    // ignore
                }
                finally
                {
                    timeout -= new TimeSpan(DateTime.Now.Ticks - now.Ticks).TotalMilliseconds;
                }
            }
        }
        public static bool StringToBool(string s)
        {
            bool result = false;

            if (string.IsNullOrEmpty(s))
                return result;

            if (s == "0") return false;
            if (s == "1") return true;

            if (s.Equals("false", StringComparison.OrdinalIgnoreCase)) return false;
            if (s.Equals("true", StringComparison.OrdinalIgnoreCase)) return true;

            if (s.Equals("nein", StringComparison.OrdinalIgnoreCase)) return false;
            if (s.Equals("ja", StringComparison.OrdinalIgnoreCase)) return true;

            if (s.Equals("no", StringComparison.OrdinalIgnoreCase)) return false;
            if (s.Equals("yes", StringComparison.OrdinalIgnoreCase)) return true;

            if (s.Equals("n", StringComparison.OrdinalIgnoreCase)) return false;
            if (s.Equals("j", StringComparison.OrdinalIgnoreCase)) return true;
            if (s.Equals("y", StringComparison.OrdinalIgnoreCase)) return true;

            if (!bool.TryParse(s, out result))
                result = false;

            return result;
        }

        private static void AddToProtokoll(string Text)
        {
            ListBox.Invoke(new Action(() =>
            {
                ListBox.Items.Add(Text);
                ListBox.SelectedIndex = ListBox.Items.Count - 1;
                ListBox.Refresh();
                ListBox.Update();
            }));

            Logger(Text);

            Application.DoEvents();
        }

        public static void Logger(string lines)
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter("c:\\temp\\TestGPSlog.txt", true);
            file.WriteLine(lines);

            file.Close();
        }

        #region Klassen
        public class clGPS
        {
            public Int16 PaketLength { get; set; }
            public long ID { get; set; }
            public long IMEI { get; set; }
            public Int32 CommandID { get; set; }
            public String CRC { get; set; }
            public List<Byte> Lby = new List<Byte>();
            public List<clGPSDatensatz> Datensaetze = new List<clGPSDatensatz>();
        }

        public class clGPSDatensatz
        {
            public DateTime TimeStamp { get; set; }
            public double Latitude { get; set; } = 0;
            public double Longitude { get; set; } = 0;
            public double Altitude { get; set; } = 0;
            public double Angle { get; set; } = 0;
            public int Satellites { get; set; } = 0;
            public double SpeedKMH { get; set; } = 0;
            public double HDOP { get; set; } = 0;
            public bool Zuendung { get; set; } = false;
            public string IButtonHex { get; set; } = String.Empty;
            public string IButtonLong { get; set; } = String.Empty;
            public int Zuordnung_kz { get; set; } = 0;
            public long Zuordnung_id { get; set; } = 0;

            public List<clIODaten> IODaten = new List<clIODaten>();
        }

        public class clIODaten
        {
            public String ID { get; set; }
            public String Value { get; set; }
        }
        #endregion
    }
}
