using System.Net.Sockets;
using System.Net;
using System.Text;

namespace TeltonikaGPS
{
    public partial class Form1 : Form
    {
        TcpListener ListenerGps = null;
        string IpAdresse = "192.168.5.149";
        int PortGps = 22479;

        public Form1()
        {
            InitializeComponent();

            txtBoxIP.Text = IpAdresse;
            txtBoxPort.Text = PortGps.ToString();
            clGPSParser.ListBox = this.listBox1;
        }

        private void FormEinstellungen_Shown(object sender, EventArgs e)
        {

        }
        private void FormEinstellungen_Load(object sender, EventArgs e)
        {

        }
        private void StartServerGps(System.Net.IPAddress addr, int port)
        {
            AddToProtokoll("START - Server wurde gestartet.");

            try
            {
                ListenerGps = new TcpListener(addr, port);
                ListenerGps.Start();

                if (addr == null)
                    AddToProtokoll("Der Server (GPS) ist gestartet. (Any:" + port.ToString() + ")");
                else
                    AddToProtokoll("Der Server (GPS) ist gestartet. (" + IpAdresse + ":" + port.ToString() + ")");

                ListenerGps.BeginAcceptTcpClient(this.OnAcceptConnectionGps, ListenerGps);
            }
            catch (Exception err)
            {
                AddToProtokoll("Der Server (GPS) konnte nicht gestartet werden. Grund:");
                AddToProtokoll(err.Message);
            }
        }

        private void OnAcceptConnectionGps(IAsyncResult asyn)
        {
            TcpListener listener = null;

            try
            {
                // Get the listener that handles the client request.
                listener = (TcpListener)asyn.AsyncState;

                // Get the newly connected TcpClient
                TcpClient client = listener.EndAcceptTcpClient(asyn);

                // Start the client work
                Thread proct = new Thread(new ParameterizedThreadStart(ClientConnectedGps));

                proct.Start(client);
            }
            catch (Exception err)
            {
                AddToProtokoll(err.Message);
            }

            // Issue another connect, only do this if you want to handle multiple clients
            if (listener != null)
                listener.BeginAcceptTcpClient(this.OnAcceptConnectionGps, listener);
        }

        private void ClientConnectedGps(object data)
        {
            // Es ist ein GPS Gerät verbunden.

            AddToProtokoll("CLIENT GPS - Es wurde ein GPS Client verbunden.");

            DateTime ProtokollStart = DateTime.Now;

            string Info = string.Empty;

            try
            {
                using (TcpClient client = data as TcpClient)
                {
                    if (client != null)
                    {
                        using (NetworkStream ns = client.GetStream())
                        {
                            if (ns != null)
                            {
                                var gps = clGPSParser.getDatensaetzeByNetworkStream(ns);

                                if (gps != null)
                                {
                                    if (gps.Datensaetze != null)
                                    {
                                        Info = "IMEI " + gps.IMEI + " " + gps.Datensaetze.Count + " Datensaetze, Command " + gps.CommandID;

                                        if (gps.Datensaetze.Count > 0)
                                            Info += " (1. DS " + gps.Datensaetze[0].TimeStamp.ToString() + ")";

                                        AddToProtokoll("SERVER GPS \n " + Info);
                                    }
                                }
                                else
                                {
                                    Info = "GPS Daten fehlerhaft.";

                                    AddToProtokoll("SERVER GPS \n " + Info);
                                }
                            }
                        }
                    }
                }
            }
            catch { }

            //if (ProtokollAktiv)
            //{
            //    // Protokoll Eintrag senden.

            //    StringBuilder sb = new StringBuilder();

            //    sb.AppendLine("[Protokoll]");
            //    sb.AppendLine("Datum=" + ProtokollStart.ToShortDateString());
            //    sb.AppendLine("Uhrzeit=" + ProtokollStart.ToShortTimeString());
            //    sb.AppendLine("Dauer=" + (DateTime.Now.Ticks - ProtokollStart.Ticks).ToString());
            //    sb.AppendLine("Quelle=GPS");
            //    sb.AppendLine("Anforderung=" + Info);

            //    ProtokollSenden(sb.ToString());
            //}
        }

        private void AddToProtokoll(string Text)
        {
            listBox1.Invoke(new Action(() =>
            {
                listBox1.Items.Add(Text);
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.Refresh();
                listBox1.Update();
            }));

            Logger(Text);

            Application.DoEvents();
        }

        private void Start_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(IpAdresse))
            {
                lblStatus.Text = "Keine IP eingetragen";
                lblStatus.ForeColor = Color.Red;
                return;
            }

            if (PortGps == 0)
            {
                lblStatus.Text = "Kein Port eingetragen";
                lblStatus.ForeColor = Color.Red;
                return;
            }

            try
            {
                StartServerGps(IPAddress.Parse(IpAdresse), PortGps);
            }
            catch (Exception err)
            {
                AddToProtokoll(err.Message);
            }
        }

        private void Stop_Click(object sender, EventArgs e)
        {
            try
            {
                ListenerGps.Stop();
                AddToProtokoll("STOP - Server wurde beendet.");
            }
            catch (Exception err)
            {
                AddToProtokoll(err.Message);
            }
        }

        private void txtBoxIP_TextChanged(object sender, EventArgs e)
        {
            IpAdresse = txtBoxIP.Text;
        }

        private void txtBoxPort_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtBoxPort.Text))
                PortGps = Convert.ToInt32(txtBoxPort.Text);
        }

        public static void Logger(string lines)
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter("c:\\temp\\TestGPSlog.txt", true);
            file.WriteLine(lines);

            file.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (File.Exists(@"c:\temp\TestGPSlog.txt"))
                File.Delete(@"c:\temp\TestGPSlog.txt");

            File.WriteAllText("c:\\temp\\TestGPSlog.txt", "");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }
    }
}