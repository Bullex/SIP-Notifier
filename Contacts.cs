using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIP_Notifier
{
    class Contacts
    {
        XmlDocument doc = new XmlDocument();

        public Contacts()
        {
        }

        public string lookup(string number)
        {
            
            try
            {
                //string homedir = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

                //doc.Load(homedir + "\\contacts.xml");
                //               XmlNode person = doc.SelectSingleNode("/contacts/person[@number = '" + number.Trim() + "']/name");
                //XmlElement root = doc.DocumentElement;
                //XmlNode person;
                //person = root.SelectSingleNode("descendant::person[number='" + number.Trim() + "']");


                //return person.FirstChild.InnerText;

                SIP_Notifier.Accounts settings = SIP_Notifier.Accounts.Default;
                //textBox1CWebService.Text = settings.WebService;
                //textBox1CUserName.Text = settings.WSUserName;
                //textBox1CPassword.Text = settings.WSPassword;
                //textBox1CHotelCode.Text = settings.WSHotelCode;

                string info = "";
                string hotel_code = "";
                if (!string.IsNullOrEmpty(settings.WSHotelCode))
                {
                    hotel_code = settings.WSHotelCode;
                }
                if (!string.IsNullOrEmpty(settings.WebService))
                {
                    //string url = "http://demo.1chotel.ru/httpservices/hs/info_by_phone";
                    info = HTTP_GET(settings.WebService, number, hotel_code, settings.WSUserName, settings.WSPassword);
                }
                if (string.IsNullOrEmpty(info))
                {
                    info = number;
                }
                return info;
//                XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
//               nsmgr.AddNamespace("ph", "urn:phones");
//              XmlNode person;
//              XmlElement root = doc.DocumentElement;
//              person = root.SelectSingleNode("descendant::person[@ph:number='" + number.Trim() + "']", nsmgr);
//              return person.InnerText;
            }
            catch (Exception e)
            {
                return number;// number;
            }
        }

        private string HTTP_GET(string Url, string Data, string hotel_code, string username, string password)
        {
            string Out = String.Empty;
            System.Net.WebRequest req = System.Net.WebRequest.Create(Url + (string.IsNullOrEmpty(hotel_code) ? "" : ("/" + hotel_code)) + (string.IsNullOrEmpty(Data) ? "" : ("/" + Data)));
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                String encoded = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));
                req.Headers.Add("Authorization", "Basic " + encoded);
            }
            try
            {
                System.Net.WebResponse resp = req.GetResponse();
                using (System.IO.Stream stream = resp.GetResponseStream())
                {
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(stream))
                    {
                        Out = sr.ReadToEnd();
                        sr.Close();
                    }
                }
            }
            catch (ArgumentException ex)
            {
                Out = "Phone: " + Data + "; " + string.Format("HTTP_ERROR :: The second HttpWebRequest object has raised an Argument Exception as 'Connection' Property is set to 'Close' :: {0}", ex.Message);
            }
            catch (System.Net.WebException ex)
            {
                Out = "Phone: " + Data + "; " + string.Format("HTTP_ERROR :: WebException raised! :: {0}", ex.Message);
            }
            catch (Exception ex)
            {
                Out = "Phone: " + Data + "; " + string.Format("HTTP_ERROR :: Exception raised! :: {0}", ex.Message);
            }

            return Out;
        }
    }
}
