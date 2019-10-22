using System;
using System.IO;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections;
using Newtonsoft.Json;



namespace Server
{
    //internal class Request
    internal class Response
    {
        //public string Method;
        //public string Path;
        //public string Date;
        //public string Body;

        public string Status { get; set; }
        public string Body { get; set; }

        //public Request(string method, string path, string date, string body)
        //{
        //Method = method;
        //Path = path;
        //Date = date;
        //Body = body;
        //}
        //}
    }
        internal class Request
    {
            public string Method { get; set; }
            public string Path { get; set; }
            public string Date { get; set; }
            public string Body { get; set; }

    }

            internal static class Program
    {
                //static TcpListener server = null;
                //static TcpClient client = null;
                //static int counter = 0;
                //static String data = null;

                private static TcpListener _server;
                private static TcpClient _client;
                private static int _counter;

                private static string _data;

                // Buffer for reading data

                //static Byte[] bytes = new Byte[256];
                private static readonly byte[] Bytes = new byte[256];

                private static bool IsIn<T>(this T source, params T[] values)
                {
                    return ((IList)values).Contains(source);
                }

                private static Response DealWithRequest(Request r)
                {
                    //var resp = Response();
                    //var resp = new Response("", "");
                    var resp = new Response();


                    if (!r.Method.IsIn("create", "read", "update", "delete", "echo"))
                    {
                        //resp.Status = 4;
                        resp.Status = "4";

                        resp.Body = "Illegal method";
                    }

                    else if (r.Method == "")
                    {
                        resp.Status = "4";
                        resp.Body = "Missing method";
                    }

                    else if (r.Path == "")
                    {
                        resp.Status = "4";
                        resp.Body = "Missing path";
                    }
                    else if (r.Date == "")
                    {
                        resp.Status = "4";
                        resp.Body = "Missing date";
                    }

                        return resp;
                }

                static void Main(string[] args)
        {
                    //TcpListener server = null;

                    //const int port = 5000;
                    //var localAddr = IPAddress.Parse("127.0.0.1");

                    const int port = 5000;
                    var localAddr = IPAddress.Parse("127.0.0.1");

                    try
                    {
                        // Set the TcpListener on port 5000.
                        //Int32 port = 5000;
                        //IPAddress localAddr = IPAddress.Parse("127.0.0.1");

                        // TcpListener server = new TcpListener(port);

                        //server = new TcpListener(localAddr, port);
                        _server = new TcpListener(localAddr, port);


                        // Start listening for client requests.
                        _server.Start();

                        // Buffer for reading data

                        //Byte[] bytes = new Byte[256];
                        //String data = null;

                        var bytes = new byte[256];


                        // Enter the listening loop.
                        while (true)
                        {
                            Console.WriteLine("Waiting for a connection... ");

                            // Perform a blocking call to accept requests.
                            // You could also user server.AcceptSocket() here.
                            //TcpClient client = server.AcceptTcpClient();
                            //var client = server.AcceptTcpClient();

                            //Console.WriteLine("Connected!");

                            //data = null;

                            // Get a stream object for reading and writing
                            //NetworkStream stream = client.GetStream();
                            //var stream = client.GetStream();


                            //int i;

                            //client = server.AcceptTcpClient();
                            //counter += 1;
                            //Console.WriteLine("Connected!" + "Client No:" + Convert.ToString(counter));
                            //Thread connect = new Thread(HandleClient);

                            _client = _server.AcceptTcpClient();
                            _counter += 1;
                            Console.WriteLine("Connected!" + "Client No:" + Convert.ToString(_counter));
                            var connect = new Thread(HandleClient);

                            connect.Start();

                            // Loop to receive all the data sent by the client.
                            // while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                            //{
                            // Translate data bytes to a ASCII string.
                            //data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                            //var data = Encoding.ASCII.GetString(bytes, 0, i);

                            //Console.WriteLine("Received: {0}", data);

                            // Process the data sent by the client.
                            //data = data.ToUpper();

                            //byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);
                            //var msg = Encoding.ASCII.GetBytes(data);


                            // Send back a response.
                            //stream.Write(msg, 0, msg.Length);
                            //Console.WriteLine("Sent: {0}", data);
                        }

                        // Shutdown and end connection
                        // client.Close();
                        //}
                    }
                    catch (SocketException e)
                    {
                        Console.WriteLine("SocketException: {0}", e);
                    }
                    finally
                    {
                        // Stop listening for new clients.

                        //server?.Stop();
                        _server.Stop();

                    }


                    Console.WriteLine("\nHit enter to continue...");
                    Console.Read();
                }
                static void HandleClient()
                {
                    //data = null;
                    _data = null;


                    // Get a stream object for reading and writing

                    // NetworkStream stream = client.GetStream();
                    var stream = _client.GetStream();


                    int i;

                    // Loop to receive all the data sent by the client.

                    //while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    while ((i = stream.Read(Bytes, 0, Bytes.Length)) != 0)


                    {
                        // Translate data bytes to a ASCII string.
                        //data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        //Console.WriteLine("Received: {0}", data);

                        _data = Encoding.ASCII.GetString(Bytes, 0, i);
                        Console.WriteLine("Received: {0}", _data);

                        // Process the data sent by the client.

                        //data = data.ToUpper();
                        _data = _data.ToUpper();
                        var r = JsonConvert.DeserializeObject<Request>(_data);


                        //byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);
                        //var msg = Encoding.ASCII.GetBytes(_data);
                        var response = DealWithRequest(r);

                        // Send back a response.

                        //stream.Write(msg, 0, msg.Length);
                        //Console.WriteLine("Sent: {0}", data);
                        //Console.WriteLine("Sent: {0}", _data);

                        var data = JsonConvert.SerializeObject(response);
                        stream.Write(Encoding.ASCII.GetBytes(data), 0, data.Length);

                    }

                    // Shutdown and end connection

                    //client.Close();
                    _client.Close();

                }
            }
        }
    


    
