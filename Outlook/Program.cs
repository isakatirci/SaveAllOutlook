using System;
using Microsoft.Office.Interop.Outlook;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace Outlook
{
    class Program
    {
        static void Main(string[] args)
        {
            Application myApp = new Application();
            NameSpace mapiNameSpace = myApp.GetNamespace("MAPI");
            MAPIFolder myInbox = mapiNameSpace.GetDefaultFolder(OlDefaultFolders.olFolderInbox);
            using (StreamWriter sw = new StreamWriter("log.txt"))
            {
                foreach (var item in myInbox.Items)
                {
                    try
                    {
                        if (item is MailItem)
                        {

                        }
                        else
                        {
                            sw.WriteLine("nameof:", nameof(item));
                            continue;
                        }
                        var item1 = (MailItem)item;

                        if (myInbox.Items.Count > 0)
                        {
                            // Grab the Subject
                            var lblSubject = item1.Subject;
                            //Grab the Attachment Name
                            if (item1.Attachments.Count > 0)
                            {
                                var lblAttachmentName = item1.Attachments[1].FileName;
                                foreach (Attachment attachment in item1.Attachments)
                                {
                                    try
                                    {
                                        sw.WriteLine(attachment.FileName);
                                        attachment.SaveAsFile("./Dosya/" + attachment.FileName);
                                    }
                                    catch (System.Exception ex)
                                    {
                                        sw.WriteLine(JsonConvert.SerializeObject(ex));
                                    }
                                }
                            }
                            else
                            {
                                var lblAttachmentName = "No Attachment";
                            }
                            // Grab the Body
                            var txtBody = item1.Body;
                            // Sender Name
                            var lblSenderName = item1.SenderName;
                            // Sender Email
                            var lblSenderEmail = item1.SenderEmailAddress;
                            // Creation date
                            var lblCreationdate = item1.CreationTime.ToString();

                        }
                        else
                        {
                            sw.WriteLine("There are no emails in your Inbox.");
                        }
                    }
                    catch (System.Exception ex)
                    {

                        sw.WriteLine(JsonConvert.SerializeObject(ex));
                    }

                }
            }
        }
    }
}
