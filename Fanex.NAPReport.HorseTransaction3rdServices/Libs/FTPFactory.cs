namespace Fanex.NAPReport.HorseTransaction3rd.Business.Libs
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;

    public class FTPFactory
    {
        private static int blockSize = 512;

        private string remoteHost, remotePath, remoteUser, remotePass, mes;
        private int remotePort, bytes;
        private Socket clientSocket;
        private int retValue;
        private bool debug;
        private bool logined;
        private string reply;
        private byte[] buffer = new byte[blockSize];
        private Encoding ascii = Encoding.ASCII;
        private string txtLocalLog = string.Empty;       

        public FTPFactory()
        {
            remoteHost = "localhost";
            remotePath = ".";
            remoteUser = "anonymous";
            remotePass = "jaimon@school2000.co.uk";
            remotePort = 21;
            debug = false;
            logined = false;
        }

        public string Log()
        {
            return txtLocalLog;
        }

        public void SetRemoteHost(string remoteHost)
        {
            this.remoteHost = remoteHost;
        }

        public string GetRemoteHost()
        {
            return remoteHost;
        }

        public void SetRemotePort(int remotePort)
        {
            this.remotePort = remotePort;
        }

        public int GetRemotePort()
        {
            return remotePort;
        }

        public void SetRemotePath(string remotePath)
        {
            this.remotePath = remotePath;
        }

        public string GetRemotePath()
        {
            return remotePath;
        }

        public void SetRemoteUser(string remoteUser)
        {
            this.remoteUser = remoteUser;
        }

        public void SetRemotePass(string remotePass)
        {
            this.remotePass = remotePass;
        }

        public string[] GetFileList(string mask)
        {
            if (!logined)
            {
                Login();
            }

            Socket socket = CreateDataSocket();

            SendCommand("NLST " + mask);

            if (!(retValue == 150 || retValue == 125))
            {
                throw new IOException(reply.Substring(4));
            }

            mes = string.Empty;

            while (true)
            {
                int bytes = socket.Receive(buffer, buffer.Length, 0);
                mes += ascii.GetString(buffer, 0, bytes);

                if (bytes < buffer.Length)
                {
                    break;
                }
            }

            char[] seperator = { '\n' };
            string[] mess = mes.Split(seperator);

            socket.Close();

            ReadReply();

            if (retValue != 226)
            {
                throw new IOException(reply.Substring(4));
            }

            return mess;
        }

        public long GetFileSize(string fileName)
        {
            if (!logined)
            {
                Login();
            }

            SendCommand("SIZE " + fileName);
            long size = 0;

            if (retValue == 213)
            {
                size = Convert.ToInt64(reply.Substring(4));
            }
            else
            {
                throw new IOException(reply.Substring(4));
            }

            return size;
        }

        public void Login()
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(remoteHost), remotePort);

            try
            {
                clientSocket.Connect(ep);
            }
            catch (Exception)
            {
                throw new IOException("Couldn't connect to remote server");
            }

            ReadReply();
            if (retValue != 220)
            {
                Close();
                throw new IOException(reply.Substring(4));
            }

            if (debug)
            {
                txtLocalLog = txtLocalLog + "\r\n" + "USER " + remoteUser;
            }

            SendCommand("USER " + remoteUser);

            if (!(retValue == 331 || retValue == 230))
            {
                CleanUp();
                throw new IOException(reply.Substring(4));
            }

            if (retValue != 230)
            {
                if (debug)
                {
                    txtLocalLog = txtLocalLog + "\r\n" + "PASS xxx";
                }

                SendCommand("PASS " + remotePass);
                if (!(retValue == 230 || retValue == 202))
                {
                    CleanUp();
                    throw new IOException(reply.Substring(4));
                }
            }

            logined = true;
            txtLocalLog = txtLocalLog + "\r\n" + "Connected to " + remoteHost;
            Chdir(remotePath);
        }

        public void SetBinaryMode(bool mode)
        {
            if (mode)
            {
                SendCommand("TYPE I");
            }
            else
            {
                SendCommand("TYPE A");
            }

            if (retValue != 200)
            {
                throw new IOException(reply.Substring(4));
            }
        }

        public void Download(string remFileName)
        {
            Download(remFileName, string.Empty, false);
        }

        public void Download(string remFileName, bool resume)
        {
            Download(remFileName, string.Empty, resume);
        }

        public void Download(string remFileName, string locFileName)
        {
            Download(remFileName, locFileName, false);
        }

        public void Download(string remFileName, string locFileName, bool resume)
        {
            if (!logined)
            {
                Login();
            }

            SetBinaryMode(true);
            txtLocalLog = txtLocalLog + "\r\n" + "Downloading file " + remFileName + " from " + remoteHost + "/" + remotePath;

            if (string.IsNullOrEmpty(locFileName))
            {
                locFileName = remFileName;
            }

            if (!File.Exists(locFileName))
            {
                Stream st = File.Create(locFileName);
                st.Close();
            }

            FileStream output = new
            FileStream(locFileName, FileMode.Open);

            Socket socket = CreateDataSocket();

            long offset = 0;

            if (resume)
            {
                offset = output.Length;

                if (offset > 0)
                {
                    SendCommand("REST " + offset);
                    if (retValue != 350)
                    {
                        offset = 0;
                    }
                }

                if (offset > 0)
                {
                    if (debug)
                    {
                        txtLocalLog = txtLocalLog + "\r\n" + "seeking to " + offset;
                    }

                    long npos = output.Seek(offset, SeekOrigin.Begin);
                    txtLocalLog = txtLocalLog + "\r\n" + "new pos=" + npos;
                }
            }

            SendCommand("RETR " + remFileName);

            if (!(retValue == 150 || retValue == 125))
            {
                throw new IOException(reply.Substring(4));
            }

            while (true)
            {
                bytes = socket.Receive(buffer, buffer.Length, 0);
                output.Write(buffer, 0, bytes);

                if (bytes <= 0)
                {
                    break;
                }
            }

            output.Close();
            if (socket.Connected)
            {
                socket.Close();
            }

            txtLocalLog = txtLocalLog + "\r\n";

            ReadReply();

            if (!(retValue == 226 || retValue == 250))
            {
                throw new IOException(reply.Substring(4));
            }
        }

        public void Upload(string fileName)
        {
            Upload(fileName, false);
        }

        public void Upload(string fileName, bool resume)
        {
            if (!logined)
            {
                Login();
            }

            Socket socket = CreateDataSocket();
            long offset = 0;

            if (resume)
            {
                try
                {
                    SetBinaryMode(true);
                    offset = GetFileSize(fileName);
                }
                catch (Exception)
                {
                    offset = 0;
                }
            }

            if (offset > 0)
            {
                SendCommand("REST " + offset);
                if (retValue != 350)
                {
                    offset = 0;
                }
            }

            SendCommand("STOR " + Path.GetFileName(fileName));

            if (!(retValue == 125 || retValue == 150))
            {
                throw new IOException(reply.Substring(4));
            }

            FileStream input = new
            FileStream(fileName, FileMode.Open);

            if (offset != 0)
            {
                if (debug)
                {
                    txtLocalLog = txtLocalLog + "\r\n" + "seeking to " + offset;
                }

                input.Seek(offset, SeekOrigin.Begin);
            }

            txtLocalLog = txtLocalLog + "\r\n" + "Uploading file " + fileName + " to " + remotePath;
            while ((bytes = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                socket.Send(buffer, bytes, 0);
            }

            input.Close();
            txtLocalLog = txtLocalLog + "\r\n";
            if (socket.Connected)
            {
                socket.Close();
            }

            ReadReply();
            if (!(retValue == 226 || retValue == 250))
            {
                throw new IOException(reply.Substring(4));
            }
        }

        public void DeleteRemoteFile(string fileName)
        {
            if (!logined)
            {
                Login();
            }

            SendCommand("DELE " + fileName);

            if (retValue != 250)
            {
                throw new IOException(reply.Substring(4));
            }
        }

        public void RenameRemoteFile(string oldFileName, string newFileName)
        {
            if (!logined)
            {
                Login();
            }

            SendCommand("RNFR " + oldFileName);

            if (retValue != 350)
            {
                throw new IOException(reply.Substring(4));
            }

            SendCommand("RNTO " + newFileName);
            if (retValue != 250)
            {
                throw new IOException(reply.Substring(4));
            }
        }

        public void Mkdir(string dirName)
        {
            if (!logined)
            {
                Login();
            }

            SendCommand("MKD " + dirName);

            if (retValue != 250)
            {
                throw new IOException(reply.Substring(4));
            }
        }

        public void Rmdir(string dirName)
        {
            if (!logined)
            {
                Login();
            }

            SendCommand("RMD " + dirName);

            if (retValue != 250)
            {
                throw new IOException(reply.Substring(4));
            }
        }

        public void Chdir(string dirName)
        {
            if (dirName.Equals("."))
            {
                return;
            }

            if (!logined)
            {
                Login();
            }

            SendCommand("CWD " + dirName);

            if (retValue != 250)
            {
                throw new IOException(reply.Substring(4));
            }

            this.remotePath = dirName;
            txtLocalLog = txtLocalLog + "\r\n" + "Current directory is " + remotePath;
        }

        public void Close()
        {
            if (clientSocket != null)
            {
                SendCommand("QUIT");
            }

            CleanUp();
            txtLocalLog = txtLocalLog + "\r\n" + "Closing...";
        }

        public void SetDebug(bool debug)
        {
            this.debug = debug;
        }

        private void ReadReply()
        {
            mes = string.Empty;
            reply = ReadLine();
            retValue = Convert.ToInt32(reply.Substring(0, 3));
        }

        private void CleanUp()
        {
            if (clientSocket != null)
            {
                clientSocket.Close();
                clientSocket = null;
            }

            logined = false;
        }

        private string ReadLine()
        {
            while (true)
            {
                bytes = clientSocket.Receive(buffer, buffer.Length, 0);
                mes += ascii.GetString(buffer, 0, bytes);
                if (bytes < buffer.Length)
                {
                    break;
                }
            }

            char[] seperator = { '\n' };
            string[] mess = mes.Split(seperator);

            if (mes.Length > 2)
            {
                mes = mess[mess.Length - 2];
            }
            else
            {
                mes = mess[0];
            }

            if (!mes.Substring(3, 1).Equals(" "))
            {
                return ReadLine();
            }

            if (debug)
            {
                for (int k = 0; k < mess.Length - 1; k++)
                {
                    Console.WriteLine(mess[k]);
                }
            }

            return mes;
        }

        private void SendCommand(string command)
        {
            byte[] cmdBytes = Encoding.ASCII.GetBytes((command + "\r\n").ToCharArray());
            clientSocket.Send(cmdBytes, cmdBytes.Length, 0);
            ReadReply();
        }

        private Socket CreateDataSocket()
        {
            SendCommand("PASV");
            if (retValue != 227)
            {
                throw new IOException(reply.Substring(4));
            }

            int index1 = reply.IndexOf('(');
            int index2 = reply.IndexOf(')');
            string ipData =
            reply.Substring(index1 + 1, index2 - index1 - 1);
            int[] parts = new int[6];

            int len = ipData.Length;
            int partCount = 0;
            string buf = string.Empty;

            for (int i = 0; i < len && partCount <= 6; i++)
            {
                char ch = Convert.ToChar(ipData.Substring(i, 1));
                if (char.IsDigit(ch))
                {
                    buf += ch;
                }
                else if (ch != ',')
                {
                    throw new IOException("Malformed PASV reply: " +
                    reply);
                }

                if (ch == ',' || i + 1 == len)
                {
                    try
                    {
                        parts[partCount++] = Convert.ToInt32(buf);
                        buf = string.Empty;
                    }
                    catch (Exception)
                    {
                        throw new IOException("Malformed PASV reply: " +
                        reply);
                    }
                }
            }

            string ipAddress = parts[0] + "." + parts[1] + "." +
            parts[2] + "." + parts[3];

            int port = (parts[4] << 8) + parts[5];
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ipAddress), port);

            try
            {
                s.Connect(ep);
            }
            catch (Exception)
            {
                throw new IOException("Can't connect to remote server");
            }

            return s;
        }
    }
}
