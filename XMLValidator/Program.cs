using System;
using System.Collections.Generic;
using System.Linq;


namespace XMLValidator
{
    class Program
    {
        private static int countCharFromString(char charToCount, string inputString)
        {
            int count = 0;
            foreach (char val in inputString.ToCharArray())
            {
                if (val == charToCount)
                {
                    count++;
                }
            }

            return count;
        }

        private static List<string> getAllNodes(string xml)
        {
            List<string> strNodes = new List<string>();

            for (int i = 0; i < xml.Length; i++)
            {
                if (xml[i] == '<')
                {
                    string node = string.Empty;
                    for (int j = i; ; j++)
                    {
                        if (xml[j] != '>')
                        {
                            node = node + xml[j];
                        }
                        else
                        {
                            node = node + xml[j];
                            break;
                            i = j;
                        }
                    }
                    strNodes.Add(node);
                }
            }
            return strNodes;
        }

        public static bool validateXML(string xml, out string message)
        {
            bool isValid = true;

            // validating &lt; and &gt; char counts
            int ltCount = countCharFromString('<', xml);
            int gtCount = countCharFromString('>', xml);
            if (ltCount != gtCount)
            {
                message = "Invalid XML<and> tag count mismatch";
                return false;
            }

            // Validate xml Nodes, does it contains the open and close tags 
            List<string> xmlNodes = getAllNodes(xml);
            string[] openNodes = xmlNodes.Where(r => !r.Contains("/")).ToArray();
            string[] closingNodes = xmlNodes.Where(r => r.Contains("/")).ToArray();

            foreach (string node in openNodes)
            {
                string strClosingNodes = String.Join(",", closingNodes).Replace(" ", "");
                string value = node.Replace("<", "").Replace(">", "");

                if (!strClosingNodes.Contains(value))
                {
                    message = "Closing tag seems missing..";
                    return false;
                }
            }

            // Validate xml node order
            string[] reverseClosingNodes = closingNodes.Reverse().ToArray();
            for (int i = 0; i < openNodes.Count(); i++)
            {
                string openNodeValue = openNodes[i].Replace("<", "").Replace(">", "");
                string closingNodeValue = reverseClosingNodes[i].Replace("<", "").Replace(">", "").Replace("/", "");
                if (openNodeValue != closingNodeValue)
                {
                    message = "Open and closing tag order is invalid..";
                    return false;
                }
            }

            message = "Given XML string is valid";

            return isValid;
        }


        static void Main(string[] args)
        {
            string xml = Console.ReadLine();
            string message = "";
            bool result = validateXML(xml, out message);
            Console.WriteLine("Output = {0}", result);
            Console.WriteLine(message);
            Console.ReadLine();
        }
    }
}
