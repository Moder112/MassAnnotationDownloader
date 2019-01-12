using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YoutubeExplode;

namespace AnnotationsMassDownloader
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(100, Console.WindowHeight);
          // string[] args = { "$(author)//$(title)-$(id)$(ext)", "https://www.youtube.com/watch?v=wsAXCI0R7VE" };
            Console.WriteLine("Youtube Annotation Mass Downloader\nCoded in an hour to preserve old youtube annotations\nlike 6 days before they die.\nDirect any bug reports to @Moder112#0247 on Discord");
            if (args.Length < 1)
            {
                Console.WriteLine("No Arguments use:\nAnnotationsMassDownloader.exe <Path> <Video,Channel or text file>");
            }
            else {
                if (args[1].Contains("list="))
                {
                    string youtube = "list=";
                    string cid = args[1].Substring(args[1].IndexOf(youtube) + youtube.Length).Replace("/", "");
                    try
                    {
                        getplaylist(cid, args[0]);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Playlist init failed\n" + e);
                    }
                }
                else
                if (args[1].Contains("watch?v="))
                {
                    string videoid = args[1].Substring(args[1].IndexOf("=")+1).Replace("/", "");
                    if (videoid.Contains("&"))videoid= videoid.Substring(0, videoid.IndexOf("&"));
                    Console.WriteLine("Downloading: " + videoid);
                    try
                    {
                        runrunshit(videoid, args[0]);
                    }catch(Exception e)
                    {
                        Console.WriteLine("Video init failed\n" + e);
                    }
                }
                else if (args[1].Contains("https://www.youtube.com/user/"))
                {
                    string youtube = "https://www.youtube.com/user/";
                    string cid = args[1].Substring(args[1].IndexOf(youtube) + youtube.Length).Replace("/", "");
                    try
                    {
                        getid(cid, args[0]);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("User init failed\n" + e);
                    }
                    // getchannelshit(cid, args[0]);
                }
                else if (args[1].Contains("https://www.youtube.com/channel/"))
                {
                    string youtube = "https://www.youtube.com/channel/";
                    string cid = args[1].Substring(args[1].IndexOf(youtube) + youtube.Length).Replace("/", "");
                    try
                    {
                        getchannelshit(cid, args[0]);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("User init failed\n" + e);
                    }
                }
               

                else
                {
                    if (File.Exists(args[1]))
                    {
                        filoop(args);
                    }
                }
                // getshit("gRPnGAyHCFQ", "$(author)//$(title)$(ext)");
                
               
            }

            Console.ReadLine();
        }
        static async void filoop(string[] args)
        {
            foreach (string ex in File.ReadAllLines(args[1]))
            {
                 if (ex.Contains("list="))
                {
                    string youtube = "list=";
                    string cid = ex.Substring(ex.IndexOf(youtube) + youtube.Length).Replace("/", "");
                    try
                    {
                        await getplaylist(cid, args[0]);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("User playlist failed\n" + e);
                    }
                }
                else if (ex.Contains("watch?v="))
                {
                    string videoid = ex.Substring(ex.IndexOf("=") + 1).Replace("/", "");
                    if (videoid.Contains("&"))videoid= videoid.Substring(0, videoid.IndexOf("&"));
                    Console.WriteLine("Downloading: " + videoid);
                    try
                    {
                        await runrunshit(videoid, args[0]);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("User init failed\n" + e);
                    }

                }
                else if (ex.Contains("https://www.youtube.com/user/"))
                {
                    string youtube = "https://www.youtube.com/user/";
                    string cid = ex.Substring(ex.IndexOf(youtube) + youtube.Length).Replace("/", "");
                    try
                    {
                        await  getid(cid, args[0]);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("User init failed\n" + e);
                    }
                    // getchannelshit(cid, args[0]);
                }
               
                else if (ex.Contains("https://www.youtube.com/channel/"))
                {
                    string youtube = "https://www.youtube.com/channel/";
                    string cid = ex.Substring(ex.IndexOf(youtube) + youtube.Length).Replace("/", "");
                    try
                    {
                        await  getchannelshit(cid, args[0]);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("User init failed\n" + e);
                    }
                }
            }
        }
        static async         Task
runrunshit(string vidid, string path)
        {
            await getshit(vidid, path);

        }
        static async         Task
getid(string channelid, string path)
        {
            var client = new YoutubeClient();
            int retry2 = 10;
            bool Bretry = false;
            string video = null;
            while (video == null)
            {
                if (retry2 == 0)
                {
                    Bretry = true;
                    break;
                }
                try
                {
                   video = await client.GetChannelIdAsync(channelid);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Retrying channel id download " + (11 - retry2) + " attempt");
                }
                retry2--;
                if (video == null)
                {
                    Thread.Sleep(1000);
                }

            }
           await getchannelshit(video, path);


        }
        static async Task
getchannelshit(string channelid, string path)
        {
            var client = new YoutubeClient();

            IReadOnlyList<YoutubeExplode.Models.Video> video = null;
            int retry2 = 10;
            bool Bretry = false;
            Console.WriteLine("Downloading channel started " + channelid);
            while (video == null)
            {
                if (retry2 == 0)
                {
                    Bretry = true;
                    break;
                }
                try
                {
                    video = await client.GetChannelUploadsAsync(channelid);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Retrying channel download " + (11 - retry2) + " attempt");
                }
                retry2--;
                if (video == null)
                {
                    Thread.Sleep(1000);
                }
                else
                {
                    break;
                }

            }

            //  var video = await client.GetChannelUploadsAsync(channelid);
           
            try
            {
                foreach (var x in video)
                {
                    await getshit(x.Id, path);
                }
            }catch(Exception x)
            {
                Console.WriteLine(x);
            }
           // Console.WriteLine("Downloading channel finished " + channelid);
        }
        static async Task getplaylist(string channelid, string path)
        {
            var client = new YoutubeClient();

            YoutubeExplode.Models.Playlist video = null;
            int retry2 = 10;
            bool Bretry = false;
            Console.WriteLine("Downloading playlist started " + channelid);
            while (video == null)
            {
                if (retry2 == 0)
                {
                    Bretry = true;
                    break;
                }
                try
                {
                    video = await client.GetPlaylistAsync(channelid);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Retrying playlist download " + (11 - retry2) + " attempt");
                }
                retry2--;
                if (video == null)
                {
                    Thread.Sleep(1000);
                }
                else
                {
                    break;
                }

            }

            //  var video = await client.GetChannelUploadsAsync(channelid);
          
            try
            {
                foreach (var x in video.Videos)
                {
                    await getshit(x.Id, path);
                }
            }
            catch (Exception x)
            {
                Console.WriteLine(x);
            }
            // Console.WriteLine("Downloading channel finished " + channelid);
        }
        static async         Task
getshit(string videoid, string path)
        {
            
                var client = new YoutubeClient();
                YoutubeExplode.Models.Video video = null;
                int retry2 = 10;
                bool Bretry = false;
                while (video == null)
                {
                    if (retry2 == 0)
                    {
                        Bretry = true;
                        break;
                    }
                    try
                    {
                     video=   await client.GetVideoAsync(videoid);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Retrying video metadata download " + (11 - retry2) + " attempt");
                    }
                    retry2--;
                    if (video == null)
                    {
                        Thread.Sleep(1000);
                }
                else
                {
                    break;
                }


            }
                var WClient = new WebClient();
                Console.WriteLine(video);
                int retry = 10;
                if (Bretry == false)
                {
                    while (retry > 0)
                    {
                        try
                        {
                            WClient.DownloadFile("http://www.youtube.com/annotations_invideo?features=1&legacy=1&video_id=" + videoid, videoid);
                            while (WClient.IsBusy) Thread.Sleep(100);
                        }
                        catch (Exception e)
                        {
                            retry--;

                            if (retry == 0)

                                Console.WriteLine(e);
                            else
                            {
                                Thread.Sleep(1000);
                                Console.WriteLine("Downloading failed, retrying " + video.Title);

                            }
                            try
                            {
                                if (File.Exists(videoid)) File.Delete(videoid);
                            }
                            catch
                            {

                            }

                            //   Console.ReadLine();
                        }
                        if (vetforformatting(videoid))
                        {
                            string titletest = title(path, videoid, video.Title, video.Author);
                            if (!File.Exists(titletest))
                            {
                                File.Copy(videoid, titletest);
                                Console.WriteLine("Downloaded annotations for video: " + video.Title);
                                if (File.Exists(videoid)) File.Delete(videoid);
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Annotations file exists already: " + video.Title);
                                if (File.Exists(videoid)) File.Delete(videoid);
                                break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Video doesn't contain old youtube annotations:  " + video.Title);
                            if (File.Exists(videoid)) File.Delete(videoid);
                            break;
                        }
                 
                }
                }
                else
                {
                    Console.WriteLine("Cannot retrieve metadata with YoutubeExplode, abandoning download");
                return;
                }
           

        }
        static bool vetforformatting(string path)
        {
            foreach(string x in File.ReadAllLines(path))
            {
                if (x.Contains("</appearance>")) return true;
                if (x.Contains("</segment>")) return true;
            }
            return false;
        }
        static string title(string path, string id, string title, string author)
        {

            path= path.Replace("$(id)", id);
            path = path.Replace("$(title)",removeinvalid( title));
            path = path.Replace("$(author)", removeinvalid(author));
            path = path.Replace("$(ext)",".xml");
            
            Console.WriteLine(path);
         Directory.CreateDirectory(Path.GetDirectoryName( path));
            return path ;

        }
        static string removeinvalid(string inv)
        {
            string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());

            foreach (char c in invalid)
            {
                inv = inv.Replace(c.ToString(), "");
            }
            return inv;
        }
    }
}
