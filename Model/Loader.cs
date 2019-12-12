using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Model
{
    public class Loader
    {
        public string Url { get; set; }

        public List<XElement> Data { get; set; }


        public Loader(string url)
        {
            this.Url = url;
        }

        public void LoadData()
        {
            XDocument xdoc = XDocument.Load(Url);
            this.Data = xdoc.Descendants("people").Elements("person")
                .Select(node => node).Where(node => {
                    //Console.WriteLine(node.Element("room").Value);
                    return node.Element("room").Value != "";
                })
                .ToList();

        }


    }
}
