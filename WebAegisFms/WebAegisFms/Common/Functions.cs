using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Globalization;
using System.ComponentModel;
using System.Data;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Text.RegularExpressions;
using System.Linq;

namespace Common
{
    public class Functions
    {
        public static string ByteToString(Object byteObject)
        {
            string returnString;
            byte[] byteValue;
            try
            {
                if (byteObject.GetType().ToString() == "System.Byte[]")
                {
                    byteValue = (byte[])byteObject;
                    System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                    returnString = asciiEncoding.GetString(byteValue);
                }
                else
                    returnString = byteObject.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnString;
        }

        public static byte[] StringToByte(string str)
        {
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            return encoding.GetBytes(str);
        }


        public static bool IsDBNull(object value)
        {
            if (value is DBNull)
                return true;
            else
                return false;
        }

        public static string ToString(object value)
        {
            if (IsDBNull(value))
                return "";
            else if (value == "")
                return "";
            else
                return (Convert.ToString(value));
        }


        public static bool IsNumeric(string s)
        {
            try
            {
                Decimal.Parse(s);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static bool IsDate(string s)
        {
            try
            {
                DateTime.Parse(s);
            }
            catch
            {
                return false;
            }
            return true;
        }


        public static Int32 ToInt32(object value)
        {
            if (IsDBNull(value))
                return 0;
            else if (value == "")
                return 0;
            else
            {
                if (!string.IsNullOrEmpty(Convert.ToString(value)))
                {
                    return (Convert.ToInt32(value));
                }
                else
                    return 0;
            }
        }

        public static double ToDouble(object value)
        {
            if (IsDBNull(value))
                return 0.00;
            else if (value == "")
                return 0.00;
            else
            {
                if (!string.IsNullOrEmpty(Convert.ToString(value)))
                {
                    return (Convert.ToDouble(value));
                }
                else
                    return 0.00;
            }
        }

        public static string Post(string url, string postData)
        {
            string responseFromServer = "";
            try
            {
                // Create a request using a URL that can receive a post. 
                WebRequest request = WebRequest.Create(url);
                // Set the Method property of the request to POST.
                request.Method = "POST";
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    // Get the stream containing content returned by the server.
                    dataStream = response.GetResponseStream();
                    // Open the stream using a StreamReader for easy access.
                    StreamReader reader = new StreamReader(dataStream);
                    // Read the content.
                    responseFromServer = reader.ReadToEnd();
                    // Clean up the streams.
                    reader.Close();
                }
                dataStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return responseFromServer;
        }

        public static string PostXML(string url, string xmlString)
        {
            string responseFromServer = "";
            try
            {
                // Create a request using a URL that can receive a post. 
                WebRequest request = WebRequest.Create(url);
                // Set the Method property of the request to POST.
                request.Method = "POST";
                byte[] byteArray = Encoding.UTF8.GetBytes(xmlString);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "text/xml";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    // Get the stream containing content returned by the server.
                    dataStream = response.GetResponseStream();
                    // Open the stream using a StreamReader for easy access.
                    StreamReader reader = new StreamReader(dataStream);
                    // Read the content.
                    responseFromServer = reader.ReadToEnd();
                    // Clean up the streams.
                    reader.Close();
                }
                dataStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return responseFromServer;
        }

        //****************************************************************************
        //This function will format the date to required Date format
        //testDate: 1=current display date, 2=database date format, 3="date" format, 4=current date for db, 5=date format to db format    
        //****************************************************************************
        /// <summary>
        /// 
        /// </summary>
        /// <param name="testdate"></param>
        /// <param name="dateFormat"></param>
        public static string GetDate(string testDate, int dateFormat)
        {
            DateTime dateTime;
            string result = "";

            if (!string.IsNullOrEmpty(testDate))
                dateTime = Convert.ToDateTime(testDate);
            else
                dateTime = DateTime.Now;

            if (testDate != "")
            {
                switch (dateFormat)
                {
                    case 1:
                        return result = (DateTime.Today).ToString("MM/d/yyyy");
                    case 2:
                        if (testDate != "")
                            return result = testDate.Substring(4, 2) + "/" + testDate.Substring(6, 2) + "/" + testDate.Substring(0, 4);
                        else
                            return result;
                    case 3:
                        return result = Convert.ToDateTime(testDate).ToString("MM/dd/yyyy");
                    case 4:
                        return result = (DateTime.Today).ToString("yyyyMMdd");
                    case 5:
                        return result = Convert.ToDateTime(testDate).ToString("yyyyMMdd");
                    case 6:
                        //Added for display date in format yyyy-mm-dd and time.
                        return result = dateTime.ToString("u").Substring(0, dateTime.ToString("u").LastIndexOf("Z"));
                    case 7:
                        //Added for display date in format yyyy-mm-dd.
                        return result = dateTime.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo);
                    case 8:
                        //Added for display date in (g) General date/short time:. 9/1/2008 7:23 PM
                        return result = dateTime.ToString("g");
                    case 9:
                        return result = Convert.ToDateTime(testDate).ToString("MM-dd-yyyy");
                    case 10:
                        return result = Convert.ToDateTime(testDate).ToString("MM-dd-yyyy HH:mm");
                    case 11:
                        return result = Convert.ToDateTime(testDate).ToString("MM-dd-yyyy hh:mm tt");   //MP_ITS@20121015:added for date formate 10-15-2012 15.00 PM

                    default: return result = "";
                }
            }
            return result;
        }
        //Chetan_ITS@20091203
        public static string SplitPhone(string phvalue)
        {
           
           
            string result="";
            if (phvalue.Contains("-"))
            {
                return phvalue;
            }
            else
            {
                if(phvalue.Length==10)
                {
                  
                    result = phvalue.Substring(0, 3) + "-" + phvalue.Substring(3, 3) + "-" + phvalue.Substring(6, 4);
                 
                }
                return result;
            }
            
            
        }
        public static string GetTime(string testTime, int timeFormat)
        {
            string result = "";
            switch (timeFormat)
            {
                case 1:
                    return result = DateTime.Now.ToLongTimeString();
                case 2:
                    return result = (DateTime.Now).ToString("hh:mm:ss");
                default: return result = "";
            }
        }

        public static string GetHostName()
        {
            string hostName = "";
            // Getting Ip address of local machine...
            // First get the host name of local machine.
            hostName = Dns.GetHostName ();
            
            return hostName;
        }

        public static string GetHostIP()
        {
            string hostName = GetHostName();
            string hostIP = "";

            // using host name, get the IP address list..
            IPHostEntry ipEntry = Dns.GetHostEntry(hostName);
            IPAddress[] ipAddress = ipEntry.AddressList;

            for (int i = 0; i < ipAddress.Length; i++)
            {
                hostIP = hostIP + ipAddress[i].ToString();
            }
            return hostIP;
        }

        public static string GetHostDetails()
        {
            string hostName = GetHostName();
            string hostIP = GetHostIP();

            return hostName + " " + hostIP;
        }

        public static void WriteToFile(string inputText, string absoluteFilePath)
        {
            try
            {
                FileStream fs = new FileStream(absoluteFilePath, FileMode.Create, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(inputText);
                sw.Close();
                fs.Close();
            }
            catch { }
        }

        public static void DeleteFile(string absoluteFilePath)
        {
            //Nilesh@20100525 : Error log Problem
            //Logger logger;
            //string ErrorLogPath = Config.Get("ErrorLogPath");
            //Logger.Configure();
            //logger = new Logger("%-5p %d %5rms %-22.22c{1} %-18.18M - %m%n", ErrorLogPath + "WebErrors_" + DateTime.Now.ToString("yyyyMMdd") + ".err");

            try
            {
                if (File.Exists(absoluteFilePath))
                {
                    File.Delete(absoluteFilePath);
                }
            }
            catch(Exception ex)
            {
                
            }
        }


        public static Int32 GetDateDiffInDays(string sourceDate, string targetDate)
        {
            TimeSpan spanPassed;
            DateTime date1 = System.Convert.ToDateTime(sourceDate);
            DateTime date2 = System.Convert.ToDateTime(targetDate);
            spanPassed = date1.Subtract(date2);

            return spanPassed.Days;
        }

        public static Int32 GetDateDiffInMonths(string CurrentDate, string targetDate)
        {
            DateTime startDate = System.Convert.ToDateTime(CurrentDate);
            DateTime endDate = System.Convert.ToDateTime(targetDate);
            
            int monthsApart = 12 * (startDate.Year - endDate.Year) + startDate.Month - endDate.Month;
            return Math.Abs(monthsApart); 
        }


        public static DataTable ToDataTable<T>(IList<T> list)
        {
            DataTable table = CreateTable<T>();
            Type entityType = typeof(T);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

            foreach (T item in list)
            {
                DataRow row = table.NewRow();

                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item);
                }

                table.Rows.Add(row);
            }

            return table;
        }

        public static DataTable CreateTable<T>()
        {
            Type entityType = typeof(T);
            DataTable table = new DataTable(entityType.Name);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, prop.PropertyType);
            }

            return table;
        }


        public static byte[] Base64ToByte(string base64)
        {
            if (base64 == null) //Then Throw New ArgumentNullException("base64")
            {
                return new byte[0];
            }
            else
            {
                return Convert.FromBase64String(base64);
            }
        }

        public static byte[] FromBase64(string base64)
        {
            if (base64 == null) //Then Throw New ArgumentNullException("base64")
            {
                return new byte[0];
            }
            else
            {
                return Convert.FromBase64String(base64);
            }
        }

        public static string ByteToBase64(byte[] byteData)
        {
            if(byteData == null)
            {
                return "" ;
            }
            else
            {
                 return Convert.ToBase64String(byteData);
            }

        }

        public static string FileToBase64(string filePath)
        {
            byte[] data = null;

            try
            {
                FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                long numBytes = new FileInfo(filePath).Length;
                //logger.LogInfo("Library.Functions.FileToBase64(): Path:" + filePath + " Bytes:" + numBytes);
                data = br.ReadBytes((int)numBytes);
                br.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                return "";
            }

            if (data == null)
            {
                return "";
            }
            else
            {
                return Convert.ToBase64String(data);
            }
        }
        public static string CapitalFirstLetter(string StrText)
        {
            return new CultureInfo("en").TextInfo.ToTitleCase(StrText.ToLower());
        }



        public static T GetXmlObject<T>(string xmlString)
        {
            //Export to String
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                TextReader tr = new StringReader(xmlString);
                XmlReader xreader = new XmlTextReader(tr);
                T xmlClassObject = (T)serializer.Deserialize(xreader);
                return xmlClassObject;
            }
            catch
            {
                throw;
            }
        }

        public static string GetXmlString(object xmlClassObject)        
        {
         
            try
            {
                XmlSerializer serializer = new XmlSerializer(xmlClassObject.GetType());
                StringWriter Output = new StringWriter(new StringBuilder());
                serializer.Serialize(Output, xmlClassObject);
                string tmpReturnString = Output.ToString().Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                return tmpReturnString;
            }
            catch
            {
                throw;
            }
        }

        public static Boolean CreateDirectory(string _targetFolder)
        {
            try
            {
                if (!Directory.Exists(_targetFolder))
                {
                    Directory.CreateDirectory(_targetFolder);
                }
                return true;
            }
            catch
            {
                throw;
                return false;
            }
        }


        public static string CreateBuildPath(string _targetCaseID)
        {
            int _configurableDigits = Convert.ToInt32(Config.Get("BuildPathConfDigits"));
            int _configurableLevel = Convert.ToInt32(Config.Get("BuildPathConfLevels"));
            string _targetPath = Convert.ToString(Config.Get("UploadDocPath"));
            int caseLevel = _configurableDigits - Convert.ToInt32(_targetCaseID.Length);
            int buildLevel = _configurableLevel - caseLevel;
            string buildPath = string.Empty;
            string buildDirStructure = string.Empty;
            try
            {                
                if( Convert.ToInt32(_targetCaseID) > 1000)
                {
                    for (int count = 0; count < caseLevel; count++)
                    {
                        buildPath = buildPath + "\\0";      
                    }
                    buildDirStructure = _targetCaseID.Substring(0, buildLevel);
                    for (int count = 1; count <= buildLevel; count++)
                    {                        
                            buildPath = buildPath + "\\" + buildDirStructure.Substring(count-1, 1);      
                    }                    

                    buildPath =   buildPath + "\\" + _targetCaseID;
                }
                else
                {
                    buildPath =  "\\0\\0\\0\\0\\0\\0\\" + _targetCaseID;
                }

                if (!Directory.Exists(_targetPath + buildPath))
                {
                    Directory.CreateDirectory(_targetPath + buildPath);
                }
                return  buildPath;
                
            }
            catch
            {
                throw;
                return "";
            }
        }

        //Kaushal_ITS@20100211:START_targetCaseID
        public static string CreateBuildPath(string _targetMessageID, string _targetPath)
        {
            int _configurableDigits = Convert.ToInt32(Config.Get("BuildPathConfDigits"));
            int _configurableLevel = Convert.ToInt32(Config.Get("BuildPathConfLevels"));
            int caseLevel = _configurableDigits - Convert.ToInt32(_targetMessageID.Length);
            int buildLevel = _configurableLevel - caseLevel;
            string buildPath = string.Empty;
            string buildDirStructure = string.Empty;
            try
            {
                if (Convert.ToInt32(_targetMessageID) > 1000)
                {
                    for (int count = 0; count < caseLevel; count++)
                    {
                        buildPath = buildPath + "\\0";
                    }
                    buildDirStructure = _targetMessageID.Substring(0, buildLevel);
                    for (int count = 1; count <= buildLevel; count++)
                    {
                        buildPath = buildPath + "\\" + buildDirStructure.Substring(count - 1, 1);
                    }

                    buildPath = buildPath + "\\" + _targetMessageID;
                }
                else
                {
                    buildPath = "0\\0\\0\\0\\0\\0\\" + _targetMessageID;
                }

                if (!Directory.Exists(_targetPath + buildPath))
                {
                    Directory.CreateDirectory(_targetPath + buildPath);
                }
                return buildPath;

            }
            catch
            {
                throw;
                return "";
            }
        }
        //Kaushal_ITS@20100211:END


        //Ankit@ITS@20100129:tkt#000212: Document Copy  : Start
        public static Boolean CopyFolder(string existingDocPath, string uploadDocPath)
        {
            bool isResult = false;
            try
            {
                string _targetPath = Convert.ToString(Config.Get("UploadDocPath"));

                DirectoryInfo dir1 = new DirectoryInfo(_targetPath + existingDocPath);

                if (Directory.Exists(_targetPath + existingDocPath) && Directory.Exists(_targetPath + uploadDocPath))
                {
                    FileInfo[] Folder1Files = dir1.GetFiles();
                    if (Folder1Files.Length > 0)
                    {
                        foreach (FileInfo aFile in Folder1Files)
                        {
                            if (!File.Exists(_targetPath + uploadDocPath + "\\" + aFile.Name))
                            {
                                aFile.CopyTo(_targetPath + uploadDocPath + "\\" + aFile.Name);
                            }
                        }
                    }
                    isResult = true;
                }
                else
                {
                    isResult = false;
                }
                    
            }
            catch (Exception ex)
            {
                
            }

            return isResult;
        }
        //Ankit@ITS@20100129:tkt#000212: Document Copy  : End

        public static string TrimZero(string byval)
        {
            string result = byval;
            int conertResult = 0;

            try
            {
                conertResult = Convert.ToInt32(byval);
                result = conertResult.ToString();
            }
            catch
            {
                return result;
            }

            return result;
        }

        /*Ankit@20100429 - USPS Address Verification - tkt:1410 - Start*/
        public static string HttpGet(string URI)
        {
            try
            {
                WebRequest req = WebRequest.Create(URI);
                //req.Proxy = new System.Net.WebProxy(ProxyString, true); //true means no proxy
                WebResponse resp = req.GetResponse();
                StreamReader sr = new StreamReader(resp.GetResponseStream());
                return sr.ReadToEnd().Trim();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /*Ankit@20100429 - USPS Address Verification - tkt:1410 - End*/

        //Parimal_ITS
        public static string DecodeFromBase64(string strDecodeFileDir, string serializedFile)
        {
            try
            {
                string fileName=string.Empty;
                if (Path.GetExtension(strDecodeFileDir)!=string.Empty)
                {
                    fileName = strDecodeFileDir;
                }
                else
                {
                    fileName = strDecodeFileDir + "RxDoc-" + DateTime.Now.Date.ToString("MMddyyyy") + "-" + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + ".docx";
                }
                using (System.IO.FileStream reader = System.IO.File.Create(fileName))
                {
                    byte[] buffer = Convert.FromBase64String(serializedFile);
                    reader.Write(buffer, 0, buffer.Length);
                    reader.Close();
                    reader.Dispose();

                    //this.DeleteFile(fileName);
                    //return Convert.ToString(strDecodeFileDir);
                    return Convert.ToString(fileName);
                }
            }
            catch
            {
                return "";
            }

        }

        /// <summary>
        /// Ankit_ITS@20100825: This function is used to delete directory at loacted path
        /// </summary>
        /// <returns></returns>
        public static Boolean DeleteDirectory(string _targetFolder)                                                                   
        {
            try
            {
                if (Directory.Exists(_targetFolder))      
                {
                    Directory.Delete(_targetFolder, true);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        /// <summary>
        /// Parimal_ITS 20100820: This function is used to return current system's temparary folder path
        /// </summary>
        /// <returns></returns>
        public static string GetTempFolderPath()
        {
            return Path.GetTempPath();
        }

        /// <summary>
        /// Ankit@20100904- Merges two object of same type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="source">The source(Object fill program).</param>
        /// <param name="destination">The destination(Object fill by excuting d/b query).</param>
        /// <returns></returns>
        public static T MergeWith<T, U>(T source, U destination) where U : class, T
        {
            Type primaryType = typeof(T);
            Type secondaryType = typeof(U);
            try
            {
                if (destination != null)
                {
                    foreach (System.Reflection.PropertyInfo primaryInfo in primaryType.GetProperties())
                    {
                        if (primaryInfo.CanWrite)
                        {
                            object currentPrimary = primaryInfo.GetValue(source, null);

                            System.Reflection.PropertyInfo secondaryInfo = secondaryType.GetProperty(primaryInfo.Name);
                            object currentSecondary = secondaryInfo.GetValue(destination, null);

                            if (currentPrimary == null && currentSecondary != null)
                            {
                                primaryInfo.SetValue(source, currentSecondary, null);
                            }
                            else if ((currentPrimary != null && currentSecondary != null) && isChildClass(primaryInfo))
                            {
                                if (isCollection(currentPrimary))
                                {
                                    // here
                                }
                                else
                                {
                                    // string NavigationModel = "RxOffice.Model.BorrowerInfo";
                                    //MethodInfo method = typeof(BorrowerInfo).GetMethod("MergeWith");
                                    //MethodInfo generic = method.MakeGenericMethod(primaryInfo.PropertyType, primaryInfo.PropertyType);
                                    //object returnChild = generic.Invoke(this, new object[2] { currentPrimary, currentSecondary });
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
            }

            return source;
        }

        private static bool isCollection(object property)
        {
            return typeof(ICollection).IsAssignableFrom(property.GetType())
                || typeof(ICollection<>).IsAssignableFrom(property.GetType());
        }

        private static bool isChildClass(System.Reflection.PropertyInfo propertyInfo)
        {
            return (propertyInfo.PropertyType.IsClass && !propertyInfo.PropertyType.IsValueType &&
                                !propertyInfo.PropertyType.IsPrimitive && propertyInfo.PropertyType.FullName != "System.String");
        }

        //PK_ITS@20110418:tkt::START: Added to get bytes from file
        public static byte[] FileToBytes(string filePath)
        {
            byte[] data = null;
            try
            {
                FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                long numBytes = new FileInfo(filePath).Length;
                //logger.LogInfo("Library.Functions.FileToBase64(): Path:" + filePath + " Bytes:" + numBytes);
                data = br.ReadBytes((int)numBytes);
            }
            catch (Exception ex)
            {
                //logger.LogInfo("Library.Functions.FileToBase64(): Path:" + filePath + "; Exception: " + ex.ToString());
                return null;
            }
            return data;
        }
        //PK_ITS@20110418:tkt::END: Added to get bytes from file



        //PK_ITS@20110418:tkt::START: Added to get bytes from file
        public static byte[] StringToBytes(string requestXML){
            byte[] data = null;
            try{
                Encoding enc = System.Text.Encoding.ASCII;
                data = enc.GetBytes(requestXML);                
            }
            catch (Exception ex){
                //logger.LogInfo("Library.Functions.FileToBase64(): Path:" + filePath + "; Exception: " + ex.ToString());
                return null;
            }
            return data;
        }
        //PK_ITS@20110418:tkt::END: Added to get bytes from file

        
        //Kaushal_ITS@20110513: Merging of two xml
        public static string MergeXML(string xml1, string xml2)
        {
            XmlDocument xmlDoc1 = new XmlDocument();
            XmlDocument xmlDoc2 = new XmlDocument();
            xmlDoc1.LoadXml(xml1);
            xmlDoc2.LoadXml(xml2);
            XmlNode newNode;
            string mergedXML = string.Empty;
            for (int j = 0; j <= xmlDoc2.DocumentElement.ChildNodes.Count - 1; j++)
            {
                newNode = xmlDoc1.ImportNode(xmlDoc2.DocumentElement.ChildNodes[j], true);
                xmlDoc1.DocumentElement.AppendChild(newNode);
            }
            mergedXML = Functions.GetXmlString(xmlDoc1);

            return mergedXML;
        }

        //PK_ITS@20110418:tkt::START: Added to write request and response xml files
        public static bool WriteFileToDisk(string xmlString, string transactionId, string eventName){
            string diskPath = Config.Get("GeneratedXMLPath");
            if (string.IsNullOrEmpty(diskPath)) { diskPath = Path.GetTempPath(); }

            if (!diskPath.EndsWith("\\")) { diskPath = diskPath + "\\"; }

            switch (eventName.ToUpper())
            {
                case "REQUEST":

                    diskPath = diskPath + "P-REQUEST" + "\\";
                    WriteFile(diskPath, "RxReqXML-" + transactionId.ToString() + ".xml", xmlString);
                    

                    break;
                case "P-REQUEST":

                    diskPath = diskPath + "P-REQUEST" + "\\";
                    WriteFile(diskPath, "RxReq-" + transactionId.ToString() + ".xml", xmlString);

                    break;

                case "P-RESPONSE":

                    diskPath = diskPath + "P-RESPONSE" + "\\";
                    WriteFile(diskPath, "RxRpn-" + transactionId.ToString() + ".xml", xmlString);

                    break;

                case "I-REQUEST":

                    diskPath = diskPath + "I-REQUEST" + "\\";
                    WriteFile(diskPath, "RxReq-" + transactionId.ToString() + ".xml", xmlString);

                    break;

                case "I-RESPONSE":

                    diskPath = diskPath + "I-RESPONSE" + "\\";
                    WriteFile(diskPath, "RxRpn-" + transactionId.ToString() + ".xml", xmlString);

                    break;
                case "W-XML":

                    diskPath = diskPath + "W-XML" + "\\";
                    WriteFile(diskPath, "RxRpn-" + transactionId.ToString() + ".xml", xmlString);

                    break;

                default:

                    return false;
                    break;
            }
            return true;
        }

        private static void WriteFile(string diskPath, string fileName, string fileData){
            if (!Directory.Exists(diskPath)){
                Directory.CreateDirectory(diskPath);
            }

            Functions.WriteToFile(fileData, diskPath + fileName);
        }
        //PK_ITS@20110418:tkt::END: Added to write request and response xml files
        public static double ConvertSize(double bytes, string type)
        {
            try
            {
                const int CONVERSION_VALUE = 1024;
                //determine what conversion they want
                switch (type)
                {
                    case "BY":
                        //convert to bytes (default)
                        return bytes;
                        break;
                    case "KB":
                        //convert to kilobytes
                        return Math.Round((bytes / CONVERSION_VALUE));
                        break;
                    case "MB":
                        //convert to megabytes
                        return Math.Round((bytes / CalculateSquare(CONVERSION_VALUE)));
                        break;
                    case "GB":
                        //convert to gigabytes
                        return Math.Round((bytes / CalculateCube(CONVERSION_VALUE)));
                        break;
                    default:
                        //default
                        return bytes;
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }
        public static double CalculateSquare(Int32 number)
        {
            return Math.Pow(number, 2);
        }
        public static double CalculateCube(Int32 number)
        {
            return Math.Pow(number, 3);
        }

         /// Added:Checks the password strength.
        /// Password Enter by user must satisfy password policy. It should contains at least any 3 combination of Lower Case Letter,
        /// Upper Case Leter, Digit,Special Character
        public static bool CheckPasswordStrength(string pwd)
        {

            var reExprDigit = new Regex("(?=.*[0-9])");
            var reExprLowerCase = new Regex("(?=.*[a-z])");
            var reExprUpperCase = new Regex("(?=.*[A-Z])");
            var reExprSpecialChar = new Regex("(?=.*[@#$%/!^&*()+|??<>,~`=;:{}-])");
            var reExprInputLen = new Regex("^.{6,18}$");

            var count = 0;
            var notfound = 0;
            var pwdLen = 0;
            //Search weather digits exists in input. If match found the validate it with digit RE.
            if (reExprDigit.IsMatch(pwd))
            {
                if (reExprDigit.IsMatch(pwd))
                {
                    //alert('reExprDigit');
                    count++;
                }
            }
            else
            {
                count++;
                notfound++;
            }
            //Search weather lower case exists in input. If match found the validate it with lower case RE.
            if (reExprLowerCase.IsMatch(pwd))
            {
                if (reExprLowerCase.IsMatch(pwd))
                {
                    //alert('reExprLowerCase');
                    count++;
                }
            }
            else
            {
                count++;
                notfound++;
            }

            //Search weather upper case exists in input. If match found the validate it with upper case RE.
            if (reExprUpperCase.IsMatch(pwd))
            {
                if (reExprUpperCase.IsMatch(pwd))
                {
                    //alert('reExprUpperCase');
                    count++;
                }
            }
            else
            {
                count++;
                notfound++;
            }

            //Search weather special char exists in input. If match found the validate it with special char RE.
            if (reExprSpecialChar.IsMatch(pwd))
            {
                //alert('match found');
                if (reExprSpecialChar.IsMatch(pwd))
                {
                    //alert('reExprSpecialChar');
                    count++;
                }
            }
            else
            {
                count++;
                notfound++;
            }

            //alert(notfound);
            if (reExprInputLen.IsMatch(pwd))
            {
                //alert('reExprInputLen');
                pwdLen++;
            }

            if (count >= 4 && notfound < 2 && pwdLen > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string GetLastSSN(string strSSN)
        {   
            strSSN = strSSN.Replace("-", "");
            if (strSSN.Length == 9)
            {
                strSSN = strSSN.Substring(5, 4);
            }
            return strSSN;
        }

        public static Hashtable ToHashTable(DataTable dt, string key,string value)
        {
            Hashtable ht=new Hashtable();
            foreach (DataRow dr in dt.Rows)
            {
                if (!ht.ContainsKey(dr[key]))
                {
                    ht.Add(dr[key], dr[value]);
                }
            }

            return ht;
        }

        public static string GetRandomString()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, 8)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());

            return result;
        }
    }
}
